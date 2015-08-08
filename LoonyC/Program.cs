using System;
using LoonyC.Compiler;
using LoonyC.Compiler.Ast;
using LoonyC.Compiler.CodeGenerator;
using LoonyC.Shared.Assembly;

namespace LoonyC
{
    class Program
    {
        static void Main()
        {
            const string source = @"
                func main(argc: int, argv: **char): int
                {
                    var x = 10 + (20 * 30 ^ 40 * 50);
                    return x * 3 + 1;
                }
            ";

            Console.WriteLine("-- Source --");
            Console.WriteLine(source);
            Console.WriteLine();

            var lexer = new LoonyLexer(source);
            var parser = new LoonyParser(lexer);

            var document = parser.ParseAll();

            Console.WriteLine("-- AST --");
            var printer = new AstPrintVisitor(Console.Out);
            printer.Visit(document);
            Console.WriteLine();
            Console.WriteLine();

            /*var simplify = new AstSimplifyTransform();
            document = simplify.Visit(document);

            Console.WriteLine("-- Simplified AST --");
            printer.Visit(document);
            Console.WriteLine();
            Console.WriteLine();*/

            Console.WriteLine("-- Assembly --");
            var assembler = new Assembler();
            var compiler = new AstCompileVisitor(assembler);
            compiler.Visit(document);

            assembler.Compile(Console.Out);

            Console.ReadLine();
        }
    }
}
