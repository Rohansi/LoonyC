﻿using System;
using System.IO;
using LoonyC.Compiler.Assembly.Instructions;

namespace LoonyC.Compiler.Assembly.Operands
{
    class LabelOperand : Operand
    {
        private readonly LabelInstruction _label;

        public LabelOperand(LabelInstruction label)
            : base(OperandType.ImmD)
        {
            if (label == null)
                throw new ArgumentNullException("label");

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
                return string.Format("<`{0}` not bound>", _label.Name);

            return _label.Name;
        }
    }
}