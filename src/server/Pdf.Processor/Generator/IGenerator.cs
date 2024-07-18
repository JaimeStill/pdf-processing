using Pdf.Models;

namespace Pdf.Processor.Generator;
public interface IGenerator
{
    Task Generate<T>(Record<T> record, string src, string dest);
}