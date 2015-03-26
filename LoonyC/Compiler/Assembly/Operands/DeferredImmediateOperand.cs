using System;
using System.IO;

namespace LoonyC.Compiler.Assembly.Operands
{
    class DeferredImmediateOperand : ImmediateOperand
    {
        private readonly Func<int> _value;

        public DeferredImmediateOperand(Func<int> value)
            : base(0)
        {
            _value = value;
        }

        public override int PayloadSize
        {
            get
            {
                Payload = _value();
                return base.PayloadSize;
            }
        }

        public override void WritePayload(BinaryWriter writer)
        {
            Payload = _value();
            base.WritePayload(writer);
        }

        public override string ToString()
        {
            Payload = _value();
            return base.ToString();
        }
    }
}
