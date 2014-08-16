using LoonyC.Compiler.Expressions;

namespace LoonyC.Compiler.Parselets
{
    class GroupParselet : IPrefixParselet
    {
        public Expression Parse(LoonyParser parser, Token token)
        {
            var expression = parser.ParseExpession();
            parser.Take(TokenType.RightParen);
            return expression;
        }
    }
}
