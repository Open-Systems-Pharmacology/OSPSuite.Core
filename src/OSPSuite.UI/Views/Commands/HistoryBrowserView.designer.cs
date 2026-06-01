using DevExpress.XtraLayout;

namespace OSPSuite.UI.Views.Commands
{
    partial class HistoryBrowserView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
         this.components = new System.ComponentModel.Container();
         this.historyLayoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.historyTreeList = new DevExpress.XtraTreeList.TreeList();
         this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
         this._barManager = new DevExpress.XtraBars.BarManager(this.components);
         this.menuBar = new DevExpress.XtraBars.Bar();
         this.btnUndo = new DevExpress.XtraBars.BarButtonItem();
         this.btnAddLabel = new DevExpress.XtraBars.BarButtonItem();
         this.btnEditComment = new DevExpress.XtraBars.BarButtonItem();
         this.lblRollBack = new DevExpress.XtraBars.BarStaticItem();
         this.tbRollBackState = new DevExpress.XtraBars.BarEditItem();
         this.tbRollBackStateEditor = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
         this.btnClearHistory = new DevExpress.XtraBars.BarButtonItem();
         this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
         this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
         this.btnExportHistory = new DevExpress.XtraBars.BarButtonItem();
         ((System.ComponentModel.ISupportInitialize)(this.historyLayoutControl)).BeginInit();
         this.historyLayoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.historyTreeList)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbRollBackStateEditor)).BeginInit();
         this.SuspendLayout();
         // 
         // historyLayoutControl
         // 
         this.historyLayoutControl.AllowCustomization = false;
         this.historyLayoutControl.Controls.Add(this.historyTreeList);
         this.historyLayoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.historyLayoutControl.Location = new System.Drawing.Point(0, 25);
         this.historyLayoutControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.historyLayoutControl.Name = "historyLayoutControl";
         this.historyLayoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1124, 253, 250, 350);
         this.historyLayoutControl.Root = this.layoutControlGroup1;
         this.historyLayoutControl.Size = new System.Drawing.Size(924, 690);
         this.historyLayoutControl.TabIndex = 6;
         this.historyLayoutControl.Text = "layoutControl1";
         // 
         // historyTreeList
         // 
         this.historyTreeList.Location = new System.Drawing.Point(3, 3);
         this.historyTreeList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.historyTreeList.MinWidth = 23;
         this.historyTreeList.Name = "historyTreeList";
         this.historyTreeList.Size = new System.Drawing.Size(918, 684);
         this.historyTreeList.TabIndex = 5;
         this.historyTreeList.TreeLevelWidth = 21;
         this.historyTreeList.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.historyTreeListFocusedNodeChanged);
         // 
         // layoutControlGroup1
         // 
         this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
         this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5});
         this.layoutControlGroup1.Name = "layoutControlGroup1";
         this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutControlGroup1.Size = new System.Drawing.Size(924, 690);
         this.layoutControlGroup1.TextVisible = false;
         // 
         // layoutControlItem5
         // 
         this.layoutControlItem5.Control = this.historyTreeList;
         this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
         this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem5.Name = "layoutControlItem5";
         this.layoutControlItem5.Size = new System.Drawing.Size(922, 688);
         this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem5.TextVisible = false;
         // 
         // _barManager
         // 
         this._barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.menuBar});
         this._barManager.DockControls.Add(this.barDockControlTop);
         this._barManager.DockControls.Add(this.barDockControlBottom);
         this._barManager.DockControls.Add(this.barDockControlLeft);
         this._barManager.DockControls.Add(this.barDockControlRight);
         this._barManager.Form = this;
         this._barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.btnUndo,
            this.btnAddLabel,
            this.btnEditComment,
            this.tbRollBackState,
            this.lblRollBack,
            this.btnClearHistory,
            this.btnExportHistory});
         this._barManager.MaxItemId = 8;
         this._barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.tbRollBackStateEditor});
         // 
         // menuBar
         // 
         this.menuBar.BarName = "Tools";
         this.menuBar.DockCol = 0;
         this.menuBar.DockRow = 0;
         this.menuBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
         this.menuBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.btnUndo),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnAddLabel),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnEditComment),
            new DevExpress.XtraBars.LinkPersistInfo(this.lblRollBack),
            new DevExpress.XtraBars.LinkPersistInfo(this.tbRollBackState),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnClearHistory),
            new DevExpress.XtraBars.LinkPersistInfo(this.btnExportHistory)});
         this.menuBar.OptionsBar.AllowQuickCustomization = false;
         this.menuBar.OptionsBar.DisableClose = true;
         this.menuBar.OptionsBar.DisableCustomization = true;
         this.menuBar.OptionsBar.UseWholeRow = true;
         this.menuBar.Text = "Tools";
         // 
         // btnUndo
         // 
         this.btnUndo.Caption = "btnUndo";
         this.btnUndo.Id = 0;
         this.btnUndo.Name = "btnUndo";
         // 
         // btnAddLabel
         // 
         this.btnAddLabel.Caption = "btnAddLabel";
         this.btnAddLabel.Id = 1;
         this.btnAddLabel.Name = "btnAddLabel";
         // 
         // btnEditComment
         // 
         this.btnEditComment.Caption = "btnEditComment";
         this.btnEditComment.Id = 2;
         this.btnEditComment.Name = "btnEditComment";
         // 
         // lblRollBack
         // 
         this.lblRollBack.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
         this.lblRollBack.Caption = "btnRollBackLabel";
         this.lblRollBack.Id = 4;
         this.lblRollBack.Name = "lblRollBack";
         // 
         // tbRollBackState
         // 
         this.tbRollBackState.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
         this.tbRollBackState.Caption = "btnRollBack";
         this.tbRollBackState.Edit = this.tbRollBackStateEditor;
         this.tbRollBackState.EditWidth = 92;
         this.tbRollBackState.Id = 3;
         this.tbRollBackState.Name = "tbRollBackState";
         // 
         // tbRollBackStateEditor
         // 
         this.tbRollBackStateEditor.AutoHeight = false;
         this.tbRollBackStateEditor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
         this.tbRollBackStateEditor.Name = "tbRollBackStateEditor";
         // 
         // btnClearHistory
         // 
         this.btnClearHistory.Caption = "btnClearHistory";
         this.btnClearHistory.Id = 6;
         this.btnClearHistory.Name = "btnClearHistory";
         // 
         // barDockControlTop
         // 
         this.barDockControlTop.CausesValidation = false;
         this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
         this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
         this.barDockControlTop.Manager = this._barManager;
         this.barDockControlTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.barDockControlTop.Size = new System.Drawing.Size(924, 25);
         // 
         // barDockControlBottom
         // 
         this.barDockControlBottom.CausesValidation = false;
         this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.barDockControlBottom.Location = new System.Drawing.Point(0, 715);
         this.barDockControlBottom.Manager = this._barManager;
         this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.barDockControlBottom.Size = new System.Drawing.Size(924, 0);
         // 
         // barDockControlLeft
         // 
         this.barDockControlLeft.CausesValidation = false;
         this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
         this.barDockControlLeft.Location = new System.Drawing.Point(0, 25);
         this.barDockControlLeft.Manager = this._barManager;
         this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.barDockControlLeft.Size = new System.Drawing.Size(0, 690);
         // 
         // barDockControlRight
         // 
         this.barDockControlRight.CausesValidation = false;
         this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
         this.barDockControlRight.Location = new System.Drawing.Point(924, 25);
         this.barDockControlRight.Manager = this._barManager;
         this.barDockControlRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.barDockControlRight.Size = new System.Drawing.Size(0, 690);
         // 
         // btnExportHistory
         // 
         this.btnExportHistory.Caption = "btnExportHistory";
         this.btnExportHistory.Id = 7;
         this.btnExportHistory.Name = "btnExportHistory";
         // 
         // HistoryBrowserView
         // 
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
         this.Controls.Add(this.historyLayoutControl);
         this.Controls.Add(this.barDockControlLeft);
         this.Controls.Add(this.barDockControlRight);
         this.Controls.Add(this.barDockControlBottom);
         this.Controls.Add(this.barDockControlTop);
         this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
         this.Name = "HistoryBrowserView";
         this.Size = new System.Drawing.Size(924, 715);
         ((System.ComponentModel.ISupportInitialize)(this.historyLayoutControl)).EndInit();
         this.historyLayoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.historyTreeList)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._barManager)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbRollBackStateEditor)).EndInit();
         this.ResumeLayout(false);
         this.PerformLayout();

        }

        #endregion

        private OSPSuite.UI.Controls.UxLayoutControl historyLayoutControl;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraTreeList.TreeList historyTreeList;
        private LayoutControlItem layoutControlItem5;
        private DevExpress.XtraBars.BarManager _barManager;
        private DevExpress.XtraBars.Bar menuBar;
        private DevExpress.XtraBars.BarButtonItem btnUndo;
        private DevExpress.XtraBars.BarButtonItem btnAddLabel;
        private DevExpress.XtraBars.BarButtonItem btnEditComment;
        private DevExpress.XtraBars.BarStaticItem lblRollBack;
        private DevExpress.XtraBars.BarEditItem tbRollBackState;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit tbRollBackStateEditor;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem btnClearHistory;
      private DevExpress.XtraBars.BarButtonItem btnExportHistory;
   }
}