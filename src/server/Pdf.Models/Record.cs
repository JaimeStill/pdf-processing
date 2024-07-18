using System.Reflection;

namespace Pdf.Models;
public class Record<T>(T data, string name = "ssn")
{
    public string Name { get; set; } = name;
    public IEnumerable<RecordProp> Properties { get; set; } = GenerateProps(data);

    static IEnumerable<RecordProp> GenerateProps(T data)
    {
        Type type = data.GetType();
        PropertyInfo[] properties = type.GetProperties();
        List<RecordProp> props = [];

        return properties.Select(prop => new RecordProp
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