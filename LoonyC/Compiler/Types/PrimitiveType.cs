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

        public Primitive Type { get; }

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

        public override string ToString()
        {
            string str;

            switch (Type)
            {
                case Primitive.Char:
                    str = "char";
                    break;
                case Primitive.Short:
                    str = "short";
                    break;
                case Primitive.Int:
                    str = "int";
                    break;
                default:
                    throw new NotSupportedException();
            }

            return (IsConstant ? "const " : null) + str;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}
