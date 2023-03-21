using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using System;
using System.Linq;

namespace XafBlazorReadFileSystem.Module.BusinessObjects
{
    [DomainComponent]
    public class FileSystemItem : NonPersistentBaseObject
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string FullPath { get; set; }
        public string Parent { get; set; }
        public bool IsDeleted { get; set; }
    }
}