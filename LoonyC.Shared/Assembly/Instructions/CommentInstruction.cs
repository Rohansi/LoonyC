using System.IO;

namespace LoonyC.Shared.Assembly.Instructions
{
    public class CommentInstruction : Instruction
    {
        public string Value { get; private set; }

        public CommentInstruction(string value)
            : base(Opcode.Count)
        {
            Value = value;
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
            return string.Format("; {0}", Value);
        }
    }
}
