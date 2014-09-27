using System.Collections.Generic;
using LoonyC.Compiler.Parselets;
using LoonyC.Compiler.Parselets.Declarations;
using LoonyC.Compiler.Parselets.Statements;

namespace LoonyC.Compiler
{
    partial class LoonyParser
    {
        private static Dictionary<TokenType, IDeclarationParselet> _declarationParselets;
        private static Dictionary<TokenType, IStatementParselet> _statementParselets;
        private static Dictionary<TokenType, IPrefixParselet> _prefixParselets;
        private static Dictionary<TokenType, IInfixParselet> _infixParselets;

        static LoonyParser()
        {
            _declarationParselets = new Dictionary<TokenType, IDeclarationParselet>();
            _statementParselets = new Dictionary<TokenType, IStatementParselet>();
            _prefixParselets = new Dictionary<TokenType, IPrefixParselet>();
            _infixParselets = new Dictionary<TokenType, IInfixParselet>();

            RegisterDeclaration(TokenType.Func, new FuncParselet());

            // leaves
            RegisterPrefix(TokenType.Number, new NumberParselet());

            // math
            RegisterInfix(TokenType.Add, new BinaryOperatorParselet((int)Precedence.Additive, false));
            RegisterInfix(TokenType.Subtract, new BinaryOperatorParselet((int)Precedence.Additive, false));
            RegisterInfix(TokenType.Multiply, new BinaryOperatorParselet((int)Precedence.Multiplicative, false));
            RegisterInfix(TokenType.Divide, new BinaryOperatorParselet((int)Precedence.Multiplicative, false));
            RegisterInfix(TokenType.Remainder, new BinaryOperatorParselet((int)Precedence.Multiplicative, false));

            // bitwise
            RegisterInfix(TokenType.BitwiseAnd, new BinaryOperatorParselet((int)Precedence.BitwiseAnd, false));
            RegisterInfix(TokenType.BitwiseOr, new BinaryOperatorParselet((int)Precedence.BitwiseOr, false));
            RegisterInfix(TokenType.BitwiseXor, new BinaryOperatorParselet((int)Precedence.BitwiseXor, false));
            RegisterInfix(TokenType.BitwiseShiftLeft, new BinaryOperatorParselet((int)Precedence.BitwiseShift, false));
            RegisterInfix(TokenType.BitwiseShiftRight, new BinaryOperatorParselet((int)Precedence.BitwiseShift, false));

            // other expression stuff
            RegisterPrefix(TokenType.LeftParen, new GroupParselet());
        }

        static void RegisterDeclaration(TokenType type, IDeclarationParselet parselet)
        {
            _declarationParselets.Add(type, parselet);
        }

        static void RegisterStatement(TokenType type, IStatementParselet parselet)
        {
            _statementParselets.Add(type, parselet);
        }

        static void RegisterPrefix(TokenType type, IPrefixParselet parselet)
        {
            _prefixParselets.Add(type, parselet);
        }

        static void RegisterInfix(TokenType type, IInfixParselet parselet)
        {
            _infixParselets.Add(type, parselet);
        }
    }
}
