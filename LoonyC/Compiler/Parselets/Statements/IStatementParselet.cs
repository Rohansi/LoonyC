using LoonyC.Compiler.Ast.Statements;

namespace LoonyC.Compiler.Parselets.Statements
{
    interface IStatementParselet
    {
        Statement Parse(LoonyParser parser, LoonyToken token, out bool trailingSemicolon);
    }
}
