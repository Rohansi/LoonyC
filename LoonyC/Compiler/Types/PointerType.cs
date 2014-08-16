using System;

namespace LoonyC.Compiler.Types
{
    internal class PointerType : TypeModifier
    {
        public override int Size { get { return 4; } }

        public PointerType(TypeBase innerType)
        {
            if (innerType == null)
                throw new ArgumentNullException("innerType");

            InnerType = innerType;
        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            return other is PointerType;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0)
        {
            if (!base.IsAssignableTo(other, depth))
                return false;

            return other is PointerType;
        }

        public override int CompareTo(TypeBase other)
        {
            if (ReferenceEquals(other, null))
                return 0;

            var otherPointer = other as PointerType;
            if (otherPointer == null)
                return 0;

            return 10 + InnerType.CompareTo(otherPointer.InnerType);
        }

        public override string ToString()
        {
            return "*" + InnerType;
        }
    }
}
