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

        public override int CompareTo(TypeBase other)
        {
            if (ReferenceEquals(other, null) || !ConstAssignableTo(other))
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
