using System;
using JetBrains.Annotations;

namespace LoonyC.Compiler
{
    public class CompilerException : Exception
    {
        [StringFormatMethod("format")]
        internal CompilerException(string fileName, int line, string format, params object[] args)
            : base(string.Format("{0}(line {1}): {2}", fileName ?? "null", line, string.Format(format, args)))
        {

        }
    }
}
