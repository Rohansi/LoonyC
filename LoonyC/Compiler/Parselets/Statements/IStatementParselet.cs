using LoonyC.Compiler.Expressions;

namespace LoonyC.Compiler.Parselets.Statements
{
    interface IStatementParselet
    {
        Expression Parse(LoonyParser parser, Token token, out bool trailingSemicolon);
    }
}
