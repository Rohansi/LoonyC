using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LoonyC.Compiler.Ast.Statements
{
    class BlockStatement : Statement
    {
        public ReadOnlyCollection<Statement> Statements { get; }

        public BlockStatement(LoonyToken start, LoonyToken end, IEnumerable<Statement> statements)
            : base(start, end)
        {
            if (statements == null)
                throw new ArgumentNullException(nameof(statements));

            Statements = statements.ToList().AsReadOnly();
        }

        public override TStmt Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
