using LoonyC.Compiler.CodeGenerator.Expressions;

namespace LoonyC.Compiler.Parselets
{
    interface IInfixParselet
    {
        int Precedence { get; }

        Expression Parse(LoonyParser parser, Expression left, Token token);
    }
}
