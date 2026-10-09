using System.Collections.Concurrent;
using System.IO;

namespace VoltigeCore.Business.Logic.Excel.OpenXml
{
    public static class TemplateCache
    {
        private static readonly ConcurrentDictionary<string, byte[]> _cache =
            new(System.StringComparer.OrdinalIgnoreCase);

        public static byte[] GetBytes(string templatePath)
        {
            return _cache.GetOrAdd(templatePath, p => File.ReadAllBytes(p));
        }
    }
}
