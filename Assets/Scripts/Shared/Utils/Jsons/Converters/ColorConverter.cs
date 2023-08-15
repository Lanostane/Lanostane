using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Utils.Jsons.Converters
{
    public sealed class ColorConverter : JsonConverter<Color>
    {
        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var token = JToken.ReadFrom(reader);
                var color = new Color
                {
                    r = (float)token["r"],
                    g = (float)token["g"],
                    b = (float)token["b"],
                    a = (float)token["a"]
                };
                return color;
            }
            else if (reader.TokenType == JsonToken.String)
            {
                var value = reader.ReadAsString();
                if (ColorUtility.TryParseHtmlString(value, out var color))
                {
                    return color;
                }
                else
                {
                    Debug.LogError($"Color Format isn't correct... \"{value}\"");
                    return default;
                }
            }
            else
            {
                Debug.LogError($"Unsupported Token Type: {reader.TokenType}");
                return default;
            }
        }

        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("r");
            writer.WriteValue(value.r);
            writer.WritePropertyName("g");
            writer.WriteValue(value.g);
            writer.WritePropertyName("b");
            writer.WriteValue(value.b);
            writer.WritePropertyName("a");
            writer.WriteValue(value.a);
            writer.WriteEndObject();
        }
    }
}
