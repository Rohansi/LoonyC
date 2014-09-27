using System.Collections.Generic;
using LoonyC.Compiler.Expressions.Declarations;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.Parselets.Declarations
{
    class FuncParselet : IDeclarationParselet
    {
        public IDeclarationExpression Parse(LoonyParser parser, Token token)
        {
            var start = token;

            var name = parser.Take(TokenType.Identifier);
            var parameters = new List<FuncExpression.Parameter>();
            TypeBase returnType = null;

            parser.Take(TokenType.LeftParen);

            if (!parser.MatchAndTake(TokenType.RightParen))
            {
                do
                {
                    var paramName = parser.Take(TokenType.Identifier);
                    parser.Take(TokenType.Colon);
                    var paramType = parser.ParseType();

                    parameters.Add(new FuncExpression.Parameter(paramName, paramType));
                } while (parser.MatchAndTake(TokenType.Comma));

                parser.Take(TokenType.RightParen);
            }

            if (parser.MatchAndTake(TokenType.Colon))
                returnType = parser.ParseType();

            var body = parser.ParseBlock(false);

            var end = parser.Previous;

            return new FuncExpression(start, end, name, parameters, returnType, body);
        }
    }
}
