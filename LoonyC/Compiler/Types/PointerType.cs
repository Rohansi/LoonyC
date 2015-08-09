namespace LoonyC.Compiler.Types
{
    class PointerType : TypeModifier
    {
        public override int Size => 4;

        public PointerType(TypeBase innerType, bool constant = false) : base(innerType, constant)
        {

        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            return IsConstant == other.IsConstant && other is PointerType;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0, bool checkConst = true)
        {
            if (!base.IsAssignableTo(other, depth, checkConst))
                return false;

            return other is PointerType;
        }

        public override string ToString()
        {
            return (IsConstant ? "const " : null) + "*" + InnerType;
        }

        public override int GetHashCode()
        {
            return (InnerType.GetHashCode() * 251) + 13;
        }
    }
}
