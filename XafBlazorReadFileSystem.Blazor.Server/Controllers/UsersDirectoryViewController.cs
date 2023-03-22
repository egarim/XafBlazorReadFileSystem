using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XafBlazorReadFileSystem.Blazor.Server.Controllers;
using XafBlazorReadFileSystem.Module.BusinessObjects;

namespace XafBlazorReadFileSystem.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class UsersDirectoryViewController : ObjectViewController<DetailView, UsersDirectory>
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public UsersDirectoryViewController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            string rootpath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot\\UsersFiles");
            UsersDirectory UsersDirectory = new UsersDirectory();
            UsersDirectory.Path= rootpath;
            UsersDirectory.UserName = SecuritySystem.CurrentUserName;
            var UsersFiles= FileSystemHelper.ReadFileSystem(rootpath);
            foreach (var item in UsersFiles)
            {
                UsersDirectory.Files.Add(item);
            }
            
            this.View.CurrentObject= UsersDirectory;
            // Perform various tasks depending on the target View.
            foreach (ListPropertyEditor lpe in ((DetailView)View).GetItems<ListPropertyEditor>())
            {
                if (lpe.Frame != null)
                {
                    PushFrameToNestedController(lpe.Frame);
                }
                else
                {
                    lpe.FrameChanged += lpe_FrameChanged;
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
      
        private void PushFrameToNestedController(Frame frame)
        {
            foreach (Controller c in frame.Controllers)
            {
                if (c is FilesViewController)
                {
                    ((FilesViewController)c).AssignMasterFrame(Frame);
                }
            }
        }
        private void lpe_FrameChanged(object sender, EventArgs e)
        {
            PushFrameToNestedController(((ListPropertyEditor)sender).Frame);
        }
       
        protected override void OnDeactivated()
        {
            foreach (ListPropertyEditor lpe in ((DetailView)View).GetItems<ListPropertyEditor>())
            {
                lpe.FrameChanged -= new EventHandler<EventArgs>(lpe_FrameChanged);
            }
            base.OnDeactivated();
        }
    }
}
