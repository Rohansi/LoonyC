namespace LoonyC.Compiler.Assembly
{
    public enum Opcode
    {
        Mov, Add, Sub, Mul, Div, Rem, Inc, Dec, Not, And, Or,
        Xor, Shl, Shr, Push, Pop, Jmp, Call, Ret, Cmp, Jz, Jnz,
        Je, Jne, Ja, Jae, Jb, Jbe, Rand, Int, Iret, Ivt, Abs,
        Retn, Xchg, Cmpxchg, Pusha, Popa, Sti, Cli, Neg, Count
    }
}
