using System.Reflection;

namespace Pdf.Processor;
public class PdfRecord<T>(T data, string name = "ssn")
{
    public string Name { get; set; } = name;
    public IEnumerable<PdfRecordProp> Properties { get; set; } = GenerateProps(data);

    static IEnumerable<PdfRecordProp> GenerateProps(T data)
    {
        return data
            .GetType()
            .GetProperties()
            .Select(prop => new PdfRecordProp
            {
                Key = prop.Name,
                Map = $"{typeof(T).Name}.{prop.Name}",
                Value = GetStringValue(prop, data)
            });
    }

    static string GetStringValue(PropertyInfo prop, T data) => data switch
    {
        DateOnly date => date.ToString("MM-dd-yyyy"),
        bool flag => flag ? "Yes" : "No",
        _ => prop.GetValue(data).ToString()
    };
}