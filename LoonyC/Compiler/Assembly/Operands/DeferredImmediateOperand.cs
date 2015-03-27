using System;
using System.IO;

namespace LoonyC.Compiler.Assembly.Operands
{
    class DeferredImmediateOperand : ImmediateOperand
    {
        private readonly Lazy<int> _value;

        public DeferredImmediateOperand(Func<int> value)
            : base(0)
        {
            _value = new Lazy<int>(value);
        }

        public override int PayloadSize
        {
            get
            {
                Payload = _value.Value;
                return base.PayloadSize;
            }
        }

        public override void WritePayload(BinaryWriter writer)
        {
            Payload = _value.Value;
            base.WritePayload(writer);
        }

        public override string ToString()
        {
            Payload = _value.Value;
            return base.ToString();
        }
    }
}
