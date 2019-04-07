using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hasher
{
    public class Md5Hasher
    {
        private SortedDictionary<Guid, List<FileInfo>> _md5Files;

        public Md5Hasher()
        {
            _md5Files = new SortedDictionary<Guid, List<FileInfo>>();
        }

        public void HashFiles(List<FileInfo> fileInfos)
        {
            foreach (var fileInfo in fileInfos)
            {
                using (var md5 = MD5.Create())
                {
                    var guid = new Guid(md5.ComputeHash(File.ReadAllBytes(fileInfo.FullName)));
                    Helpers.AddFileInfo(_md5Files, guid, fileInfo);
                }
            }
        }

        public override string ToString()
        {
            return Helpers.GetInfos(_md5Files);
        }


        public void CheckDoublons()
        {
            foreach (var md5File in _md5Files)
            {
                if (md5File.Value.Count > 1)
                {
                    Console.WriteLine($"MD5 Conflict for: {Helpers.GetInfo(md5File, true)}");
                    // Traiter les fichiers en doublons
                }
            }
        }

    }
}
