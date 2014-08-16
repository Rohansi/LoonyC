using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace LoonyC.Compiler.Assembly
{
    class Assembler
    {
        private readonly List<AssemblerContext> _contexts;
        private ReadOnlyCollection<Instruction> _instructions;
         
        public Assembler()
        {
            _contexts = new List<AssemblerContext>();
            _instructions = null;
        }

        public AssemblerContext CreateContext()
        {
            var context = new AssemblerContext();
            _contexts.Add(context);
            return context;
        }

        public byte[] Compile(TextWriter writer = null)
        {
            var length = PatchLabels();
            var bytecode = GenerateBytecode(length);

            if (writer != null)
            {
                foreach (var instruction in AllInstructions())
                {
                    writer.WriteLine(instruction);
                }
            }

            return bytecode;
        }

        private int PatchLabels()
        {
            var offset = 0;

            foreach (var instruction in AllInstructions())
            {
                instruction.Offset = offset;
                offset += instruction.Length;
            }

            return offset;
        }

        private byte[] GenerateBytecode(int bufferSize)
        {
            var bytecode = new byte[bufferSize];
            var memoryStream = new MemoryStream(bytecode);
            var writer = new BinaryWriter(memoryStream);

            foreach (var instruction in AllInstructions())
            {
                instruction.Write(writer);
            }

            return bytecode;
        }

        private IEnumerable<Instruction> AllInstructions(bool clearCache = false)
        {
            if (clearCache)
                _instructions = null;

            return _instructions ?? (_instructions = _contexts.SelectMany(c => c.Instructions).ToList().AsReadOnly());
        }
    }
}
