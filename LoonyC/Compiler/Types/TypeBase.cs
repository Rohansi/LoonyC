using System;

namespace LoonyC.Compiler.Types
{
    abstract class TypeBase : IEquatable<TypeBase>, IComparable<TypeBase>
    {
        public bool IsConstant { get; private set; }

        protected TypeBase(bool constant)
        {
            IsConstant = constant;
        }

        public abstract int Size { get; }

        public virtual bool Equals(TypeBase other)
        {
            if (ReferenceEquals(other, null))
                return false;

            return true;
        }

        /// <summary>
        /// Check if the type can be assigned to another type.
        /// </summary>
        public virtual bool IsAssignableTo(TypeBase other, int depth = 0, bool checkConst = true)
        {
            if (ReferenceEquals(other, null) || (checkConst && !ConstAssignableTo(other)))
                return false;

            return true;
        }

        /// <summary>
        /// Compare two types for similarity. Higher is better.
        /// Being similar does not necessarily make them compatible.
        /// </summary>
        public abstract int CompareTo(TypeBase other);

        public override string ToString()
        {
            throw new NotSupportedException();
        }

        public override int GetHashCode()
        {
            throw new NotSupportedException();
        }

        public override bool Equals(object obj)
        {
            var objType = obj as TypeBase;
            if (ReferenceEquals(objType, null))
                return false;

            return Equals(objType);
        }

        public static bool operator ==(TypeBase a, TypeBase b)
        {
            if (ReferenceEquals(a, b))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(TypeBase a, TypeBase b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Checks if the const value of this type is compatible with another type.
        /// </summary>
        protected static bool ConstAssignableTo(TypeBase other)
        {
            // this -> other = 
            //  0       0      1
            //  0       1      0
            //  1       0      1
            //  1       1      0

            return !other.IsConstant;
        }
    }
}
