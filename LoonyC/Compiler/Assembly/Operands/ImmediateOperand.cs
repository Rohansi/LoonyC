namespace LoonyC.Compiler.Assembly.Operands
{
    class ImmediateOperand : AutoSizedOperand
    {
        public ImmediateOperand(int value)
            : base(OperandType.ImmD, payload: value)
        {

        }
    }
}
 