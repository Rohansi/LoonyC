using System;
using System.Collections.Generic;
using LoonyC.Compiler.Ast;
using LoonyC.Compiler.Ast.Declarations;
using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Ast.Statements;
using LoonyC.Compiler.Parselets;
using LoonyC.Compiler.Parselets.Declarations;
using LoonyC.Compiler.Parselets.Statements;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler
{
    partial class LoonyParser : Parser
    {
        public LoonyParser(IEnumerable<LoonyToken> tokens)
            : base(tokens)
        {

        }

        /// <summary>
        /// Parse an expression into an expression tree. You can think of expressions as sub-statements.
        /// </summary>
        public Expression ParseExpression(int precendence = 0)
        {
            var token = Take();

            IPrefixParselet prefixParselet;
            _prefixParselets.TryGetValue(token.Type, out prefixParselet);

            if (prefixParselet == null)
                throw new CompilerException(token, CompilerError.ExpectedButFound, "Expression", token);

            var left = prefixParselet.Parse(this, token);

            while (GetPrecedence() > precendence) // swapped because resharper
            {
                token = Take();

                IInfixParselet infixParselet;
                _infixParselets.TryGetValue(token.Type, out infixParselet);

                if (infixParselet == null)
                    throw new Exception("probably can't happen");

                left = infixParselet.Parse(this, left, token);
            }

            return left;
        }

        /// <summary>
        /// Parse a statement into an expression tree.
        /// </summary>
        public Statement ParseStatement(bool takeTrailingSemicolon = true)
        {
            var token = Peek();

            IStatementParselet statementParselet;
            _statementParselets.TryGetValue(token.Type, out statementParselet);

            if (statementParselet == null)
            {
                var expr = ParseExpression();

                if (takeTrailingSemicolon)
                    Take(LoonyTokenType.Semicolon);

                return new NakedStatement(expr);
            }

            token = Take();

            bool hasTrailingSemicolon;
            var result = statementParselet.Parse(this, token, out hasTrailingSemicolon);

            if (takeTrailingSemicolon && hasTrailingSemicolon)
                Take(LoonyTokenType.Semicolon);

            return result;
        }

        /// <summary>
        /// Parse a block of code into an expression tree. Blocks can either be a single statement or 
        /// multiple surrounded by braces.
        /// </summary>
        public BlockStatement ParseBlock(bool allowSingle = true)
        {
            LoonyToken start;
            LoonyToken end;
            var statements = new List<Statement>();

            if (allowSingle && !Match(LoonyTokenType.LeftBrace))
            {
                start = Peek();

                statements.Add(ParseStatement());

                end = Previous;

                return new BlockStatement(start, end, statements);
            }

            start = Take(LoonyTokenType.LeftBrace);

            while (!Match(LoonyTokenType.RightBrace))
            {
                statements.Add(ParseStatement());
            }

            end = Take(LoonyTokenType.RightBrace);

            return new BlockStatement(start, end, statements);
        }

        /// <summary>
        /// Parses declarations until there are no more tokens available.
        /// </summary>
        public Document ParseAll()
        {
            var declarations = new List<Declaration>();

            while (!Match(LoonyTokenType.Eof))
            {
                var token = Take();

                IDeclarationParselet declarationParselet;
                if (!_declarationParselets.TryGetValue(token.Type, out declarationParselet))
                    throw new CompilerException(token, "expected declaration"); // TODO

                declarations.Add(declarationParselet.Parse(this, token));
            }

            return new Document(declarations);
        }

        /// <summary>
        /// Recursively parses a type.
        /// </summary>
        public TypeBase ParseType(bool canUseAny = false)
        {
            bool isConstant = MatchAndTake(LoonyTokenType.Const);

            if (MatchAndTake(LoonyTokenType.Multiply))
                return new PointerType(ParseType(true), isConstant);

            if (MatchAndTake(LoonyTokenType.LeftSquare))
            {
                var countToken = Take(LoonyTokenType.Number);
                var count = int.Parse(countToken.Contents);

                if (count <= 0)
                    throw new CompilerException(countToken, "array length must be above 0"); // TODO

                MatchAndTake(LoonyTokenType.RightSquare);

                return new ArrayType(ParseType(), count, isConstant);
            }

            if (Match(LoonyTokenType.Any))
            {
                var anyToken = Take(LoonyTokenType.Any);

                if (!canUseAny)
                    throw new CompilerException(anyToken, "'any' type must be a pointer"); // TODO

                return new AnyType(isConstant);
            }

            if (MatchAndTake(LoonyTokenType.Func))
            {
                var parameterTypes = new List<TypeBase>();
                TypeBase returnType = null;

                Take(LoonyTokenType.LeftParen);

                if (!MatchAndTake(LoonyTokenType.RightParen))
                {
                    do
                    {
                        parameterTypes.Add(ParseType());
                    } while (MatchAndTake(LoonyTokenType.Comma));

                    Take(LoonyTokenType.RightParen);
                }

                if (MatchAndTake(LoonyTokenType.Colon))
                    returnType = ParseType();

                return new FuncType(parameterTypes, returnType, isConstant);
            }

            if (MatchAndTake(LoonyTokenType.Bool))
                return new BoolType(isConstant);

            if (MatchAndTake(LoonyTokenType.Char))
                return new PrimitiveType(Primitive.Char, isConstant);

            if (MatchAndTake(LoonyTokenType.Short))
                return new PrimitiveType(Primitive.Short, isConstant);

            if (MatchAndTake(LoonyTokenType.Int))
                return new PrimitiveType(Primitive.Int, isConstant);

            if (Match(LoonyTokenType.Identifier))
            {
                var nameToken = Take(LoonyTokenType.Identifier);
                return new NamedType(nameToken.Contents, isConstant);
            }

            var errToken = Take();
            throw new CompilerException(errToken, "expected type"); // TODO
        }

        private int GetPrecedence()
        {
            IInfixParselet infixParselet;
            _infixParselets.TryGetValue(Peek().Type, out infixParselet);

            return infixParselet?.Precedence ?? 0;
        }
    }
}
