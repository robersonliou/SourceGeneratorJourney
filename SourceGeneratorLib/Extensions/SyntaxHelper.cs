using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace SourceGeneratorLib.Extensions
{
    public static class SyntaxHelper
    {
        public static Compilation AddSyntaxTreeToCompilation(Compilation sourceCompilation, string sourceText)
        {
            var options = (sourceCompilation as CSharpCompilation).SyntaxTrees[0].Options as CSharpParseOptions;
            var compilation =
                sourceCompilation.AddSyntaxTrees(
                    CSharpSyntaxTree.ParseText(SourceText.From(sourceText, Encoding.UTF8), options));
            return compilation;
        }

        public static IEnumerable<AttributeSyntax> GetMatchedAttributeSyntaxes(Compilation compilation, string @namespace, string attributeName)
        {
            var syntaxNodes = compilation.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
            var attrSyntaxes = syntaxNodes.Where((d) => d.IsKind(SyntaxKind.Attribute)).OfType<AttributeSyntax>();
            var matchedAttrSyntaxes = attrSyntaxes.Where(x =>
                x.Name.ToString() == attributeName || x.Name.ToString() == $"{@namespace}.{attributeName}");
            return matchedAttrSyntaxes;
        }
    }
}