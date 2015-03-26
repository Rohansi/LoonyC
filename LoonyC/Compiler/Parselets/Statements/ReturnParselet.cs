using LoonyC.Compiler.Ast.Statements;

namespace LoonyC.Compiler.Parselets.Statements
{
    class ReturnParselet : IStatementParselet
    {
        public Statement Parse(LoonyParser parser, LoonyToken token, out bool trailingSemicolon)
        {
            trailingSemicolon = true;

            var value = parser.ParseExpression();
            return new ReturnStatement(token, parser.Previous, value);
        }
    }
}
