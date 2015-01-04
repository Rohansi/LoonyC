using System;
using System.Collections.Generic;

namespace LoonyC.Compiler.Expressions
{
    class BinaryOperatorExpression : Expression
    {
        public LoonyTokenType Operation { get; private set; }
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }

        public BinaryOperatorExpression(LoonyToken token, Expression left, Expression right)
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

        private static Dictionary<LoonyTokenType, LoonyTokenType> _assignMap;

        static BinaryOperatorExpression()
        {
            _assignMap = new Dictionary<LoonyTokenType, LoonyTokenType>
            {
                { LoonyTokenType.AddAssign, LoonyTokenType.Add },
                { LoonyTokenType.SubtractAssign, LoonyTokenType.Subtract },
                { LoonyTokenType.MultiplyAssign, LoonyTokenType.Multiply },
                { LoonyTokenType.DivideAssign, LoonyTokenType.Divide },
                { LoonyTokenType.RemainderAssign, LoonyTokenType.Remainder },

                { LoonyTokenType.BitwiseAndAssign, LoonyTokenType.BitwiseAnd },
                { LoonyTokenType.BitwiseOrAssign, LoonyTokenType.BitwiseOr },
                { LoonyTokenType.BitwiseXorAssign, LoonyTokenType.BitwiseXor },
                { LoonyTokenType.BitwiseShiftLeftAssign, LoonyTokenType.BitwiseShiftLeft },
                { LoonyTokenType.BitwiseShiftRightAssign, LoonyTokenType.BitwiseShiftRight },
            };
        }
    }
}
