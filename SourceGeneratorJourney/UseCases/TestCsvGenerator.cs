using System;
using System.Linq;

namespace SourceGeneratorJourney.UseCases
{
    public static class TestCsvGenerator
    {
        public static void Run()
        {
            Print("## CARS");
            CSV.Cars.All.ToList().ForEach(c => Print($"{c.Brand}\t{c.Model}\t{c.Year}\t{c.Cc}"));
            Print("\n## PEOPLE");
            CSV.People.All.ToList().ForEach(p => Print($"{p.Name}\t{p.Address}\t{p._11Age}")); 
        }
        public static void Print(string message) => Console.WriteLine(message);
    }
}