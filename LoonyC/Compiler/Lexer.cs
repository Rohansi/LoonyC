using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;

namespace LoonyC.Compiler
{
    partial class Lexer : IEnumerable<Token>
    {
        private readonly IEnumerator<char> _source;
        private int _length;
        private List<char> _read;

        private readonly string _fileName;

        private int _index;
        private int _currentLine;

        public Lexer(string source, string fileName = null)
            : this(source, source == null ? -1 : source.Length, fileName)
        {

        }

        public Lexer(IEnumerable<char> source, int length, string fileName = null)
        {
            if (source == null || length <= 0)
                throw new ArgumentNullException("source");

            _source = source.GetEnumerator();
            _length = length;
            _read = new List<char>(16);

            _fileName = fileName;

            _index = 0;
            _currentLine = 1;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            while (_index < _length)
            {
                SkipWhiteSpace();

                if (_index >= _length)
                    break;

                // single line comment (discarded)
                if (TakeIfNext("//"))
                {
                    while (!IsNext("\n"))
                    {
                        TakeChar();
                    }

                    continue;
                }

                // multi line comment (discarded)
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

                    continue;
                }

                var startLine = _currentLine;
                var ch = PeekChar();

                // operators
                var opList = _operators.Lookup(ch);
                if (opList != null)
                {
                    var op = opList.FirstOrDefault(o => TakeIfNext(o.Item1));

                    if (op != null)
                    {
                        yield return new Token(_fileName, _currentLine, op.Item2, op.Item1);
                        continue;
                    }
                }

                // string/char
                if (TakeIfNext('"') || TakeIfNext('\''))
                {
                    var stringTerminator = ch;
                    var stringContentsBuilder = new StringBuilder();

                    while (true)
                    {
                        if (_index >= _length)
                            throw new CompilerException(_fileName, startLine, CompilerError.UnterminatedString);

                        ch = TakeChar();

                        if (ch == stringTerminator)
                            break;

                        switch (ch)
                        {
                            case '\\':
                                ch = TakeChar();

                                if (_index >= _length)
                                    throw new CompilerException(_fileName, _currentLine, CompilerError.UnexpectedEofString);

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
                                        throw new CompilerException(_fileName, _currentLine, CompilerError.InvalidEscapeSequence, ch);
                                }

                                break;

                            default:
                                stringContentsBuilder.Append(ch);
                                break;
                        }
                    }

                    var stringContents = stringContentsBuilder.ToString(); // TODO: we need to convert this to CP437 byte array or something

                    if (stringTerminator == '\'')
                    {
                        if (stringContents.Length != 1)
                            throw new CompilerException(_fileName, _currentLine, CompilerError.CharLiteralLength);

                        yield return new Token(_fileName, _currentLine, TokenType.SingleString, stringContents);
                        continue;
                    }

                    yield return new Token(_fileName, _currentLine, TokenType.String, stringContents);
                    continue;
                }

                // keyword/word
                if (char.IsLetter(ch) || ch == '_')
                {
                    var wordContents = TakeWhile(c => char.IsLetterOrDigit(c) || c == '_');
                    TokenType keywordType;
                    var isKeyword = _keywords.TryGetValue(wordContents, out keywordType);

                    yield return new Token(_fileName, _currentLine, isKeyword ? keywordType : TokenType.Identifier, wordContents);
                    continue;
                }

                // number
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
                        throw new CompilerException(_fileName, _currentLine, CompilerError.InvalidNumber, format.ToString().ToLower(), numberContents);

                    yield return new Token(_fileName, _currentLine, TokenType.Number, number.ToString("G", CultureInfo.InvariantCulture));
                    continue;
                }

                throw new CompilerException(_fileName, _currentLine, CompilerError.UnexpectedCharacter, ch);
            }

            while (true)
                yield return new Token(_fileName, _currentLine, TokenType.Eof, null);
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

        private bool TakeIfNext(char value)
        {
            if (PeekChar() != value)
                return false;

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
                _currentLine++;

            if (result == '\r')
            {
                if (PeekChar() == '\n')
                    TakeCharImpl();

                _currentLine++;
            }

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
                    throw new CompilerException(_fileName, _currentLine, "Unsupported NumberFormat");
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
    }
}
