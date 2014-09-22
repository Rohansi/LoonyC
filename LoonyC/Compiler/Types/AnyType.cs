namespace LoonyC.Compiler.Types
{
    class AnyType : TypeBase
    {
        public override int Size { get { return 1; } }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            return other is AnyType;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0)
        {
            if (!base.IsAssignableTo(other, depth))
                return false;

            return !(other is TypeModifier);
        }

        public override int CompareTo(TypeBase other)
        {
            return ReferenceEquals(other, null) ? 0 : 1;
        }

        public override string ToString()
        {
            return "any";
        }
    }
}
