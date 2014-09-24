using LoonyC.Compiler.Expressions;

namespace LoonyC.Compiler.Parselets
{
    interface IPrefixParselet
    {
        Expression Parse(LoonyParser parser, Token token);
    }
}
