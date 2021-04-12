using System;
using FluentAssertions;
using NUnit.Framework;

namespace CSharpier.Tests
{
    public class DocPrinterTests
    {
        private static string NewLine = System.Environment.NewLine;

        [Test]
        public void Basic_Concat()
        {
            var doc = Docs.Concat("1", "2", "3");

            var result = this.Print(doc);

            result.Should().Be("123");
        }

        [Test]
        public void Concat_With_Hardline()
        {
            var doc = Docs.Concat("1", Docs.HardLine, "3");

            var result = this.Print(doc);

            result.Should().Be($"1{NewLine}3");
        }

        [Test]
        public void Concat_With_Line()
        {
            var doc = Docs.Concat("1", Docs.Line, "3");

            var result = this.Print(doc);

            result.Should().Be($"1{NewLine}3");
        }

        [Test]
        public void Group_With_Line()
        {
            var doc = Docs.Group(Docs.Concat("1", Docs.Line, "3"));

            var result = this.Print(doc);

            result.Should().Be("1 3");
        }

        [Test]
        public void Group_With_HardLine()
        {
            var doc = Docs.Group(Docs.Concat("1", Docs.HardLine, "3"));

            var result = this.Print(doc);

            result.Should().Be($"1{NewLine}3");
        }

        [Test]
        public void Group_With_Line_And_Hardline()
        {
            var doc = Docs.Group(
                Docs.Concat("1", Docs.Line, "2", Docs.HardLine, "3")
            );

            var result = this.Print(doc);

            result.Should().Be($"1{NewLine}2{NewLine}3");
        }

        [Test]
        public void Group_With_Line_And_BreakParent()
        {
            var doc = Docs.Group(
                Docs.Concat(
                    "1",
                    Docs.Line,
                    "2",
                    Docs.Line,
                    "3",
                    Docs.BreakParent
                )
            );

            var result = this.Print(doc);

            result.Should().Be($"1{NewLine}2{NewLine}3");
        }

        [Test]
        public void Indent_With_BreakParent()
        {
            var doc = Docs.Concat(
                "0",
                Docs.Group(
                    Docs.Indent(
                        Docs.Concat(
                            Docs.SoftLine,
                            "1",
                            Docs.Line,
                            "2",
                            Docs.Line,
                            "3",
                            Docs.BreakParent
                        )
                    )
                )
            );

            var result = this.Print(doc);

            result.Should().Be($"0{NewLine}    1{NewLine}    2{NewLine}    3");
        }

        [Test]
        public void Large_Group_Concat_With_Line()
        {
            const string longText = "LongTextLongTextLongTextLongText";
            var doc = Docs.Group(
                Docs.Concat(longText, Docs.Line, longText, Docs.Line, longText)
            );

            var result = this.Print(doc);

            result.Should()
                .Be($"{longText}{NewLine}{longText}{NewLine}{longText}");
        }

        [Test]
        public void Indent_With_Hardline()
        {
            var doc = Docs.Concat(
                "0",
                Docs.Indent(Docs.Concat(Docs.HardLine, "1", Docs.HardLine, "2"))
            );

            var result = this.Print(doc);

            result.Should().Be($"0{NewLine}    1{NewLine}    2");
        }

        [Test]
        public void Two_Indents_With_Hardline()
        {
            var doc = Docs.Concat(
                "0",
                Docs.Concat(
                    Docs.Indent(Docs.Concat(Docs.HardLine, "1")),
                    Docs.HardLine,
                    Docs.Indent(Docs.Concat(Docs.HardLine, "2"))
                )
            );

            var result = this.Print(doc);

            result.Should().Be($"0{NewLine}    1{NewLine}{NewLine}    2");
        }

        [Test]
        public void Lines_Removed_From_Beginning()
        {
            var doc = Docs.Concat(Docs.HardLine, "1");

            var result = this.Print(doc);

            result.Should().Be($"1");
        }

        [Test]
        public void Literal_Lines_Removed_From_Beginning()
        {
            var doc = Docs.Concat(Docs.LiteralLine, "1");

            var result = this.Print(doc);

            result.Should().Be($"1");
        }

        [Test]
        public void ForceFlat_Prevents_Breaking()
        {
            var doc = Docs.ForceFlat("1", Docs.HardLine, "2");

            var result = this.Print(doc);

            result.Should().Be("1 2");
        }

        [Test]
        public void LiteralLine_Trims_Space()
        {
            var doc = Docs.Concat(
                "{",
                Docs.Indent(Docs.HardLine, "indent", Docs.LiteralLine),
                "}"
            );

            var result = this.Print(doc);

            result.Should().Be($"{{{NewLine}    indent{NewLine}}}");
        }

