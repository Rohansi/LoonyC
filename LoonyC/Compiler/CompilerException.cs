using System;
using JetBrains.Annotations;

namespace LoonyC.Compiler
{
    public class CompilerException : Exception
    {
        protected CompilerException(string message)
            : base(message)
        {
            
        }
        
        [StringFormatMethod("format")]
        internal CompilerException(LoonyToken token, string format, params object[] args)
            : base($"{token.FileName ?? "null"}({token.RangeString}): {string.Format(format, args)}")
        {

        }
    }
}
