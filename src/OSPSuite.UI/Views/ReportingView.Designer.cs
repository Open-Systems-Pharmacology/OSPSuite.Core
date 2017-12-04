namespace OSPSuite.UI.Views
{
   partial class ReportingView
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
         _screenBinder.Dispose();
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl1 = new OSPSuite.UI.Controls.UxLayoutControl();
         this.chkOpenReportAfterCreation = new OSPSuite.UI.Controls.UxCheckEdit();
         this.cbFont = new DevExpress.XtraEditors.ComboBoxEdit();
         this.chkDraft = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkDeleteWorkingFolder = new OSPSuite.UI.Controls.UxCheckEdit();
         this.lblTemplateDescription = new DevExpress.XtraEditors.LabelControl();
         this.cbReportTemplates = new DevExpress.XtraEditors.ComboBoxEdit();
         this.chkSaveReportArtifacts = new OSPSuite.UI.Controls.UxCheckEdit();
         this.rgColor = new DevExpress.XtraEditors.RadioGroup();
         this.chkVerbose = new OSPSuite.UI.Controls.UxCheckEdit();
         this.tbAuthor = new DevExpress.XtraEditors.TextEdit();
         this.tbSubtitle = new DevExpress.XtraEditors.TextEdit();
         this.tbTitle = new DevExpress.XtraEditors.TextEdit();
         this.layoutControl = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupOptions = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemColor = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemFont = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSaveReportArtifacts = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemDeleteWorkingFolder = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
         this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
         this.layoutGroupTemplate = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemDescription = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemTemplate = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutGroupFirstPage = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemTitle = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemAuthor = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSubtitle = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).BeginInit();
         this.layoutControlBase.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
         this.layoutControl1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.chkOpenReportAfterCreation.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFont.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkDraft.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkDeleteWorkingFolder.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbReportTemplates.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkSaveReportArtifacts.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.rgColor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkVerbose.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbAuthor.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbSubtitle.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbTitle.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemColor)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFont)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSaveReportArtifacts)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDeleteWorkingFolder)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupTemplate)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTemplate)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupFirstPage)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTitle)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAuthor)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSubtitle)).BeginInit();
         this.SuspendLayout();
         // 
         // btnCancel
         // 
         this.btnCancel.Location = new System.Drawing.Point(420, 12);
         this.btnCancel.Size = new System.Drawing.Size(85, 22);
         // 
         // btnOk
         // 
         this.btnOk.Location = new System.Drawing.Point(315, 12);
         this.btnOk.Size = new System.Drawing.Size(101, 22);
         // 
         // layoutControlBase
         // 
         this.layoutControlBase.Location = new System.Drawing.Point(0, 420);
         this.layoutControlBase.Size = new System.Drawing.Size(517, 46);
         this.layoutControlBase.Controls.SetChildIndex(this.btnCancel, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnOk, 0);
         this.layoutControlBase.Controls.SetChildIndex(this.btnExtra, 0);
         // 
         // btnExtra
         // 
         this.btnExtra.Size = new System.Drawing.Size(147, 22);
         // 
         // layoutControlGroupBase
         // 
         this.layoutControlGroupBase.Size = new System.Drawing.Size(517, 46);
         // 
         // layoutItemOK
         // 
         this.layoutItemOK.Location = new System.Drawing.Point(303, 0);
         this.layoutItemOK.Size = new System.Drawing.Size(105, 26);
         // 
         // layoutItemCancel
         // 
         this.layoutItemCancel.Location = new System.Drawing.Point(408, 0);
         this.layoutItemCancel.Size = new System.Drawing.Size(89, 26);
         // 
         // emptySpaceItemBase
         // 
         this.emptySpaceItemBase.Location = new System.Drawing.Point(151, 0);
         this.emptySpaceItemBase.Size = new System.Drawing.Size(152, 26);
         // 
         // layoutItemExtra
         // 
         this.layoutItemExtra.Size = new System.Drawing.Size(151, 26);
         // 
         // layoutControl1
         // 
         this.layoutControl1.Controls.Add(this.chkOpenReportAfterCreation);
         this.layoutControl1.Controls.Add(this.cbFont);
         this.layoutControl1.Controls.Add(this.chkDraft);
         this.layoutControl1.Controls.Add(this.chkDeleteWorkingFolder);
         this.layoutControl1.Controls.Add(this.lblTemplateDescription);
         this.layoutControl1.Controls.Add(this.cbReportTemplates);
         this.layoutControl1.Controls.Add(this.chkSaveReportArtifacts);
         this.layoutControl1.Controls.Add(this.rgColor);
         this.layoutControl1.Controls.Add(this.chkVerbose);
         this.layoutControl1.Controls.Add(this.tbAuthor);
         this.layoutControl1.Controls.Add(this.tbSubtitle);
         this.layoutControl1.Controls.Add(this.tbTitle);
         this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl1.Location = new System.Drawing.Point(0, 0);
         this.layoutControl1.Name = "layoutControl1";
         this.layoutControl1.Root = this.layoutControl;
         this.layoutControl1.Size = new System.Drawing.Size(517, 420);
         this.layoutControl1.TabIndex = 34;
         this.layoutControl1.Text = "layoutControl1";
         // 
         // chkOpenReportAfterCreation
         // 
         this.chkOpenReportAfterCreation.Location = new System.Drawing.Point(24, 318);
         this.chkOpenReportAfterCreation.Name = "chkOpenReportAfterCreation";
         this.chkOpenReportAfterCreation.Properties.Caption = "chkOpenReportWhenCreated";
         this.chkOpenReportAfterCreation.Size = new System.Drawing.Size(469, 19);
         this.chkOpenReportAfterCreation.StyleController = this.layoutControl1;
         this.chkOpenReportAfterCreation.TabIndex = 20;
         // 
         // cbFont
         // 
         this.cbFont.Location = new System.Drawing.Point(123, 294);
         this.cbFont.Name = "cbFont";
         this.cbFont.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbFont.Size = new System.Drawing.Size(370, 20);
         this.cbFont.StyleController = this.layoutControl1;
         this.cbFont.TabIndex = 19;
         // 
         // chkDraft
         // 
         this.chkDraft.Location = new System.Drawing.Point(260, 242);
         this.chkDraft.Name = "chkDraft";
         this.chkDraft.Properties.Caption = "chkDraft";
         this.chkDraft.Size = new System.Drawing.Size(233, 19);
         this.chkDraft.StyleController = this.layoutControl1;
         this.chkDraft.TabIndex = 18;
         // 
         // chkDeleteWorkingFolder
         // 
         this.chkDeleteWorkingFolder.Location = new System.Drawing.Point(24, 364);
         this.chkDeleteWorkingFolder.Name = "chkDeleteWorkingFolder";
         this.chkDeleteWorkingFolder.Properties.Caption = "ckDeleteWorkingFolder";
         this.chkDeleteWorkingFolder.Size = new System.Drawing.Size(469, 19);
         this.chkDeleteWorkingFolder.StyleController = this.layoutControl1;
         this.chkDeleteWorkingFolder.TabIndex = 17;
         // 
         // lblTemplateDescription
         // 
         this.lblTemplateDescription.Location = new System.Drawing.Point(24, 182);
         this.lblTemplateDescription.Name = "lblTemplateDescription";
         this.lblTemplateDescription.Size = new System.Drawing.Size(107, 13);
         this.lblTemplateDescription.StyleController = this.layoutControl1;
         this.lblTemplateDescription.TabIndex = 16;
         this.lblTemplateDescription.Text = "lblTemplateDescription";
         // 
         // cbReportTemplates
         // 
         this.cbReportTemplates.Location = new System.Drawing.Point(123, 158);
         this.cbReportTemplates.Name = "cbReportTemplates";
         this.cbReportTemplates.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.cbReportTemplates.Size = new System.Drawing.Size(370, 20);
         this.cbReportTemplates.StyleController = this.layoutControl1;
         this.cbReportTemplates.TabIndex = 15;
         // 
         // chkSaveReportArtifacts
         // 
         this.chkSaveReportArtifacts.Location = new System.Drawing.Point(24, 341);
         this.chkSaveReportArtifacts.Name = "chkSaveReportArtifacts";
         this.chkSaveReportArtifacts.Properties.Caption = "chkSaveReportArtifacts";
         this.chkSaveReportArtifacts.Size = new System.Drawing.Size(469, 19);
         this.chkSaveReportArtifacts.StyleController = this.layoutControl1;
         this.chkSaveReportArtifacts.TabIndex = 14;
         // 
         // rgColor
         // 
         this.rgColor.Location = new System.Drawing.Point(123, 265);
         this.rgColor.Name = "rgColor";
         this.rgColor.Size = new System.Drawing.Size(370, 25);
         this.rgColor.StyleController = this.layoutControl1;
         this.rgColor.TabIndex = 11;
         // 
         // chkVerbose
         // 
         this.chkVerbose.Location = new System.Drawing.Point(24, 242);
         this.chkVerbose.Name = "chkVerbose";
         this.chkVerbose.Properties.Caption = "chkVerbose";
         this.chkVerbose.Size = new System.Drawing.Size(232, 19);
         this.chkVerbose.StyleController = this.layoutControl1;
         this.chkVerbose.TabIndex = 9;
         // 
         // tbAuthor
         // 
         this.tbAuthor.Location = new System.Drawing.Point(123, 91);
         this.tbAuthor.Name = "tbAuthor";
         this.tbAuthor.Size = new System.Drawing.Size(370, 20);
         this.tbAuthor.StyleController = this.layoutControl1;
         this.tbAuthor.TabIndex = 8;
         // 
         // tbSubtitle
         // 
         this.tbSubtitle.Location = new System.Drawing.Point(123, 67);
         this.tbSubtitle.Name = "tbSubtitle";
         this.tbSubtitle.Size = new System.Drawing.Size(370, 20);
         this.tbSubtitle.StyleController = this.layoutControl1;
         this.tbSubtitle.TabIndex = 7;
         // 
         // tbTitle
         // 
         this.tbTitle.Location = new System.Drawing.Point(123, 43);
         this.tbTitle.Name = "tbTitle";
         this.tbTitle.Size = new System.Drawing.Size(370, 20);
         this.tbTitle.StyleController = this.layoutControl1;
         this.tbTitle.TabIndex = 6;
         // 
         // layoutControl
         // 
         this.layoutControl.CustomizationFormText = "layoutControl";
         this.layoutControl.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutControl.GroupBordersVisible = false;
         this.layoutControl.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutGroupOptions,
            this.emptySpaceItem1,
            this.layoutGroupTemplate,
            this.layoutGroupFirstPage});
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.Size = new System.Drawing.Size(517, 420);
         this.layoutControl.Text = "layoutControl";
         this.layoutControl.TextVisible = false;
         // 
         // layoutGroupOptions
         // 
         this.layoutGroupOptions.CustomizationFormText = "layoutGroupOptions";
         this.layoutGroupOptions.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutItemColor,
            this.layoutControlItem2,
            this.layoutItemFont,
            this.layoutItemSaveReportArtifacts,
            this.layoutItemDeleteWorkingFolder,
            this.layoutControlItem3});
         this.layoutGroupOptions.Location = new System.Drawing.Point(0, 199);
         this.layoutGroupOptions.Name = "layoutGroupOptions";
         this.layoutGroupOptions.Size = new System.Drawing.Size(497, 188);
         this.layoutGroupOptions.Text = "layoutGroupOptions";
         // 
         // layoutControlItem1
         // 
         this.layoutControlItem1.Control = this.chkVerbose;
         this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
         this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
         this.layoutControlItem1.Name = "layoutControlItem1";
         this.layoutControlItem1.Size = new System.Drawing.Size(236, 23);
         this.layoutControlItem1.Text = "layoutControlItem1";
         this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem1.TextToControlDistance = 0;
         this.layoutControlItem1.TextVisible = false;
         // 
         // layoutItemColor
         // 
         this.layoutItemColor.Control = this.rgColor;
         this.layoutItemColor.CustomizationFormText = "layoutItemColor";
         this.layoutItemColor.Location = new System.Drawing.Point(0, 23);
         this.layoutItemColor.MaxSize = new System.Drawing.Size(0, 29);
         this.layoutItemColor.MinSize = new System.Drawing.Size(164, 29);
         this.layoutItemColor.Name = "layoutItemColor";
         this.layoutItemColor.Size = new System.Drawing.Size(473, 29);
         this.layoutItemColor.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
         this.layoutItemColor.Text = "layoutItemColor";
         this.layoutItemColor.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutControlItem2
         // 
         this.layoutControlItem2.Control = this.chkDraft;
         this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
         this.layoutControlItem2.Location = new System.Drawing.Point(236, 0);
         this.layoutControlItem2.Name = "layoutControlItem2";
         this.layoutControlItem2.Size = new System.Drawing.Size(237, 23);
         this.layoutControlItem2.Text = "layoutControlItem2";
         this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem2.TextToControlDistance = 0;
         this.layoutControlItem2.TextVisible = false;
         // 
         // layoutItemFont
         // 
         this.layoutItemFont.Control = this.cbFont;
         this.layoutItemFont.CustomizationFormText = "layoutItemFont";
         this.layoutItemFont.Location = new System.Drawing.Point(0, 52);
         this.layoutItemFont.Name = "layoutItemFont";
         this.layoutItemFont.Size = new System.Drawing.Size(473, 24);
         this.layoutItemFont.Text = "layoutItemFont";
         this.layoutItemFont.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemSaveReportArtifacts
         // 
         this.layoutItemSaveReportArtifacts.Control = this.chkSaveReportArtifacts;
         this.layoutItemSaveReportArtifacts.CustomizationFormText = "layoutItemDeleteWorkingFolder";
         this.layoutItemSaveReportArtifacts.Location = new System.Drawing.Point(0, 99);
         this.layoutItemSaveReportArtifacts.Name = "layoutItemSaveReportArtifacts";
         this.layoutItemSaveReportArtifacts.Size = new System.Drawing.Size(473, 23);
         this.layoutItemSaveReportArtifacts.Text = "layoutItemSaveReportArtifacts";
         this.layoutItemSaveReportArtifacts.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemSaveReportArtifacts.TextToControlDistance = 0;
         this.layoutItemSaveReportArtifacts.TextVisible = false;
         // 
         // layoutItemDeleteWorkingFolder
         // 
         this.layoutItemDeleteWorkingFolder.Control = this.chkDeleteWorkingFolder;
         this.layoutItemDeleteWorkingFolder.CustomizationFormText = "layoutControlItem2";
         this.layoutItemDeleteWorkingFolder.Location = new System.Drawing.Point(0, 122);
         this.layoutItemDeleteWorkingFolder.Name = "layoutItemDeleteWorkingFolder";
         this.layoutItemDeleteWorkingFolder.Size = new System.Drawing.Size(473, 23);
         this.layoutItemDeleteWorkingFolder.Text = "layoutItemDeleteWorkingFolder";
         this.layoutItemDeleteWorkingFolder.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemDeleteWorkingFolder.TextToControlDistance = 0;
         this.layoutItemDeleteWorkingFolder.TextVisible = false;
         // 
         // layoutControlItem3
         // 
         this.layoutControlItem3.Control = this.chkOpenReportAfterCreation;
         this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
         this.layoutControlItem3.Location = new System.Drawing.Point(0, 76);
         this.layoutControlItem3.Name = "layoutControlItem3";
         this.layoutControlItem3.Size = new System.Drawing.Size(473, 23);
         this.layoutControlItem3.Text = "layoutControlItem3";
         this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
         this.layoutControlItem3.TextToControlDistance = 0;
         this.layoutControlItem3.TextVisible = false;
         // 
         // emptySpaceItem1
         // 
         this.emptySpaceItem1.AllowHotTrack = false;
         this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
         this.emptySpaceItem1.Location = new System.Drawing.Point(0, 387);
         this.emptySpaceItem1.Name = "emptySpaceItem1";
         this.emptySpaceItem1.Size = new System.Drawing.Size(497, 13);
         this.emptySpaceItem1.Text = "emptySpaceItem1";
         this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
         // 
         // layoutGroupTemplate
         // 
         this.layoutGroupTemplate.CustomizationFormText = "layoutGroupTemplate";
         this.layoutGroupTemplate.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemDescription,
            this.layoutItemTemplate});
         this.layoutGroupTemplate.Location = new System.Drawing.Point(0, 115);
         this.layoutGroupTemplate.Name = "layoutGroupTemplate";
         this.layoutGroupTemplate.Size = new System.Drawing.Size(497, 84);
         this.layoutGroupTemplate.Text = "layoutGroupTemplate";
         // 
         // layoutItemDescription
         // 
         this.layoutItemDescription.Control = this.lblTemplateDescription;
         this.layoutItemDescription.CustomizationFormText = "layoutItemDescription";
         this.layoutItemDescription.Location = new System.Drawing.Point(0, 24);
         this.layoutItemDescription.Name = "layoutItemDescription";
         this.layoutItemDescription.Size = new System.Drawing.Size(473, 17);
         this.layoutItemDescription.Text = "layoutItemDescription";
         this.layoutItemDescription.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemDescription.TextToControlDistance = 0;
         this.layoutItemDescription.TextVisible = false;
         // 
         // layoutItemTemplate
         // 
         this.layoutItemTemplate.Control = this.cbReportTemplates;
         this.layoutItemTemplate.CustomizationFormText = "layoutItemTemplate";
         this.layoutItemTemplate.Location = new System.Drawing.Point(0, 0);
         this.layoutItemTemplate.Name = "layoutItemTemplate";
         this.layoutItemTemplate.Size = new System.Drawing.Size(473, 24);
         this.layoutItemTemplate.Text = "layoutItemTemplate";
         this.layoutItemTemplate.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutGroupFirstPage
         // 
         this.layoutGroupFirstPage.CustomizationFormText = "layoutGroupFirstPage";
         this.layoutGroupFirstPage.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemTitle,
            this.layoutItemAuthor,
            this.layoutItemSubtitle});
         this.layoutGroupFirstPage.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupFirstPage.Name = "layoutGroupFirstPage";
         this.layoutGroupFirstPage.Size = new System.Drawing.Size(497, 115);
         this.layoutGroupFirstPage.Text = "layoutGroupFirstPage";
         // 
         // layoutItemTitle
         // 
         this.layoutItemTitle.Control = this.tbTitle;
         this.layoutItemTitle.CustomizationFormText = "layoutItemTitle";
         this.layoutItemTitle.Location = new System.Drawing.Point(0, 0);
         this.layoutItemTitle.Name = "layoutItemTitle";
         this.layoutItemTitle.Size = new System.Drawing.Size(473, 24);
         this.layoutItemTitle.Text = "layoutItemTitle";
         this.layoutItemTitle.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemAuthor
         // 
         this.layoutItemAuthor.Control = this.tbAuthor;
         this.layoutItemAuthor.CustomizationFormText = "layoutItemAuthor";
         this.layoutItemAuthor.Location = new System.Drawing.Point(0, 48);
         this.layoutItemAuthor.Name = "layoutItemAuthor";
         this.layoutItemAuthor.Size = new System.Drawing.Size(473, 24);
         this.layoutItemAuthor.Text = "layoutItemAuthor";
         this.layoutItemAuthor.TextSize = new System.Drawing.Size(96, 13);
         // 
         // layoutItemSubtitle
         // 
         this.layoutItemSubtitle.Control = this.tbSubtitle;
         this.layoutItemSubtitle.CustomizationFormText = "layoutItemSubtitle";
         this.layoutItemSubtitle.Location = new System.Drawing.Point(0, 24);
         this.layoutItemSubtitle.Name = "layoutItemSubtitle";
         this.layoutItemSubtitle.Size = new System.Drawing.Size(473, 24);
         this.layoutItemSubtitle.Text = "layoutItemSubtitle";
         this.layoutItemSubtitle.TextSize = new System.Drawing.Size(96, 13);
         // 
         // ReportingView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Caption = "ReportingView";
         this.ClientSize = new System.Drawing.Size(517, 466);
         this.Controls.Add(this.layoutControl1);
         this.Name = "ReportingView";
         this.Text = "ReportingView";
         this.Controls.SetChildIndex(this.layoutControlBase, 0);
         this.Controls.SetChildIndex(this.layoutControl1, 0);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlBase)).EndInit();
         this.layoutControlBase.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemOK)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCancel)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItemBase)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemExtra)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this._errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
         this.layoutControl1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.chkOpenReportAfterCreation.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbFont.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkDraft.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkDeleteWorkingFolder.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.cbReportTemplates.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkSaveReportArtifacts.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.rgColor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkVerbose.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbAuthor.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbSubtitle.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.tbTitle.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemColor)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemFont)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSaveReportArtifacts)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDeleteWorkingFolder)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupTemplate)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemDescription)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTemplate)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupFirstPage)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemTitle)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemAuthor)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSubtitle)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl1;
      private DevExpress.XtraLayout.LayoutControlGroup layoutControl;
      private DevExpress.XtraEditors.CheckEdit chkVerbose;
      private DevExpress.XtraEditors.TextEdit tbAuthor;
      private DevExpress.XtraEditors.TextEdit tbSubtitle;
      private DevExpress.XtraEditors.TextEdit tbTitle;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupFirstPage;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemTitle;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemAuthor;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSubtitle;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupOptions;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
      private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
      private DevExpress.XtraEditors.RadioGroup rgColor;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemColor;
      private DevExpress.XtraEditors.CheckEdit chkSaveReportArtifacts;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSaveReportArtifacts;
      private DevExpress.XtraEditors.LabelControl lblTemplateDescription;
      private DevExpress.XtraEditors.ComboBoxEdit cbReportTemplates;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupTemplate;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDescription;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemTemplate;
      private DevExpress.XtraEditors.CheckEdit chkDeleteWorkingFolder;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemDeleteWorkingFolder;
      private DevExpress.XtraEditors.CheckEdit chkDraft;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
      private DevExpress.XtraEditors.ComboBoxEdit cbFont;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemFont;
      private DevExpress.XtraEditors.CheckEdit chkOpenReportAfterCreation;
      private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;

   }
}