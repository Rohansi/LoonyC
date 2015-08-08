using System;

namespace LoonyC.Compiler.Ast.Expressions
{
    abstract class Expression
    {
        public LoonyToken Start { get; }
        public LoonyToken End { get; }

        protected Expression(LoonyToken start, LoonyToken end = null)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));

            Start = start;
            End = end ?? start;
        }

        public abstract TExpr Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor);
    }
}
