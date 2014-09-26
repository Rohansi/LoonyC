using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using JetBrains.Annotations;

namespace LoonyC.Compiler
{
    partial class Lexer : IEnumerable<Token>
    {
        private readonly string _fileName;
        private readonly IEnumerable<char> _sourceEnumerable;

        private IEnumerator<char> _source;
        private int _length;
        private List<char> _read;

        private int _index;
        private int _line;
        private int _column;
        private int _startLine;
        private int _startColumn;

        public Lexer(IEnumerable<char> source, string fileName = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            _fileName = fileName;
            _sourceEnumerable = source;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            _length = int.MaxValue;
            _source = _sourceEnumerable.GetEnumerator();
            _read = new List<char>(16);

            _index = 0;
            _line = 1;
            _column = 1;

            while (_index < _length)
            {
                SkipWhiteSpace();

                if (SkipComment())
                    continue;

                if (_index >= _length)
                    break;

                _startLine = _line;
                _startColumn = _column;

                var ch = PeekChar();
                Token token;

                if (!TryLexOperator(ch, out token) &&
                    !TryLexString(ch, out token) &&
                    !TryLexWord(ch, out token) &&
                    !TryLexNumber(ch, out token))
                {
                    throw Error(CompilerError.UnexpectedCharacter, ch);
                }

                yield return token;
            }

            while (true)
                yield return Token(TokenType.Eof, null);
        }

        private bool TryLexOperator(char ch, out Token token)
        {
            var opList = _operators.Lookup(ch);
            if (opList != null)
            {
                var op = opList.FirstOrDefault(o => TakeIfNext(o.Item1));

                if (op != null)
                {
                    token = Token(op.Item2, op.Item1);
                    return true;
                }
            }

            token = null;
            return false;
        }

        private bool TryLexString(char ch, out Token token)
        {
            if (ch == '"' || ch == '\'')
            {
                TakeChar();

                var stringTerminator = ch;
                var stringContentsBuilder = new StringBuilder();

                while (true)
                {
                    if (_index >= _length)
                        throw Error(CompilerError.UnterminatedString);

                    ch = TakeChar();

                    if (ch == stringTerminator)
                        break;

                    if (ch != '\\')
                    {
                        stringContentsBuilder.Append(ch);
                        continue;
                    }

                    // escape sequence
                    ch = TakeChar();

                    if (_index >= _length)
                        throw Error(CompilerError.UnexpectedEofString);

                    switch (ch)
                    {
                        case '\\':
                            stringContentsBuilder.Append('\\');
                            break;

                        case '"':
                            stringContentsBuilder.Append('"');
                            break;

                        case '\'':
                            stringContentsBuilder.Append('\'');
                            break;

                        case 'n':
                            stringContentsBuilder.Append('\n');
                            break;

                        case 't':
                            stringContentsBuilder.Append('\t');
                            break;

                        // TODO: more escape sequences

                        default:
                            throw Error(CompilerError.InvalidEscapeSequence, ch);
                    }
                }

                var stringContents = stringContentsBuilder.ToString(); // TODO: we need to convert this to CP437 byte array or something

                if (stringTerminator == '\'')
                {
                    if (stringContents.Length != 1)
                        throw Error(CompilerError.CharLiteralLength);

                    token = Token(TokenType.SingleString, stringContents);
                    return true;
                }

                token = Token(TokenType.String, stringContents);
                return true;
            }

            token = null;
            return false;
        }

        private bool TryLexWord(char ch, out Token token)
        {
            if (char.IsLetter(ch) || ch == '_')
            {
                var wordContents = TakeWhile(c => char.IsLetterOrDigit(c) || c == '_');
                TokenType keywordType;
                var isKeyword = _keywords.TryGetValue(wordContents, out keywordType);

                token = Token(isKeyword ? keywordType : TokenType.Identifier, wordContents);
                return true;
            }

            token = null;
            return false;
        }

