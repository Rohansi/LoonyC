using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LoonyC.Compiler.Expressions.Statements;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.Expressions.Declarations
{
    class FuncExpression : Expression, IDeclarationExpression
    {
        public class Parameter
        {
            public readonly Token Name;
            public readonly TypeBase Type;

            public Parameter(Token name, TypeBase type)
            {
                Name = name;
                Type = type;
            }
        }

        public Token Name { get; private set; }
        public ReadOnlyCollection<Parameter> Parameters { get; private set; }
        public TypeBase ReturnType { get; private set; }
        public BlockExpression Body { get; private set; }

        public FuncExpression(Token start, Token end, Token name, IEnumerable<Parameter> parameters, TypeBase returnType, BlockExpression body)
            : base(start, end)
        {
            Name = name;
            Parameters = parameters.ToList().AsReadOnly();
            ReturnType = returnType;
            Body = body;
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override void SetParent(Expression parent)
        {
            Body.SetParent(this);
        }
    }
}
