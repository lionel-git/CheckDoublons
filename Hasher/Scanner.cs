using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hasher
{
    public class Scanner : HashFileInfo<long>
    {
        public Scanner() : base("Size")
        {
        }


        public void ScanDirectory(string path, string filter)
        {
            var files = Directory.GetFiles(path, filter, SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                Add(fileInfo.Length, fileInfo);
            }
        }
    }
}