        private bool TryLexNumber(char ch, out Token token)
        {
            if (char.IsDigit(ch))
            {
                var format = NumberFormat.Decimal;

                if (ch == '0')
                {
                    var nextChar = PeekChar(1);

                    if (nextChar == 'x' || nextChar == 'X')
                        format = NumberFormat.Hexadecimal;

                    if (nextChar == 'b' || nextChar == 'B')
                        format = NumberFormat.Binary;

                    if (format != NumberFormat.Decimal)
                    {
                        TakeChar(); // '0'
                        TakeChar(); // 'x' or 'b'
                    }
                }

                Func<char, bool> isDigit = c => char.IsDigit(c) || (format == NumberFormat.Hexadecimal && _hexChars.Contains(c));

                var numberContents = TakeWhile(c =>
                {
                    if (c == '_' && isDigit(PeekChar(1)))
                    {
                        TakeChar();
                        return true;
                    }

                    return isDigit(c);
                });

                int number;
                if (!TryParseNumber(numberContents, format, out number))
                    throw Error(CompilerError.InvalidNumber, format.ToString().ToLower(), numberContents);

                token = Token(TokenType.Number, number.ToString("G", CultureInfo.InvariantCulture));
                return true;
            }

            token = null;
            return false;
        }

        private bool SkipComment()
        {
            // single line comment
            if (TakeIfNext("//"))
            {
                while (!IsNext("\n"))
                {
                    TakeChar();
                }

                return true;
            }

            // multi line comment
            if (TakeIfNext("/*"))
            {
                var depth = 1;

                while (_index < _length && depth > 0)
                {
                    if (TakeIfNext("/*"))
                    {
                        depth++;
                        continue;
                    }

                    if (TakeIfNext("*/"))
                    {
                        depth--;
                        continue;
                    }

                    TakeChar();
                }

                return true;
            }

            return false;
        }

        private void SkipWhiteSpace()
        {
            while (_index < _length)
            {
                var ch = PeekChar();

                if (!char.IsWhiteSpace(ch))
                    break;

                TakeChar();
            }
        }

        private bool TakeIfNext(string value)
        {
            if (!IsNext(value))
                return false;

            for (var i = 0; i < value.Length; i++)
                TakeChar();

            return true;
        }

        private bool IsNext(string value)
        {
            if (_index + value.Length > _length)
                return false;

            return !value.Where((t, i) => PeekChar(i) != t).Any();
        }

        private string TakeWhile(Func<char, bool> condition)
        {
            var sb = new StringBuilder();

            while (_index < _length)
            {
                var ch = PeekChar();

                if (!condition(ch))
                    break;

                sb.Append(TakeChar());
            }

            return sb.ToString();
        }

        public char TakeChar()
        {
            var result = TakeCharImpl();

            if (result == '\n')
            {
                _line++;
                _column = 0;
            }

            if (result == '\r')
            {
                if (PeekChar() == '\n')
                    TakeCharImpl();

                _line++;
                _column = 0;
            }

            _column++;

            return result;
        }

        private char TakeCharImpl()
        {
            PeekChar();

            var result = _read[0];
            _read.RemoveAt(0);
            _index++;

            return result;
        }

        public string PeekString(int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException("length", "distance must be at least 1");

            var sb = new StringBuilder(length);

            for (var i = 0; i < length; i++)
            {
                sb.Append(PeekChar(i));
            }

            return sb.ToString();
        }

        public char PeekChar(int distance = 0)
        {
            if (distance < 0)
                throw new ArgumentOutOfRangeException("distance", "distance can't be negative");

            while (_read.Count <= distance)
            {
                var success = _source.MoveNext();
                _read.Add(success ? _source.Current : '\0');

                if (!success)
                    _length = _index + _read.Count - 1;
            }

            return _read[distance];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        enum NumberFormat
        {
            Decimal, Hexadecimal, Binary
        }

        private bool TryParseNumber(string value, NumberFormat format, out int result)
        {
            switch (format)
            {
                case NumberFormat.Decimal:
                    if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                        return true;

                    break;

                case NumberFormat.Hexadecimal:
                    if (TryParseBase(value, 16, out result))
                        return true;

                    break;

                case NumberFormat.Binary:
                    if (TryParseBase(value, 2, out result))
                        return true;

                    break;

                default:
                    throw Error("Unsupported NumberFormat");
            }

            result = 0;
            return false;
        }

        private static bool TryParseBase(string value, int fromBase, out int result)
        {
            try
            {
                result = Convert.ToInt32(value, fromBase);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }

        private Token Token(TokenType type, string contents)
        {
            return new Token(_fileName, _startLine, _startColumn, _line, _column - 1, type, contents);
        }

        [StringFormatMethod("format")]
        private Exception Error(string format, params object[] args)
        {
            return new LexerException(_fileName, new SourcePosition(_line, _column), format, args);
        }

        [StringFormatMethod("format")]
        private Exception ErrorStart(string format, params object[] args)
        {
            return new LexerException(_fileName, new SourcePosition(_startLine, _startColumn), format, args);
        }
    }
}
