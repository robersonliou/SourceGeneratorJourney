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
    class DataBuilderGenerator : ISourceGenerator
    {

        private const string NameSpace = "twMVC";
        private const string AttributeName = "DataBuilder";
        
        public void Initialize(GeneratorInitializationContext context)
        {
            #region manully toggle debugger
            // Debugger.Launch();
            #endregion
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var attributeText = $@"
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

            var matchedAttributeSyntaxes =
                SyntaxHelper.GetMatchedAttributeSyntaxes(compilation, NameSpace, AttributeName);

            foreach (var attr in matchedAttributeSyntaxes)
            {

                #region Get Mapping Target Class Info

                var targetClassSyntax = attr.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>()
                    .Last();
                var targetClassModel = compilation.GetSemanticModel(attr.SyntaxTree);
                var targetClassNamedTypeSymbol = targetClassModel.GetDeclaredSymbol(targetClassSyntax);
                var targetClassFullName = targetClassNamedTypeSymbol.OriginalDefinition.ToString();
                var targetClassName = targetClassFullName.Split('.').Last();
                var targetNameSpace = targetClassFullName.Replace($".{targetClassName}", "");
                var targetClassProperties = targetClassSyntax.GetProperties(targetClassModel);

                #endregion

                #region Generate Builder method


                var sourceBuilder = new StringBuilder($@"
{SharedMeta.GeneratedByDataBuilderGeneratorPreamble}
using System;
namespace {targetNameSpace}
{{
    public class {targetClassName}Builder
    {{");

                var buildMethodBuilder = new StringBuilder($@"
        public {targetClassName} Build()
        {{
                var instance = new {targetClassName}();
");
                
                foreach (var (propertyType, propertyName, _) in targetClassProperties)
                {
                    var lowerPropName = propertyName.ToLower();
                    var fieldName = $"_{lowerPropName}";
                    var isNullable = propertyType.ToLower() != "string";
                    sourceBuilder.Append(@$"
        private {propertyType}{(isNullable?"?":"")} {fieldName};
        public {targetClassName}Builder With{propertyName}({propertyType} {lowerPropName})
        {{
            this.{fieldName} = {lowerPropName};
            return this;
        }}
");

                    if (isNullable)
                    {
                     buildMethodBuilder.Append($@"
                if({fieldName}.HasValue)
                {{
                    instance.{propertyName} = {fieldName}.Value;
                }}");
                    }
                    
                    else
                    {
                        buildMethodBuilder.Append($@"
                if(!string.IsNullOrEmpty({fieldName}))
                {{
                    instance.{propertyName} = {fieldName};
                }}");
                    }

                }

                buildMethodBuilder.Append($@"
                return instance;
        }}");
                
                sourceBuilder.Append($@"
                {buildMethodBuilder.ToString()}
    }}
}}");

                context.AddSource($"{targetClassName}Builder", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                #endregion
            }
        }
    }
}
