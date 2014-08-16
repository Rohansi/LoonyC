using System;

namespace LoonyC.Compiler.Types
{
    internal class ArrayType : TypeModifier
    {
        public override int Size { get { return Count * InnerType.Size; } }
        public readonly int Count;

        public ArrayType(TypeBase innerType, int count)
        {
            if (innerType == null)
                throw new ArgumentNullException("innerType");

            if (count <= 0)
                throw new ArgumentOutOfRangeException("count");

            InnerType = innerType;
            Count = count;
        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            var otherArray = other as ArrayType;
            if (otherArray == null)
                return false;

            return Count == otherArray.Count;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0)
        {
            if (!base.IsAssignableTo(other, depth))
                return false;

            if (other is PointerType)
                return true;

            var otherArray = other as ArrayType;
            if (otherArray == null)
                return false;

            return Count >= otherArray.Count;
        }

        public override int CompareTo(TypeBase other)
        {
            if (ReferenceEquals(other, null))
                return 0;

            var otherArray = other as ArrayType;
            if (otherArray == null)
                return 0;

            if (Count < otherArray.Count)
                return 0;

            return (Count == otherArray.Count ? 10 : 5) + InnerType.CompareTo(otherArray.InnerType);
        }

        public override string ToString()
        {
            return "[" + Count.ToString("D") + "]" + InnerType;
        }
    }
}
