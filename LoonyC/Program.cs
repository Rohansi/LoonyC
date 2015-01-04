using System;
using LoonyC.Compiler;
using LoonyC.Compiler.Assembly;
using LoonyC.Compiler.CodeGenerator;
using LoonyC.Compiler.CodeGenerator.Transforms;
using LoonyC.Compiler.Expressions;

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

            var lexer = new Lexer(source);
            var parser = new LoonyParser(lexer);

            var expr = parser.ParseExpession();

            Console.WriteLine("-- AST --");
            var printer = new ExpressionPrintVisitor(Console.Out);
            expr.Accept(printer);
            Console.WriteLine();
            Console.WriteLine();

            var simplify = new ExpressionSimplifyTransform();
            expr = expr.Accept(simplify);

            Console.WriteLine("-- Simplified AST --");
            expr.Accept(printer);
            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("-- Assembly --");
            var assembler = new Assembler();
            var compiler = new ExpressionCompileVisitor(assembler);
            expr.Accept(compiler);

            assembler.Compile(Console.Out);

            Console.ReadLine();
        }
    }
}
