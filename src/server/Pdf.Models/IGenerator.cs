namespace Pdf.Models;
public interface IGenerator
{
    Task Generate<T>(Record<T> record);
}