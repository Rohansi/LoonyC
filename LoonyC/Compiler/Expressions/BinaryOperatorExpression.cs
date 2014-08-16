using System;
using System.Collections.Generic;

namespace LoonyC.Compiler.Expressions
{
    class BinaryOperatorExpression : Expression
    {
        public TokenType Operation { get; private set; }
        public Expression Left { get; private set; }
        public Expression Right { get; private set; }

        public BinaryOperatorExpression(Token token, Expression left, Expression right)
            : base(token.FileName, token.Line)
        {
            Operation = token.Type;
            Left = left;
            Right = right;
        }

        public override T Accept<T>(ExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override Expression Simplify()
        {
            Left = Left.Simplify();
            Right = Right.Simplify();

            Func<int, int, int> simplifyOp;
            if (_simplifyMap.TryGetValue(Operation, out simplifyOp))
            {
                var leftNum = Left as NumberExpression;
                var rigthNum = Right as NumberExpression;

                if (leftNum != null && rigthNum != null)
                {
                    try
                    {
                        var result = simplifyOp(leftNum.Value, rigthNum.Value);
                        var token = new Token(FileName, Line, TokenType.Number, null);
                        return new NumberExpression(token, result);
                    }
                    catch (DivideByZeroException)
                    {
                        throw new CompilerException(FileName, Line, CompilerError.DivisionByZero);
                    }
                }
            }

            return this;
        }

        public override void SetParent(Expression parent)
        {
            base.SetParent(parent);

            Left.SetParent(this);
            Right.SetParent(this);
        }
        
        private static Dictionary<TokenType, TokenType> _assignMap;
        private static Dictionary<TokenType, Func<int, int, int>> _simplifyMap;

        static BinaryOperatorExpression()
        {
            _assignMap = new Dictionary<TokenType, TokenType>
            {
                { TokenType.AddAssign, TokenType.Add },
                { TokenType.SubtractAssign, TokenType.Subtract },
                { TokenType.MultiplyAssign, TokenType.Multiply },
                { TokenType.DivideAssign, TokenType.Divide },
                { TokenType.RemainderAssign, TokenType.Remainder },

                { TokenType.AndAssign, TokenType.And },
                { TokenType.OrAssign, TokenType.Or },
                { TokenType.XorAssign, TokenType.Xor },
                { TokenType.ShiftLeftAssign, TokenType.ShiftLeft },
                { TokenType.ShiftRightAssign, TokenType.ShiftRight },
            };

            _simplifyMap = new Dictionary<TokenType, Func<int, int, int>>
            {
                { TokenType.Add, (x, y) => x + y },
                { TokenType.Subtract, (x, y) => x - y },
                { TokenType.Multiply, (x, y) => x * y },
                { TokenType.Divide, (x, y) => x / y },
                { TokenType.Remainder, (x, y) => x % y },

                { TokenType.And, (x, y) => x & y },
                { TokenType.Or, (x, y) => x | y },
                { TokenType.Xor, (x, y) => x ^ y },
                { TokenType.ShiftLeft, (x, y) => x << y },
                { TokenType.ShiftRight, (x, y) => x >> y },
            };
        }
    }
}
