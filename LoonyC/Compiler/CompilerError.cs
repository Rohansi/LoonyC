﻿namespace LoonyC.Compiler
{
    static class CompilerError
    {
        public const string UnterminatedString = "Unterminated string";
        public const string UnexpectedEofString = "Unexpected end of file (bad escape sequence)";
        public const string InvalidEscapeSequence = "Invalid escape sequence '{0}'";
        public const string CharLiteralLength = "Character literals must be one byte long";
        public const string InvalidNumber = "Invalid number";
        public const string UnexpectedCharacter = "Unexpected character '{0}'";
        public const string ExpectedButFound = "Expected {0} but found {1}";

        public const string DivisionByZero = "Division by zero";
    }
}
