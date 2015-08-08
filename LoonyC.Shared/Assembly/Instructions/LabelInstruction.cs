using System;
using System.IO;

namespace LoonyC.Shared.Assembly.Instructions
{
    public class LabelInstruction : Instruction
    {
        public string Name { get; }

        public LabelInstruction(string name)
            : base(Opcode.Count)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public override int Length => 0;

        public override void Write(BinaryWriter writer)
        {

        }

        public override string ToString()
        {
            return $"{Name}:";
        }
    }

}
