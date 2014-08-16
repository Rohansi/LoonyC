using System;

namespace LoonyC.Compiler.Types
{
    internal class StructType : TypeBase
    {
        public override int Size { get { throw new NotImplementedException(); } } // TODO
        public readonly string Name;

        public StructType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");

            Name = name;
        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            var otherStruct = other as StructType;
            if (otherStruct == null)
                return false;

            return Name == otherStruct.Name;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0)
        {
            if (!base.IsAssignableTo(other, depth))
                return false;

            if (depth > 0 && other is AnyType)
                return true;

            var otherStruct = other as StructType;
            if (otherStruct == null)
                return false;

            return Name == otherStruct.Name;
        }

        public override int CompareTo(TypeBase other)
        {
            if (ReferenceEquals(other, null))
                return 0;

            if (other is AnyType)
                return 1;

            var otherStruct = other as StructType;
            if (otherStruct == null)
                return 0;

            return Name == otherStruct.Name ? 10 : 0;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
