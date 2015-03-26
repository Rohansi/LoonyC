namespace LoonyC.Compiler
{
    enum Precedence
    {
        Invalid,
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
