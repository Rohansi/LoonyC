namespace LoonyC.Compiler
{
    enum TokenType
    {
        Identifier,

        Number,
        SingleString,
        String,

        Null,
        True,
        False,

        Any,
        Int,
        Short,
        Char,

        Func,
        Struct,
        Static,
        Var,
        Return,
        If,
        Else,
        For,
        While,
        Do,
        Break,
        Continue,

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

        And,
        AndAssign,
        Or,
        OrAssign,
        Xor,
        XorAssign,
        ShiftLeft,
        ShiftLeftAssign,
        ShiftRight,
        ShiftRightAssign,
        Not,

        EqualTo,
        NotEqualTo,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        ConditionalAnd,
        ConditionalOr,
        ConditionalNot,

        Eof
    }

    class Token
    {
        public readonly string FileName;
        public readonly int Line;
        public readonly TokenType Type;
        public readonly string Contents;

        public Token(string fileName, int line, TokenType type, string contents)
        {
            FileName = fileName;
            Line = line;
            Type = type;
            Contents = contents;
        }
    }
}
