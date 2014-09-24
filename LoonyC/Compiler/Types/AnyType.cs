using System;

namespace LoonyC.Compiler.Types
{
    class AnyType : TypeBase
    {
        public override int Size { get { return 1; } }

        public AnyType(bool constant = false) : base(constant)
        {
            if (constant)
                throw new ArgumentException("'any' type can not be constant");
        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            return other is AnyType;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0, bool checkConst = true)
        {
            if (!base.IsAssignableTo(other, depth, checkConst))
                return false;

            return true;
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
