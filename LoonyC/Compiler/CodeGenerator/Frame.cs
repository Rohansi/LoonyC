﻿using System;
using System.Collections.Generic;
using LoonyC.Compiler.Types;
using LoonyC.Shared.Assembly;

namespace LoonyC.Compiler.CodeGenerator
{
    /// <summary>
    /// Handles resource allocation for variables.
    /// </summary>
    class Frame
    {
        private List<bool> _registerAllocations;
        private List<bool> _dirtyRegisters;

        private List<bool> _stackAllocations;

        public Frame()
        {
            _registerAllocations = new List<bool>();
            _dirtyRegisters = new List<bool>();

            for (var i = 0; i <= (int)Register.R9; i++)
            {
                _registerAllocations.Add(false);
                _dirtyRegisters.Add(false);
            }

            _registerAllocations[0] = true; // r0 reserved for return values

            _stackAllocations = new List<bool>();
        }

        public IEnumerable<Register> DirtyRegisters
        {
            get
            {
                for (var i = 0; i < _dirtyRegisters.Count; i++)
                {
                    if (_dirtyRegisters[i])
                        yield return (Register)i;
                }
            }
        }

        public int RequiredStackSpace => _stackAllocations.Count;

        public FrameResource Allocate(TypeBase type, bool canUseRegister = true)
        {
            if (canUseRegister)
            {
                var register = TryAllocateRegister();
                if (register.HasValue)
                    return new RegisterFrameResource(this, type, register.Value);
            }

            var offset = AllocateStack(type.Size);
            return new StackFrameResource(this, type, offset, type.Size);
        }

        public void Free(RegisterFrameResource register)
        {
            _registerAllocations[(int)register.Register] = false;
        }

        public void Free(StackFrameResource stack)
        {
            for (var i = 0; i < stack.Size; i++)
            {
                _stackAllocations[stack.Offset + i] = false;
            }
        }

        private int AllocateStack(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException(nameof(size));

            for (var i = 0; i < _stackAllocations.Count - size; i++)
            {
                var found = false;

                for (var j = 0; j < size; j++)
                {
                    if (!_stackAllocations[j + i])
                        break;

                    if (j == size - 1)
                        found = true;
                }

                if (!found)
                    continue;

                for (var j = 0; j < size; j++)
                {
                    _stackAllocations[i + j] = true;
                }

                return i;
            }

            // not found, expand stack
            var offset = _stackAllocations.Count;

            for (var i = 0; i < size; i++)
            {
                _stackAllocations.Add(true);
            }

            return offset;
        }

        private Register? TryAllocateRegister()
        {
            for (var i = 0; i < _registerAllocations.Count; i++)
            {
                if (_registerAllocations[i])
                    continue;

                _registerAllocations[i] = true;
                _dirtyRegisters[i] = true;

                return (Register)i;
            }

            return null;
        }
    }
}
