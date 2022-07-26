using System.Reflection;

namespace Pdf.Models;
public class Record<T>
{
    public string Name { get; set; }
    public IEnumerable<RecordProp> Properties { get; set; }

    public Record(T data, string name = "ssn")
    {
        Name = name;
        Properties = GenerateProps(data);
    }

    static IEnumerable<RecordProp> GenerateProps(T data)
    {
        Type type = data.GetType();
        PropertyInfo[] properties = type.GetProperties();
        List<RecordProp> props = new();

        return properties.Select(prop => new RecordProp
        {
            Key = prop.Name,
            Map = $"{type.Name}.{prop.Name}",
            Value = GetStringValue(prop, data)
        });
    }

    static string GetStringValue(PropertyInfo prop, T data) => data switch
    {
        "DateTime" => ((DateTime)prop.GetValue(data)).ToString("MM-dd-yyyy"),
        "Boolean" => (bool)prop.GetValue(data) ? "Yes" : "No",
        _ => prop.GetValue(data).ToString()
    };
}