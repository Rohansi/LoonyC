namespace LoonyC.Compiler.Assembly.Operands
{
    class RegisterOperand : Operand
    {
        public RegisterOperand(Register register, OperandValueType type = OperandValueType.Dword)
            : base((OperandType)register, type)
        {

        }
    }
}
