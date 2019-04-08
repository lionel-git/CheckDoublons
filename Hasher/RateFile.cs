using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Hasher
{
    public class RateFile : HashFileInfo<int>
    {
        public RateFile() : base("RateFile")
        {
        }

        private int Rate(FileInfo fileInfo)
        {
            int rate = 1000;
            if (Regex.Match(fileInfo.FullName, "Good").Success)
                rate = -1;
            if (Regex.Match(fileInfo.FullName, "Very.*Good").Success)
                rate = -2;
            return rate;
        }

        public void Rate(List<FileInfo> fileInfos)
        {
            foreach (var fileInfo in fileInfos)
            {
                int rate = Rate(fileInfo);
                Add(rate, fileInfo);
            }
        }

    }
}
