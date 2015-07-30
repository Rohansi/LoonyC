using LoonyC.Compiler.Ast.Expressions;

namespace LoonyC.Compiler.Parselets
{
    class IdentifierParselet : IPrefixParselet
    {
        public Expression Parse(LoonyParser parser, LoonyToken token)
        {
            return new IdentifierExpression(token, token.Contents);
        }
    }
}
