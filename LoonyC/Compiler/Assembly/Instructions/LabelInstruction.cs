using System;
using System.IO;

namespace LoonyC.Compiler.Assembly.Instructions
{
    class LabelInstruction : Instruction
    {
        public string Name { get; private set; }

        public LabelInstruction(string name)
            : base(Opcode.Count)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            Name = name;
        }

        public override int Length
        {
            get { return 0; }
        }

        public override void Write(BinaryWriter writer)
        {

        }

        public override string ToString()
        {
            return string.Format("{0}:", Name);
        }
    }

}
