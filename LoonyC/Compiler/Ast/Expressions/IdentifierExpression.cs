using System;

namespace LoonyC.Compiler.Ast.Expressions
{
    class IdentifierExpression : Expression
    {
        public string Name { get; private set; }

        public IdentifierExpression(LoonyToken token, string name)
            : base(token)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            Name = name;
        }

        public override TExpr Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
