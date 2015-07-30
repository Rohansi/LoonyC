using System;
using LoonyC.Compiler.Ast.Declarations;
using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Ast.Statements;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.Ast
{
    class AstTypeInferenceVisitor : IAstVisitor<int, int, int, TypeBase>
    {
        public TypeBase Visit(BinaryOperatorExpression expression)
        {
            var leftType = expression.Left.Accept(this);
            var rightType = expression.Right.Accept(this);

            var leftPrim = leftType as PrimitiveType;
            var rightPrim = rightType as PrimitiveType;
            if (leftPrim != null && rightPrim != null)
            {
                var maxPrim = (Primitive)Math.Max((int)leftPrim.Type, (int)rightPrim.Type);
                return new PrimitiveType(maxPrim);
            }

            throw new NotImplementedException();
        }

        public TypeBase Visit(IdentifierExpression expression)
        {
            throw new NotImplementedException();
        }

        public TypeBase Visit(NumberExpression expression)
        {
            var value = expression.Value;

            if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
                return new PrimitiveType(Primitive.CharOrLarger);

            if (value >= short.MinValue && value <= short.MaxValue)
                return new PrimitiveType(Primitive.ShortOrLarger);

            return new PrimitiveType(Primitive.Int);
        }

        #region Declarations and Statements

        private const string ExpressionsOnly = "AstTypeInferenceVisitor only supports expressions";

        public int Visit(Document document)
        {
            throw new NotSupportedException(ExpressionsOnly);
        }

        public int Visit(StructDeclaration declaration)
        {
            throw new NotSupportedException(ExpressionsOnly);
        }

        public int Visit(FuncDeclaration declaration)
        {
            throw new NotSupportedException(ExpressionsOnly);
        }

        public int Visit(NakedStatement statement)
        {
            throw new NotSupportedException(ExpressionsOnly);
        }

        public int Visit(BlockStatement statement)
        {
            throw new NotSupportedException(ExpressionsOnly);
        }

        public int Visit(ReturnStatement statement)
        {
            throw new NotSupportedException(ExpressionsOnly);
        }

        public int Visit(VariableStatement statement)
        {
            throw new NotSupportedException(ExpressionsOnly);
        }

        #endregion
    }
}
