using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using SourceGeneratorLib.Extensions;

namespace SourceGeneratorLib.Generators
{
    [Generator]
    public class BotGenerator
    {
        private const string NameSpace = "NetConf2020";
        private const string AttributeName = "BotMessage";

        public void Initialize(GeneratorInitializationContext context)
        {
            if (!context.CancellationToken.IsCancellationRequested)
            {
                //Enagle to manually attach debugger.
                //Debugger.Launch();
            }
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (!context.CancellationToken.IsCancellationRequested)
            {
                var attributeText = @$"
{SharedMeta.GeneratedByDataBuilderGeneratorPreamble}
using System;
namespace {NameSpace}
{{
    public class {AttributeName}Attribute : Attribute
    {{
        public string Message {{ get; set; }}

        public {AttributeName}Attribute(string message)
        {{
            this.Message = message;
        }}
    }}
}}";
                context.AddSource($"{AttributeName}Attribute", SourceText.From(attributeText, Encoding.UTF8));

                //Create a new compilation that contains the attribute
                var options = (context.Compilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
                var compilation =
                    context.Compilation.AddSyntaxTrees(
                        CSharpSyntaxTree.ParseText(SourceText.From(attributeText, Encoding.UTF8), options));

                var syntaxNodes = compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
                var attributeSyntaxs =
                    syntaxNodes.Where((d) => d.IsKind(SyntaxKind.Attribute)).OfType<AttributeSyntax>();
                var botAttributeSyntax = attributeSyntaxs.FirstOrDefault(x =>
                    x.Name.ToString() == AttributeName || x.Name.ToString() == $"{NameSpace}.{AttributeName}");

                //Generate DotNetBot class if any BotAttribute exist.
                if (botAttributeSyntax != null)
                {
                    if (botAttributeSyntax.ArgumentList is null)
                        throw new Exception("Attribute argument can't be null.");

                    var messageArgSyntax = botAttributeSyntax.ArgumentList.Arguments.First();
                    var message = messageArgSyntax.Expression.NormalizeWhitespace().ToFullString();

                    var dotnetBotText = $@"
{SharedMeta.GeneratedByDataBuilderGeneratorPreamble}
using System;
namespace {NameSpace}
{{
    public class DotNetBot
    {{
        public void Say()
        {{
            Console.WriteLine({message});          
        }}
    }}
}}
";
                    context.AddSource("DotNetBot", SourceText.From(dotnetBotText, Encoding.UTF8));
                }
            }
        }
    }
}