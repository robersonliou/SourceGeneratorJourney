using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using SourceGeneratorLib.Extensions;

namespace SourceGeneratorLib.Generators
{
    [Generator]
    class DiagnosticGenerator : ISourceGenerator
    {

        private const string NameSpace = "twMVC";
        private const string AttributeName = "TestDiagnostic";
        
        public void Initialize(GeneratorInitializationContext context)
        {
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
    }}
}}";

                context.AddSource($"{AttributeName}Attribute", SourceText.From(attributeText, Encoding.UTF8));
                var compilation = SyntaxHelper.AddSyntaxTreeToCompilation(context.Compilation, attributeText);
                var matchedAttrSyntaxes = SyntaxHelper.GetMatchedAttributeSyntaxes(compilation, NameSpace, AttributeName);

                if (matchedAttrSyntaxes.Any()) ReportDiagnostics(context);
            }
        }

        private static void ReportDiagnostics(GeneratorExecutionContext context)
        {
            var infoDiagnostic = new DiagnosticDescriptor("MYINF001", "TestDiagnostic",
                $"Here is a info.", "source generator",
                DiagnosticSeverity.Info, true);
            var warningDiagnostic = new DiagnosticDescriptor("MYWAR001", "TestDiagnostic",
                $"Here is a warning.", "source generator",
                DiagnosticSeverity.Warning, true);
            var errorDiagnostic = new DiagnosticDescriptor("MYERR001", "TestDiagnostic",
                $"Here is a error.", "source generator",
                DiagnosticSeverity.Error, true);
            context.ReportDiagnostic(Diagnostic.Create(infoDiagnostic, Location.None));
            context.ReportDiagnostic(Diagnostic.Create(warningDiagnostic, Location.None));
            context.ReportDiagnostic(Diagnostic.Create(errorDiagnostic, Location.None));
        }
    }
}
