using LoonyC.Compiler.Ast.Expressions;

namespace LoonyC.Compiler.Parselets
{
    interface IInfixParselet
    {
        int Precedence { get; }

        Expression Parse(LoonyParser parser, Expression left, LoonyToken token);
    }
}
