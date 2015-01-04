using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using LoonyC.Shared.Lexer;

namespace LoonyC.Compiler
{
    partial class LoonyLexer : Lexer<LoonyToken, LoonyTokenType>
    {
        public LoonyLexer(IEnumerable<char> source, string fileName = null)
            : base(source, fileName)
        {
            Rules = new List<LexerRule>
            {
                TryLexOperator,
                TryLexString,
                TryLexWord,
                TryLexNumber
            };
        }

        private bool TryLexOperator(char ch, out LoonyToken token)
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

        private bool TryLexString(char ch, out LoonyToken token)
        {
            if (ch == '"' || ch == '\'')
            {
                TakeChar();

                var stringTerminator = ch;
                var stringContentsBuilder = new StringBuilder();

                while (true)
                {
                    if (AtEof)
                        throw ErrorStart(CompilerError.UnterminatedString);

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

                    if (AtEof)
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

                    token = Token(LoonyTokenType.SingleString, stringContents);
                    return true;
                }

                token = Token(LoonyTokenType.String, stringContents);
                return true;
            }

            token = null;
            return false;
        }

        private bool TryLexWord(char ch, out LoonyToken token)
        {
            if (char.IsLetter(ch) || ch == '_')
            {
                var wordContents = TakeWhile(c => char.IsLetterOrDigit(c) || c == '_');
                LoonyTokenType keywordType;
                var isKeyword = _keywords.TryGetValue(wordContents, out keywordType);

                token = Token(isKeyword ? keywordType : LoonyTokenType.Identifier, wordContents);
                return true;
            }

            token = null;
            return false;
        }

        private bool TryLexNumber(char ch, out LoonyToken token)
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

                Func<char, bool> isHexLetter = c => (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f');
                Func<char, bool> isDigit = c => char.IsDigit(c) || (format == NumberFormat.Hexadecimal && isHexLetter(c));

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

                token = Token(LoonyTokenType.Number, number.ToString("G", CultureInfo.InvariantCulture));
                return true;
            }

            token = null;
            return false;
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

        private LoonyToken Token(LoonyTokenType type, string contents)
        {
            return new LoonyToken(FileName, new SourcePosition(StartLine, StartColumn), new SourcePosition(Line, Column - 1), type, contents);
        }
    }
}
