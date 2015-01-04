namespace LoonyC.Compiler.Expressions
{
    class NumberExpression : Expression
    {
        public int Value { get; private set; }

        public NumberExpression(LoonyToken token, int value)
            : base(token)
        {
            Value = value;
        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
