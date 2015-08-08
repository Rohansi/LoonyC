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

        public override int CompareTo(TypeBase other)
        {
            if (ReferenceEquals(other, null) || !ConstAssignableTo(other))
                return 0;

            var otherArray = other as ArrayType;
            if (otherArray == null || Count < otherArray.Count)
                return 0;

            return (Count == otherArray.Count ? 10 : 5) + InnerType.CompareTo(otherArray.InnerType);
        }

        public override string ToString()
        {
            return "[" + Count.ToString("D") + "]" + InnerType;
        }
    }
}
