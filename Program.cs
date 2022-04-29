using Newtonsoft.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

await ExampleJson.ExampleOne();

public static class ExampleJson
{
    private static readonly JsonSerializerOptions jsonOptions = new()
    {
        // https://docs.microsoft.com/ko-kr/dotnet/standard/serialization/system-text-json-character-encoding
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task ExampleOne()
    {
        JsonObject result = new();
        JsonObject jsonObjectOne = new()
        {
            ["age"] = 16,
            ["name"] = "dhddldid"
        };
        
        JsonObject jsonObjectTwo = new()
        {
            ["age"] = 13,
            ["name"] = "dhddldid2"
        };

        JsonObject jsonObjectFour = new()
        {
            ["age"] = 23,
            ["name"] = "dhddldid3"
        };

        JsonArray studentArray = new();
        studentArray.Add(new JsonObject()
        {
            ["age"] = 55,
            ["name"] = "FakeDhddldid"
        });

        // Error
        //studentArray.Add(jsonObjectOne); 
        studentArray.Add(jsonObjectTwo);
        studentArray.Add(jsonObjectFour);

        result = new()
        {
            ["responseStatus"] = new JsonObject
            {
                ["code"] = 1000,
                ["message"] = "Success"
            },
            ["Student"] = jsonObjectOne,
            ["StudentArray"] = studentArray,
        };

        jsonObjectOne["age"] = 20;
        studentArray[0]["age"] = 75;
        Console.WriteLine(result.ToJsonString(jsonOptions));

        // ResponseStatus는 멤버변수의 이름만 json의 key값과 맞춰준다면 어느 이유인지 모르겠지만 대소문자를 구별하지 않고도 잘 됨
        // 나머지는 멤버변수의 이름을 key값과 맞춰도 Deserialize가 되지 않음.
        // Deserialize를 위해서는 JsonPropertyName을 key값과 대조 대,소문자를 모두 구별
        var systemDeserialize = System.Text.Json.JsonSerializer.Deserialize<ResultReturn>(result, jsonOptions);
        
        // ResponseStatus는 잘 됨
        // 아래 항목에 대해서 대,소문자를 구별하지 않음
        // 나머지는 클래스의 멤버변수 이름을 key값과 맞추기만하면 잘 됨 
        // 멤버변수와의 json의 key값이 다르다면
        // Deserialize를 위해서는 JsonProperty key값과 대조
        var newtonSoft = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultReturn>(result.ToJsonString(jsonOptions));
    }
}

public class ResultReturn
{
    public ResponseStatus? responseStatus { get; set; }
    [JsonPropertyName("Student")]
    [JsonProperty("student")]
    public StudentInfo? StudentInfo { get; set; }
    [JsonPropertyName("student")] // 일부러 대소문자를 구별하지 않음
    [JsonProperty("studentArray")]
    public List<StudentInfo>? StudentInfos { get; set; }

}

public class ResponseStatus
{
    public int? Code { get; set; }
    public string? Message { get; set; }
}

public class StudentInfo
{
    public int? Age { get; set; }
    public string? Name { get; set; }
}

