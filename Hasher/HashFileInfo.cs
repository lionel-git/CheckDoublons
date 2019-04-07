using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hasher
{
    public class HashFileInfo<T>
    {
        private SortedDictionary<T, List<FileInfo>> _hashFiles;

        private string _keyName;

        public HashFileInfo(string keyName = "hash")
        {
            _hashFiles = new SortedDictionary<T, List<FileInfo>>();
            _keyName = keyName;
        }

        private void Add(T key, FileInfo fileInfo)
        {
            if (_hashFiles.ContainsKey(key))
                _hashFiles[key].Add(fileInfo);
            else
                _hashFiles.Add(key, new List<FileInfo>() { fileInfo });
        }

        public string ToString(KeyValuePair<T, List<FileInfo>> hashFile, bool full = false)
        {
            if (!full)
                return $"{hashFile.Key} => {hashFile.Value.Count} ({hashFile.Value[0].FullName})";
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine($"{_keyName} = {hashFile.Key}");
                foreach (var file in hashFile.Value)
                {
                    sb.AppendLine($"{file.FullName}");
                }
                return sb.ToString();
            }
        }


        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool full)
        {
            var sb = new StringBuilder();
            foreach (var hashFile in _hashFiles)
            {
                sb.AppendLine(ToString(hashFile, full));
            }
            return sb.ToString();
        }

        public void ScanDirectory(string path, string filter, Func<FileInfo, T> selector)
        {
            var files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                Add(selector(fileInfo), fileInfo);
            }
        }

        public void Md5Hash(List<FileInfo> fileInfos, Func<Guid, T> converter)
        {
            foreach (var fileInfo in fileInfos)
            {
                using (var md5 = MD5.Create())
                {
                    var guid = new Guid(md5.ComputeHash(File.ReadAllBytes(fileInfo.FullName)));
                    Add(converter(guid), fileInfo);
                }
            }
        }

        public void RateFile(List<FileInfo> fileInfos, Func<int, T> converter)
        {
            foreach (var fileInfo in fileInfos)
            {
                int rate = 1;
                Add(converter(rate), fileInfo);
            }
        }

        public void MoveDoublons()
        {
            Console.WriteLine("Move doublons:");
            Console.WriteLine(ToString(true));
        }

        public void CheckDoublons(Action<List<FileInfo>> treatDoublons)
        {
            foreach (var hashFile in _hashFiles)
            {
                if (hashFile.Value.Count > 1)
                {
                    Console.WriteLine($"{_keyName} conflict for: {ToString(hashFile, true)}");
                    treatDoublons(hashFile.Value);
                }
            }
        }
    }
}
