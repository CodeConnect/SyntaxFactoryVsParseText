using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeConnect.SyntaxFactoryVsParseText.Benchmark
{
    class Program
    {
        const int _parseTextId = 0;
        const int _syntaxFactoryId = 1;
        const int _samples = 100;

        static void Main(string[] args)
        {
            if (args != null && args.Count() == 3)
            {
                var desiredCalls = Int32.Parse(args[0]);
                var desiredComplexity = Boolean.Parse(args[1]);
                var generatorId = Int32.Parse(args[2]);
                SingleRun(desiredCalls, desiredComplexity, generatorId);
            }
            else
            {
                Console.WriteLine("Benchmark");

                Benchmark(1, false);
                Benchmark(4, false);
                Benchmark(16, false);
                Benchmark(64, false);
                Benchmark(128, false);
                Benchmark(256, false);
                Benchmark(512, false);
                Benchmark(1024, false);
                Benchmark(2048, false);
                Benchmark(4096, false);

                Benchmark(1, true);
                Benchmark(4, true);
                Benchmark(16, true);
                Benchmark(64, true);
                Benchmark(128, true);
                Benchmark(256, true);
                Benchmark(512, true);
                Benchmark(1024, true);
                Benchmark(2048, true);
                Benchmark(4096, true);

                Benchmark(129, true);
                Benchmark(129, false);

                Console.ReadLine();
            }
        }

        private static void SingleRun(int methodCalls, bool complexMethods, int generatorId)
        {
            CodeGenerator generator = generatorId == _parseTextId ? new ParseTextCodeGenerator() as CodeGenerator : new SyntaxFactoryCodeGenerator() as CodeGenerator;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            generator.GenerateType(methodCalls, complexMethods);
            stopwatch.Stop();
            var outputPath = $"D:\\benchmark {generator.ToString()} {methodCalls} {complexMethods}.csv";
            File.AppendAllText(outputPath, $"{stopwatch.ElapsedMilliseconds}\n");
        }

        static void Benchmark(int methodCalls, bool complexMethods)
        {
            string what = complexMethods ? "methods" : "empty methods";
            Console.WriteLine($"Generating {methodCalls} {what}, {_samples} times.");
            ProcessStartInfo startInfo;
            Process singleRun;
            for (int sample = 0; sample < _samples; sample++)
            {
                // Parse text
                startInfo = Process.GetCurrentProcess().StartInfo;
                startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
                startInfo.Arguments = $"{methodCalls} {complexMethods} {_parseTextId}";
                singleRun = Process.Start(startInfo);
                singleRun.WaitForExit();

                // Syntax factory
                startInfo = Process.GetCurrentProcess().StartInfo;
                startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
                startInfo.Arguments = $"{methodCalls} {complexMethods} {_syntaxFactoryId}";
                singleRun = Process.Start(startInfo);
                singleRun.WaitForExit();
            }
        }

    }
}
