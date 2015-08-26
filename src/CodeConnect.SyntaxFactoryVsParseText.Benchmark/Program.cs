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
        const string _outputPath = "benchmark.csv";
        const int _samples = 1000;
        const int _runs = 5;

        static void Main(string[] args)
        {
            Console.WriteLine("Benchmark");

            ResetOutput();

            Benchmark(10, false);
            Benchmark(50, false);
            Benchmark(100, false);
            Benchmark(500, false);
            Benchmark(1000, false);

            Benchmark(10, true);
            Benchmark(50, true);
            Benchmark(100, true);
            Benchmark(500, true);
            Benchmark(1000, true);

            Console.ReadLine();
        }

        private static void ResetOutput()
        {
            // WriteAllText overwrites existing files
            File.WriteAllText(_outputPath, $"Generator name, Method count, Complex methods, Run, Elapsed ms\n");
        }

        static void Benchmark(int methodCalls, bool complexMethods)
        {
            for (int run = 0; run < _runs; run++)
            {
                string what = complexMethods ? "methods" : "empty methods";
                Console.WriteLine($"Generating {methodCalls} {what}, run {run}.");
                CodeGenerator generator;
                var stopwatch = new Stopwatch();

                generator = new ParseTextCodeGenerator();
                stopwatch.Restart();
                for (int samples = 0; samples < _samples; samples++)
                {
                    generator.GenerateType(methodCalls, complexMethods);
                }
                stopwatch.Stop();
                Console.WriteLine($"{generator.ToString()} finished {_samples} samples in {stopwatch.Elapsed}.");
                File.AppendAllText(_outputPath, $"{generator.ToString()},{methodCalls},{complexMethods},{run},{stopwatch.ElapsedMilliseconds}\n");

                generator = new SyntaxFactoryCodeGenerator();
                stopwatch.Restart();
                for (int samples = 0; samples < 1000; samples++)
                {
                    generator.GenerateType(methodCalls, complexMethods);
                }
                stopwatch.Stop();
                Console.WriteLine($"{generator.ToString()} finished {_samples} samples in {stopwatch.Elapsed}.");
                File.AppendAllText(_outputPath, $"{generator.ToString()},{methodCalls},{complexMethods},{run},{stopwatch.ElapsedMilliseconds}\n");
            }
        }

    }
}
