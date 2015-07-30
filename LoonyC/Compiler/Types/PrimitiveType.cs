using System;

namespace LoonyC.Compiler.Types
{
    enum Primitive
    {
        CharOrLarger, Char, ShortOrLarger, Short, Int
    }

    class PrimitiveType : TypeBase
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

        public Primitive Type { get; private set; }

        public PrimitiveType(Primitive type, bool constant = false)
            : base(constant)
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

            return IsConstant == other.IsConstant && Type == otherPrim.Type;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0, bool checkConst = true)
        {
            if (!base.IsAssignableTo(other, depth, checkConst))
                return false;

            if (depth > 0 && other is AnyType)
                return true;

            var otherPrim = other as PrimitiveType;
            if (otherPrim == null)
                return false;

            return Type == otherPrim.Type || (depth == 0 && otherPrim.Type >= Type);
        }

        public override int CompareTo(TypeBase other)
        {
            if (ReferenceEquals(other, null) || !ConstAssignableTo(other))
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
                case Primitive.Char:
                    return "char";
                case Primitive.Short:
                    return "short";
                case Primitive.Int:
                    return "int";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
