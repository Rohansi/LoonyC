using LoonyC.Compiler.Ast.Declarations;

namespace LoonyC.Compiler.Parselets.Declarations
{
    interface IDeclarationParselet
    {
        Declaration Parse(LoonyParser parser, LoonyToken token);
    }
}
