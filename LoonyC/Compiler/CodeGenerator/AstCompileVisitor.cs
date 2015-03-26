using System;
using System.Collections.Generic;
using LoonyC.Compiler.Assembly;
using LoonyC.Compiler.Ast;
using LoonyC.Compiler.Ast.Declarations;
using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Ast.Statements;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.CodeGenerator
{
    class AstCompileVisitor : IAstVisitor<int, int, int, FrameResource>
    {
        private AssemblerContext _context;
        private Frame _frame;
        private Scope _scope;

        public AstCompileVisitor(Assembler assembler)
        {
            _context = assembler.CreateContext();
        }

        public int Visit(Document document)
        {
            foreach (var declaraction in document.Declarations)
            {
                declaraction.Accept(this);
            }

            return 0;
        }

        public int Visit(FuncDeclaration declaration)
        {
            _frame = new Frame();
            _scope = new Scope(_frame, null);

            // TODO: emit func type
            _context.Emit(new LabelInstruction(declaration.Name.Contents));

            // TODO: emit prologue

            declaration.Body.Accept(this);

            // TODO: emit epilogue

            _frame = null;
            _scope = null;

            return 0;
        }

        public int Visit(StructDeclaration declaration)
        {
            throw new NotImplementedException();
        }

        public int Visit(BlockStatement statement)
        {
            _scope = new Scope(_frame, _scope);

            foreach (var subStatement in statement.Statements)
            {
                subStatement.Accept(this);
            }

            _scope = _scope.Previous;

            return 0;
        }

        public int Visit(NakedStatement statement)
        {
            throw new NotImplementedException();
        }

        public int Visit(ReturnStatement statement)
        {
            if (statement.Value == null)
            {
                _context.Emit(new CommentInstruction("TODO: jmp .return"));
                return 0;
            }

            var valueNumber = statement.Value as NumberExpression;
            if (valueNumber != null)
            {
                _context.Emit(new Instruction(Opcode.Mov, new RegisterOperand(Register.R0), new ImmediateOperand(valueNumber.Value)));
                _context.Emit(new CommentInstruction("TODO: jmp .return"));

                return 0;
            }

            var value = statement.Value.Accept(this);
            _context.Emit(new Instruction(Opcode.Mov, new RegisterOperand(Register.R0), value.Operand));
            _context.Emit(new CommentInstruction("TODO: jmp .return"));
            value.Dispose();

            return 0;
        }

        public FrameResource Visit(NumberExpression expression)
        {
            var res = _frame.Allocate(new PrimitiveType(Primitive.Int));

            _context.Emit(new Instruction(Opcode.Mov, res.Operand, new ImmediateOperand(expression.Value)));

            return res;
        }

        public FrameResource Visit(BinaryOperatorExpression expression)
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
