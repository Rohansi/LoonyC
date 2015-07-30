using System;
using System.IO;
using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Ast.Declarations;
using LoonyC.Compiler.Ast.Statements;
using LoonyC.Shared;

namespace LoonyC.Compiler.Ast
{
    class AstPrintVisitor : IAstVisitor<int, int, int, int>
    {
        private readonly IndentTextWriter _writer;

        public AstPrintVisitor(TextWriter writer)
        {
            _writer = new IndentTextWriter(writer);
        }

        public int Visit(Document document)
        {
            foreach (var e in document.Declarations)
            {
                e.Accept(this);
            }

            return 0;
        }

        public int Visit(FuncDeclaration declaration)
        {
            _writer.Write("func ");
            _writer.Write(declaration.Name.Contents);
            _writer.Write('(');

            var sep = "";
            foreach (var param in declaration.Parameters)
            {
                _writer.Write(sep);
                sep = ", ";

                _writer.Write(param.Name.Contents);
                _writer.Write(": ");
                _writer.Write(param.Type);
            }

            _writer.Write(')');

            if (declaration.ReturnType != null)
            {
                _writer.Write(": ");
                _writer.Write(declaration.ReturnType);
            }

            _writer.WriteLine();

            declaration.Body.Accept(this);

            return 0;
        }

        public int Visit(StructDeclaration declaration)
        {
            throw new NotImplementedException();
        }

        public int Visit(BlockStatement statement)
        {
            _writer.WriteLine('{');
            _writer.Indent++;

            foreach (var subStatement in statement.Statements)
            {
                subStatement.Accept(this);
                _writer.WriteLine();
            }

            _writer.Indent--;
            _writer.WriteLine('}');

            return 0;
        }

        public int Visit(NakedStatement statement)
        {
            statement.Expression.Accept(this);
            return 0;
        }

        public int Visit(ReturnStatement statement)
        {
            _writer.Write("return ");
            statement.Value.Accept(this);
            _writer.Write(";");

            return 0;
        }

        public int Visit(VariableStatement statement)
        {
            _writer.Write("var ");
            _writer.Write(statement.Name);

            if (statement.Type != null)
            {
                _writer.Write(": ");
                _writer.Write(statement.Type);
            }

            if (statement.Initializer != null)
            {
                _writer.Write(" = ");
                statement.Initializer.Accept(this);
            }

            _writer.Write(";");

            return 0;
        }

        public int Visit(BinaryOperatorExpression expression)
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

        public int Visit(IdentifierExpression expression)
        {
            _writer.Write(expression.Name);
            return 0;
        }

        public int Visit(NumberExpression expression)
        {
            _writer.Write(expression.Value);
            return 0;
        }
    }
}
