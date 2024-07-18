using Pdf.Models;

namespace Pdf.Processor;
public static class PdfManager
{
    public static IManager CreateIronManager(string src = "") =>
        new IronManager(src);

    public static IManager CreateITextManager(string src = "") =>
        new ITextManager(src);
}