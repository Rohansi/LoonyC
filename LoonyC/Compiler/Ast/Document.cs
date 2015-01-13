using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LoonyC.Compiler.Ast.Declarations;

namespace LoonyC.Compiler.Ast
{
    class Document
    {
        public ReadOnlyCollection<Declaration> Declarations { get; private set; }

        public Document(IEnumerable<Declaration> declarations)
        {
            Declarations = declarations
                .ToList()
                .AsReadOnly();
        }
    }
}
