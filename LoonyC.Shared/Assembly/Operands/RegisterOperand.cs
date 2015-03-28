namespace LoonyC.Shared.Assembly.Operands
{
    public class RegisterOperand : Operand
    {
        public RegisterOperand(Register register, OperandValueType type = OperandValueType.Dword)
            : base((OperandType)register, type)
        {

        }
    }
}
