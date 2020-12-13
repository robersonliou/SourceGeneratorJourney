using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SourceGeneratorLib.SyntaxReceivers
{
    internal class MyClassSyntaxReceiver: ISyntaxReceiver
    {
        public ClassDeclarationSyntax MyClassSyntax { get; private set; }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is ClassDeclarationSyntax classSyntax &&
                classSyntax.Identifier.ValueText == "MyClass")
            {
                MyClassSyntax = classSyntax;
            }
        }
    }
}
