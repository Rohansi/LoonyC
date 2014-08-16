using System;
using System.IO;
using System.Text;

namespace LoonyC.Compiler.Assembly
{
    enum OperandType
    {
        R0, R1, R2, R3, R4, R5, R6, R7, R8, R9, BP, SP, IP, ImmB, ImmW, ImmD
    }

    enum OperandValueType
    {
        Byte, Word, Dword
    }

    abstract class Operand
    {
        public OperandType Type { get; protected set; }

        public OperandValueType ValueType { get; protected set; }

        public bool IsPointer { get; protected set; }

        public bool IsOffset { get; protected set; }

        public Register OffsetRegister { get; protected set; }

        public int Payload { get; protected set; }

        protected Operand(OperandType type,
                          OperandValueType valueType = OperandValueType.Dword,
                          bool isPointer = false,
                          bool isOffset = false,
                          Register offsetRegister = Register.R0,
                          int payload = 0)
        {
            Type = type;
            ValueType = valueType;
            IsPointer = isPointer;
            IsOffset = isOffset;
            OffsetRegister = offsetRegister;
            Payload = payload;
        }

        public virtual int PayloadSize
        {
            get
            {
                switch (Type)
                {
                    case OperandType.ImmB:
                        return 1;
                    case OperandType.ImmW:
                        return 2;
                    case OperandType.ImmD:
                        return 4;
                    default:
                        return 0;
                }
            }
        }

        public virtual void WritePayload(BinaryWriter writer)
        {
            switch (Type)
            {
                case OperandType.ImmB:
                    writer.Write((sbyte)Payload);
                    break;
                case OperandType.ImmW:
                    writer.Write((short)Payload);
                    break;
                case OperandType.ImmD:
                    writer.Write(Payload);
                    break;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            switch (ValueType)
            {
                case OperandValueType.Byte:
                    sb.Append("byte ");
                    break;
                case OperandValueType.Word:
                    sb.Append("word ");
                    break;
                case OperandValueType.Dword:
                    break;
                default:
                    throw new Exception("Invalid operand value type"); // TODO
            }

            if (IsPointer)
                sb.Append('[');

            if (IsOffset)
            {
                sb.Append(OffsetRegister.ToString().ToLower());
                sb.Append(" + ");
            }

            if (Type >= OperandType.ImmB && Type <= OperandType.ImmD)
                sb.Append(Payload);
            else
                sb.Append(Type.ToString().ToLower());

            if (IsPointer)
                sb.Append(']');

            return sb.ToString();
        }
    }

    abstract class AutoSizedOperand : Operand
    {
        protected AutoSizedOperand(OperandType type,
                                   OperandValueType valueType = OperandValueType.Dword,
                                   bool isPointer = false,
                                   bool isOffset = false,
                                   Register offsetRegister = Register.R0,
                                   int payload = 0)

            : base(type, valueType, isPointer, isOffset, offsetRegister, payload)
        {

        }

        public override int PayloadSize
        {
            get
            {
                AdjustSize();
                return base.PayloadSize;
            }
        }

        public override void WritePayload(BinaryWriter writer)
        {
            AdjustSize();
            base.WritePayload(writer);
        }

        private void AdjustSize()
        {
            if (Type != OperandType.ImmB && Type != OperandType.ImmW && Type != OperandType.ImmD)
                return;

            if (Payload >= sbyte.MinValue && Payload <= sbyte.MaxValue)
                Type = OperandType.ImmB;
            else if (Payload >= short.MinValue && Payload <= short.MaxValue)
                Type = OperandType.ImmW;
            else
                Type = OperandType.ImmD;
        }
    }
}
