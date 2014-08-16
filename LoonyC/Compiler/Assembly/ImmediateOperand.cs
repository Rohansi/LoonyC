namespace LoonyC.Compiler.Assembly
{
    class ImmediateOperand : AutoSizedOperand
    {
        public ImmediateOperand(int value)
            : base(OperandType.ImmD, payload: value)
        {

        }
    }
}
 