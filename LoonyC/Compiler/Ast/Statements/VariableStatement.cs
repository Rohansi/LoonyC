using System;
using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.Ast.Statements
{
    class VariableStatement : Statement
    {
        public string Name { get; private set; }
        public TypeBase Type { get; private set; }
        public Expression Initializer { get; private set; }

        public VariableStatement(LoonyToken start, LoonyToken end, string name, TypeBase type, Expression initializer)
            : base(start, end)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            if (type == null && initializer == null)
                throw new ArgumentNullException("initializer", "initializer can't be null when no type is specified");
                
            Name = name;
            Type = type;
            Initializer = initializer;
        }

        public override TStmt Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
