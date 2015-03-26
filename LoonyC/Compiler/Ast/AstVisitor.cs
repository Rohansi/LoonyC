using LoonyC.Compiler.Ast.Expressions;
using LoonyC.Compiler.Ast.Declarations;
using LoonyC.Compiler.Ast.Statements;

namespace LoonyC.Compiler.Ast
{
    abstract class AstVisitor<TDoc, TDecl, TStmt, TExpr> : IAstVisitor<TDoc, TDecl, TStmt, TExpr>
    {
        public virtual TDoc Visit(Document declaration)
        {
            foreach (var e in declaration.Declarations)
            {
                e.Accept(this);
            }

            return default(TDoc);
        }

        public virtual TDecl Visit(FuncDeclaration declaration)
        {
            return default(TDecl);
        }

        public virtual TDecl Visit(StructDeclaration declaration)
        {
            return default(TDecl);
        }

        public virtual TStmt Visit(BlockStatement statement)
        {
            foreach (var e in statement.Statements)
            {
                e.Accept(this);
            }

            return default(TStmt);
        }

        public virtual TStmt Visit(NakedStatement statement)
        {
            statement.Expression.Accept(this);

            return default(TStmt);
        }

        public virtual TStmt Visit(ReturnStatement statement)
        {
            statement.Value.Accept(this);

            return default(TStmt);
        }

        public virtual TExpr Visit(BinaryOperatorExpression expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);
            
            return default(TExpr);
        }

        public virtual TExpr Visit(NumberExpression expression)
        {
            return default(TExpr);
        }
    }
}
