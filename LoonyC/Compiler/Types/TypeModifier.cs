using System;

namespace LoonyC.Compiler.Types
{
    internal abstract class TypeModifier : TypeBase
    {
        public TypeBase InnerType { get; protected set; }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            var otherMod = other as TypeModifier;
            if (otherMod == null)
                return false;

            return InnerType.Equals(otherMod.InnerType);
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0)
        {
            if (!base.IsAssignableTo(other, depth))
                return false;

            var otherMod = other as TypeModifier;
            if (otherMod == null)
                return false;

            return InnerType.IsAssignableTo(otherMod.InnerType, depth + 1);
        }

        public override string ToString()
        {
            throw new NotSupportedException();
        }
    }
}
