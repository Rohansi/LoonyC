using System.Collections.Generic;
using LoonyC.Compiler.Assembly;
using LoonyC.Compiler.Expressions;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.CodeGenerator
{
    class ExpressionCompileVisitor : ExpressionVisitor<FrameResource>
    {
        private AssemblerContext _context;
        private Frame _frame;

        public ExpressionCompileVisitor(Assembler assembler)
        {
            _context = assembler.CreateContext();
            _frame = new Frame();
        }

        public override FrameResource Visit(NumberExpression expression)
        {
            var res = _frame.Allocate(new PrimitiveType(Primitive.Int));

            _context.Emit(new Instruction(Opcode.Mov, res.Operand, new ImmediateOperand(expression.Value)));

            return res;
        }

        public override FrameResource Visit(BinaryOperatorExpression expression)
        {
            var left = expression.Left;
            var right = expression.Right;

            if (_reverseBinOps.Contains(expression.Operation) && left is NumberExpression && !(right is NumberExpression))
            {
                var temp = left;
                left = right;
                right = temp;
            }

            var leftRes = left.Accept(this);

            var rightNum = right as NumberExpression;
            if (rightNum != null)
            {
                _context.Emit(new Instruction(_binOps[expression.Operation], leftRes.Operand, new ImmediateOperand(rightNum.Value)));
                return leftRes;
            }

            var rightRes = right.Accept(this);

            _context.Emit(new Instruction(_binOps[expression.Operation], leftRes.Operand, rightRes.Operand));

            rightRes.Dispose();

            return leftRes;
        }

        private static Dictionary<TokenType, Opcode> _binOps = new Dictionary<TokenType, Opcode>
        {
            { TokenType.Add, Opcode.Add },
            { TokenType.Subtract, Opcode.Sub },
            { TokenType.Multiply, Opcode.Mul },
            { TokenType.Divide, Opcode.Div },
            { TokenType.Remainder, Opcode.Rem },

            { TokenType.BitwiseAnd, Opcode.And },
            { TokenType.BitwiseOr, Opcode.Or },
            { TokenType.BitwiseXor, Opcode.Xor },
            { TokenType.BitwiseShiftLeft, Opcode.Shl },
            { TokenType.BitwiseShiftRight, Opcode.Shr },
        };

        private static HashSet<TokenType> _reverseBinOps = new HashSet<TokenType>
        {
            TokenType.Add, TokenType.Multiply
        }; 
    }
}
