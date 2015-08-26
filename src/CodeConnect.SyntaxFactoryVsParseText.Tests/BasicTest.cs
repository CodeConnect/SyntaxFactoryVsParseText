using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CodeConnect.SyntaxFactoryVsParseText.Benchmark;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeConnect.SyntaxFactoryVsParseText.Tests
{
    [TestClass]
    public class BasicTest
    {
        [TestMethod]
        public void ParseTextGeneratorCreatesCorrectType()
        {
            var parseTextGenerator = new ParseTextCodeGenerator();

            int numberOfMethods = 5;
            var typeSyntax = parseTextGenerator.GenerateType(numberOfMethods, true);

            var methods = typeSyntax.ChildNodes().OfType<MethodDeclarationSyntax>();
            Assert.AreEqual(numberOfMethods + 1, methods.Count());
            Assert.AreEqual("Run", methods.First().Identifier.ToString());
            Assert.IsTrue(TestHelpers.TypeDeclarationCompiles(typeSyntax));
        }

        [TestMethod]
        public void SyntaxFactoryGeneratorCreatesCorrectType()
        {
            var syntaxFactoryGenerator = new SyntaxFactoryCodeGenerator();

            int numberOfMethods = 5;
            var typeSyntax = syntaxFactoryGenerator.GenerateType(numberOfMethods, true);

            var methods = typeSyntax.ChildNodes().OfType<MethodDeclarationSyntax>();
            Assert.AreEqual(numberOfMethods + 1, methods.Count());
            Assert.AreEqual("Run", methods.First().Identifier.ToString());
            Assert.IsTrue(TestHelpers.TypeDeclarationCompiles(typeSyntax));
        }
    }
}
