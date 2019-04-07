using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

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

        private int RateFile(FileInfo fileInfo)
        {
            int rate = 1000;
            if (Regex.Match(fileInfo.FullName, "Good").Success)
                rate = -1;
            if (Regex.Match(fileInfo.FullName, "Very.*Good").Success)
                rate = -2;
            return rate;
        }

        public void RateFile(List<FileInfo> fileInfos, Func<int, T> converter)
        {
            foreach (var fileInfo in fileInfos)
            {
                int rate = RateFile(fileInfo);
                Add(converter(rate), fileInfo);
            }
        }

        public void MoveDoublons()
        {
            Console.WriteLine("Move doublons");
            int count = 0;
            foreach (var hashFile in _hashFiles)
            {
                if (++count > 1)
                {
                    foreach (var fileInfo in hashFile.Value)
                    {
                        Console.WriteLine($"Should move {fileInfo.FullName} ({hashFile.Key})");
                    }
                }
                else
                    Console.WriteLine($"Best {_keyName} = {hashFile.Key} ({hashFile.Value.Count})");
            }
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
