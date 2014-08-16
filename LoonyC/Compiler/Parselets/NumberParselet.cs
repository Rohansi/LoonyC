using LoonyC.Compiler.Expressions;

namespace LoonyC.Compiler.Parselets
{
    class NumberParselet : IPrefixParselet
    {
        public Expression Parse(LoonyParser parser, Token token)
        {
            var value = int.Parse(token.Contents);
            return new NumberExpression(token, value);
        }
    }
}
