using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LoonyC.Compiler.Expressions.Declarations
{
    class DocumentExpression : Expression
    {
        public ReadOnlyCollection<Expression> Declarations { get; private set; }

        public DocumentExpression(Token start, Token end, IEnumerable<IDeclarationExpression> declarations)
            : base(start, end)
        {
            Declarations = declarations
                .Cast<Expression>()
                .ToList()
                .AsReadOnly();
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
