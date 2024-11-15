using System;

namespace vdp_reader
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new VdfParser();
            var data = parser.ParseFile("ruta/a/tu/archivo.");

            Console.ReadKey();
        }
    }
}