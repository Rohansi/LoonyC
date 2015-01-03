using System;
using System.Collections.Generic;

namespace LoonyC.Compiler.Expressions
{
    class BinaryOperatorExpression : Expression
    {
        public TokenType Operation { get; private set; }
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }

        public BinaryOperatorExpression(Token token, Expression left, Expression right)
            : base(token)
        {
            Operation = token.Type;
            Left = left;
            Right = right;
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void SetParent(Expression parent)
        {
            base.SetParent(parent);

            Left.SetParent(this);
            Right.SetParent(this);
        }

        private static Dictionary<TokenType, TokenType> _assignMap;

        static BinaryOperatorExpression()
        {
            _assignMap = new Dictionary<TokenType, TokenType>
            {
                { TokenType.AddAssign, TokenType.Add },
                { TokenType.SubtractAssign, TokenType.Subtract },
                { TokenType.MultiplyAssign, TokenType.Multiply },
                { TokenType.DivideAssign, TokenType.Divide },
                { TokenType.RemainderAssign, TokenType.Remainder },

                { TokenType.BitwiseAndAssign, TokenType.BitwiseAnd },
                { TokenType.BitwiseOrAssign, TokenType.BitwiseOr },
                { TokenType.BitwiseXorAssign, TokenType.BitwiseXor },
                { TokenType.BitwiseShiftLeftAssign, TokenType.BitwiseShiftLeft },
                { TokenType.BitwiseShiftRightAssign, TokenType.BitwiseShiftRight },
            };
        }
    }
}
