using LoonyC.Compiler.Expressions.Declarations;

namespace LoonyC.Compiler.Parselets.Declarations
{
    interface IDeclarationParselet
    {
        IDeclarationExpression Parse(LoonyParser parser, Token token);
    }
}
