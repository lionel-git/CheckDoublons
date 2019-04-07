using System;
using System.Collections.Generic;
using System.IO;
using Hasher;

namespace CheckDoublons
{
    class Program
    {
        static void Test1()
        {
            string path = @"d:\v";
            var scanner = new Scanner();
            scanner.ScanDirectory(path, "");
            Console.WriteLine($"Nb sizes: {scanner.Size}");
            scanner.CheckDoublons();
        }


        static void HashMd5(List<FileInfo> fileInfos)
        {
            var md5FileInfo = new HashFileInfo<Guid>();
            md5FileInfo.Md5Hash(fileInfos, t => t);
            Console.WriteLine(md5FileInfo);
            md5FileInfo.CheckDoublons(x => RateFile(x) );
        }


        static void RateFile(List<FileInfo> fileInfos)
        {
            var rateFileInfo = new HashFileInfo<int>();
            rateFileInfo.RateFile(fileInfos, t => t);
            rateFileInfo.MoveDoublons();
        }

        static void Main(string[] args)
        {
            var sizeFileInfo = new HashFileInfo<long>();
            sizeFileInfo.ScanDirectory(@"c:\tmp", "", x => x.Length);
           // Console.WriteLine(sizeFileInfo.ToString(true));

            sizeFileInfo.CheckDoublons(x => { HashMd5(x); });
        }
    }
}
