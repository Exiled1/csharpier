using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpier
{
    public partial class Printer
    {
        private Doc PrintQueryBodySyntax(QueryBodySyntax node)
        {
            var parts = new Parts();
            parts.Push(Join(Line, node.Clauses.Select(this.Print)));
            if (node.Clauses.Count > 0)
            {
                parts.Push(Line);
            }

            parts.Push(this.Print(node.SelectOrGroup));
            if (node.Continuation != null)
            {
                parts.Push(
                    " ",
                    this.PrintQueryContinuationSyntax(node.Continuation)
                );
            }

            return Concat(parts);
        }
    }
}
