﻿using System;
using System.Collections.Generic;
using System.Linq;
using LoonyC.Compiler.Ast;
using LoonyC.Compiler.Ast.Declarations;
using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Ast.Statements;
using LoonyC.Compiler.Types;
using LoonyC.Shared.Assembly;
using LoonyC.Shared.Assembly.Instructions;
using LoonyC.Shared.Assembly.Operands;

namespace LoonyC.Compiler.CodeGenerator
{
    class AstCompileVisitor : IAstVisitor<int, int, int, FrameResource>
    {
        private AstTypeInferenceVisitor _inference;

        private AssemblerContext _context;
        private Frame _frame;
        private Scope _scope;
        private LabelInstruction _return;

        public AstCompileVisitor(Assembler assembler)
        {
            _inference = new AstTypeInferenceVisitor();
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

            // prologue
            EmitFunctionPart(true);

            _return = new LabelInstruction(".return");

            // body
            declaration.Body.Accept(this);

            // epilogue
            _context.Emit(_return);
            EmitFunctionPart(false);

            _context.Emit(new Instruction(Opcode.Retn, new ImmediateOperand(declaration.Parameters.Count * 4)));

            _frame = null;
            _scope = null;
            _return = null;

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
                _context.Emit(new Instruction(Opcode.Jmp, new LabelOperand(_return)));
                return 0;
            }

            var valueNumber = statement.Value as NumberExpression;
            if (valueNumber != null)
            {
                _context.Emit(new Instruction(Opcode.Mov, new RegisterOperand(Register.R0), new ImmediateOperand(valueNumber.Value)));
                _context.Emit(new Instruction(Opcode.Jmp, new LabelOperand(_return)));

                return 0;
            }

            var value = statement.Value.Accept(this);
            _context.Emit(new Instruction(Opcode.Mov, new RegisterOperand(Register.R0), value.Operand));
            _context.Emit(new Instruction(Opcode.Jmp, new LabelOperand(_return)));
            value.Dispose();

            return 0;
        }

        public int Visit(VariableStatement statement)
        {
            var initializerType = statement.Initializer?.Accept(_inference);
            var type = statement.Type ?? initializerType;

            var typePrim = type as PrimitiveType;
            if (statement.Type == null && typePrim != null && (typePrim.Type == Primitive.CharOrLarger || typePrim.Type == Primitive.ShortOrLarger))
                type = new PrimitiveType(Primitive.Int);

            FrameResource resource;
            if (!_scope.TryDefine(statement.Name, type, out resource))
                throw new Exception(); // TODO

            if (statement.Initializer != null)
            {
                if (initializerType != null && !initializerType.IsAssignableTo(type, checkConst: false))
                    throw new Exception(); // TODO

                var initializerResource = statement.Initializer.Accept(this);
                _context.Emit(new Instruction(Opcode.Mov, resource.Operand, initializerResource.Operand));
                initializerResource.Dispose();
            }

            return 0;
        }

        public FrameResource Visit(BinaryOperatorExpression expression)
        {
            var left = expression.Left;
            var right = expression.Right;

            if (_reversableBinOps.Contains(expression.Operation) && left is NumberExpression && !(right is NumberExpression))
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

        public FrameResource Visit(IdentifierExpression expression)
        {
            var resource = _scope.GetOrDefault(expression.Name);
            if (resource == null)
                throw new Exception(); // TODO

            var copyResource = _frame.Allocate(resource.Type);
            _context.Emit(new Instruction(Opcode.Mov, copyResource.Operand, resource.Operand));
            return copyResource;
        }

        public FrameResource Visit(NumberExpression expression)
        {
            var res = _frame.Allocate(new PrimitiveType(Primitive.Int));

            _context.Emit(new Instruction(Opcode.Mov, res.Operand, new ImmediateOperand(expression.Value)));

            return res;
        }

        private void EmitFunctionPart(bool prologue)
        {
            var frame = _frame; // copy to a local because we need to capture it

            if (prologue)
            {
                _context.Emit(new Instruction(Opcode.Push, new RegisterOperand(Register.BP)));
                _context.Emit(new Instruction(Opcode.Mov, new RegisterOperand(Register.BP), new RegisterOperand(Register.SP)));

                var stackAlloc = new Instruction(Opcode.Sub, new RegisterOperand(Register.SP), new DeferredImmediateOperand(() => frame.RequiredStackSpace));
                _context.Emit(new ConditionalInstruction(stackAlloc, () => frame.RequiredStackSpace > 0));

                _context.Emit(new DeferredInstruction(() =>
                {
                    var count = frame.DirtyRegisters.Count();

                    if (count == 0)
                        return new ConditionalInstruction(null, () => false);

                    if (count > 3)
                        return new Instruction(Opcode.Pusha);

                    return new MultiInstruction(frame.DirtyRegisters.Select(r => new Instruction(Opcode.Push, new RegisterOperand(r))));
                }));
            }
            else
            {
                _context.Emit(new DeferredInstruction(() =>
                {
                    var count = frame.DirtyRegisters.Count();

                    if (count == 0)
                        return new ConditionalInstruction(null, () => false);

                    if (count > 3)
                        return new Instruction(Opcode.Popa);

                    return new MultiInstruction(frame.DirtyRegisters.Reverse().Select(r => new Instruction(Opcode.Pop, new RegisterOperand(r))));
                }));

                var stackFree = new Instruction(Opcode.Add, new RegisterOperand(Register.SP), new DeferredImmediateOperand(() => frame.RequiredStackSpace));
                _context.Emit(new ConditionalInstruction(stackFree, () => frame.RequiredStackSpace > 0));

                _context.Emit(new Instruction(Opcode.Pop, new RegisterOperand(Register.BP)));
            }
        }

        #region Operator Tables

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

        private static HashSet<LoonyTokenType> _reversableBinOps = new HashSet<LoonyTokenType>
        {
            LoonyTokenType.Add, LoonyTokenType.Multiply
        };

        #endregion
    }
}
