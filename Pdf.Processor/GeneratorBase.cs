using Pdf.Models;

namespace Pdf.Processor;
internal abstract class GeneratorBase : IGenerator
{
    protected readonly string src;
    protected readonly string dest;
    protected static string FindPdf(string source) =>
        Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "files",
            source
        );

    public GeneratorBase(string src, string dest)
    {
        this.src = FindPdf(src);
        this.dest = dest;
    }

    public abstract Task Generate<T>(Record<T> record);
}