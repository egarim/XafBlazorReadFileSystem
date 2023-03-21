using System.Diagnostics;
using XafBlazorReadFileSystem.Module;
using XafBlazorReadFileSystem.Module.BusinessObjects;

namespace Tests
{
    public class Tests
    {


        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            const string Path = @"..\..\..\..\XafBlazorReadFileSystem.Blazor.Server";
            var Fs = FileSystemHelper.ReadFileSystem(Path);

            var AllDirectories = Fs.Where(x => x.Type == "directory");

            var DirectoriesUnderRoot = AllDirectories.Where(x => !string.IsNullOrEmpty(x.Parent) && x.Parent.EndsWith("XafBlazorReadFileSystem.Blazor.Server"));

            foreach (FileSystemItem fileSystemItem in DirectoriesUnderRoot)
            {
                Debug.WriteLine($"{fileSystemItem.Name} IsDeleted:{fileSystemItem.IsDeleted}");
            }
            var BinFolder = DirectoriesUnderRoot.FirstOrDefault(x => x.Name == "bin");
            FileSystemHelper.DeleteItem(BinFolder);

            Fs = FileSystemHelper.ReadFileSystem(Path);

            AllDirectories = Fs.Where(x => x.Type == "directory");

            DirectoriesUnderRoot = AllDirectories.Where(x => !string.IsNullOrEmpty(x.Parent) && x.Parent.EndsWith("XafBlazorReadFileSystem.Blazor.Server"));

            Debug.WriteLine(System.Environment.NewLine);
            Debug.WriteLine(System.Environment.NewLine);

            foreach (FileSystemItem fileSystemItem in DirectoriesUnderRoot)
            {
                Debug.WriteLine($"{fileSystemItem.Name} IsDeleted:{fileSystemItem.IsDeleted}");
            }

            Assert.Pass();
        }
    }
}