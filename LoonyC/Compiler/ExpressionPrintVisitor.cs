using System;
using System.IO;
using LoonyC.Compiler.Expressions;
using LoonyC.Compiler.Expressions.Statements;

namespace LoonyC.Compiler
{
    class ExpressionPrintVisitor : IExpressionVisitor<object>
    {
        private readonly IndentTextWriter _writer;

        public ExpressionPrintVisitor(TextWriter writer)
        {
            _writer = new IndentTextWriter(writer);
        }

        public object Visit(FuncExpression expression)
        {
            throw new NotImplementedException();
        }

        public object Visit(BinaryOperatorExpression expression)
        {
            _writer.WriteIndent();
            _writer.WriteLine("Operator {0}", expression.Operation);

            _writer.Indent++;
            expression.Left.Accept(this);
            expression.Right.Accept(this);
            _writer.Indent--;

            return null;
        }

        public object Visit(BlockExpression expression)
        {
            foreach (var statement in expression.Statements)
            {
                statement.Accept(this);
            }

            return null;
        }

        public object Visit(NumberExpression expression)
        {
            _writer.WriteIndent();
            _writer.WriteLine("number: {0}", expression.Value);

            return null;
        }

        public object Visit(StructExpression expression)
        {
            throw new NotImplementedException();
        }
    }
}
