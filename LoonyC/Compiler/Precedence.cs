namespace LoonyC.Compiler
{
    enum Precedence
    {
        Assignment,
        Ternary,
        LogicalOr,
        LogicalAnd,
        BitwiseOr,
        BitwiseXor,
        BitwiseAnd,
        Equality,
        Relational,
        BitwiseShift,
        Additive,
        Multiplicative,
        Prefix,
        Suffix
    }
}
