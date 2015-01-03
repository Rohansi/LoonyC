using System;
using System.Collections.Generic;
using LoonyC.Compiler.Expressions;

namespace LoonyC.Compiler.CodeGenerator.Transforms
{
    class ExpressionSimplifyTransform : ExpressionTransformVisitor
    {
        #region BinaryOperator

        private static Dictionary<TokenType, Func<int, int, int>> _simplifyMap =
            new Dictionary<TokenType, Func<int, int, int>>
            {
                { TokenType.Add, (x, y) => x + y },
                { TokenType.Subtract, (x, y) => x - y },
                { TokenType.Multiply, (x, y) => x * y },
                { TokenType.Divide, (x, y) => x / y },
                { TokenType.Remainder, (x, y) => x % y },

                { TokenType.BitwiseAnd, (x, y) => x & y },
                { TokenType.BitwiseOr, (x, y) => x | y },
                { TokenType.BitwiseXor, (x, y) => x ^ y },
                { TokenType.BitwiseShiftLeft, (x, y) => x << y },
                { TokenType.BitwiseShiftRight, (x, y) => x >> y },
            };


        public override Expression Visit(BinaryOperatorExpression expression)
        {
            var left = expression.Left.Accept(this);
            var right = expression.Right.Accept(this);

            var leftNum = left as NumberExpression;
            var rightNum = right as NumberExpression;

            // n <op> n
            Func<int, int, int> simplifyOp;
            if (_simplifyMap.TryGetValue(expression.Operation, out simplifyOp) &&
                leftNum != null &&
                rightNum != null)
            {
                try
                {
                    var result = simplifyOp(leftNum.Value, rightNum.Value);
                    var token = new Token(expression.Start, TokenType.Number, null);
                    return new NumberExpression(token, result);
                }
                catch (DivideByZeroException)
                {
                    throw new CompilerException(expression.Start, CompilerError.DivisionByZero);
                }
            }

            // 0 * e || e * 0
            if ((leftNum != null && leftNum.Value == 0) ||
                (rightNum != null && rightNum.Value == 0))
            {
                return new NumberExpression(expression.Start, 0);
            }

            return new BinaryOperatorExpression(expression.Start, left, right);
        }

        #endregion
    }
}
