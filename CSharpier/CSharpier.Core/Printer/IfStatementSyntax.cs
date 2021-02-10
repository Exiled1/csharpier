using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier.Core
{
    public partial class Printer
    {
        private Doc PrintIfStatementSyntax(IfStatementSyntax node)
        {
            var parts = new Parts(node.IfKeyword.Text, " ", "(", this.Print(node.Condition), ")");
            var statement = this.Print(node.Statement);
            if (node.Statement is BlockSyntax)
            {
                parts.Add(statement);
            }
            else
            {
                parts.Add(Indent(Concat(HardLine, statement)));
            }

            if (node.Else != null)
            {
                parts.Push(HardLine, this.Print(node.Else));
            }

            return Concat(parts);
        }
    }
}