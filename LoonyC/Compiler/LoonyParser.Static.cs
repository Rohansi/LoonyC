using System.Collections.Generic;
using LoonyC.Compiler.Parselets;
using LoonyC.Compiler.Parselets.Declarations;
using LoonyC.Compiler.Parselets.Statements;

namespace LoonyC.Compiler
{
    partial class LoonyParser
    {
        private static Dictionary<LoonyTokenType, IDeclarationParselet> _declarationParselets;
        private static Dictionary<LoonyTokenType, IStatementParselet> _statementParselets;
        private static Dictionary<LoonyTokenType, IPrefixParselet> _prefixParselets;
        private static Dictionary<LoonyTokenType, IInfixParselet> _infixParselets;

        static LoonyParser()
        {
            _declarationParselets = new Dictionary<LoonyTokenType, IDeclarationParselet>();
            _statementParselets = new Dictionary<LoonyTokenType, IStatementParselet>();
            _prefixParselets = new Dictionary<LoonyTokenType, IPrefixParselet>();
            _infixParselets = new Dictionary<LoonyTokenType, IInfixParselet>();

            RegisterDeclaration(LoonyTokenType.Func, new FuncParselet());

            RegisterStatement(LoonyTokenType.Return, new ReturnParselet());

            // leaves
            RegisterPrefix(LoonyTokenType.Number, new NumberParselet());

            // math
            RegisterInfix(LoonyTokenType.Add, new BinaryOperatorParselet((int)Precedence.Additive, false));
            RegisterInfix(LoonyTokenType.Subtract, new BinaryOperatorParselet((int)Precedence.Additive, false));
            RegisterInfix(LoonyTokenType.Multiply, new BinaryOperatorParselet((int)Precedence.Multiplicative, false));
            RegisterInfix(LoonyTokenType.Divide, new BinaryOperatorParselet((int)Precedence.Multiplicative, false));
            RegisterInfix(LoonyTokenType.Remainder, new BinaryOperatorParselet((int)Precedence.Multiplicative, false));

            // bitwise
            RegisterInfix(LoonyTokenType.BitwiseAnd, new BinaryOperatorParselet((int)Precedence.BitwiseAnd, false));
            RegisterInfix(LoonyTokenType.BitwiseOr, new BinaryOperatorParselet((int)Precedence.BitwiseOr, false));
            RegisterInfix(LoonyTokenType.BitwiseXor, new BinaryOperatorParselet((int)Precedence.BitwiseXor, false));
            RegisterInfix(LoonyTokenType.BitwiseShiftLeft, new BinaryOperatorParselet((int)Precedence.BitwiseShift, false));
            RegisterInfix(LoonyTokenType.BitwiseShiftRight, new BinaryOperatorParselet((int)Precedence.BitwiseShift, false));

            // other expression stuff
            RegisterPrefix(LoonyTokenType.LeftParen, new GroupParselet());
        }

        static void RegisterDeclaration(LoonyTokenType type, IDeclarationParselet parselet)
        {
            _declarationParselets.Add(type, parselet);
        }

        static void RegisterStatement(LoonyTokenType type, IStatementParselet parselet)
        {
            _statementParselets.Add(type, parselet);
        }

        static void RegisterPrefix(LoonyTokenType type, IPrefixParselet parselet)
        {
            _prefixParselets.Add(type, parselet);
        }

        static void RegisterInfix(LoonyTokenType type, IInfixParselet parselet)
        {
            _infixParselets.Add(type, parselet);
        }
    }
}
