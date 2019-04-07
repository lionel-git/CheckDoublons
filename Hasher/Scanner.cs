using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hasher
{
    public class Scanner
    {
        private SortedDictionary<long, List<FileInfo>> _sizeFiles;

        public int Size => _sizeFiles.Count;

        public Scanner()
        {
            _sizeFiles = new SortedDictionary<long, List<FileInfo>>();
        }

        public void ScanDirectory(string path, string filter)
        {
            var files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                Helpers.AddFileInfo(_sizeFiles, fileInfo.Length, fileInfo);
            }
        }

        public override string ToString()
        {
            return Helpers.GetInfos(_sizeFiles);
        }

        public void CheckDoublons()
        {
            foreach (var sizeFile in _sizeFiles)
            {
                if (sizeFile.Value.Count>1)
                {
                    //Console.WriteLine($"Conflict for: {Helpers.GetInfo(sizeFile, true)}");
                    // Calculer hash des fichiers
                    var md5Hasher = new Md5Hasher();
                    md5Hasher.HashFiles(sizeFile.Value);
                    md5Hasher.CheckDoublons();
                }
            }
        }
    }
}
