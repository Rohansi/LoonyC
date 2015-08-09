using System;
using System.Collections.Generic;
using LoonyC.Compiler;
using LoonyC.Compiler.Types;
using NUnit.Framework;

namespace LoonyC.Tests
{
    [TestFixture]
    public class Types
    {
        #region Constraints

        [Test]
        public void ConstraintsNamed()
        {
            Assert.Throws<ArgumentNullException>(() => Discard(new NamedType(null)));

            Assert.Throws<ArgumentNullException>(() => Discard(new NamedType("")));

            Assert.Throws<ArgumentNullException>(() => Discard(new NamedType(" ")));
        }

        [Test]
        public void ConstraintsPointer()
        {
            Assert.Throws<ArgumentNullException>(() => Discard(new PointerType(null)));
        }

        [Test]
        public void ConstraintsArray()
        {
            Assert.Throws<ArgumentNullException>(() => Discard(new ArrayType(null, 1)));

            Assert.Throws<ArgumentOutOfRangeException>(() => Discard(new ArrayType(new PrimitiveType(Primitive.Char), 0)));

            Assert.Throws<ArgumentOutOfRangeException>(() => Discard(new ArrayType(new PrimitiveType(Primitive.Char), -1)));
        }

        [Test]
        public void ConstraintsFunc()
        {
            Assert.Throws<ArgumentNullException>(() => Discard(new FuncType(null, null)));

            Assert.Throws<ArgumentNullException>(() => Discard(new FuncType(new List<TypeBase> { null }, null)));
        }

        [Test]
        public void ConstraintsAny()
        {
            Assert.Throws<ArgumentException>(() => Discard(new AnyType(true)));
        }

        #endregion

        #region Equality

        [Test]
        public void EqualityBool()
        {
            AssertEqual(new BoolType(),
                        new BoolType());

            AssertNotEqual(new BoolType(),
                           new PrimitiveType(Primitive.Char));
        }

        [Test]
        public void EqualityPrimitive()
        {
            AssertEqual(new PrimitiveType(Primitive.Char),
                        new PrimitiveType(Primitive.Char));

            AssertEqual(new PrimitiveType(Primitive.Short),
                        new PrimitiveType(Primitive.Short));

            AssertEqual(new PrimitiveType(Primitive.Int),
                        new PrimitiveType(Primitive.Int));


            AssertNotEqual(new PrimitiveType(Primitive.Char),
                           new PrimitiveType(Primitive.Short));

            AssertNotEqual(new PrimitiveType(Primitive.Char),
                           new PrimitiveType(Primitive.Int));

            AssertNotEqual(new PrimitiveType(Primitive.Short),
                           new PrimitiveType(Primitive.Char));

            AssertNotEqual(new PrimitiveType(Primitive.Short),
                           new PrimitiveType(Primitive.Int));

            AssertNotEqual(new PrimitiveType(Primitive.Int),
                           new PrimitiveType(Primitive.Short));

            AssertNotEqual(new PrimitiveType(Primitive.Int),
                           new PrimitiveType(Primitive.Char));
        }

        [Test]
        public void EqualityNamed()
        {
            AssertEqual(new NamedType("Test"),
                        new NamedType("Test"));

            AssertNotEqual(new NamedType("Test"),
                           new NamedType("Test2"));

            AssertNotEqual(new NamedType("Test"),
                           new PrimitiveType(Primitive.Char));

            AssertNotEqual(new PrimitiveType(Primitive.Char),
                           new NamedType("Test"));
        }

        [Test]
        public void EqualityPointer()
        {
            AssertEqual(new PointerType(new PrimitiveType(Primitive.Char)),
                        new PointerType(new PrimitiveType(Primitive.Char)));

            AssertEqual(new PointerType(new PointerType(new PrimitiveType(Primitive.Char))),
                        new PointerType(new PointerType(new PrimitiveType(Primitive.Char))));

            AssertNotEqual(new PointerType(new PointerType(new PrimitiveType(Primitive.Char))),
                           new PointerType(new PrimitiveType(Primitive.Char)));

            AssertNotEqual(new PointerType(new PrimitiveType(Primitive.Char)),
                           new PrimitiveType(Primitive.Char));
        }

