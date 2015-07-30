using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Ast.Statements;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.Parselets.Statements
{
    class VariableParselet : IStatementParselet
    {
        public Statement Parse(LoonyParser parser, LoonyToken token, out bool trailingSemicolon)
        {
            trailingSemicolon = true;

            var name = parser.Take(LoonyTokenType.Identifier).Contents;
            TypeBase type = null;
            Expression initializer = null;

            if (parser.MatchAndTake(LoonyTokenType.Colon))
                type = parser.ParseType();

            if (type == null)
            {
                parser.Take(LoonyTokenType.Assign);
                initializer = parser.ParseExpression();
            }
            else if (parser.MatchAndTake(LoonyTokenType.Assign))
            {
                initializer = parser.ParseExpression();
            }

            return new VariableStatement(token, parser.Previous, name, type, initializer);
        }
    }
}
