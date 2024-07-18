namespace Pdf.Models;
public interface IManager : IDisposable
{
    void ReadFields();
    void UpdateFieldName(string old, string update);
}