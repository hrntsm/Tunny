using System.IO;
using System.Reflection;

namespace Optuna.Util
{
    public static class ReadFileFromResource
    {
        public static string Text(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return Text(assembly, resourceName);
        }

        public static string Text(Assembly assembly, string resourceName)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
