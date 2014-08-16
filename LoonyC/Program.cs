using System;
using LoonyC.Compiler;
using LoonyC.Compiler.Assembly;
using LoonyC.Compiler.CodeGenerator;

namespace LoonyC
{
    class Program
    {
        static void Main(string[] args)
        {
            const string source = "10 + (20 * 30 ^ 40 * 50)";

            Console.WriteLine("-- Source --");
            Console.WriteLine(source);
            Console.WriteLine();

            var lexer = new Lexer(source);
            var parser = new LoonyParser(lexer);

            var expr = parser.ParseExpession();//.Simplify();

            Console.WriteLine("-- AST --");
            var printer = new ExpressionPrintVisitor(Console.Out);
            expr.Accept(printer);
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
