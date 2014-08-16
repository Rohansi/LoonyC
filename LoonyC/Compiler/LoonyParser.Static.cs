using System.Collections.Generic;
using LoonyC.Compiler.Parselets;
using LoonyC.Compiler.Parselets.Statements;

namespace LoonyC.Compiler
{
    internal partial class LoonyParser
    {
        private static Dictionary<TokenType, IPrefixParselet> _prefixParselets;
        private static Dictionary<TokenType, IInfixParselet> _infixParselets;
        private static Dictionary<TokenType, IStatementParselet> _statementParselets;

        static LoonyParser()
        {
            _prefixParselets = new Dictionary<TokenType, IPrefixParselet>();
            _infixParselets = new Dictionary<TokenType, IInfixParselet>();
            _statementParselets = new Dictionary<TokenType, IStatementParselet>();

            // leaves
            RegisterPrefix(TokenType.Number, new NumberParselet());

            // math
            RegisterInfix(TokenType.Add, new BinaryOperatorParselet((int)Precedence.Additive, false));
            RegisterInfix(TokenType.Subtract, new BinaryOperatorParselet((int)Precedence.Additive, false));
            RegisterInfix(TokenType.Multiply, new BinaryOperatorParselet((int)Precedence.Multiplicative, false));
            RegisterInfix(TokenType.Divide, new BinaryOperatorParselet((int)Precedence.Multiplicative, false));
            RegisterInfix(TokenType.Remainder, new BinaryOperatorParselet((int)Precedence.Multiplicative, false));

            // bitwise
            RegisterInfix(TokenType.And, new BinaryOperatorParselet((int)Precedence.BitwiseAnd, false));
            RegisterInfix(TokenType.Or, new BinaryOperatorParselet((int)Precedence.BitwiseOr, false));
            RegisterInfix(TokenType.Xor, new BinaryOperatorParselet((int)Precedence.BitwiseXor, false));
            RegisterInfix(TokenType.ShiftLeft, new BinaryOperatorParselet((int)Precedence.BitwiseShift, false));
            RegisterInfix(TokenType.ShiftRight, new BinaryOperatorParselet((int)Precedence.BitwiseShift, false));

            // other expression stuff
            RegisterPrefix(TokenType.LeftParen, new GroupParselet());
        }

        static void RegisterPrefix(TokenType type, IPrefixParselet parselet)
        {
            _prefixParselets.Add(type, parselet);
        }

        static void RegisterInfix(TokenType type, IInfixParselet parselet)
        {
            _infixParselets.Add(type, parselet);
        }

        static void RegisterStatement(TokenType type, IStatementParselet parselet)
        {
            _statementParselets.Add(type, parselet);
        }
    }
}
