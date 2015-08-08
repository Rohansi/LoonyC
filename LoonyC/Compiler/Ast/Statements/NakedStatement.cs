using LoonyC.Compiler.Ast.Expressions;

namespace LoonyC.Compiler.Ast.Statements
{
    class NakedStatement : Statement
    {
        public Expression Expression { get; }

        public NakedStatement(Expression expression)
            : base(expression.Start, expression.End)
        {
            Expression = expression;
        }

        public override TStmt Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
