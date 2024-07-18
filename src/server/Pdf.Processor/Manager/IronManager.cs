using IronPdf;

namespace Pdf.Processor.Manager;
public class IronManager : ManagerBase
{
    readonly PdfDocument doc;
    public IronManager(string src) : base(src)
    {
        doc = PdfDocument.FromFile(filepath);
    }

    public override void Dispose()
    {
        doc.Dispose();
        base.Dispose();
    }

    public override void ReadFields() =>
        doc.Form
           .ToList()
           .ForEach(x => Console.WriteLine($"{x.Type} - {x.Name} - {x.Value} - {x.PageIndex}"));

    public override void UpdateFieldName(string old, string update)
    {
        doc.Form
            .FindFormField(old)
            .Name = update;

        doc.SaveAs(filepath);
    }
}