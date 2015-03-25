using System;
using LoonyC.Compiler;
using LoonyC.Compiler.Assembly;
using LoonyC.Compiler.Ast;
using LoonyC.Compiler.CodeGenerator;
using LoonyC.Compiler.CodeGenerator.Transforms;

namespace LoonyC
{
    class Program
    {
        static void Main(string[] args)
        {
            /*const string source = @"
                func main(argc: int, argv: **char): int
                {
                    10 + (20 * 30 ^ 40 * 50);
                }
            ";*/

            const string source = @"10 + (20 * 30 ^ 40 * 50);";

            Console.WriteLine("-- Source --");
            Console.WriteLine(source);
            Console.WriteLine();

            var lexer = new LoonyLexer(source);
            var parser = new LoonyParser(lexer);

            var expr = parser.ParseExpression();

            Console.WriteLine("-- AST --");
            var printer = new AstPrintVisitor(Console.Out);
            expr.Accept(printer);
            Console.WriteLine();
            Console.WriteLine();

            var simplify = new AstSimplifyTransform();
            expr = expr.Accept(simplify);

            Console.WriteLine("-- Simplified AST --");
            expr.Accept(printer);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("-- Assembly --");
            var assembler = new Assembler();
            var compiler = new AstCompileVisitor(assembler);
            expr.Accept(compiler);

            assembler.Compile(Console.Out);

            Console.ReadLine();
        }
    }
}
