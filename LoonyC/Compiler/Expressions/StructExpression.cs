﻿using System;

namespace LoonyC.Compiler.Expressions
{
    class StructExpression : Expression, IDeclarationExpression
    {
        public StructExpression(Token token)
            : base(token.FileName, token.Line)
        {

        }

        public override T Accept<T>(IExpressionVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }

        public override Expression Simplify()
        {
            throw new NotImplementedException();
        }
    }
}