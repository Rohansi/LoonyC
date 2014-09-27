using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LoonyC.Compiler.Expressions.Statements
{
    class BlockExpression : Expression, IBlockExpression, IStatementExpression
    {
        public ReadOnlyCollection<Expression> Statements { get; private set; }

        public BlockExpression(Token start, Token end, IList<Expression> statements)
            : base(start, end)
        {
            if (statements == null)
                throw new ArgumentNullException("statements");

            Statements = new ReadOnlyCollection<Expression>(statements);
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override Expression Simplify()
        {
            Statements = Statements
                .Select(s => s.Simplify())
                .ToList()
                .AsReadOnly();

            return this;
        }

        public override void SetParent(Expression parent)
        {
            base.SetParent(parent);

            foreach (var statement in Statements)
            {
                statement.SetParent(this);
            }
        }
    }
}
