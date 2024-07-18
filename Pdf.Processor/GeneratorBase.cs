using Pdf.Models;

namespace Pdf.Processor;
internal abstract class GeneratorBase(string src, string dest) : IGenerator
{
    protected readonly string src = FindPdf(src);
    protected readonly string dest = dest;
    protected static string FindPdf(string source) =>
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "files",
            source
        );

    public abstract Task Generate<T>(Record<T> record);
}