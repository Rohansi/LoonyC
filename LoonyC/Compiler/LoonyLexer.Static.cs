using System.Collections.Generic;
using LoonyC.Shared.Lexer;

namespace LoonyC.Compiler
{
    partial class LoonyLexer
    {
        private static OperatorDictionary<LoonyTokenType> _operators;
        private static Dictionary<string, LoonyTokenType> _keywords;

        static LoonyLexer()
        {
            EofToken = new LoonyToken(null, default(SourcePosition), default(SourcePosition), LoonyTokenType.Eof, null);

            _operators = new OperatorDictionary<LoonyTokenType>
            {
                { ";", LoonyTokenType.Semicolon },
                { ",", LoonyTokenType.Comma },
                { ".", LoonyTokenType.Dot },
                { "=", LoonyTokenType.Assign },
                { "?", LoonyTokenType.QuestionMark },
                { ":", LoonyTokenType.Colon },
                { "->", LoonyTokenType.Pointy },

                { "(", LoonyTokenType.LeftParen },
                { ")", LoonyTokenType.RightParen },

                { "{", LoonyTokenType.LeftBrace },
                { "}", LoonyTokenType.RightBrace },

                { "[", LoonyTokenType.LeftSquare },
                { "]", LoonyTokenType.RightSquare },
                
                { "+", LoonyTokenType.Add },
                { "+=", LoonyTokenType.AddAssign },
                { "-", LoonyTokenType.Subtract },
                { "-=", LoonyTokenType.SubtractAssign },
                { "*", LoonyTokenType.Multiply },
                { "*=", LoonyTokenType.MultiplyAssign },
                { "/", LoonyTokenType.Divide },
                { "/=", LoonyTokenType.DivideAssign },
                { "%", LoonyTokenType.Remainder },
                { "%=", LoonyTokenType.RemainderAssign },
                { "++", LoonyTokenType.Increment },
                { "--", LoonyTokenType.Decrement },
                
                { "==", LoonyTokenType.EqualTo },
                { "!=", LoonyTokenType.NotEqualTo },
                { ">", LoonyTokenType.GreaterThan },
                { ">=", LoonyTokenType.GreaterThanOrEqual },
                { "<", LoonyTokenType.LessThan },
                { "<=", LoonyTokenType.LessThanOrEqual },

                { "&&", LoonyTokenType.LogicalAnd },
                { "||", LoonyTokenType.LogicalOr },
                { "!", LoonyTokenType.LogicalNot },
                
                { "~", LoonyTokenType.BitwiseNot },
                { "&", LoonyTokenType.BitwiseAnd },
                { "&=", LoonyTokenType.BitwiseAndAssign },
                { "|", LoonyTokenType.BitwiseOr },
                { "|=", LoonyTokenType.BitwiseOrAssign },
                { "^", LoonyTokenType.BitwiseXor },
                { "^=", LoonyTokenType.BitwiseXorAssign },
                { "<<", LoonyTokenType.BitwiseShiftLeft },
                { "<<=", LoonyTokenType.BitwiseShiftLeftAssign },
                { ">>", LoonyTokenType.BitwiseShiftRight },
                { ">>=", LoonyTokenType.BitwiseShiftRightAssign },
            };

            _keywords = new Dictionary<string, LoonyTokenType>
            {
                { "null", LoonyTokenType.Null },
                { "true", LoonyTokenType.True },
                { "false", LoonyTokenType.False },

                { "any", LoonyTokenType.Any },
                { "int", LoonyTokenType.Int },
                { "short", LoonyTokenType.Short },
                { "char", LoonyTokenType.Char },
                
                { "var", LoonyTokenType.Var },
                { "return", LoonyTokenType.Return },
                { "if", LoonyTokenType.If },
                { "else", LoonyTokenType.Else },
                { "for", LoonyTokenType.For },
                { "while", LoonyTokenType.While },
                { "do", LoonyTokenType.Do },
                { "break", LoonyTokenType.Break },
                { "continue", LoonyTokenType.Continue },
                
                { "const", LoonyTokenType.Const },
                { "static", LoonyTokenType.Static },
                { "func", LoonyTokenType.Func },
                { "struct", LoonyTokenType.Struct },
                { "union", LoonyTokenType.Union },
                { "extern", LoonyTokenType.Extern },
            };
        }
    }
}
