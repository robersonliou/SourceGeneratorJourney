using System;
using System.Linq;
using twMVC;

namespace SourceGeneratorJourney.UseCases
{
    public static class TestCsvGenerator
    {
        public static void Run()
        {
            Print("## CARS");
            Cars.All.ToList().ForEach(c => Print($"{c.Brand}\t{c.Model}\t{c.Year}\t{c.Cc}"));
            Print("\n## PEOPLE");
            People.All.ToList().ForEach(p => Print($"{p.Name}\t{p.Address}\t{p._11Age}")); 
        }
        public static void Print(string message) => Console.WriteLine(message);
    }
}