﻿using System.Linq;
using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Ast.Declarations;
using LoonyC.Compiler.Ast.Statements;

namespace LoonyC.Compiler.Ast
{
    abstract class AstTransformVisitor : IAstVisitor<Document, Declaration, Statement, Expression>
    {
        public Document Visit(Document document)
        {
            var declarations = document
                .Declarations
                .Select(d => d.Accept(this));

            return new Document(declarations);
        }

        public Declaration Visit(FuncDeclaration declaration)
        {
            return new FuncDeclaration(
                declaration.Start,
                declaration.End,
                declaration.Name,
                declaration.Parameters,
                declaration.ReturnType,
                (BlockStatement)declaration.Body.Accept(this));
        }

        public Declaration Visit(StructDeclaration declaration)
        {
            return declaration;
        }

        public Statement Visit(BlockStatement statement)
        {
            var statements = statement.Statements.Select(s => s.Accept(this));
            return new BlockStatement(statement.Start, statement.End, statements);
        }

        public Statement Visit(NakedStatement statement)
        {
            return new NakedStatement(statement.Expression.Accept(this));
        }

        public Statement Visit(ReturnStatement statement)
        {
            var value = statement.Value?.Accept(this);
            return new ReturnStatement(statement.Start, statement.End, value);
        }

        public Statement Visit(VariableStatement statement)
        {
            var initializer = statement.Initializer?.Accept(this);
            return new VariableStatement(statement.Start, statement.End, statement.Name, statement.Type, initializer);
        }

        public virtual Expression Visit(BinaryOperatorExpression expression)
        {
            return new BinaryOperatorExpression(
                expression.Start,
                expression.Left.Accept(this),
                expression.Right.Accept(this));
        }

        public Expression Visit(IdentifierExpression expression)
        {
            return expression;
        }

        public Expression Visit(NumberExpression expression)
        {
            return expression;
        }
    }
}
