using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.SyntaxFactoryVsParseText.Tests
{
    internal class TestHelpers
    {
        internal static bool TypeDeclarationCompiles(TypeDeclarationSyntax generatedType)
        {
            var tree = GetTestSyntaxTreeWithCode(generatedType.ToFullString());
            var compilation = CreateCompilation(tree);
            var diags = compilation.GetDiagnostics();
            return !diags.Any(diag => diag.Severity == DiagnosticSeverity.Error);
        }

        internal static SyntaxTree GetTestSyntaxTreeWithCode(string testCode)
        {
            var tree = CSharpSyntaxTree.ParseText(@"
using System;

namespace CodeConnect.SyntaxFactoryVsParseText.MockNamespace
{
    interface IDemo
    {
        void Run();
    }

" + testCode + @"
}");
            return tree;
        }

        internal static Compilation CreateCompilation(SyntaxTree tree)
        {
            var mscorlib = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var compilation = CSharpCompilation.Create("MyCompilation",
                syntaxTrees: new[] { tree }, references: new[] { mscorlib }, options: new CSharpCompilationOptions(outputKind: OutputKind.DynamicallyLinkedLibrary));
            return compilation;
        }
    }
}
