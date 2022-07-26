using IronPdf;
using IronPdf.Forms;
using Pdf.Models;

namespace Pdf.Processor;
internal class IronGenerator : GeneratorBase
{
    public IronGenerator(string src = "ssn.pdf", string dest = "iron-ssn.pdf")
        : base(src, dest) { }

    public override Task Generate<T>(Record<T> record) => Task.Run(() =>
    {
        using var doc = PdfDocument.FromFile(src);
        SetProperties(doc, record);
        doc.SaveAs(dest);
    });

    static void SetField(PdfForm form, string field, string value) =>
        form.GetFieldByName(field).Value = value;

    static IEnumerable<FormField> GetFormMatches(RecordProp prop, List<FormField> fields) =>
        fields.Where(x => x.Name.Contains(prop.Map));

    static void SetProperties<T>(PdfDocument doc, Record<T> record)
    {
        foreach (var prop in record.Properties)
        {
            var matches = GetFormMatches(prop, doc.Form.Fields);

            if (matches.Count() > 1)
                ProcessMultiple(doc, prop, matches);
            else if (matches.Any())
                ProcessProperty(doc, prop, matches.First());
        }
    }

    static void ProcessProperty(PdfDocument doc, RecordProp prop, FormField match) =>
        SetField(doc.Form, match.Name, prop.Value);

    static void ProcessMultiple(PdfDocument doc, RecordProp prop, IEnumerable<FormField> matches)
    {
        var type = matches.First().Name.Split('.').Last();
        var isSeries = int.TryParse(type, out int index);

        if (isSeries)
            ProcessSeries(doc, prop, matches, index);
        else
            ProcessBoolean(doc, prop, matches);
    }

    static void ProcessSeries(PdfDocument doc, RecordProp prop, IEnumerable<FormField> matches, int index)
    {
        foreach (var m in matches.OrderBy(x => x.Name))
        {
            index = int.Parse(m.Name.Split('.').Last());
            SetField(doc.Form, m.Name, prop.Value.ElementAt(index).ToString());
        }
    }

    static void ProcessBoolean(PdfDocument doc, RecordProp prop, IEnumerable<FormField> matches)
    {
        var match = matches.FirstOrDefault(m =>
            m.Name
                .Split('.')
                .Last()
                .ToLower() == prop.Value.ToLower()
        );

        if (match.Name is not null)
            SetField(doc.Form, match.Name, "Yes");
    }
}