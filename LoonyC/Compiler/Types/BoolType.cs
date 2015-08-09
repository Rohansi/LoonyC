namespace LoonyC.Compiler.Types
{
    class BoolType : TypeBase
    {
        public override int Size => 1;

        public BoolType(bool constant = false)
            : base(constant)
        {

        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            return other is BoolType;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0, bool checkConst = true)
        {
            if (!base.IsAssignableTo(other, depth, checkConst))
                return false;

            return other is BoolType;
        }

        public override string ToString()
        {
            return (IsConstant ? "const " : null) + "bool";
        }

        public override int GetHashCode()
        {
            return 1;
        }
    }
}
