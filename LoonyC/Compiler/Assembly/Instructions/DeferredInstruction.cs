using System;
using System.IO;

namespace LoonyC.Compiler.Assembly.Instructions
{
    class DeferredInstruction : Instruction
    {
        private readonly Lazy<Instruction> _instruction;

        public DeferredInstruction(Func<Instruction> instruction)
            : base(Opcode.Count)
        {
            _instruction = new Lazy<Instruction>(instruction);
        }

        public override int Length
        {
            get { return _instruction.Value.Length; }
        }

        public override void Write(BinaryWriter writer)
        {
            _instruction.Value.Write(writer);
        }

        public override string ToString()
        {
            return _instruction.Value.ToString();
        }
    }
}
