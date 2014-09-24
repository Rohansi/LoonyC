using System;
using System.Collections;
using System.Collections.Generic;

namespace LoonyC.Compiler
{
    partial class Lexer
    {
        private static OperatorDictionary _operators;
        private static Dictionary<string, TokenType> _keywords;
        private static HashSet<char> _hexChars;

        static Lexer()
        {
            _operators = new OperatorDictionary
            {
                { ";", TokenType.Semicolon },
                { ",", TokenType.Comma },
                { ".", TokenType.Dot },
                { "=", TokenType.Assign },
                { "?", TokenType.QuestionMark },
                { ":", TokenType.Colon },
                { "->", TokenType.Pointy },

                { "(", TokenType.LeftParen },
                { ")", TokenType.RightParen },

                { "{", TokenType.LeftBrace },
                { "}", TokenType.RightBrace },

                { "[", TokenType.LeftSquare },
                { "]", TokenType.RightSquare },
                
                { "+", TokenType.Add },
                { "+=", TokenType.AddAssign },
                { "-", TokenType.Subtract },
                { "-=", TokenType.SubtractAssign },
                { "*", TokenType.Multiply },
                { "*=", TokenType.MultiplyAssign },
                { "/", TokenType.Divide },
                { "/=", TokenType.DivideAssign },
                { "%", TokenType.Remainder },
                { "%=", TokenType.RemainderAssign },
                { "++", TokenType.Increment },
                { "--", TokenType.Decrement },
                
                { "==", TokenType.EqualTo },
                { "!=", TokenType.NotEqualTo },
                { ">", TokenType.GreaterThan },
                { ">=", TokenType.GreaterThanOrEqual },
                { "<", TokenType.LessThan },
                { "<=", TokenType.LessThanOrEqual },

                { "&&", TokenType.LogicalAnd },
                { "||", TokenType.LogicalOr },
                { "!", TokenType.LogicalNot },
                
                { "~", TokenType.BitwiseNot },
                { "&", TokenType.BitwiseAnd },
                { "&=", TokenType.BitwiseAndAssign },
                { "|", TokenType.BitwiseOr },
                { "|=", TokenType.BitwiseOrAssign },
                { "^", TokenType.BitwiseXor },
                { "^=", TokenType.BitwiseXorAssign },
                { "<<", TokenType.BitwiseShiftLeft },
                { "<<=", TokenType.BitwiseShiftLeftAssign },
                { ">>", TokenType.BitwiseShiftRight },
                { ">>=", TokenType.BitwiseShiftRightAssign },
            };

            _keywords = new Dictionary<string, TokenType>
            {
                { "null", TokenType.Null },
                { "true", TokenType.True },
                { "false", TokenType.False },

                { "any", TokenType.Any },
                { "int", TokenType.Int },
                { "short", TokenType.Short },
                { "char", TokenType.Char },
                
                { "var", TokenType.Var },
                { "return", TokenType.Return },
                { "if", TokenType.If },
                { "else", TokenType.Else },
                { "for", TokenType.For },
                { "while", TokenType.While },
                { "do", TokenType.Do },
                { "break", TokenType.Break },
                { "continue", TokenType.Continue },
                
                { "const", TokenType.Const },
                { "static", TokenType.Static },
                { "func", TokenType.Func },
                { "struct", TokenType.Struct },
                { "union", TokenType.Union },
                { "extern", TokenType.Extern },
            };

            _hexChars = new HashSet<char>
            {
                'a', 'b', 'c', 'd', 'e', 'f',
                'A', 'B', 'C', 'D', 'E', 'F',
            };
        }

        class OperatorDictionary : IEnumerable<object>
        {
            private readonly GenericComparer<Tuple<string, TokenType>> _comparer;
            private Dictionary<char, List<Tuple<string, TokenType>>> _operatorDictionary;

            public OperatorDictionary()
            {
                _comparer = new GenericComparer<Tuple<string, TokenType>>((a, b) => b.Item1.Length - a.Item1.Length);
                _operatorDictionary = new Dictionary<char, List<Tuple<string, TokenType>>>();
            }

            public void Add(string op, TokenType type)
            {
                List<Tuple<string, TokenType>> list;
                if (!_operatorDictionary.TryGetValue(op[0], out list))
                {
                    list = new List<Tuple<string, TokenType>>();
                    _operatorDictionary.Add(op[0], list);
                }

                list.Add(Tuple.Create(op, type));
                list.Sort(_comparer);
            }

            public IEnumerable<Tuple<string, TokenType>> Lookup(char ch)
            {
                List<Tuple<string, TokenType>> list;
                if (!_operatorDictionary.TryGetValue(ch, out list))
                    return null;

                return list;
            }

            public IEnumerator<object> GetEnumerator()
            {
                throw new NotSupportedException();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
