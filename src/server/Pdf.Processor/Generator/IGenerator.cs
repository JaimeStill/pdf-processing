namespace Pdf.Processor.Generator;
public interface IGenerator
{
    Task Generate<T>(PdfRecord<T> record, string src, string dest);
}