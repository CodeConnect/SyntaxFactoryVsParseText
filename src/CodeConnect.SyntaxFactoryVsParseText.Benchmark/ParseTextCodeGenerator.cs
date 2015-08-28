using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;

namespace CodeConnect.SyntaxFactoryVsParseText.Benchmark
{
    public class ParseTextCodeGenerator : CodeGenerator
    {
        IEnumerable<string> _methodNames;
        bool _complexMethods;
        StringBuilder _sb;

        public override string ToString()
        {
            return "ParseText";
        }

        public override TypeDeclarationSyntax GenerateType(int methodCalls, bool complexMethods)
        {
            _methodNames = getIdentifierNames(methodCalls).ToList();
            _complexMethods = complexMethods;
            _sb = new StringBuilder();

            _sb.Append(@"    public class DemoClass : IDemo
    {
");
            getAllMembers();
            _sb.Append(@"    }");

            var parsedTree = CSharpSyntaxTree.ParseText(_sb.ToString());
            var root = parsedTree.GetRoot();
            var targetType = root.ChildNodes().OfType<TypeDeclarationSyntax>().Single();
            return targetType;
        }

        private void getAllMembers()
        {
            getRunMethod();
            getMethods();
        }

        private void getRunMethod()
        {
            _sb.Append(@"        public void Run()
        {");
            getMethodInvocations();
            _sb.Append(@"
        }
");
        }

        private void getMethodInvocations()
        {
            foreach (var methodName in _methodNames)
            {
                _sb.Append($@"
            {methodName}();");
            }
        }

        private void getMethods()
        {
            foreach (var methodName in _methodNames)
            {
                _sb.Append($@"        private void {methodName}()
        {{");
                getMethodBody(methodName);
                _sb.Append(@"
        }
");
            }
        }

        private void getMethodBody(string methodName)
        {
            if (!_complexMethods)
            {
                // Method's body is empty
                _sb.AppendLine();
            }
            else
            {
                // We're making a few more calls in the method's body
                _sb.Append($@"
            var myNumber = Int32.Parse(""{methodName}"".Substring(6));
            Console.WriteLine(""{methodName} says hi to "" + (myNumber + 1));
            System.Diagnostics.Debug.WriteLine(""Method {{0}} executed."", new[] {{ myNumber }});
            var test = myNumber * 2; ");
            }
        }
    }
}
