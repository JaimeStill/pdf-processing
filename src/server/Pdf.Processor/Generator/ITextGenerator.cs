using iText.Forms;
using iText.Forms.Fields;
using iText.Kernel.Pdf;

namespace Pdf.Processor.Generator;
public class ITextGenerator() : IGenerator
{
    public Task Generate<T>(PdfRecord<T> record, string src, string dest) => Task.Run(() => 
    {
        using var doc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
        SetProperties(doc, record);
        doc.Close();
    });

    static void SetField(PdfAcroForm form, string field, string value) =>
        form.GetField(field)
            .SetValue(value);

    static IEnumerable<KeyValuePair<string, PdfFormField>> GetFormMatches(PdfRecordProp prop, IDictionary<string, PdfFormField> fields) =>
        fields.Where(x => x.Key.Contains(prop.Map));

    static void SetProperties<T>(PdfDocument doc, PdfRecord<T> record)
    {
        var form = PdfAcroForm.GetAcroForm(doc, false);
        var fields = form.GetAllFormFields();

        foreach (var prop in record.Properties)
        {
            var matches = GetFormMatches(prop, fields);

            if (matches.Count() > 1)
                ProcessMultiple(form, prop, matches.Skip(1));
            else if (matches.Any())
                ProcessProperty(form, prop, matches.First());
        }
    }

    static void ProcessProperty(PdfAcroForm form, PdfRecordProp prop, KeyValuePair<string, PdfFormField> match) =>
        SetField(form, match.Key, prop.Value);

    static void ProcessMultiple(PdfAcroForm form, PdfRecordProp prop, IEnumerable<KeyValuePair<string, PdfFormField>> matches)
    {
        var type = matches.First().Key.Split('.').Last();
        var isSeries = int.TryParse(type, out int index);

        if (isSeries)
            ProcessSeries(form, prop, matches, index);
        else
            ProcessBoolean(form, prop, matches);
 
    }

    static void ProcessSeries(PdfAcroForm form, PdfRecordProp prop, IEnumerable<KeyValuePair<string, PdfFormField>> matches, int index)
    {
        foreach (var m in matches.OrderBy(x => x.Key))
        {
            index = int.Parse(m.Key.Split('.').Last());
            SetField(form, m.Key, prop.Value.ElementAt(index).ToString());
        }
    }

    static void ProcessBoolean(PdfAcroForm form, PdfRecordProp prop, IEnumerable<KeyValuePair<string, PdfFormField>> matches)
    {
        var match = matches.FirstOrDefault(m =>
            m.Key
                .Split('.')
                .Last()
                .ToLower() == prop.Value.ToLower()
        );

        if (match.Key is not null)
            SetField(form, match.Key, "Yes");
    }
}