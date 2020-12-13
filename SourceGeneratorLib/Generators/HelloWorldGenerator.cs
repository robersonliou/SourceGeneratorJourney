using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SourceGeneratorLib.Extensions;

namespace SourceGeneratorLib.Generators
{
    [Generator]
    public class HelloWorldGenerator :ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var generatedCode = $@"
{SharedMeta.GeneratedByDataBuilderGeneratorPreamble}
using System;

namespace NetConf2020
{{
    public class HelloWorld
    {{
        public static void Print()
        {{
            Console.WriteLine(""Hello World from Source Generator~~~"");
        }}
    }}
}}
";
            
            context.AddSource("helloWorldGenerated", 
                SourceText.From(generatedCode, Encoding.UTF8));
        }
    }
}