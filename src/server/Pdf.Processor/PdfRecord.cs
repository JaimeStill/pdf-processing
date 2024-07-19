using System.Reflection;

namespace Pdf.Processor;
public class PdfRecord<T>(T data, string name = "ssn")
{
    public string Name { get; set; } = name;
    public IEnumerable<PdfRecordProp> Properties { get; set; } = GenerateProps(data);

    static IEnumerable<PdfRecordProp> GenerateProps(T data)
    {
        Type type = data.GetType();
        PropertyInfo[] properties = type.GetProperties();
        List<PdfRecordProp> props = [];

        return properties.Select(prop => new PdfRecordProp
        {
            Key = prop.Name,
            Map = $"{type.Name}.{prop.Name}",
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