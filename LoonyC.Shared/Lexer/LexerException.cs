using System;
using JetBrains.Annotations;

namespace LoonyC.Shared.Lexer
{
    public class LexerException : Exception
    {
        [StringFormatMethod("format")]
        internal LexerException(string fileName, SourcePosition pos, string format, params object[] args)
            : base($"{fileName ?? "null"}({pos}): {string.Format(format, args)}")
        {

        }
    }
}
