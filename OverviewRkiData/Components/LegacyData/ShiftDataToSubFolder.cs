using OverviewRkiData.Components.Data;
using System;
using System.IO;
using System.Linq;

namespace OverviewRkiData.Components.LegacyData
{
    internal class ShiftDataToSubFolder
    {
        internal void MoveRkiDataFilesFromCurrentApplicationFolderToSubFolder()
        {
            var files = Directory.GetFiles($"{Environment.CurrentDirectory}")
                .Where(w => w.Contains(HelperExtension.RkiFilename))
                .ToArray();

            if (!files.Any())
            {
                return;
            }

            var subFolder = $"{Environment.CurrentDirectory}/{HelperExtension.DataFolderName}";

            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
            }

            foreach (var file in files)
            {
                if (!File.Exists(file))
                {
                    continue;
                }
                
                var fileInfo = new FileInfo(file);
                var newFilename = $"{subFolder}/{fileInfo.Name}";

                if (File.Exists(newFilename))
                {
                    continue;
                }

                File.Copy(file, newFilename);
            }
        }
    }
}
