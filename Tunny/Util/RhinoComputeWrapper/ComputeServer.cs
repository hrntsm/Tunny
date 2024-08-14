using System;
using System.IO;

using Newtonsoft.Json;

namespace Tunny.Util.RhinoComputeWrapper
{
    public static class ComputeServer
    {
        public static string WebAddress { get; set; } = "http://localhost:6500";
        public static string AuthToken { get; set; }
        public static string ApiKey { get; set; }
        public static string Version => "0.12.0";

        public static T Post<T>(string function, params object[] postData)
        {
            return PostWithConverter<T>(function, null, postData);
        }

        public static T PostWithConverter<T>(string function, JsonConverter converter, params object[] postData)
        {
            if (string.IsNullOrWhiteSpace(AuthToken) && WebAddress.Equals("https://compute.rhino3d.com", StringComparison.Ordinal))
                throw new UnauthorizedAccessException("AuthToken must be set for compute.rhino3d.com");

            for (int i = 0; i < postData.Length; i++)
            {
                if (postData[i] != null &&
                    postData[i].GetType().IsGenericType &&
                    postData[i].GetType().GetGenericTypeDefinition() == typeof(Remote<>))
                {
                    System.Reflection.MethodInfo mi = postData[i].GetType().GetMethod("JsonObject");
                    postData[i] = mi.Invoke(postData[i], null);
                }
            }

            string json = converter == null ?
                JsonConvert.SerializeObject(postData, Formatting.None) :
                JsonConvert.SerializeObject(postData, Formatting.None, converter);
            System.Net.WebResponse response = DoPost(function, json);
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();
                return converter == null ? JsonConvert.DeserializeObject<T>(result) : JsonConvert.DeserializeObject<T>(result, converter);
            }
        }

        public static T0 Post<T0, T1>(string function, out T1 out1, params object[] postData)
        {
            if (string.IsNullOrWhiteSpace(AuthToken) && WebAddress.Equals("https://compute.rhino3d.com", StringComparison.Ordinal))
                throw new UnauthorizedAccessException("AuthToken must be set for compute.rhino3d.com");

            string json = JsonConvert.SerializeObject(postData);
            System.Net.WebResponse response = DoPost(function, json);
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string jsonString = streamReader.ReadToEnd();
                object data = JsonConvert.DeserializeObject(jsonString);
                var ja = data as Newtonsoft.Json.Linq.JArray;
                out1 = ja[1].ToObject<T1>();
                return ja[0].ToObject<T0>();
            }
        }

        public static T0 Post<T0, T1, T2>(string function, out T1 out1, out T2 out2, params object[] postData)
        {
            if (string.IsNullOrWhiteSpace(AuthToken) && WebAddress.Equals("https://compute.rhino3d.com", StringComparison.Ordinal))
                throw new UnauthorizedAccessException("AuthToken must be set for compute.rhino3d.com");

            string json = JsonConvert.SerializeObject(postData);
            System.Net.WebResponse response = DoPost(function, json);
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string jsonString = streamReader.ReadToEnd();
                object data = JsonConvert.DeserializeObject(jsonString);
                var ja = data as Newtonsoft.Json.Linq.JArray;
                out1 = ja[1].ToObject<T1>();
                out2 = ja[2].ToObject<T2>();
                return ja[0].ToObject<T0>();
            }
        }

        // run all requests through here
        private static System.Net.WebResponse DoPost(string function, string json)
        {

            if (!function.StartsWith("/")) // add leading /
                function = "/" + function; // if not present

            string uri = $"{WebAddress}{function}".ToLower();
            var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(uri);
            request.ContentType = "application/json";
            request.UserAgent = $"compute.rhino3d.cs/{Version}";
            request.Method = "POST";

            // try auth token (compute.rhino3d.com only)
            if (!string.IsNullOrWhiteSpace(AuthToken))
                request.Headers.Add("Authorization", "Bearer " + AuthToken);

            // try api key (self-hosted compute)
            if (!string.IsNullOrWhiteSpace(ApiKey))
                request.Headers.Add("RhinoComputeKey", ApiKey);

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            return request.GetResponse();
        }

        public static string ApiAddress(System.Type t, string function)
        {
            string s = t.ToString().Replace('.', '/');
            return s + "/" + function;
        }
    }
}
