using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.SyntaxFactoryVsParseText.Benchmark
{
    public abstract class CodeGenerator
    {
        static Random _random;
        const string IDENTIFIER_NAME = "method";

        public abstract TypeDeclarationSyntax GenerateType(int methodCalls, bool complexMethods);

        public CodeGenerator()
        {
            _random = new Random();
        }

        protected string getRandomIdentifierName()
        {
            return String.Concat(IDENTIFIER_NAME, _random.Next());
        }

        protected IEnumerable<string> getIdentifierNames(int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                yield return getRandomIdentifierName();
            }
        }
    }

}
