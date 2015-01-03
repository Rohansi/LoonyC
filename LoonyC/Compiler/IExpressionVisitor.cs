using LoonyC.Compiler.Expressions;
using LoonyC.Compiler.Expressions.Declarations;
using LoonyC.Compiler.Expressions.Statements;

namespace LoonyC.Compiler
{
    interface IExpressionVisitor<out T>
    {
        T Visit(DocumentExpression expression);

        T Visit(StructExpression expression);

        T Visit(FuncExpression expression);

        T Visit(BlockExpression expression);

        T Visit(BinaryOperatorExpression expression);

        T Visit(NumberExpression expression);
    }
}
