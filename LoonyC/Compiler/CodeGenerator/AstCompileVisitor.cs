using System.Collections.Generic;
using LoonyC.Compiler.Assembly;
using LoonyC.Compiler.Ast;
using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.CodeGenerator
{
    class AstCompileVisitor : AstVisitor<int, int, int, FrameResource>
    {
        private AssemblerContext _context;
        private Frame _frame;

        public AstCompileVisitor(Assembler assembler)
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

        private static Dictionary<LoonyTokenType, Opcode> _binOps = new Dictionary<LoonyTokenType, Opcode>
        {
            { LoonyTokenType.Add, Opcode.Add },
            { LoonyTokenType.Subtract, Opcode.Sub },
            { LoonyTokenType.Multiply, Opcode.Mul },
            { LoonyTokenType.Divide, Opcode.Div },
            { LoonyTokenType.Remainder, Opcode.Rem },

            { LoonyTokenType.BitwiseAnd, Opcode.And },
            { LoonyTokenType.BitwiseOr, Opcode.Or },
            { LoonyTokenType.BitwiseXor, Opcode.Xor },
            { LoonyTokenType.BitwiseShiftLeft, Opcode.Shl },
            { LoonyTokenType.BitwiseShiftRight, Opcode.Shr },
        };

        private static HashSet<LoonyTokenType> _reverseBinOps = new HashSet<LoonyTokenType>
        {
            LoonyTokenType.Add, LoonyTokenType.Multiply
        }; 
    }
}
