using System.IO;
using System.Reflection;

namespace Tunny.Core.Util
{
    public static class ReadFileFromResource
    {
        public static string Text(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
