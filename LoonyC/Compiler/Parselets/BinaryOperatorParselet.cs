using LoonyC.Compiler.Ast.Expressions;

namespace LoonyC.Compiler.Parselets
{
    class BinaryOperatorParselet : IInfixParselet
    {
        private readonly int _precedence;
        private readonly bool _isRight;

        public int Precedence { get { return _precedence; } }

        public BinaryOperatorParselet(int precedence, bool isRight)
        {
            _precedence = precedence;
            _isRight = isRight;
        }

        public Expression Parse(LoonyParser parser, Expression left, LoonyToken token)
        {
            var right = parser.ParseExpression(Precedence - (_isRight ? 1 : 0));
            return new BinaryOperatorExpression(token, left, right);
        }
    }
}
