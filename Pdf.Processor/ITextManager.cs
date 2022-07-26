using iText.Forms;
using iText.Kernel.Pdf;

namespace Pdf.Processor;
internal class ITextManager : ManagerBase
{
    readonly PdfDocument doc;
    public ITextManager(string src) : base(src)
    {
        doc = new PdfDocument(new PdfReader(filepath));
    }

    public override void Dispose()
    {
        doc.Close();
        base.Dispose();
    }

    public override void ReadFields() =>
        PdfAcroForm.GetAcroForm(doc, false)
            .GetFormFields()
            .ToList()
            .ForEach(x => Console.WriteLine($"{x.Key} - {x.Value.GetValueAsString()} - {x.Value.GetType()}"));

    public override void UpdateFieldName(string old, string update)
    {
        PdfAcroForm.GetAcroForm(doc, false)
            .GetField(old)
            .SetFieldName(update);
    }
}