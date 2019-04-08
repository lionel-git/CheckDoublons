using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Hasher
{
    public class Md5 : HashFileInfo<Guid>
    {
        public Md5() : base("Md5")
        {
        }

        public void Md5Hash(List<FileInfo> fileInfos)
        {
            foreach (var fileInfo in fileInfos)
            {
                using (var md5 = MD5.Create())
                {
                    var guid = new Guid(md5.ComputeHash(File.ReadAllBytes(fileInfo.FullName)));
                    Add(guid, fileInfo);
                }
            }
        }

    }
}
