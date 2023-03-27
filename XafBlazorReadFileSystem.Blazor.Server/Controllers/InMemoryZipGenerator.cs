using System;
using System.Linq;
using System.IO;
using System.IO.Compression;

namespace XafBlazorReadFileSystem.Blazor.Server.Controllers
{
  
    public class InMemoryZipGenerator
    {
        public MemoryStream GenerateZip(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Path must not be null or empty.", nameof(path));
            }

            if (!Directory.Exists(path) && !File.Exists(path))
            {
                throw new ArgumentException("Path must point to an existing file or directory.", nameof(path));
            }

            var memoryStream = new MemoryStream();

            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                if (File.Exists(path))
                {
                    AddFileToZip(zipArchive, path, Path.GetFileName(path));
                }
                else
                {
                    AddDirectoryToZip(zipArchive, path, string.Empty);
                }
            }

            memoryStream.Position = 0;
            return memoryStream;
        }

        private void AddFileToZip(ZipArchive zipArchive, string filePath, string entryName)
        {
            var zipEntry = zipArchive.CreateEntry(entryName, CompressionLevel.Optimal);
            using (var fileStream = File.OpenRead(filePath))
            using (var entryStream = zipEntry.Open())
            {
                fileStream.CopyTo(entryStream);
            }
        }

        private void AddDirectoryToZip(ZipArchive zipArchive, string directoryPath, string entryPath)
        {
            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                var relativePath = Path.Combine(entryPath, Path.GetFileName(filePath));
                AddFileToZip(zipArchive, filePath, relativePath);
            }

            foreach (var subDirectoryPath in Directory.GetDirectories(directoryPath))
            {
                var relativePath = Path.Combine(entryPath, Path.GetFileName(subDirectoryPath));
                AddDirectoryToZip(zipArchive, subDirectoryPath, relativePath);
            }
        }
    }
}