        [Test]
        public void HardLine_LiteralLine_Skips_HardLine_And_Trims()
        {
            var doc = Docs.Concat(
                "{",
                Docs.Indent(Docs.HardLine, Docs.LiteralLine, "noindent"),
                Docs.HardLine,
                "}"
            );

            var result = this.Print(doc);

            result.Should().Be($"{{{NewLine}noindent{NewLine}}}");
        }

        [Test]
        public void HardLine_LiteralLine_Skips_HardLine()
        {
            var doc = Docs.Concat("1", Docs.HardLine, Docs.LiteralLine, "2");

            var result = this.Print(doc);

            result.Should().Be($"1{NewLine}2");
        }

        [Test]
        public void ForceFlat_Prevents_Breaking_With_Long_Content()
        {
            var doc = Docs.ForceFlat(
                "lkjasdkfljalsjkdfkjlasdfjklakljsdfjkasdfkljsdafjk",
                Docs.Line,
                "jaksdlflkasdlfjkajklsdfkljasfjklaslfkjasdfkj"
            );

            var result = this.Print(doc);

            result.Should()
                .Be(
                    $"lkjasdkfljalsjkdfkjlasdfjklakljsdfjkasdfkljsdafjk jaksdlflkasdlfjkajklsdfkljasfjklaslfkjasdfkj"
                );
        }

        [Test]
        public void Long_Statement_With_Line_Should_Not_Break_Unrelated_Group() {
            var doc = Docs.Concat(
                "1",
                Docs.Group(Docs.Line, Docs.Concat("2")),
                Docs.HardLine,
                Docs.Concat(
                    "1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111",
                    Docs.Line,
                    "2"
                )
            );
            var result = this.Print(doc);
            result.Should()
                .Be(
                    $"1 2{NewLine}1111111111111111111111111111111111111111111111111111111111111111111111111111111111111111{NewLine}2"
                );
        }

        [Test]
        public void ForceFlat_Should_Be_Included_In_Fits_Logic_Of_Printer()
        {
            var doc = Docs.Group(
                Docs.ForceFlat(
                    Docs.Concat(
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111"
                    )
                ),
                Docs.Line,
                Docs.ForceFlat(
                    Docs.Concat(
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111",
                        " ",
                        "1111111111"
                    )
                )
            );

            var result = this.Print(doc);
            result.Should()
                .Be(
                    $"1111111111 1111111111 1111111111 1111111111 1111111111{NewLine}1111111111 1111111111 1111111111 1111111111 1111111111"
                );
        }

        [Test]
        public void IfBreak_Should_Print_Flat_Contents()
        {
            var doc = Docs.Group(Docs.IfBreak("break", "flat"));

            var result = this.Print(doc);

            result.Should().Be("flat");
        }

        [Test]
        public void IfBreak_Should_Print_Break_Contents()
        {
            var doc = Docs.Group(Docs.HardLine, Docs.IfBreak("break", "flat"));

            var result = this.Print(doc);

            result.Should().Be("break");
        }

        [Test]
        public void IfBreak_Should_Print_Break_Contents_When_Group_Does_Not_Fit() {
            var doc = Docs.Group(
                "another",
                Docs.Line,
                Docs.IfBreak("break", "flat")
            );

            var result = this.Print(doc, 10);

            result.Should().Be($"another{NewLine}break");
        }

        [Test]
        public void IfBreak_Should_Print_Flat_Contents_When_GroupId_Does_Not_Break() {
            var doc = Docs.Concat(
                Docs.GroupWithId("1", "1"),
                Docs.IfBreak("break", "flat", "1")
            );

            var result = this.Print(doc);

            result.Should().Be("1flat");
        }

        [Test]
        public void IfBreak_Should_Print_Break_Contents_When_GroupId_Breaks()
        {
            var doc = Docs.Concat(
                "1",
                Docs.GroupWithId("hl", Docs.HardLine),
                Docs.IfBreak("break", "flat", "hl")
            );

            var result = this.Print(doc);

            result.Should().Be($"1{NewLine}break");
        }

        [Test]
        public void Scratch()
        {
            var doc = "";
            var result = this.Print(doc);
            result.Should().Be("");
        }

        private string Print(Doc doc, int width = 80)
        {
            return DocPrinter.Print(doc, new Options { Width = width })
                .TrimEnd('\r', '\n');
        }
    }
}
