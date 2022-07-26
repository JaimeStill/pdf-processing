using Pdf.Models;

namespace Pdf.Processor;
internal abstract class ManagerBase : IManager, IDisposable
{
    protected readonly string filepath;

    public ManagerBase(string src)
    {
        filepath = string.IsNullOrWhiteSpace(src)
            ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "ssn.pdf")
            : src;
    }

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public abstract void ReadFields();
    public abstract void UpdateFieldName(string old, string update);
}