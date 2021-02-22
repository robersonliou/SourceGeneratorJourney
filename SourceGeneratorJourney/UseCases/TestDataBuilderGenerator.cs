

using SourceGeneratorJourney.Entities;

namespace SourceGeneratorJourney.UseCases
{
    public static class TestDataBuilderGenerator
    {
        public static void Run()
        {
            var personEntityBuilder = new PersonEntityBuilder();
            // var person = personEntityBuilder
            //     .WithId(1)
            //     .WithGender("Male")
            //     .WithName("John")
            //     .WithCountry("Taiwan").Build();
        }
    }
    
}