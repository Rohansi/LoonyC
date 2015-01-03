using System;

namespace LoonyC.Compiler.Expressions
{
    abstract class Expression
    {
        public readonly Token Start;
        public readonly Token End;

        public Expression Parent { get; private set; }

        protected Expression(Token start, Token end = null)
        {
            if (start == null)
                throw new ArgumentNullException("start");

            Start = start;
            End = end ?? start;
        }

        public abstract T Accept<T>(IExpressionVisitor<T> visitor);

        public virtual void SetParent(Expression parent)
        {
            Parent = parent;
        }
    }
}
