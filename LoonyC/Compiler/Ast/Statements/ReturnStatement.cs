using LoonyC.Compiler.Ast.Expressions;

namespace LoonyC.Compiler.Ast.Statements
{
    class ReturnStatement : Statement
    {
        public Expression Value { get; }

        public ReturnStatement(LoonyToken start, LoonyToken end, Expression value)
            : base(start, end)
        {
            Value = value;
        }

        public override TStmt Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
