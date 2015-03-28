using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoonyC.Shared.Assembly.Instructions
{
    public class MultiInstruction : Instruction
    {
        private readonly List<Instruction> _instructions;

        public MultiInstruction(IEnumerable<Instruction> instructions)
            : base(Opcode.Count)
        {
            _instructions = instructions.ToList();
        }

        public override int Length
        {
            get { return _instructions.Sum(i => i.Length); }
        }

        public override void Write(BinaryWriter writer)
        {
            foreach (var instruction in _instructions)
            {
                instruction.Write(writer);
            }
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, _instructions.Select(i => i.ToString()));
        }
    }
}
