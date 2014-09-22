﻿namespace LoonyC.Compiler.CodeGenerator.Expressions
{
    abstract class Expression
    {
        public readonly string FileName;
        public readonly int Line;

        public Expression Parent { get; private set; }

        protected Expression(string fileName, int line)
        {
            FileName = fileName;
            Line = line;
        }

        public abstract T Accept<T>(IExpressionVisitor<T> visitor);

        public abstract Expression Simplify();

        public virtual void SetParent(Expression parent)
        {
            Parent = parent;
        }
    }
}
