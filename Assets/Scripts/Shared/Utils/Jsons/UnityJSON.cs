﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Jsons.Converters;

namespace Utils.Jsons
{
    public static class UnityJSON
    {
        private static readonly JsonSerializerSettings s_Settings = new()
        {
            Converters = new JsonConverter[]
            {
                new ColorConverter(),
                new StringEnumConverter()
            },
            Formatting = Formatting.Indented
        };

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, s_Settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, s_Settings);
        }
    }
}