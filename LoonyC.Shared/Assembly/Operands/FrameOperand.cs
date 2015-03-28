namespace LoonyC.Shared.Assembly.Operands
{
    public class FrameOperand : Operand
    {
        public FrameOperand(OperandValueType type, int offset)
            : base(OperandType.ImmD, type, true, true, Register.BP, offset)
        {
            
        }

        public FrameOperand(OperandValueType type, Register offset)
            : base((OperandType)offset, type, true, true, Register.BP)
        {

        }
    }
}
