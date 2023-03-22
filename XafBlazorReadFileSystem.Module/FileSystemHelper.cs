using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XafBlazorReadFileSystem.Module.BusinessObjects;

namespace XafBlazorReadFileSystem.Module
{
   

    public class FileSystemHelper
    {
        public static List<FileSystemItem> ReadFileSystem(string path)
        {
            var result = new List<FileSystemItem>();

            // Add the root directory item
            var rootItem = new FileSystemItem()
            {
                CreatedAt = Directory.GetCreationTime(path),
                UpdatedAt = Directory.GetLastWriteTime(path),
                Name = Path.GetFileName(path),
                Type = "directory",
                FullPath = Path.GetFullPath(path),
                Parent = null,
                IsDeleted = false
            };
            result.Add(rootItem);

            // Recursively add child items
            var childItems = GetChildItems(path, rootItem.FullPath);
            result.AddRange(childItems);

            return result;
        }

        private static readonly string[] Base32Chars = {
        "0", "1", "2", "3", "4", "5", "6", "7",
        "8", "9", "A", "B", "C", "D", "E", "F",
        "G", "H", "J", "K", "M", "N", "P", "Q",
        "R", "S", "T", "V", "W", "X", "Y", "Z"
    };

        public static string NewShortGuid()
        {
            byte[] guidBytes = Guid.NewGuid().ToByteArray();
            long longValue = BitConverter.ToInt64(guidBytes, 0);
            string base32Value = ConvertToBase32(Math.Abs(longValue));
            return base32Value.Substring(0, 8);
        }

        private static string ConvertToBase32(long value)
        {
            string result = "";
            do
            {
                int remainder = (int)(value % 32);
                result = Base32Chars[remainder] + result;
                value /= 32;
            } while (value > 0);
            return result;
        }


        public static void DeleteItem(FileSystemItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            // Rename the item with prefix "GC-"
            var newName = $"GC-{NewShortGuid()}-" + item.Name;
            var newPath = Path.Combine(item.Parent, newName);
            if (item.Type == "directory")
            {
                Directory.Move(item.FullPath, newPath);
            }
            else if (item.Type == "file")
            {
                File.Move(item.FullPath, newPath);
            }
            else
            {
                throw new NotSupportedException($"Unsupported item type: {item.Type}");
            }

            // Set the IsDeleted property to true
            item.IsDeleted = true;
            item.Name = newName;
            item.FullPath = newPath;
        }

        public static void CreateFolder(FileSystemItem parentFolder, string newFolderName)
        {
            if (parentFolder == null)
            {
                throw new ArgumentNullException(nameof(parentFolder));
            }

            if (string.IsNullOrEmpty(newFolderName))
            {
                throw new ArgumentException("New folder name cannot be null or empty", nameof(newFolderName));
            }

            var newFolderPath = Path.Combine(parentFolder.FullPath, newFolderName);
            Directory.CreateDirectory(newFolderPath);
        }

        private static List<FileSystemItem> GetChildItems(string path, string parentPath)
        {
            var result = new List<FileSystemItem>();

            // Add files in current directory
            var files = Directory.GetFiles(path);
            foreach (var filePath in files)
            {
                var item = new FileSystemItem()
                {
                    CreatedAt = File.GetCreationTime(filePath),
                    UpdatedAt = File.GetLastWriteTime(filePath),
                    Name = Path.GetFileName(filePath),
                    Type = "file",
                    FullPath = Path.GetFullPath(filePath),
                    Parent = parentPath,
                    IsDeleted = false
                };
                if (item.Name.StartsWith("GC-"))
                {
                    item.IsDeleted = true;
                }
                result.Add(item);
            }

            // Add subdirectories
            var directories = Directory.GetDirectories(path);
            foreach (var directoryPath in directories)
            {
                var item = new FileSystemItem()
                {
                    CreatedAt = Directory.GetCreationTime(directoryPath),
                    UpdatedAt = Directory.GetLastWriteTime(directoryPath),
                    Name = Path.GetFileName(directoryPath),
                    Type = "directory",
                    FullPath = Path.GetFullPath(directoryPath),
                    Parent = parentPath,
                    IsDeleted = false
                };
                if (item.Name.StartsWith("GC-"))
                {
                    item.IsDeleted = true;
                }
                result.Add(item);

                // Recursively add child items
                var childItems = GetChildItems(directoryPath, item.FullPath);
                result.AddRange(childItems);
            }

            return result;
        }
    }

}
