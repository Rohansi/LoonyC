using System;

namespace LoonyC.Compiler.Types
{
    abstract class TypeModifier : TypeBase
    {
        public TypeBase InnerType { get; }

        protected TypeModifier(TypeBase innerType, bool constant = false) : base(constant)
        {
            if (innerType == null)
                throw new ArgumentNullException(nameof(innerType));

            InnerType = innerType;
        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            var otherMod = other as TypeModifier;
            if (otherMod == null)
                return false;

            return InnerType.Equals(otherMod.InnerType);
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0, bool checkConst = true)
        {
            if (!base.IsAssignableTo(other, depth, checkConst))
                return false;

            var otherMod = other as TypeModifier;
            if (otherMod == null)
                return false;

            return InnerType.IsAssignableTo(otherMod.InnerType, depth + 1);
        }
    }
}
