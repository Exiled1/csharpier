using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintUsingDirectiveSyntax(UsingDirectiveSyntax node)
        {
            return Concat(this.PrintExtraNewLines(node),
                this.PrintSyntaxToken(node.UsingKeyword, " "),
                this.PrintSyntaxToken(node.StaticKeyword, " "),
                node.Alias == null ? null : this.PrintNameEqualsSyntax(node.Alias),
                this.Print(node.Name),
                this.PrintSyntaxToken(node.SemicolonToken)
                );
        }
    }
}