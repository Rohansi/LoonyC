﻿using LoonyC.Compiler.Ast.Expressions;

namespace LoonyC.Compiler.Parselets
{
    class NumberParselet : IPrefixParselet
    {
        public Expression Parse(LoonyParser parser, LoonyToken token)
        {
            var value = int.Parse(token.Contents);
            return new NumberExpression(token, value);
        }
    }
}
