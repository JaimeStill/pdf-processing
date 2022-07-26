using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;
using Pdf.Models;

namespace Pdf.Processor;
internal class ITextGenerator : GeneratorBase
{
    public ITextGenerator(string src = "ssn.pdf", string dest = "itext-ssn.pdf")
        : base(src, dest) { }

    public override Task Generate<T>(Record<T> record) => Task.Run(() => 
    {
        using var doc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
        SetProperties(doc, record);
        doc.Close();
    });

    static void SetField(PdfAcroForm form, string field, string value) =>
        form.GetField(field)
            .SetValue(value);

    static void SetField(PdfAcroForm form, string field, string value, bool generateAppearance) =>
        form.GetField(field)
            .SetValue(value, generateAppearance);

    static IEnumerable<KeyValuePair<string, PdfFormField>> GetFormMatches(RecordProp prop, IDictionary<string, PdfFormField> fields) =>
        fields.Where(x => x.Key.Contains(prop.Map));

    static void SetProperties<T>(PdfDocument doc, Record<T> record)
    {
        var form = PdfAcroForm.GetAcroForm(doc, false);
        var fields = form.GetFormFields();

        foreach (var prop in record.Properties)
        {
            var matches = GetFormMatches(prop, fields);

            if (matches.Count() > 1)
                ProcessMultiple(form, prop, matches.Skip(1));
            else if (matches.Any())
                ProcessProperty(form, prop, matches.First());
        }
    }

    static void ProcessProperty(PdfAcroForm form, RecordProp prop, KeyValuePair<string, PdfFormField> match) =>
        SetField(form, match.Key, prop.Value);

    static void ProcessMultiple(PdfAcroForm form, RecordProp prop, IEnumerable<KeyValuePair<string, PdfFormField>> matches)
    {
        var type = matches.First().Key.Split('.').Last();
        var isSeries = int.TryParse(type, out int index);

        if (isSeries)
            ProcessSeries(form, prop, matches, index);
        else
            ProcessBoolean(form, prop, matches);
 
    }

    static void ProcessSeries(PdfAcroForm form, RecordProp prop, IEnumerable<KeyValuePair<string, PdfFormField>> matches, int index)
    {
        foreach (var m in matches.OrderBy(x => x.Key))
        {
            index = int.Parse(m.Key.Split('.').Last());
            SetField(form, m.Key, prop.Value.ElementAt(index).ToString());
        }
    }

    static void ProcessBoolean(PdfAcroForm form, RecordProp prop, IEnumerable<KeyValuePair<string, PdfFormField>> matches)
    {
        var match = matches.FirstOrDefault(m =>
            m.Key
                .Split('.')
                .Last()
                .ToLower() == prop.Value.ToLower()
        );

        if (match.Key is not null)
            SetField(form, match.Key, "Yes", true);
    }
}