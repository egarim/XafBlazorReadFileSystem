using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XafBlazorReadFileSystem.Module.BusinessObjects;

namespace XafBlazorReadFileSystem.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DownloadController : ObjectViewController<ListView, FileSystemItem>
    {
        SimpleAction Download;
        InMemoryZipGenerator zipGenerator = new InMemoryZipGenerator();
        IJSRuntime js;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public DownloadController()
        {
            InitializeComponent();
            Download = new SimpleAction(this, "Download", "View");
            Download.Execute += Download_Execute;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private async void Download_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            var CurrentFileSystemItem = e.CurrentObject as FileSystemItem;
            var InMemoryZip= zipGenerator.GenerateZip(CurrentFileSystemItem.FullPath);

             await FileUtil.SaveAs(js, CurrentFileSystemItem.Name + ".zip", InMemoryZip.ToArray());

            // Execute your business logic (https://docs.devexpress.com/eXpressAppFramework/112737/).
        }
        protected override void OnActivated()
        {
            var CurrentServiceProvider = this.Application as BlazorApplication;
            js = CurrentServiceProvider.ServiceProvider.GetRequiredService<IJSRuntime>();
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
