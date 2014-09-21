using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LoonyC.Compiler.Expressions;
using LoonyC.Compiler.Expressions.Statements;
using LoonyC.Compiler.Parselets;
using LoonyC.Compiler.Parselets.Statements;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler
{
    internal partial class LoonyParser : Parser
    {
        public LoonyParser(IEnumerable<Token> tokens)
            : base(tokens)
        {

        }

        /// <summary>
        /// Parse an expression into an expression tree. You can think of expressions as sub-statements.
        /// </summary>
        public Expression ParseExpession(int precendence = 0)
        {
            var token = Take();

            IPrefixParselet prefixParselet;
            _prefixParselets.TryGetValue(token.Type, out prefixParselet);

            if (prefixParselet == null)
                throw new CompilerException(token.FileName, token.Line, CompilerError.ExpectedButFound, "Expression", token.Type);

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
        public Expression ParseStatement(bool takeTrailingSemicolon = true)
        {
            var token = Peek();

            IStatementParselet statementParselet;
            _statementParselets.TryGetValue(token.Type, out statementParselet);

            Expression result;

            if (statementParselet == null)
            {
                result = ParseExpession();

                if (takeTrailingSemicolon)
                    Take(TokenType.Semicolon);

                return result;
            }

            token = Take();

            bool hasTrailingSemicolon;
            result = statementParselet.Parse(this, token, out hasTrailingSemicolon);

            if (takeTrailingSemicolon && hasTrailingSemicolon)
                Take(TokenType.Semicolon);

            return result;
        }

        /// <summary>
        /// Parse a block of code into an expression tree. Blocks can either be a single statement or 
        /// multiple surrounded by braces.
        /// </summary>
        public BlockExpression ParseBlock(bool allowSingle = true)
        {
            var statements = new List<Expression>();

            if (allowSingle && !Match(TokenType.LeftBrace))
            {
                statements.Add(ParseStatement());
                return new BlockExpression(statements);
            }

            Take(TokenType.LeftBrace);

            while (!Match(TokenType.RightBrace))
            {
                statements.Add(ParseStatement());
            }

            Take(TokenType.RightBrace);
            return new BlockExpression(statements);
        }

        /// <summary>
        /// Parses declarations until there are no more tokens available.
        /// </summary>
        public ReadOnlyCollection<Expression> ParseAll()
        {
            var statements = new List<Expression>();

            while (!Match(TokenType.Eof))
            {
                statements.Add(ParseStatement()); // TODO: parse declaration (struct, func, var)
            }

            return statements.AsReadOnly();
        }

        /// <summary>
        /// Recursively parses a type.
        /// </summary>
        public TypeBase ParseType(bool canUseAny = false)
        {
            if (MatchAndTake(TokenType.Multiply))
                return new PointerType(ParseType(true));

            if (MatchAndTake(TokenType.LeftSquare))
            {
                var countToken = Take(TokenType.Number);
                var count = int.Parse(countToken.Contents);

                if (count <= 0)
                    throw new CompilerException(countToken.FileName, countToken.Line, "array length must be above 0"); // TODO

                MatchAndTake(TokenType.RightSquare);

                return new ArrayType(ParseType(), count);
            }

            if (Match(TokenType.Any))
            {
                var token = Take(TokenType.Any);

                if (!canUseAny)
                    throw new CompilerException(token.FileName, token.Line, "'any' type must be a pointer"); // TODO

                return new AnyType();
            }

            if (MatchAndTake(TokenType.Func))
            {
                var parameterTypes = new List<TypeBase>();
                TypeBase returnType = null;

                Take(TokenType.LeftParen);

                if (!MatchAndTake(TokenType.RightParen))
                {
                    do
                    {
                        parameterTypes.Add(ParseType());
                    } while (MatchAndTake(TokenType.Comma));

                    Take(TokenType.RightParen);
                }

                if (MatchAndTake(TokenType.Colon))
                    returnType = ParseType();

                return new FuncType(parameterTypes, returnType);
            }

            if (MatchAndTake(TokenType.Int))
                return new PrimitiveType(Primitive.Int);

            if (MatchAndTake(TokenType.Short))
                return new PrimitiveType(Primitive.Short);

            if (MatchAndTake(TokenType.Char))
                return new PrimitiveType(Primitive.Char);

            var name = Take(TokenType.Identifier).Contents;
            return new StructType(name);
        }

        private int GetPrecedence()
        {
            IInfixParselet infixParselet;
            _infixParselets.TryGetValue(Peek().Type, out infixParselet);

            return infixParselet != null ? infixParselet.Precedence : 0;
        }
    }
}
