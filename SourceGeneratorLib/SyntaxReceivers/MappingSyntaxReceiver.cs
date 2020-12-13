using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGeneratorLib.SyntaxReceivers
{
    public class MappingSyntaxReceiver : ISyntaxReceiver
    {
        public List<TypeDeclarationSyntax> Candidates { get; } = new List<TypeDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is TypeDeclarationSyntax typeDeclarationSyntax)
            {
                var className = typeDeclarationSyntax.Identifier.ToString();
                foreach (var attributeList in
                    typeDeclarationSyntax.AttributeLists)
                {
                    foreach (var attribute in attributeList.Attributes)
                    {
                        if (attribute.Name.ToString() == "Mapping" ||
                            attribute.Name.ToString() == "MappingAttribute")
                        {
                            this.Candidates.Add(typeDeclarationSyntax);
                        }
                    }
                }
            }
        }
    }
}