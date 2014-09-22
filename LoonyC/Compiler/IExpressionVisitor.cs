using LoonyC.Compiler.CodeGenerator.Expressions;
using LoonyC.Compiler.CodeGenerator.Expressions.Statements;

namespace LoonyC.Compiler
{
    interface IExpressionVisitor<out T>
    {
        T Visit(FuncExpression expression);

        T Visit(BinaryOperatorExpression expression);

        T Visit(BlockExpression expression);

        T Visit(NumberExpression expression);

        T Visit(StructExpression expression);
    }
}
