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
        PopupWindowShowAction CreateDirectory;
        SimpleAction CreateDirectory2;
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public FilesViewController()
        {
            InitializeComponent();

            this.TargetViewNesting = Nesting.Nested;

            //TODO use this https://www.syncfusion.com/blazor-components/blazor-file-manager

            //CreateDirectory2 = new SimpleAction(this, "CreateDirectory2", "View");
            //CreateDirectory2.Execute += CreateDirectory_Execute;
            //CreateDirectory2.TargetObjectsCriteria = CriteriaOperator.FromLambda<FileSystemItem>(f => f.Type == "directory").ToString();
            //CreateDirectory2.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueAtLeastForOne;
            //CreateDirectory2.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;


            CreateDirectory = new PopupWindowShowAction(this, "CreateDirectory", "View");
            CreateDirectory.Execute += CreateDirectory_Execute;
            CreateDirectory.CustomizePopupWindowParams += CreateDirectory_CustomizePopupWindowParams;
            CreateDirectory.TargetObjectsCriteria = CriteriaOperator.FromLambda<FileSystemItem>(f => f.Type == "directory").ToString();
            CreateDirectory.TargetObjectsCriteriaMode = TargetObjectsCriteriaMode.TrueAtLeastForOne;
            CreateDirectory.SelectionDependencyType = SelectionDependencyType.RequireSingleObject;





            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        private void CreateDirectory_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            var selectedPopupWindowObjects = e.PopupWindowViewSelectedObjects[0] as DirectoryName;
            var selectedSourceViewObjects = e.SelectedObjects[0];

            var fsi = selectedSourceViewObjects as FileSystemItem;
            if (fsi != null)
            {
                FileSystemHelper.CreateFolder(fsi, selectedPopupWindowObjects.Name);
                var CurrentUsersDirectory = masterFrame.View.CurrentObject as UsersDirectory;
                var NewFiles = FileSystemHelper.ReadFileSystem(CurrentUsersDirectory.Path);
                foreach (var item in NewFiles)
                {
                    if (CurrentUsersDirectory.Files.FirstOrDefault(f => f.FullPath == item.FullPath) != null)
                    {
                        continue;
                    }
                    CurrentUsersDirectory.Files.Add(item);

                }

            }

            // Execute your business logic (https://docs.devexpress.com/eXpressAppFramework/112723/).
        }
        private void CreateDirectory_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var Os=this.Application.CreateObjectSpace(typeof(DirectoryName));
            var Params= Os.CreateObject<DirectoryName>();
            e.View=this.Application.CreateDetailView(Os, Params);

            // Set the e.View parameter to a newly created view (https://docs.devexpress.com/eXpressAppFramework/112723/).
        }
        private Frame masterFrame;
        public void AssignMasterFrame(Frame parentFrame)
        {
            masterFrame = parentFrame;
            // Use this Frame to get Controllers and Actions. 
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
