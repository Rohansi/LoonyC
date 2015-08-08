using System.Collections.Generic;
using LoonyC.Shared.Assembly.Instructions;

namespace LoonyC.Shared.Assembly
{
    public class AssemblerContext
    {
        private readonly List<Instruction> _instructions;

        public AssemblerContext()
        {
            _instructions = new List<Instruction>();
        }

        public IEnumerable<Instruction> Instructions => _instructions;

        public void Emit(Instruction instruction)
        {
            _instructions.Add(instruction);
        }
    }
}
