using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Ast.Declarations;
using LoonyC.Compiler.Ast.Statements;

namespace LoonyC.Compiler.Ast
{
    interface IAstVisitor<out TDoc, out TDecl, out TStmt, out TExpr>
    {
        TDoc Visit(Document document);

        TDecl Visit(StructDeclaration declaration);
        TDecl Visit(FuncDeclaration declaration);

        TStmt Visit(NakedStatement statement);
        TStmt Visit(BlockStatement statement);
        TStmt Visit(ReturnStatement statement);

        TExpr Visit(BinaryOperatorExpression expression);
        TExpr Visit(NumberExpression expression);
    }
}