        [Test]
        public void EqualityArray()
        {
            AssertEqual(new ArrayType(new PrimitiveType(Primitive.Char), 10),
                        new ArrayType(new PrimitiveType(Primitive.Char), 10));

            AssertNotEqual(new ArrayType(new PrimitiveType(Primitive.Char), 10),
                           new ArrayType(new PrimitiveType(Primitive.Char), 11));

            AssertNotEqual(new ArrayType(new PrimitiveType(Primitive.Char), 10),
                           new PointerType(new PrimitiveType(Primitive.Char)));
        }

        [Test]
        public void EqualityAny()
        {
            AssertEqual(new AnyType(),
                        new AnyType());

            AssertEqual(new PointerType(new AnyType()),
                        new PointerType(new AnyType()));

            AssertNotEqual(new AnyType(),
                           new PrimitiveType(Primitive.Char));
        }

        [Test]
        public void EqualityFunc()
        {
            AssertEqual(new FuncType(new List<TypeBase>(), null),
                        new FuncType(new List<TypeBase>(), null));

            AssertEqual(new FuncType(new List<TypeBase> { new PrimitiveType(Primitive.Int) }, null),
                        new FuncType(new List<TypeBase> { new PrimitiveType(Primitive.Int) }, null));

            AssertEqual(new FuncType(new List<TypeBase>(), new PrimitiveType(Primitive.Int)),
                        new FuncType(new List<TypeBase>(), new PrimitiveType(Primitive.Int)));

            AssertNotEqual(new FuncType(new List<TypeBase> { new PrimitiveType(Primitive.Int) }, null),
                           new FuncType(new List<TypeBase>(), null));

            AssertNotEqual(new FuncType(new List<TypeBase>(), new PrimitiveType(Primitive.Int)),
                           new FuncType(new List<TypeBase>(), null));
        }

        [Test]
        public void EqualityConst()
        {
            AssertEqual(new PrimitiveType(Primitive.Int, true),
                        new PrimitiveType(Primitive.Int, true));

            AssertNotEqual(new PrimitiveType(Primitive.Int, true),
                           new PrimitiveType(Primitive.Int));

            AssertEqual(new NamedType("Test", true),
                        new NamedType("Test", true));

            AssertNotEqual(new NamedType("Test", true),
                           new NamedType("Test"));

            AssertEqual(new PointerType(new PrimitiveType(Primitive.Int), true),
                        new PointerType(new PrimitiveType(Primitive.Int), true));

            AssertNotEqual(new PointerType(new PrimitiveType(Primitive.Int), true),
                           new PointerType(new PrimitiveType(Primitive.Int)));

            AssertEqual(new ArrayType(new PrimitiveType(Primitive.Int), 10, true),
                        new ArrayType(new PrimitiveType(Primitive.Int), 10, true));

            AssertNotEqual(new ArrayType(new PrimitiveType(Primitive.Int), 10, true),
                           new ArrayType(new PrimitiveType(Primitive.Int), 10));

            AssertEqual(new PointerType(new AnyType(), true),
                        new PointerType(new AnyType(), true));

            AssertNotEqual(new PointerType(new AnyType(), true),
                           new PointerType(new AnyType()));

            AssertEqual(new FuncType(new List<TypeBase>(), null, true),
                        new FuncType(new List<TypeBase>(), null, true));

            AssertNotEqual(new FuncType(new List<TypeBase>(), null, true),
                           new FuncType(new List<TypeBase>(), null));
        }

        private static void AssertNotEqual(TypeBase typeA, TypeBase typeB, string message = null)
        {
            Assert.Throws<AssertionException>(() => AssertEqual(typeA, typeB), message);
        }
        
        // ReSharper disable UnusedParameter.Local
        private static void AssertEqual(TypeBase typeA, TypeBase typeB, string message = null)
        {
            Assert.True(typeA == typeB, message + " ==");
            Assert.True(typeA.Equals(typeB), message + " Equals");
            Assert.True(typeA.Equals((object)typeB), message + " Equals(object)");
        }
        // ReSharper restore UnusedParameter.Local

        #endregion

        #region Parsing

        [Test]
        public void ParseBool()
        {
            AssertParseEqual("bool", new BoolType());
        }

        [Test]
        public void ParsePrimitives()
        {
            AssertParseEqual("char", new PrimitiveType(Primitive.Char));

            AssertParseEqual("short", new PrimitiveType(Primitive.Short));

            AssertParseEqual("int", new PrimitiveType(Primitive.Int));
        }

