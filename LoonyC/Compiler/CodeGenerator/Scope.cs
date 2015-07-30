using System.Collections.Generic;
using LoonyC.Compiler.Types;

namespace LoonyC.Compiler.CodeGenerator
{
    /// <summary>
    /// Manages variable declarations.
    /// </summary>
    class Scope
    {
        private readonly Dictionary<string, FrameResource> _identifiers;

        public readonly Frame Frame;
        public readonly Scope Previous;

        public Scope(Frame frame, Scope previous)
        {
            _identifiers = new Dictionary<string, FrameResource>();

            Frame = frame;
            Previous = previous;
        }

        public bool TryDefine(string name, TypeBase type, out FrameResource resource)
        {
            resource = null;

            if (IsDefined(name))
                return false;

            resource = Frame.Allocate(type, false);

            _identifiers.Add(name, resource);
            return true;
        }

        public FrameResource GetOrDefault(string name)
        {
            var cur = this;

            while (cur != null)
            {
                FrameResource resource;

                if (cur._identifiers.TryGetValue(name, out resource))
                    return resource;

                cur = cur.Previous;
            }

            return null;
        }

        public bool IsDefined(string name)
        {
            if (_identifiers.ContainsKey(name))
                return true;

            if (Previous != null)
                return Previous.IsDefined(name);

            return false;
        }
    }
}
