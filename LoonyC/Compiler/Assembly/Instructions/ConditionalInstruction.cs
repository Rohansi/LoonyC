﻿using System;
using System.IO;

namespace LoonyC.Compiler.Assembly.Instructions
{
    class ConditionalInstruction : Instruction
    {
        private readonly Func<bool> _condition;

        public Instruction Instruction { get; private set; }

        public ConditionalInstruction(Instruction instruction, Func<bool> condition)
            : base(instruction.Opcode, instruction.Left, instruction.Right)
        {
            Instruction = instruction;
            _condition = condition;
        }

        public override int Length
        {
            get { return _condition() ? Instruction.Length : 0; }
        }

        public override void Write(BinaryWriter writer)
        {
            if (!_condition())
                return;

            Instruction.Write(writer);
        }

        public override string ToString()
        {
            return _condition() ? Instruction.ToString() : "";
        }
    }
}
