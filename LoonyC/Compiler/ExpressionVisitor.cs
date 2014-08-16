using LoonyC.Compiler.Expressions;
using LoonyC.Compiler.Expressions.Statements;

namespace LoonyC.Compiler
{
    abstract class ExpressionVisitor<T>
    {
        public virtual T Visit(FuncExpression expression)
        {
            // TODO
            return default(T);
        }

        public virtual T Visit(BinaryOperatorExpression expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);
            
            return default(T);
        }

        public virtual T Visit(BlockExpression expression)
        {
            foreach (var e in expression.Statements)
            {
                e.Accept(this);
            }
            
            return default(T);
        }

        public virtual T Visit(NumberExpression expression)
        {
            return default(T);
        }

        public virtual T Visit(StructExpression expression)
        {
            return default(T);
        }
    }
}
