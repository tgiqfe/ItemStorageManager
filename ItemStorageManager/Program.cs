using ItemStorageManager.ItemStorage;
using System.Text.Json;
using System.Text.Json.Serialization;

var item1 = new FileItem(@"D:\Test\Images\Image_0002.jpg");
string json1 = JsonSerializer.Serialize(item1,
    new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    });
Console.WriteLine(json1);

var item2 = new DirectoryItem(@"D:\Test\Images2");
string json2 = JsonSerializer.Serialize(item2,
    new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    });
Console.WriteLine(json2);

var item3 = new RegistryKeyItem(@"HKEY_CURRENT_USER\Software\Test\Test01");
string json3 = JsonSerializer.Serialize(item3,
    new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    });
Console.WriteLine(json3);

var item4 = new RegistryValueItem(@"HKEY_CURRENT_USER\Software\Test\Test02", "bbbb");
string json4 = JsonSerializer.Serialize(item4,
    new JsonSerializerOptions
    {
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    });
Console.WriteLine(json4);



Console.ReadLine();

