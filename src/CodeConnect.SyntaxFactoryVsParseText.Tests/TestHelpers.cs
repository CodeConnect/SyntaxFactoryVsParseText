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
            var newTypes = new List<TypeDeclarationSyntax>();
            newTypes.Add(generatedType);
            var tree = GetTestSyntaxTreeWithTypes(newTypes);
            var compilation = CreateCompilation(tree);
            var diags = compilation.GetDiagnostics();
            return !diags.Any(diag => diag.Severity == DiagnosticSeverity.Error);
        }

        internal static SyntaxTree GetTestSyntaxTreeWithTypes(IEnumerable<TypeDeclarationSyntax> types)
        {
            var tree = CSharpSyntaxTree.ParseText(@"
using System;

namespace CodeConnect.SyntaxFactoryVsParseText.MockNamespace
{
    interface IDemo
    {
        void Run();
    }
}");
            var root = tree.GetRoot();
            var n = root.ChildNodes().OfType<NamespaceDeclarationSyntax>().First();
            var newN = n.WithMembers(SyntaxFactory.List(n.Members.Union(types)));
            var newRoot = root.ReplaceNode(n, newN);
            return newRoot.SyntaxTree;
        }

        internal static Compilation CreateCompilation(SyntaxTree tree)
        {
            var mscorlibAssembly = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            var debugAssembly = MetadataReference.CreateFromFile(typeof(System.Diagnostics.Debug).Assembly.Location);
            var compilation = CSharpCompilation.Create("MyCompilation",
                syntaxTrees: new[] { tree }, references: new[] { mscorlibAssembly, debugAssembly }, options: new CSharpCompilationOptions(outputKind: OutputKind.DynamicallyLinkedLibrary));
            return compilation;
        }
    }
}
