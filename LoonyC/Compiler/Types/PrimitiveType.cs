using System;

namespace LoonyC.Compiler.Types
{
    internal enum Primitive
    {
        Char, Short, Int
    }

    internal class PrimitiveType : TypeBase
    {
        public override int Size
        {
            get
            {
                switch (Type)
                {
                    case Primitive.Char:
                        return 1;
                    case Primitive.Short:
                        return 2;
                    case Primitive.Int:
                        return 4;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public readonly Primitive Type;

        public PrimitiveType(Primitive type)
        {
            Type = type;
        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            var otherPrim = other as PrimitiveType;
            if (otherPrim == null)
                return false;

            return Type == otherPrim.Type;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0)
        {
            if (!base.IsAssignableTo(other, depth))
                return false;

            if (depth > 0 && other is AnyType)
                return true;

            var otherPrim = other as PrimitiveType;
            if (otherPrim == null)
                return false;

            return Type == otherPrim.Type || (depth == 0 && other.Size >= Size);
        }

        public override int CompareTo(TypeBase other)
        {
            if (ReferenceEquals(other, null))
                return 0;

            if (other is AnyType)
                return 1;

            var otherPrim = other as PrimitiveType;
            if (otherPrim == null)
                return 0;

            return Type == otherPrim.Type ? 10 : 5;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case Primitive.Int:
                    return "int";
                case Primitive.Short:
                    return "short";
                case Primitive.Char:
                    return "char";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
