using System;

namespace LoonyC.Compiler.Types
{
    class NamedType : TypeBase
    {
        public override int Size { get { throw new NotImplementedException(); } } // TODO
        public readonly string Name;

        public NamedType(string name, bool constant = false) : base(constant)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            var otherNamed = other as NamedType;
            if (otherNamed == null)
                return false;

            return IsConstant == other.IsConstant && Name == otherNamed.Name;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0, bool checkConst = true)
        {
            if (!base.IsAssignableTo(other, depth, checkConst))
                return false;

            if (depth > 0 && other is AnyType)
                return true;

            return Equals(other);
        }

        public override string ToString()
        {
            return (IsConstant ? "const " : null) + Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
