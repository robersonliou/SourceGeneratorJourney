using MapperGenerator;
using SourceGeneratorJourney.Entities;

namespace SourceGeneratorJourney.UseCases
{
    public static class TestMapperGenerator
    {
        public static void Run()
        {
            var entity = new PersonEntity()
            {
                Id = 1,
                Name = "Roberson",
                Country = "Taiwan",
                Gender = "Male",
            };
            var viewModel = entity.ToPersonViewModel();
        }
    }
}