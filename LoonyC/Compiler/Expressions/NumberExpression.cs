namespace LoonyC.Compiler.Expressions
{
    class NumberExpression : Expression
    {
        public int Value { get; private set; }

        public NumberExpression(Token token, int value)
            : base(token.FileName, token.Line)
        {
            Value = value;
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override Expression Simplify()
        {
            return this;
        }
    }
}
