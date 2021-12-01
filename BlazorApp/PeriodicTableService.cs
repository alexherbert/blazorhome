using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlazorApp;

public class PeriodicTableService : IPeriodicTableService
{
    public Task<IEnumerable<Element>> GetElements()
    {
        return GetElements(string.Empty);
    }

    public async Task<IEnumerable<Element>> GetElements(string search = "")
    {
        var elements = new List<Element>();
        var key = GetResourceKey(typeof(PeriodicTableService).Assembly, "Elements.json");
        using var stream = typeof(PeriodicTableService).Assembly.GetManifestResourceStream(key);
        var table = await JsonSerializer.DeserializeAsync<Table>(stream,
            new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
        foreach (var elementGroup in table.ElementGroups)
        {
            elements = elements.Concat(elementGroup.Elements).ToList();
        }

        if (search == string.Empty)
            return elements;
        else
            return elements.Where(elm =>
                (elm.Sign + elm.Name).Contains(search, StringComparison.InvariantCultureIgnoreCase));
    }

    public static string GetResourceKey(Assembly assembly, string embeddedFile)
    {
        return assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains(embeddedFile));
    }
}

public interface IPeriodicTableService
{
    Task<IEnumerable<Element>> GetElements();
    Task<IEnumerable<Element>> GetElements(string search = "");
}

public class Table
{
    [JsonPropertyName("table")] public IList<ElementGroup> ElementGroups { get; set; }
}

public class ElementGroup
{
    public string Wiki { get; set; }
    public IList<Element> Elements { get; set; }
}