        [Test]
        public void ParseNamed()
        {
            AssertParseEqual("Test", new NamedType("Test"));
        }

        [Test]
        public void ParsePointer()
        {
            AssertParseEqual("*char", new PointerType(new PrimitiveType(Primitive.Char)));

            AssertParseEqual("**char", new PointerType(new PointerType(new PrimitiveType(Primitive.Char))));

            Assert.Throws<CompilerException>(() => Parse("*"));
        }

        [Test]
        public void ParseArray()
        {
            AssertParseEqual("[10]char", new ArrayType(new PrimitiveType(Primitive.Char), 10));

            AssertParseEqual("[5][10]char", new ArrayType(new ArrayType(new PrimitiveType(Primitive.Char), 10), 5));

            Assert.Throws<CompilerException>(() => Parse("[0]char"));

            Assert.Throws<CompilerException>(() => Parse("[n]char"));

            Assert.Throws<CompilerException>(() => Parse("[]char"));
        }

        [Test]
        public void ParseAny()
        {
            AssertParseEqual("*any", new PointerType(new AnyType()));

            Assert.Throws<CompilerException>(() => Parse("any"));

            Assert.Throws<CompilerException>(() => Parse("const any"));

            Assert.Throws<CompilerException>(() => Parse("[10]any"));
        }

        [Test]
        public void ParseFunc()
        {
            AssertParseEqual("func()", new FuncType(new List<TypeBase>(), null));

            AssertParseEqual("func(int)", new FuncType(new List<TypeBase> { new PrimitiveType(Primitive.Int) }, null));

            AssertParseEqual("func(int,int)", new FuncType(new List<TypeBase> { new PrimitiveType(Primitive.Int), new PrimitiveType(Primitive.Int) }, null));

            AssertParseEqual("func():int", new FuncType(new List<TypeBase>(), new PrimitiveType(Primitive.Int)));

            AssertParseEqual("func(int):int", new FuncType(new List<TypeBase> { new PrimitiveType(Primitive.Int) }, new PrimitiveType(Primitive.Int)));
        }

        [Test]
        public void ParseConst()
        {
            AssertParseEqual("const int", new PrimitiveType(Primitive.Int, true));
            AssertParseEqual("const Test", new NamedType("Test", true));
            AssertParseEqual("const *int", new PointerType(new PrimitiveType(Primitive.Int), true));
            AssertParseEqual("const [10]int", new ArrayType(new PrimitiveType(Primitive.Int), 10, true));
            AssertParseEqual("const *any", new PointerType(new AnyType(), true));

            Assert.Throws<CompilerException>(() => AssertParseEqual("const const int", null));
        }

        private static void AssertParseEqual(string typeStr, TypeBase type)
        {
            AssertEqual(Parse(typeStr), type, typeStr);
        }

        #endregion

        #region Assignable

        [Test]
        public void AssignableBool()
        {
            AssertAssignable("bool", "bool");

            AssertNotAssignable("bool", "int");
            AssertNotAssignable("int", "bool");
        }

        [Test]
        public void AssignablePrimitive()
        {
            AssertAssignable("char", "char");
            AssertAssignable("char", "short");
            AssertAssignable("char", "int");

            AssertNotAssignable("short", "char");
            AssertAssignable("short", "short");
            AssertAssignable("short", "int");

            AssertNotAssignable("int", "char");
            AssertNotAssignable("int", "short");
            AssertAssignable("int", "int");
        }

        [Test]
        public void AssignableNamed()
        {
            AssertAssignable("Test", "Test");
            AssertNotAssignable("Test", "Test2");

            AssertNotAssignable("int", "Test");
            AssertNotAssignable("Test", "int");
        }

        [Test]
        public void AssignablePointer()
        {
            AssertAssignable("*char", "*char");

            AssertNotAssignable("*char", "*int");

            AssertNotAssignable("**char", "*char");
        }

        [Test]
        public void AssignableArray()
        {
            AssertAssignable("[10]char", "[10]char");
            AssertAssignable("[20]char", "[10]char");
            AssertNotAssignable("[5]char", "[10]char");

            AssertAssignable("[10]char", "*char");
            AssertNotAssignable("*char", "[10]char");

            AssertNotAssignable("[10]char", "**char");

            AssertNotAssignable("[10]char", "char");
        }

