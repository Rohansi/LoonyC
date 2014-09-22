using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LoonyC.Compiler.Types
{
    class FuncType : TypeBase
    {
        public override int Size { get { return 4; } }

        public ReadOnlyCollection<TypeBase> ParameterTypes { get; private set; }
        public TypeBase ReturnType { get; private set; }

        public FuncType(IList<TypeBase> parameterTypes, TypeBase returnType)
        {
            if (parameterTypes == null)
                throw new ArgumentNullException("parameterTypes");

            if (parameterTypes.Any(p => p == null))
                throw new ArgumentNullException("parameterTypes", "contains null entry");
                
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

            return ParameterTypes.SequenceEqual(otherFunc.ParameterTypes) && ReturnType == otherFunc.ReturnType;
        }

        public override bool IsAssignableTo(TypeBase other, int depth = 0)
        {
            if (!base.IsAssignableTo(other, depth))
                return false;

            if (depth > 0 && other is AnyType)
                return true;

            return Equals(other);
        }

        public override int CompareTo(TypeBase other)
        {
            if (ReferenceEquals(other, null))
                return 0;

            if (other is AnyType)
                return 1;

            return Equals(other) ? 10 : 0;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(64);

            sb.Append("func(");

            string sep = "";

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
    }
}
