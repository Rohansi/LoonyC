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

    class Token
    {
        public readonly string FileName;
        public readonly SourcePosition Start;
        public readonly SourcePosition End;

        public readonly TokenType Type;
        public readonly string Contents;

        public Token(string fileName, SourcePosition start, SourcePosition end, TokenType type, string contents)
        {
            FileName = fileName;
            Start = start;
            End = end;

            Type = type;
            Contents = contents;
        }

        public Token(string fileName, int startLine, int startColumn, int endLine, int endColumn, TokenType type, string contents)
            : this(fileName, new SourcePosition(startLine, startColumn), new SourcePosition(endLine, endColumn), type, contents)
        {

        }

        public Token(Token token, TokenType type, string contents)
            : this(token.FileName, token.Start, token.End, type, contents)
        {

        }

        public Token(TokenType type, string contents)
            : this(null, new SourcePosition(-1), new SourcePosition(-1), type, contents)
        {

        }

        public override string ToString()
        {
            switch (Type)
            {
                case TokenType.Identifier:
                case TokenType.Number:
                case TokenType.String:
                    var contentsStr = Contents;
                    if (contentsStr.Length > 16)
                        contentsStr = contentsStr.Substring(0, 13) + "...";

                    return string.Format("{0}('{1}')", Type, contentsStr);

                default:
                    return Type.ToString();
            }
        }

        public string RangeString
        {
            get { return Start.ToRangeString(End); }
        }
    }
}
