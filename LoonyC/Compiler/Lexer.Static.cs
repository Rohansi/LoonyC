using System;
using System.Collections.Generic;
using System.Linq;

namespace LoonyC.Compiler
{
    internal partial class Lexer
    {
        private static HashSet<char> _punctuation;
        private static List<Tuple<string, TokenType>> _operators;
        private static Dictionary<string, TokenType> _keywords;

        static Lexer()
        {
            _operators = new List<Tuple<string, TokenType>>
            {
                Tuple.Create(";", TokenType.Semicolon),
                Tuple.Create(",", TokenType.Comma),
                Tuple.Create(".", TokenType.Dot),
                Tuple.Create("=", TokenType.Assign),
                Tuple.Create("?", TokenType.QuestionMark),
                Tuple.Create(":", TokenType.Colon),
                Tuple.Create("->", TokenType.Pointy),

                Tuple.Create("(", TokenType.LeftParen),
                Tuple.Create(")", TokenType.RightParen),

                Tuple.Create("{", TokenType.LeftBrace),
                Tuple.Create("}", TokenType.RightBrace),

                Tuple.Create("[", TokenType.LeftSquare),
                Tuple.Create("]", TokenType.RightSquare),
                
                Tuple.Create("+", TokenType.Add),
                Tuple.Create("+=", TokenType.AddAssign),
                Tuple.Create("-", TokenType.Subtract),
                Tuple.Create("-=", TokenType.SubtractAssign),
                Tuple.Create("*", TokenType.Multiply),
                Tuple.Create("*=", TokenType.MultiplyAssign),
                Tuple.Create("/", TokenType.Divide),
                Tuple.Create("/=", TokenType.DivideAssign),
                Tuple.Create("%", TokenType.Remainder),
                Tuple.Create("%=", TokenType.RemainderAssign),
                Tuple.Create("++", TokenType.Increment),
                Tuple.Create("--", TokenType.Decrement),

                Tuple.Create("&", TokenType.And),
                Tuple.Create("&=", TokenType.AndAssign),
                Tuple.Create("|", TokenType.Or),
                Tuple.Create("|=", TokenType.OrAssign),
                Tuple.Create("^", TokenType.Xor),
                Tuple.Create("^=", TokenType.XorAssign),
                Tuple.Create("<<", TokenType.ShiftLeft),
                Tuple.Create("<<=", TokenType.ShiftLeftAssign),
                Tuple.Create(">>", TokenType.ShiftRight),
                Tuple.Create(">>=", TokenType.ShiftRightAssign),
                Tuple.Create("~", TokenType.Not),
                
                Tuple.Create("==", TokenType.EqualTo),
                Tuple.Create("!=", TokenType.NotEqualTo),
                Tuple.Create(">", TokenType.GreaterThan),
                Tuple.Create(">=", TokenType.GreaterThanOrEqual),
                Tuple.Create("<", TokenType.LessThan),
                Tuple.Create("<=", TokenType.LessThanOrEqual),
                Tuple.Create("&&", TokenType.ConditionalAnd),
                Tuple.Create("||", TokenType.ConditionalOr),
                Tuple.Create("!", TokenType.ConditionalNot),
            };

            _keywords = new Dictionary<string, TokenType>
            {
                { "any", TokenType.Any },
                { "int", TokenType.Int },
                { "short", TokenType.Short },
                { "char", TokenType.Char },
                
                { "func", TokenType.Func },
                { "struct", TokenType.Struct },
                { "static", TokenType.Static },
                { "var", TokenType.Var },
                { "return", TokenType.Return },
                { "if", TokenType.If },
                { "else", TokenType.Else },
                { "for", TokenType.For },
                { "while", TokenType.While },
                { "do", TokenType.Do },
                { "break", TokenType.Break },
                { "continue", TokenType.Continue },

                { "true", TokenType.True },
                { "false", TokenType.False },
                { "null", TokenType.Null },
            };

            // longest operators need to be first
            _operators = _operators.OrderByDescending(o => o.Item1.Length).ToList();

            // punctuation characters trigger operator detection, this should
            // contain the first character of each operator
            _punctuation = new HashSet<char>();

            foreach (var ch in _operators.Select(t => t.Item1[0]))
            {
                _punctuation.Add(ch);
            }
        }
    }
}
