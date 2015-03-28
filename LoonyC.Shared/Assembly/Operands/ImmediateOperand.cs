namespace LoonyC.Shared.Assembly.Operands
{
    public class ImmediateOperand : AutoSizedOperand
    {
        public ImmediateOperand(int value)
            : base(OperandType.ImmD, payload: value)
        {

        }
    }
}
 