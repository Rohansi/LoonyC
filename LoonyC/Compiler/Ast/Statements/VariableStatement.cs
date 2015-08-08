using System;
using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.Ast.Statements
{
    class VariableStatement : Statement
    {
        public string Name { get; }
        public TypeBase Type { get; }
        public Expression Initializer { get; }

        public VariableStatement(LoonyToken start, LoonyToken end, string name, TypeBase type, Expression initializer)
            : base(start, end)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            if (type == null && initializer == null)
                throw new ArgumentNullException(nameof(initializer), "initializer can't be null when no type is specified");
                
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
