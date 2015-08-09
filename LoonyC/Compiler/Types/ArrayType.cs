using System;

namespace LoonyC.Compiler.Types
{
    class ArrayType : TypeModifier
    {
        public override int Size => Count * InnerType.Size;
        public readonly int Count;

        public ArrayType(TypeBase innerType, int count, bool constant = false)
            : base(innerType, constant)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            Count = count;
        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            var otherArray = other as ArrayType;
            if (otherArray == null)
                return false;

            return IsConstant == other.IsConstant && Count == otherArray.Count;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0, bool checkConst = true)
        {
            if (!base.IsAssignableTo(other, depth, checkConst))
                return false;

            if (other is PointerType)
                return true;

            var otherArray = other as ArrayType;
            return Count >= otherArray?.Count;
        }

        public override string ToString()
        {
            return (IsConstant ? "const " : null) + "[" + Count.ToString("D") + "]" + InnerType;
        }

        public override int GetHashCode()
        {
            return (InnerType.GetHashCode() * 251) + 7;
        }
    }
}
