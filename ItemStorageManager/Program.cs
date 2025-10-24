using ItemStorageManager.ItemStorage;
using System.Text.Json;
using System.Text.Json.Serialization;

var item = new FileItem(@"D:\Test\Test01\Image001.png");
string json = JsonSerializer.Serialize(item,
    new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    });
Console.WriteLine(json);


Console.ReadLine();

