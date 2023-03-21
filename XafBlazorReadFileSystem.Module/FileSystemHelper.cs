using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XafBlazorReadFileSystem.Module
{
    public class FileSystemItem
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string FullPath { get; set; }
        public string Parent { get; set; }
        public bool IsDeleted { get; set; }
    }

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

        public static void DeleteItem(FileSystemItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            // Rename the item with prefix "GC-"
            var newName = "GC-" + item.Name;
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
