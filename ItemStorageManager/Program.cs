using ItemStorageManager.ItemStorage;
using System.Text.Json;
using System.Text.Json.Serialization;

var item = new FileItem(@"D:\Test\Images\Image_0002.jpg");
string json = JsonSerializer.Serialize(item,
    new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    });
Console.WriteLine(json);


Console.ReadLine();

