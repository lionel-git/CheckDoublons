using System;
using System.Collections.Generic;
using System.IO;
using Hasher;

namespace CheckDoublons
{
    class Program
    {
        static void HashMd5(List<FileInfo> fileInfos)
        {
            var md5 = new Md5();
            md5.Md5Hash(fileInfos);            
            md5.CheckDoublons(x => RateFile(x) );
        }

        static void RateFile(List<FileInfo> fileInfos)
        {
            var rateFile = new RateFile();            
            rateFile.Rate(fileInfos);
            rateFile.MoveDoublons();
        }

        static void Main(string[] args)
        {
            string path = @"c:\tmp";
            var scanner = new Scanner();
            scanner.ScanDirectory(path, "");
            scanner.CheckDoublons(x => { HashMd5(x); });
        }
    }
}
