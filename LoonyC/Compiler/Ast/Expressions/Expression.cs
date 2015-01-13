using System;

namespace LoonyC.Compiler.Ast.Expressions
{
    abstract class Expression
    {
        public readonly LoonyToken Start;
        public readonly LoonyToken End;

        protected Expression(LoonyToken start, LoonyToken end = null)
        {
            if (start == null)
                throw new ArgumentNullException("start");

            Start = start;
            End = end ?? start;
        }

        public abstract TExpr Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor);
    }
}
