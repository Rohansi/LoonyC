namespace LoonyC.Compiler
{
    enum Precedence
    {
        Assignment,
        Conditional,
        ConditionalOr,
        ConditionalAnd,
        BitwiseOr,
        BitwiseXor,
        BitwiseAnd,
        Equality,
        Relational,
        BitwiseShift,
        Additive,
        Multiplicative,
        Unary,
        Primary
    }
}
