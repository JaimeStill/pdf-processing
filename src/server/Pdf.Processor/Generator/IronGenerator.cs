using IronPdf;
using IronSoftware;
using IronSoftware.Forms;

namespace Pdf.Processor.Generator;
public class IronGenerator : IGenerator
{
    public Task Generate<T>(PdfRecord<T> record, string src, string dest) => Task.Run(() =>
    {
        using var doc = PdfDocument.FromFile(src);
        SetProperties(doc, record);
        doc.SaveAs(dest);
    });

    static void SetField(FormFieldCollection form, string field, string value) =>
        form.FindFormField(field).Value = value;

    static IEnumerable<IFormField> GetFormMatches(PdfRecordProp prop, FormFieldCollection fields) =>
        fields.Where(x => x.Name.Contains(prop.Map));

    static void SetProperties<T>(PdfDocument doc, PdfRecord<T> record)
    {
        foreach (var prop in record.Properties)
        {
            var matches = GetFormMatches(prop, doc.Form);

            if (matches.Count() > 1)
                ProcessMultiple(doc, prop, matches);
            else if (matches.Any())
                ProcessProperty(doc, prop, matches.First());
        }
    }

    static void ProcessProperty(PdfDocument doc, PdfRecordProp prop, IFormField match) =>
        SetField(doc.Form, match.Name, prop.Value);

    static void ProcessMultiple(PdfDocument doc, PdfRecordProp prop, IEnumerable<IFormField> matches)
    {
        var type = matches.First().Name.Split('.').Last();
        var isSeries = int.TryParse(type, out int index);

        if (isSeries)
            ProcessSeries(doc, prop, matches, index);
        else
            ProcessBoolean(doc, prop, matches);
    }

    static void ProcessSeries(PdfDocument doc, PdfRecordProp prop, IEnumerable<IFormField> matches, int index)
    {
        foreach (var m in matches.OrderBy(x => x.Name))
        {
            index = int.Parse(m.Name.Split('.').Last());
            SetField(doc.Form, m.Name, prop.Value.ElementAt(index).ToString());
        }
    }

    static void ProcessBoolean(PdfDocument doc, PdfRecordProp prop, IEnumerable<IFormField> matches)
    {
        var match = matches.FirstOrDefault(m =>
            m.Name
                .Split('.')
                .Last()
                .ToLower() == prop.Value.ToLower()
        );

        if (match?.Name is not null)
            SetField(doc.Form, match.Name, "Yes");
    }
}