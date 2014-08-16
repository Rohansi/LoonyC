﻿using System;
using LoonyC.Compiler.Assembly;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.CodeGenerator
{
    abstract class FrameResource : IDisposable
    {
        protected readonly Frame Frame;
        protected readonly TypeBase Type;
        protected bool Disposed;

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
                    throw new Exception(); // TODO

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
                        throw new Exception();
                }
            }
        }

        public override Operand Operand
        {
            get { return new RegisterOperand(Register, _type); }
        }

        public override void Dispose()
        {
            if (Disposed)
                throw new Exception(); // TODO

            Frame.Free(this);
            Disposed = true;
        }
    }

    class StackFrameResource : FrameResource
    {
        public readonly int Offset;
        public readonly int Size;

        public StackFrameResource(Frame frame, TypeBase type, int offset, int size)
            : base(frame, type)
        {
            Offset = offset;
            Size = size;
        }

        public override Operand Operand
        {
            get { throw new NotImplementedException(); }
        }

        public override void Dispose()
        {
            if (Disposed)
                throw new Exception(); // TODO

            Frame.Free(this);
            Disposed = true;
        }
    }
}
