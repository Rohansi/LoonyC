using System.Collections.Generic;

namespace LoonyC.Compiler.Ast.Expressions
{
    class BinaryOperatorExpression : Expression
    {
        public LoonyTokenType Operation { get; }
        public Expression Left { get; }
        public Expression Right { get; }

        public BinaryOperatorExpression(LoonyToken token, Expression left, Expression right)
            : base(token)
        {
            Operation = token.Type;
            Left = left;
            Right = right;
        }

        public override TExpr Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor)
        {
            return visitor.Visit(this);
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
