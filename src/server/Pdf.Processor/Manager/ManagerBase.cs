using Pdf.Models;

namespace Pdf.Processor.Manager;
public abstract class ManagerBase(string src) : IManager, IDisposable
{
    protected readonly string filepath = string.IsNullOrWhiteSpace(src)
            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "ssn.pdf")
            : src;

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public abstract void ReadFields();
    public abstract void UpdateFieldName(string old, string update);
}