using System;
using System.IO;
using LoonyC.Shared.Assembly.Instructions;

namespace LoonyC.Shared.Assembly.Operands
{
    public class LabelOperand : Operand
    {
        private readonly LabelInstruction _label;

        public LabelOperand(LabelInstruction label)
            : base(OperandType.ImmD)
        {
            if (label == null)
                throw new ArgumentNullException(nameof(label));

            _label = label;
        }

        public override void WritePayload(BinaryWriter writer)
        {
            if (!_label.Offset.HasValue)
                throw new Exception(); // TODO: error message

            Payload = _label.Offset.Value;
            base.WritePayload(writer);
        }

        public override string ToString()
        {
            if (!_label.Offset.HasValue)
                return $"<`{_label.Name}` not bound>";

            return _label.Name;
        }
    }
}
