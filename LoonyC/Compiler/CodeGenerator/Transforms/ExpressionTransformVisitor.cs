using System.Linq;
using LoonyC.Compiler.Expressions;
using LoonyC.Compiler.Expressions.Declarations;
using LoonyC.Compiler.Expressions.Statements;

namespace LoonyC.Compiler.CodeGenerator.Transforms
{
    abstract class ExpressionTransformVisitor : IExpressionVisitor<Expression>
    {
        public Expression Visit(DocumentExpression expression)
        {
            var declarations = expression
                .Declarations
                .Select(d => d.Accept(this))
                .Cast<IDeclarationExpression>();

            return new DocumentExpression(expression.Start, expression.End, declarations);
        }

        public Expression Visit(StructExpression expression)
        {
            return expression;
        }

        public Expression Visit(FuncExpression expression)
        {
            return new FuncExpression(
                expression.Start,
                expression.End,
                expression.Name,
                expression.Parameters,
                expression.ReturnType,
                (BlockExpression)expression.Body.Accept(this));
        }

        public Expression Visit(BlockExpression expression)
        {
            var statements = expression.Statements.Select(s => s.Accept(this));
            return new BlockExpression(expression.Start, expression.End, statements);
        }

        public virtual Expression Visit(BinaryOperatorExpression expression)
        {
            return expression;
        }

        public Expression Visit(NumberExpression expression)
        {
            return expression;
        }
    }
}
