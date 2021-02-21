using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintTypeParameterListSyntax(TypeParameterListSyntax node)
        {
            if (node.Parameters.Count == 0) {
                return null;
            }
            return Group(this.PrintSyntaxToken(node.LessThanToken),
                Indent(SoftLine,
                    this.PrintSeparatedSyntaxList(node.Parameters, this.PrintTypeParameterSyntax, Line)),
                this.PrintSyntaxToken(node.GreaterThanToken));
        }
    }
}