        [Test]
        public void AssignableAny()
        {
            AssertAssignable("*any", "*any");
            AssertAssignable("*any", "*int");
            AssertAssignable("*int", "*any");

            AssertAssignable("*char", "*any");
            AssertAssignable("*any", "*char");

            AssertAssignable("*test", "*any");
            AssertAssignable("*any", "*test");

            AssertAssignable("[10]char", "*any");
            AssertNotAssignable("*any", "[10]char");

            AssertAssignable("*any", "**char");
            AssertNotAssignable("**char", "*any");
        }

        [Test]
        public void AssignableFunc()
        {
            AssertAssignable("func()", "func()");
            AssertAssignable("func(int)", "func(int)");
            AssertAssignable("func(int,int)", "func(int,int)");

            AssertAssignable("func(int):int", "func(int):int");

            AssertNotAssignable("func(short)", "func(int)");
            AssertNotAssignable("func():short", "func():int");

            AssertNotAssignable("func(int)", "func(int,int)");

            AssertNotAssignable("func():int", "func()");
        }

        [Test]
        public void AssignableConst()
        {
            AssertAssignable("const int", "int");
            AssertNotAssignable("int", "const int");
            AssertNotAssignable("const int", "const int");
        }

        private static void AssertNotAssignable(string typeA, string typeB)
        {
            Assert.Throws<AssertionException>(() => AssertAssignable(typeA, typeB));
        }

        private static void AssertAssignable(string typeA, string typeB)
        {
            Assert.True(Parse(typeA).IsAssignableTo(Parse(typeB)));
        }

        #endregion

        #region ToString

        [Test]
        [TestCase("bool")]
        [TestCase("const bool")]
        public void BoolToString(string type)
        {
            TypeToString(type);
        }

        [Test]
        [TestCase("char")]
        [TestCase("short")]
        [TestCase("int")]
        [TestCase("const char")]
        [TestCase("const short")]
        [TestCase("const int")]
        public void PrimitiveToString(string type)
        {
            TypeToString(type);
        }

        [Test]
        [TestCase("Test")]
        [TestCase("const Test")]
        public void NamedToString(string type)
        {
            TypeToString(type);
        }

        [Test]
        [TestCase("*char")]
        [TestCase("**char")]
        [TestCase("const *char")]
        [TestCase("*const char")]
        [TestCase("const **char")]
        [TestCase("const *const *char")]
        public void PointerToString(string type)
        {
            TypeToString(type);
        }

        [Test]
        [TestCase("[10]char")]
        [TestCase("*[10]char")]
        [TestCase("[10]*char")]
        [TestCase("const [10]char")]
        [TestCase("const *[10]char")]
        [TestCase("const [10]*char")]
        [TestCase("const [10]const *const char")]
        public void ArrayToString(string type)
        {
            TypeToString(type);
        }

        [Test]
        [TestCase("*any")]
        [TestCase("**any")]
        [TestCase("const *any")]
        [TestCase("const **any")]
        [TestCase("const *const *any")]
        public void AnyToString(string type)
        {
            TypeToString(type);
        }

        [Test]
        [TestCase("func()")]
        [TestCase("func():int")]
        [TestCase("func(int)")]
        [TestCase("func(int):int")]
        [TestCase("func(int,int)")]
        [TestCase("func(int,int):int")]
        [TestCase("func(*int,*int):*int")]
        [TestCase("const func()")]
        [TestCase("const func():const int")]
        [TestCase("const func(const int)")]
        [TestCase("const func(const int):const int")]
        [TestCase("const func(const int,const int)")]
        [TestCase("const func(const int,const int):const int")]
        [TestCase("const func(*const int,*const int):*const int")]
        public void FuncToString(string type)
        {
            TypeToString(type);
        }

        public void TypeToString(string type)
        {
            Assert.AreEqual(type, Parse(type).ToString());
        }

        #endregion

        private static TypeBase Parse(string type)
        {
            var lexer = new LoonyLexer(type);
            var parser = new LoonyParser(lexer);
            return parser.ParseType();
        }

        // ReSharper disable once UnusedParameter.Local
        private static void Discard(object obj)
        {

        }
    }
}
