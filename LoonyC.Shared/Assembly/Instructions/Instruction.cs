using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LoonyC.Shared.Assembly.Operands;

namespace LoonyC.Shared.Assembly.Instructions
{
    public class Instruction
    {
        public Opcode Opcode { get; }
        public Operand Left { get; }
        public Operand Right { get; }

        public int? Offset;

        public Instruction(Opcode opcode, Operand left = null, Operand right = null)
        {
            Opcode = opcode;
            Left = left;
            Right = right;

            Offset = null;

            if (Right != null && Left == null)
                throw new ArgumentNullException(nameof(left));

            var count = 0;
            if (Left != null)
                count++;
            if (Right != null)
                count++;

            if (Opcode != Opcode.Count && OperandCounts[Opcode] != count)
                throw new Exception(); // TODO: error message
        }

        public virtual int Length
        {
            get
            {
                var result = 4;

                if (Left != null)
                    result += Left.PayloadSize;
                if (Right != null)
                    result += Right.PayloadSize;

                return result;
            }
        }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write((byte)Opcode);

            var types = 0;

            if (Left != null)
                types |= (int)Left.Type << 4;

            if (Right != null)
                types |= (int)Right.Type;

            writer.Write((byte)types);

            var offsetRegs = 0;

            if (Left != null)
                offsetRegs |= (int)Left.OffsetRegister << 4;

            if (Right != null)
                offsetRegs |= (int)Right.OffsetRegister;

            writer.Write((byte)offsetRegs);

            var flags = 0;

            if (Left != null)
            {
                if (Left.IsPointer)
                    flags |= 0x80;

                if (Left.IsOffset)
                    flags |= 0x40;

                flags |= (int)Left.ValueType << 4;
            }

            if (Right != null)
            {
                if (Right.IsPointer)
                    flags |= 0x08;

                if (Right.IsOffset)
                    flags |= 0x04;

                flags |= (int)Right.ValueType;
            }

            writer.Write((byte)flags);

            Left?.WritePayload(writer);
            Right?.WritePayload(writer);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var operands = OperandCounts[Opcode];

            sb.Append("  ");
            sb.Append(Opcode.ToString().ToLower());

            if (operands >= 1)
            {
                sb.Append(' ');
                sb.Append(Left);
            }

            if (operands >= 2)
            {
                sb.Append(", ");
                sb.Append(Right);
            }

            return sb.ToString();
        }

        #region OperandCounts

        public static readonly Dictionary<Opcode, int> OperandCounts = new Dictionary<Opcode, int>
        {
            { Opcode.Mov,     2 },
            { Opcode.Add,     2 },
            { Opcode.Sub,     2 },
            { Opcode.Mul,     2 },
            { Opcode.Div,     2 },
            { Opcode.Rem,     2 },
            { Opcode.Inc,     1 },
            { Opcode.Dec,     1 },
            { Opcode.Not,     1 },
            { Opcode.And,     2 },
            { Opcode.Or,      2 },
            { Opcode.Xor,     2 },
            { Opcode.Shl,     2 },
            { Opcode.Shr,     2 },
            { Opcode.Push,    1 },
            { Opcode.Pop,     1 },
            { Opcode.Jmp,     1 },
            { Opcode.Call,    1 },
            { Opcode.Ret,     0 },
            { Opcode.Cmp,     2 },
            { Opcode.Jz,      1 },
            { Opcode.Jnz,     1 },
            { Opcode.Je,      1 },
            { Opcode.Jne,     1 },
            { Opcode.Ja,      1 },
            { Opcode.Jae,     1 },
            { Opcode.Jb,      1 },
            { Opcode.Jbe,     1 },
            { Opcode.Rand,    1 },
            { Opcode.Int,     1 },
            { Opcode.Iret,    0 },
            { Opcode.Ivt,     1 },
            { Opcode.Abs,     1 },
            { Opcode.Retn,    1 },
            { Opcode.Xchg,    2 },
            { Opcode.Cmpxchg, 2 },
            { Opcode.Pusha,   0 },
            { Opcode.Popa,    0 }, 
            { Opcode.Sti,     0 },
            { Opcode.Cli,     0 },
            { Opcode.Neg,     1 }
        };

        #endregion
    }
}
