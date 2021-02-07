using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintElseClauseSyntax(ElseClauseSyntax node)
        {
            var parts = new Parts();
            parts.Add(node.ElseKeyword.Text);
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                parts.Add(statement);
            }
            else if (node.Statement is IfStatementSyntax)
            {
                parts.Push(String(" "), statement);
            }
            else
            {
                parts.Add(Indent(Concat(HardLine, statement)));
            }

            return Concat(parts);
        }
    }
}