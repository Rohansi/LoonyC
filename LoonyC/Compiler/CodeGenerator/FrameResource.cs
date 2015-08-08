using System;
using LoonyC.Compiler.Types;
using LoonyC.Shared.Assembly;
using LoonyC.Shared.Assembly.Operands;
    
namespace LoonyC.Compiler.CodeGenerator
{
    abstract class FrameResource : IDisposable
    {
        protected bool Disposed;

        public Frame Frame { get; }
        public TypeBase Type { get; private set; }

        protected FrameResource(Frame frame, TypeBase type)
        {
            Frame = frame;
            Type = type;
            Disposed = false;
        }

        public abstract Operand Operand { get; }

        public abstract void Dispose();
    }

    class RegisterFrameResource : FrameResource
    {
        public readonly Register Register;
        private readonly OperandValueType _type;

        public RegisterFrameResource(Frame frame, TypeBase type, Register register)
            : base(frame, type)
        {
            Register = register;

            if (type is PointerType)
            {
                _type = OperandValueType.Dword;
            }
            else
            {
                var typePrim = type as PrimitiveType;
                if (typePrim == null)
                    throw new ArgumentException("Type must be a primitive.", nameof(type));

                switch (typePrim.Type)
                {
                    case Primitive.Int:
                        _type = OperandValueType.Dword;
                        break;
                    case Primitive.Short:
                        _type = OperandValueType.Word;
                        break;
                    case Primitive.Char:
                        _type = OperandValueType.Byte;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public override Operand Operand => new RegisterOperand(Register, _type);

        public override void Dispose()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(RegisterFrameResource));

            Frame.Free(this);
            Disposed = true;
        }
    }

    class StackFrameResource : FrameResource
    {
        public readonly int Offset;
        public readonly int Size;
        private readonly OperandValueType? _type;

        public StackFrameResource(Frame frame, TypeBase type, int offset, int size)
            : base(frame, type)
        {
            Offset = offset;
            Size = size;

            if (type is PointerType)
            {
                _type = OperandValueType.Dword;
            }
            else
            {
                var typePrim = type as PrimitiveType;
                if (typePrim == null)
                    return;

                switch (typePrim.Type)
                {
                    case Primitive.Int:
                        _type = OperandValueType.Dword;
                        break;
                    case Primitive.Short:
                        _type = OperandValueType.Word;
                        break;
                    case Primitive.Char:
                        _type = OperandValueType.Byte;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public override Operand Operand
        {
            get
            {
                if (!_type.HasValue)
                    throw new InvalidOperationException();

                return new FrameOperand(_type.Value, -Size - Offset);
            }
        }

        public override void Dispose()
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(StackFrameResource));

            Frame.Free(this);
            Disposed = true;
        }
    }
}
