using LoonyC.Compiler.Ast.Expressions;

namespace LoonyC.Compiler.Parselets
{
    class GroupParselet : IPrefixParselet
    {
        public Expression Parse(LoonyParser parser, LoonyToken token)
        {
            var expression = parser.ParseExpession();
            parser.Take(LoonyTokenType.RightParen);
            return expression;
        }
    }
}
