using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hasher
{
    public class Helpers
    {
        public static string GetInfo<T>(KeyValuePair<T, List<FileInfo>> sizeFile, bool full = false)
        {
            if (!full)
                return $"{sizeFile.Key} => {sizeFile.Value.Count} ({sizeFile.Value[0].FullName})";
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Hash = {sizeFile.Key}");
                foreach (var file in sizeFile.Value)
                {
                    sb.AppendLine($"{file.FullName}");
                }
                return sb.ToString();
            }
        }

        public static string GetInfos<T>(IDictionary<T, List<FileInfo>> xxxFiles, bool full = false)
        {
            var sb = new StringBuilder();
            foreach (var xxxFile in xxxFiles)
            {
                sb.AppendLine(Helpers.GetInfo(xxxFile));
            }
            return sb.ToString();
        }

        public static void AddFileInfo<T>(IDictionary<T, List<FileInfo>> xxxFiles, T key, FileInfo fileInfo)
        {
            if (xxxFiles.ContainsKey(key))
                xxxFiles[key].Add(fileInfo);
            else
                xxxFiles.Add(key, new List<FileInfo>() { fileInfo });
        }
    }
}
