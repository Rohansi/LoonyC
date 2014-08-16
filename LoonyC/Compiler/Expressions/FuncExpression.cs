using System;

namespace LoonyC.Compiler.Expressions
{
    class FuncExpression : Expression, IDeclarationExpression
    {
        public FuncExpression(Token token)
            : base(token.FileName, token.Line)
        {

        }

        public override T Accept<T>(ExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override Expression Simplify()
        {
            throw new NotImplementedException();
        }

        public override void SetParent(Expression parent)
        {
            base.SetParent(parent);
        }
    }
}
