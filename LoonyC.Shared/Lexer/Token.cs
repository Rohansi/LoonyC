namespace LoonyC.Shared.Lexer
{
    public abstract class Token<T>
    {
        public readonly string FileName;
        public readonly SourcePosition Start;
        public readonly SourcePosition End;

        public readonly T Type;
        public readonly string Contents;

        protected Token(string fileName, SourcePosition start, SourcePosition end, T type, string contents)
        {
            FileName = fileName;
            Start = start;
            End = end;

            Type = type;
            Contents = contents;
        }

        protected Token(Token<T> token, T type, string contents)
            : this(token.FileName, token.Start, token.End, type, contents)
        {

        }

        protected Token(T type, string contents)
            : this(null, new SourcePosition(-1), new SourcePosition(-1), type, contents)
        {

        }

        public string RangeString
        {
            get { return Start.ToRangeString(End); }
        }
    }
}
