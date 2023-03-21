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
using XafBlazorReadFileSystem.Module;
using XafBlazorReadFileSystem.Module.BusinessObjects;

namespace XafBlazorReadFileSystem.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class FilesViewController : ObjectViewController<ListView, FileSystemItem>
    {
        SimpleAction CreateDirectory;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public FilesViewController()
        {
            InitializeComponent();


            //TODO use this https://www.syncfusion.com/blazor-components/blazor-file-manager

            CreateDirectory = new SimpleAction(this, "CreateDirectory", "View");
            CreateDirectory.Execute += CreateDirectory_Execute;
            CreateDirectory.TargetObjectsCriteria = CriteriaOperator.FromLambda<FileSystemItem>(f => f.Type == "directory").ToString();
            CreateDirectory.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueAtLeastForOne;
            CreateDirectory.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private void CreateDirectory_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            // Execute your business logic (https://docs.devexpress.com/eXpressAppFramework/112737/).
        }
        protected override void OnActivated()
        {
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
