namespace LoonyC.Compiler.Ast.Expressions
{
    class NumberExpression : Expression
    {
        public int Value { get; private set; }

        public NumberExpression(LoonyToken token, int value)
            : base(token)
        {
            Value = value;
        }

        public override TExpr Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
