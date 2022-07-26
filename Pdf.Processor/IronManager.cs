using IronPdf;

namespace Pdf.Processor;
internal class IronManager : ManagerBase
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
           .Fields
           .ToList()
           .ForEach(x => Console.WriteLine($"{x.Type} - {x.Name} - {x.Value} - {x.AnnotationIndex}"));

    public override void UpdateFieldName(string old, string update)
    {
        doc.Form
           .RenameField(old, update);
        
        doc.SaveAs(filepath);
    }
}