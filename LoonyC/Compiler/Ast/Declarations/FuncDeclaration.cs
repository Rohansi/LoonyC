using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LoonyC.Compiler.Ast.Statements;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.Ast.Declarations
{
    class FuncDeclaration : Declaration
    {
        public class Parameter
        {
            public LoonyToken Name { get; }
            public TypeBase Type { get; }

            public Parameter(LoonyToken name, TypeBase type)
            {
                Name = name;
                Type = type;
            }
        }

        public LoonyToken Name { get; }
        public ReadOnlyCollection<Parameter> Parameters { get; }
        public TypeBase ReturnType { get; }
        public BlockStatement Body { get; }

        public FuncDeclaration(LoonyToken start, LoonyToken end, LoonyToken name, IEnumerable<Parameter> parameters, TypeBase returnType, BlockStatement body)
            : base(start, end)
        {
            Name = name;
            Parameters = parameters.ToList().AsReadOnly();
            ReturnType = returnType;
            Body = body;
        }

        public override TDecl Accept<TDoc, TDecl, TStmt, TExpr>(IAstVisitor<TDoc, TDecl, TStmt, TExpr> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
