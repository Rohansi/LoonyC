using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LoonyC.Compiler.Types
{
    class FuncType : TypeBase
    {
        public override int Size => 4;

        public ReadOnlyCollection<TypeBase> ParameterTypes { get; }
        public TypeBase ReturnType { get; }

        public FuncType(IList<TypeBase> parameterTypes, TypeBase returnType, bool constant = false) : base(constant)
        {
            if (parameterTypes == null)
                throw new ArgumentNullException(nameof(parameterTypes));

            if (parameterTypes.Any(p => p == null))
                throw new ArgumentNullException(nameof(parameterTypes), "contains null entry");
                
            ParameterTypes = new ReadOnlyCollection<TypeBase>(parameterTypes);
            ReturnType = returnType;
        }

        public override bool Equals(TypeBase other)
        {
            if (!base.Equals(other))
                return false;

            var otherFunc = other as FuncType;
            if (otherFunc == null)
                return false;

            return IsConstant == other.IsConstant &&
                   ParameterTypes.SequenceEqual(otherFunc.ParameterTypes) &&
                   ReturnType == otherFunc.ReturnType;
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
            var sb = new StringBuilder(64);

            if (IsConstant)
                sb.Append("const ");

            sb.Append("func(");

            var sep = "";

            foreach (var param in ParameterTypes)
            {
                sb.Append(sep);
                sb.Append(param);
                sep = ",";
            }

            sb.Append(")");

            if (ReturnType != null)
            {
                sb.Append(":");
                sb.Append(ReturnType);
            }

            return sb.ToString();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 19;

                if (ReturnType != null)
                    hash = hash * 31 + ReturnType.GetHashCode();

                foreach (var param in ParameterTypes)
                {
                    hash = hash * 31 + param.GetHashCode();
                }

                return hash;
            }
        }
    }
}
