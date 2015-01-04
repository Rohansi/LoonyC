using LoonyC.Compiler.Expressions;

namespace LoonyC.Compiler.Parselets.Statements
{
    interface IStatementParselet
    {
        Expression Parse(LoonyParser parser, LoonyToken token, out bool trailingSemicolon);
    }
}
