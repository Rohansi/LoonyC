using System;
using System.Collections.Generic;
using LoonyC.Compiler.Ast;
using LoonyC.Compiler.Ast.Expressions;

namespace LoonyC.Compiler.CodeGenerator.Transforms
{
    class AstSimplifyTransform : AstTransformVisitor
    {
        #region BinaryOperator

        private static Dictionary<LoonyTokenType, Func<int, int, int>> _simplifyMap =
            new Dictionary<LoonyTokenType, Func<int, int, int>>
            {
                { LoonyTokenType.Add, (x, y) => x + y },
                { LoonyTokenType.Subtract, (x, y) => x - y },
                { LoonyTokenType.Multiply, (x, y) => x * y },
                { LoonyTokenType.Divide, (x, y) => x / y },
                { LoonyTokenType.Remainder, (x, y) => x % y },

                { LoonyTokenType.BitwiseAnd, (x, y) => x & y },
                { LoonyTokenType.BitwiseOr, (x, y) => x | y },
                { LoonyTokenType.BitwiseXor, (x, y) => x ^ y },
                { LoonyTokenType.BitwiseShiftLeft, (x, y) => x << y },
                { LoonyTokenType.BitwiseShiftRight, (x, y) => x >> y },
            };


        public override Expression Visit(BinaryOperatorExpression expression)
        {
            var left = expression.Left.Accept(this);
            var right = expression.Right.Accept(this);

            var leftNum = left as NumberExpression;
            var rightNum = right as NumberExpression;

            if (leftNum == null && rightNum == null)
                return new BinaryOperatorExpression(expression.Start, left, right);

            // n <op> n
            Func<int, int, int> simplifyOp;
            if (_simplifyMap.TryGetValue(expression.Operation, out simplifyOp) &&
                leftNum != null &&
                rightNum != null)
            {
                try
                {
                    var result = simplifyOp(leftNum.Value, rightNum.Value);
                    var token = new LoonyToken(expression.Start, LoonyTokenType.Number, null);
                    return new NumberExpression(token, result);
                }
                catch (DivideByZeroException)
                {
                    throw new CompilerException(expression.Start, CompilerError.DivisionByZero);
                }
            }

            // 0 + e || e + 0
            if (expression.Operation == LoonyTokenType.Add)
            {
                if (leftNum != null && leftNum.Value == 0)
                    return right;

                if (rightNum != null && rightNum.Value == 0)
                    return left;
            }

            return new BinaryOperatorExpression(expression.Start, left, right);
        }

        #endregion
    }
}
