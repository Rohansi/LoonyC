using System;

namespace LoonyC.Compiler.Ast.Statements
{
    abstract class Statement
    {
        public LoonyToken Start { get; }
        public LoonyToken End { get; }

        protected Statement(LoonyToken start, LoonyToken end = null)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));

            Start = start;
            End = end ?? start;
        }

        public abstract TStmt Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor);
    }
}
