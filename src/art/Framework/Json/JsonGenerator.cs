//..............................
// UI Lab Inc. Arthur Amshukov .
//..............................
using System.Security.Cryptography;
using Newtonsoft.Json;
using UILab.Art.Framework.Core.Text;

namespace Art.Framework.Json;

public sealed class JsonGenerator
{
    public enum DataType
    {
        Unknown,
        Boolean = 1,
        Integer,
        Double,
        String,
        Array,
        Object
    }

    private static readonly Random Random = new();

    private int MaxStringLength { get; init; }

    private int MaxArrayLength { get; init; }

    private int MaxObjectProperties { get; init; }

    private int MaxObjectPropertyLength { get; init; }

    private int MaxDepth { get; init; }

    private double DoubleMinValue { get; init; }

    private double DoubleMaxValue { get; init; }


    public JsonGenerator(int maxStringLength = 64,
                         int maxArrayLength = 16,
                         int maxObjectProperties = 8,
                         int maxObjectPropertyLength = 12,
                         int maxDepth = 16,
                         double doubleMinValue = 1.0e+10,
                         double doubleMaxValue = 1.0e-10)
    {
        MaxStringLength = maxStringLength;
        MaxArrayLength = maxArrayLength;
        MaxObjectProperties = maxObjectProperties;
        MaxObjectPropertyLength = maxObjectPropertyLength;
        MaxDepth = maxDepth;

        DoubleMinValue = doubleMinValue;
        DoubleMaxValue = doubleMaxValue;
    }

    public string GenerateRandomJson(DataType dataType = DataType.Unknown)
    {
        var jsonObject = GenerateRandomJson(MaxDepth, dataType);
        var jsonText = Serialize(jsonObject ?? JsonPatchBuilder.EmptyDocument);
        return jsonText;
    }

    private object? GenerateRandomJson(int maxDepth, DataType dataType = DataType.Unknown)
    {
        if(maxDepth < 0)
            return default;

        dataType = dataType == DataType.Unknown ? GenerateRandomDataType() : dataType;

        if(maxDepth == 0)
        {
            int choice = RandomNumberGenerator.GetInt32(1, int.MaxValue - 1);

            if((choice % 3) == 0)
                dataType = DataType.Boolean;
            else if((choice % 5) == 0)
                dataType = DataType.Integer;
            else if((choice % 7) == 0)
                dataType = DataType.Double;
            else if((choice % 9) == 0)
                dataType = DataType.String;
        }

        object? jsonObject = dataType switch
        {
            DataType.Boolean => GenerateRandomBoolean(),
            DataType.Integer => GenerateRandomInteger(),
            DataType.Double => GenerateRandomDouble(),
            DataType.String => GenerateRandomString(),
            DataType.Array => GenerateRandomArray(maxDepth),
            DataType.Object => GenerateRandomObject(maxDepth),
            _ => JsonPatchBuilder.EmptyDocument
        };

        return jsonObject;
    }

    private static string GenerateRandomBoolean()
    {
        var result = RandomNumberGenerator.GetInt32(0, int.MaxValue - 1) % 2 == 0 ? "true" : "false";
        return result;
    }

    private static int GenerateRandomInteger()
    {
        int result = RandomNumberGenerator.GetInt32(0, int.MaxValue - 1);

        if(RandomNumberGenerator.GetInt32(0, int.MaxValue - 1) % 2 == 0) // negative?
            result = -result;

        return result;
    }

    private double GenerateRandomDouble()
    {
        double nextRandom = Random.NextDouble();
        double minValue = DoubleMinValue;
        double maxValue = DoubleMaxValue;

        var result = minValue + (nextRandom * (maxValue - minValue));

        if(RandomNumberGenerator.GetInt32(0, int.MaxValue - 1) % 2 == 0) // negative?
            result = -result;

        return result;
    }

    private string GenerateRandomString()
    {
        var result = Text.GetRandomText(RandomNumberGenerator.GetInt32(1, MaxStringLength));
        return result;
    }

    private object? GenerateRandomArray(int maxDepth)
    {
        if(maxDepth == 0)
            return default;

        int length = RandomNumberGenerator.GetInt32(1, MaxArrayLength);

        object?[] randomArray = new object?[length];

        for(int k = 0; k < randomArray.Length; k++)
        {
            randomArray[k] = GenerateRandomJson(maxDepth - 1);
        }

        return randomArray;
    }

    private object? GenerateRandomObject(int maxDepth)
    {
        if(maxDepth == 0)
            return default;

        int count = RandomNumberGenerator.GetInt32(1, MaxObjectProperties);

        Dictionary<string, object?> randomObject = new();

        for(int k = 0; k < count; k++)
        {
            string property = Text.GetRandomText(RandomNumberGenerator.GetInt32(1, MaxObjectPropertyLength));

            if(randomObject.ContainsKey(property))
                continue;

            randomObject[property] = GenerateRandomJson(maxDepth - 1);
        }

        return randomObject;
    }

    private static DataType GenerateRandomDataType()
    {
        var enumValues = Enum.GetValues<DataType>();
        var index = RandomNumberGenerator.GetInt32(1, enumValues.Length - 1);

        if(index % 3 == 0)
        {
            return DataType.Object;
        }

        return enumValues[index];
    }

    private static string Serialize(object randomObject)
    {
        return JsonConvert.SerializeObject(randomObject,
                                           new JsonSerializerSettings
                                           {
                                               MissingMemberHandling = MissingMemberHandling.Error,
                                               NullValueHandling = NullValueHandling.Include,
                                               DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                                               Formatting = Formatting.Indented,
                                               DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                               DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind,
                                               DateParseHandling = DateParseHandling.DateTimeOffset,
                                               StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                                           });
    }
}
