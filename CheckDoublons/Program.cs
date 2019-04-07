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
            var md5FileInfo = new HashFileInfo<Guid>("MD5");
            md5FileInfo.Md5Hash(fileInfos, t => t);
            Console.WriteLine(md5FileInfo);
            md5FileInfo.CheckDoublons(x => RateFile(x) );
        }

        static void RateFile(List<FileInfo> fileInfos)
        {
            var rateFileInfo = new HashFileInfo<int>("Rate");
            rateFileInfo.RateFile(fileInfos, t => t);
            rateFileInfo.MoveDoublons();
        }

        static void Main(string[] args)
        {
            string path = @"d:\v";
            var sizeFileInfo = new HashFileInfo<long>("Size");
            sizeFileInfo.ScanDirectory(path, "", x => x.Length);
           // Console.WriteLine(sizeFileInfo.ToString(true));

            sizeFileInfo.CheckDoublons(x => { HashMd5(x); });
        }
    }
}
