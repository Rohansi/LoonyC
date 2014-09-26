using System;
using System.Collections.Generic;

namespace LoonyC.Compiler
{
    abstract class Parser
    {
        private IEnumerator<Token> _tokens;
        private List<Token> _read;

        protected Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens.GetEnumerator();
            _read = new List<Token>(8);
        }

        /// <summary>
        /// Returns the token that was most recently taken.
        /// </summary>
        public Token Previous { get; private set; }

        /// <summary>
        /// Check if the next token matches the given type. If they match, take the token.
        /// </summary>
        public bool MatchAndTake(TokenType type)
        {
            var isMatch = Match(type);
            if (isMatch)
                Take();

            return isMatch;
        }

        /// <summary>
        /// Check if the next token matches the given type.
        /// </summary>
        public bool Match(TokenType type, int distance = 0)
        {
            return Peek(distance).Type == type;
        }

        /// <summary>
        /// Take a token from the stream. Throws an exception if the given type does not match the token type.
        /// </summary>
        public Token Take(TokenType type)
        {
            var token = Take();

            if (token.Type != type)
                throw new CompilerException(token, CompilerError.ExpectedButFound, type, token);

            return token;
        }

        /// <summary>
        /// Take a token from the stream.
        /// </summary>
        public Token Take()
        {
            Peek();

            var result = _read[0];
            _read.RemoveAt(0);

            Previous = result;

            return result;
        }

        /// <summary>
        /// Peek at future tokens in the stream. Distance is the number of tokens from the current one.
        /// </summary>
        public Token Peek(int distance = 0)
        {
            if (distance < 0)
                throw new ArgumentOutOfRangeException("distance", "distance can't be negative");

            while (_read.Count <= distance)
            {
                _tokens.MoveNext();
                _read.Add(_tokens.Current);

                //Console.WriteLine(_tokens.Current.Type);
            }

            return _read[distance];
        }
    }
}
