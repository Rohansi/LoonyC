using LoonyC.Shared.Lexer;

namespace LoonyC.Compiler
{
    enum LoonyTokenType
    {
        Identifier,

        Number,
        SingleString,
        String,

        Null,
        True,
        False,

        Any,
        Bool,
        Char,
        Short,
        Int,

        Var,
        Return,
        If,
        Else,
        For,
        While,
        Do,
        Break,
        Continue,

        Const,
        Static,
        Func,
        Struct,
        Union,
        Extern,

        Semicolon,
        Comma,
        Dot,
        Assign,
        QuestionMark,
        Colon,
        Pointy,

        LeftParen,
        RightParen,

        LeftBrace,
        RightBrace,

        LeftSquare,
        RightSquare,

        Add,
        AddAssign,
        Subtract,
        SubtractAssign,
        Multiply,
        MultiplyAssign,
        Divide,
        DivideAssign,
        Remainder,
        RemainderAssign,
        Increment,
        Decrement,

        EqualTo,
        NotEqualTo,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,

        LogicalAnd,
        LogicalOr,
        LogicalNot,

        BitwiseNot,
        BitwiseAnd,
        BitwiseAndAssign,
        BitwiseOr,
        BitwiseOrAssign,
        BitwiseXor,
        BitwiseXorAssign,
        BitwiseShiftLeft,
        BitwiseShiftLeftAssign,
        BitwiseShiftRight,
        BitwiseShiftRightAssign,

        Eof
    }

    class LoonyToken : Token<LoonyTokenType>
    {
        public LoonyToken(string fileName, SourcePosition start, SourcePosition end, LoonyTokenType type, string contents)
            : base(fileName, start, end, type, contents)
        {

        }

        public LoonyToken(Token<LoonyTokenType> token, LoonyTokenType type, string contents)
            : base(token, type, contents)
        {

        }

        public LoonyToken(LoonyTokenType type, string contents)
            : base(type, contents)
        {

        }

        public override string ToString()
        {
            switch (Type)
            {
                case LoonyTokenType.Identifier:
                case LoonyTokenType.Number:
                case LoonyTokenType.String:
                    var contentsStr = Contents;
                    if (contentsStr.Length > 16)
                        contentsStr = contentsStr.Substring(0, 13) + "...";

                    return $"{Type}('{contentsStr}')";

                default:
                    return Type.ToString();
            }
        }
    }
}
