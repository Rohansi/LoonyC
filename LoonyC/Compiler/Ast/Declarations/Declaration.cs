using System;

namespace LoonyC.Compiler.Ast.Declarations
{
    abstract class Declaration
    {
        public readonly LoonyToken Start;
        public readonly LoonyToken End;

        protected Declaration(LoonyToken start, LoonyToken end = null)
        {
            if (start == null)
                throw new ArgumentNullException("start");

            Start = start;
            End = end ?? start;
        }

        public abstract TDecl Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor);
    }
}
