using LoonyC.Compiler.Ast.Expressions;

namespace LoonyC.Compiler.Parselets
{
    interface IPrefixParselet
    {
        Expression Parse(LoonyParser parser, LoonyToken token);
    }
}
