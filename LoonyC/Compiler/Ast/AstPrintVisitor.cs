using System;
using System.IO;
using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Ast.Declarations;
using LoonyC.Compiler.Ast.Statements;
using LoonyC.Shared;

namespace LoonyC.Compiler.Ast
{
    class AstPrintVisitor : AstVisitor<int, int, int, int>
    {
        private readonly IndentTextWriter _writer;

        public AstPrintVisitor(TextWriter writer)
        {
            _writer = new IndentTextWriter(writer);
        }

        public override int Visit(FuncDeclaration expression)
        {
            _writer.Write("func ");
            _writer.Write(expression.Name.Contents);
            _writer.Write('(');

            var sep = "";
            foreach (var param in expression.Parameters)
            {
                _writer.Write(sep);
                sep = ", ";

                _writer.Write(param.Name.Contents);
                _writer.Write(": ");
                _writer.Write(param.Type);
            }

            _writer.Write(')');

            if (expression.ReturnType != null)
            {
                _writer.Write(": ");
                _writer.Write(expression.ReturnType);
            }

            _writer.WriteLine();

            expression.Body.Accept(this);

            return 0;
        }

        public override int Visit(StructDeclaration expression)
        {
            throw new NotImplementedException();
        }

        public override int Visit(NakedStatement expression)
        {
            expression.Expression.Accept(this);
            return 0;
        }

        public override int Visit(BlockStatement expression)
        {
            _writer.WriteLine('{');
            _writer.Indent++;

            foreach (var statement in expression.Statements)
            {
                statement.Accept(this);
                _writer.WriteLine();
            }

            _writer.Indent--;
            _writer.WriteLine('}');

            return 0;
        }

        public override int Visit(BinaryOperatorExpression expression)
        {
            _writer.Write('(');
            expression.Left.Accept(this);

            _writer.Write(' ');
            _writer.Write(expression.Start.Contents);
            _writer.Write(' ');

            expression.Right.Accept(this);
            _writer.Write(')');

            return 0;
        }

        public override int Visit(NumberExpression expression)
        {
            _writer.Write(expression.Value);
            return 0;
        }
    }
}
