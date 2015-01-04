using System;

namespace LoonyC.Compiler.Expressions
{
    abstract class Expression
    {
        public readonly LoonyToken Start;
        public readonly LoonyToken End;

        public Expression Parent { get; private set; }

        protected Expression(LoonyToken start, LoonyToken end = null)
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
