using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace XafBlazorReadFileSystem.Module.BusinessObjects
{
    [NavigationItem("Files")]
    [DomainComponent]
    [DefaultClassOptions]
    public class UsersDirectory : NonPersistentBaseObject
    {
        public string UserName { get; set; }
        public string Path { get; set; }
        public UsersDirectory()
        {
            Files = new BindingList<FileSystemItem>();
        }

        public BindingList<FileSystemItem> Files { get; set; }
    }
}