namespace LoonyC.Compiler.Ast.Declarations
{
    class StructDeclaration : Declaration
    {
        public StructDeclaration(LoonyToken start, LoonyToken end)
            : base(start, end)
        {

        }

        public override TDecl Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
