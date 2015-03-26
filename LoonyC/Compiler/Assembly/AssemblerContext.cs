using System.Collections.Generic;
using LoonyC.Compiler.Assembly.Instructions;

namespace LoonyC.Compiler.Assembly
{
    class AssemblerContext
    {
        private readonly List<Instruction> _instructions;

        public AssemblerContext()
        {
            _instructions = new List<Instruction>();
        }

        public IEnumerable<Instruction> Instructions
        {
            get { return _instructions; }
        }

        public void Emit(Instruction instruction)
        {
            _instructions.Add(instruction);
        }
    }
}
