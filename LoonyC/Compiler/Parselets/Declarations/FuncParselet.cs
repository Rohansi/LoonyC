using System.Collections.Generic;
using LoonyC.Compiler.Expressions.Declarations;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.Parselets.Declarations
{
    class FuncParselet : IDeclarationParselet
    {
        public IDeclarationExpression Parse(LoonyParser parser, LoonyToken token)
        {
            var start = token;

            var name = parser.Take(LoonyTokenType.Identifier);
            var parameters = new List<FuncExpression.Parameter>();
            TypeBase returnType = null;

            parser.Take(LoonyTokenType.LeftParen);

            if (!parser.MatchAndTake(LoonyTokenType.RightParen))
            {
                do
                {
                    var paramName = parser.Take(LoonyTokenType.Identifier);
                    parser.Take(LoonyTokenType.Colon);
                    var paramType = parser.ParseType();

                    parameters.Add(new FuncExpression.Parameter(paramName, paramType));
                } while (parser.MatchAndTake(LoonyTokenType.Comma));

                parser.Take(LoonyTokenType.RightParen);
            }

            if (parser.MatchAndTake(LoonyTokenType.Colon))
                returnType = parser.ParseType();

            var body = parser.ParseBlock(false);

            var end = parser.Previous;

            return new FuncExpression(start, end, name, parameters, returnType, body);
        }
    }
}
