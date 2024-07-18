using Pdf.Models;

namespace Pdf.Processor;
public static class PdfGenerator
{
    public static IGenerator CreateIronGenerator(string src, string dest) =>
        new IronGenerator(src, dest);

    public static IGenerator CreateITextGenerator(string src, string dest) =>
        new ITextGenerator(src, dest);
}