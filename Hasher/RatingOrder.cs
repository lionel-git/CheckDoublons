using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Hasher
{
    public class RatingOrder
    {
        private SortedDictionary<int, List<FileInfo>> _rateFiles;

        public RatingOrder()
        {
            _rateFiles = new SortedDictionary<int, List<FileInfo>>();
        }

        public void RateFiles(List<FileInfo> fileInfos)
        {
            foreach (var fileInfo in fileInfos)
            {
                int rate = 1;
                Helpers.AddFileInfo(_rateFiles, rate, fileInfo);
            }
        }
    }
}
