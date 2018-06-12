using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views.Journal
{
   partial class JournalSearchView
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.layoutControl = new OSPSuite.UI.Controls.UxLayoutControl();
         this.tbSearch = new UxMRUEdit();
         this.chkCaseSensitive = new OSPSuite.UI.Controls.UxCheckEdit();
         this.btnClear = new DevExpress.XtraEditors.SimpleButton();
         this.btnFind = new DevExpress.XtraEditors.SimpleButton();
         this.chkMatchWholeWord = new OSPSuite.UI.Controls.UxCheckEdit();
         this.chkMatchAny = new OSPSuite.UI.Controls.UxCheckEdit();
         this.layoutGroupSearch = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutGroupOptions = new DevExpress.XtraLayout.LayoutControlGroup();
         this.layoutItemMatchAny = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemMatchWholeWord = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemCaseSensitive = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemButtonSearch = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemButtonClear = new DevExpress.XtraLayout.LayoutControlItem();
         this.layoutItemSearch = new DevExpress.XtraLayout.LayoutControlItem();
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).BeginInit();
         this.layoutControl.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.tbSearch.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkCaseSensitive.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkMatchWholeWord.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkMatchAny.Properties)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupSearch)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupOptions)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMatchAny)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMatchWholeWord)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCaseSensitive)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonSearch)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonClear)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSearch)).BeginInit();
         this.SuspendLayout();
         // 
         // layoutControl
         // 
         this.layoutControl.Controls.Add(this.tbSearch);
         this.layoutControl.Controls.Add(this.chkCaseSensitive);
         this.layoutControl.Controls.Add(this.btnClear);
         this.layoutControl.Controls.Add(this.btnFind);
         this.layoutControl.Controls.Add(this.chkMatchWholeWord);
         this.layoutControl.Controls.Add(this.chkMatchAny);
         this.layoutControl.Dock = System.Windows.Forms.DockStyle.Fill;
         this.layoutControl.Location = new System.Drawing.Point(0, 0);
         this.layoutControl.Name = "layoutControl";
         this.layoutControl.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(722, 36, 250, 350);
         this.layoutControl.Root = this.layoutGroupSearch;
         this.layoutControl.Size = new System.Drawing.Size(385, 71);
         this.layoutControl.TabIndex = 0;
         this.layoutControl.Text = "layoutControl1";
         // 
         // tbSearch
         // 
         this.tbSearch.Location = new System.Drawing.Point(90, 2);
         this.tbSearch.Name = "tbSearch";
         this.tbSearch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
         this.tbSearch.Size = new System.Drawing.Size(101, 20);
         this.tbSearch.StyleController = this.layoutControl;
         this.tbSearch.TabIndex = 11;
         // 
         // chkCaseSensitive
         // 
         this.chkCaseSensitive.Location = new System.Drawing.Point(222, 28);
         this.chkCaseSensitive.Name = "chkCaseSensitive";
         this.chkCaseSensitive.Properties.Caption = "chkCaseSensitive";
         this.chkCaseSensitive.Size = new System.Drawing.Size(144, 19);
         this.chkCaseSensitive.StyleController = this.layoutControl;
         this.chkCaseSensitive.TabIndex = 10;
         // 
         // btnClear
         // 
         this.btnClear.Location = new System.Drawing.Point(314, 2);
         this.btnClear.Name = "btnClear";
         this.btnClear.Size = new System.Drawing.Size(52, 22);
         this.btnClear.StyleController = this.layoutControl;
         this.btnClear.TabIndex = 9;
         this.btnClear.Text = "btnClear";
         // 
         // btnFind
         // 
         this.btnFind.Location = new System.Drawing.Point(195, 2);
         this.btnFind.Name = "btnFind";
         this.btnFind.Size = new System.Drawing.Size(115, 22);
         this.btnFind.StyleController = this.layoutControl;
         this.btnFind.TabIndex = 8;
         this.btnFind.Text = "btnFind";
         // 
         // chkMatchWholeWord
         // 
         this.chkMatchWholeWord.Location = new System.Drawing.Point(2, 51);
         this.chkMatchWholeWord.Name = "chkMatchWholeWord";
         this.chkMatchWholeWord.Properties.Caption = "chkMatchWholeWord";
         this.chkMatchWholeWord.Size = new System.Drawing.Size(364, 19);
         this.chkMatchWholeWord.StyleController = this.layoutControl;
         this.chkMatchWholeWord.TabIndex = 7;
         // 
         // chkMatchAny
         // 
         this.chkMatchAny.Location = new System.Drawing.Point(2, 28);
         this.chkMatchAny.Name = "chkMatchAny";
         this.chkMatchAny.Properties.Caption = "chkMatchAny";
         this.chkMatchAny.Size = new System.Drawing.Size(216, 19);
         this.chkMatchAny.StyleController = this.layoutControl;
         this.chkMatchAny.TabIndex = 6;
         // 
         // layoutGroupSearch
         // 
         this.layoutGroupSearch.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
         this.layoutGroupSearch.GroupBordersVisible = false;
         this.layoutGroupSearch.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutGroupOptions,
            this.layoutItemButtonSearch,
            this.layoutItemButtonClear,
            this.layoutItemSearch});
         this.layoutGroupSearch.Location = new System.Drawing.Point(0, 0);
         this.layoutGroupSearch.Name = "layoutGroupSearch";
         this.layoutGroupSearch.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
         this.layoutGroupSearch.Size = new System.Drawing.Size(368, 72);
         this.layoutGroupSearch.TextVisible = false;
         // 
         // layoutGroupOptions
         // 
         this.layoutGroupOptions.GroupBordersVisible = false;
         this.layoutGroupOptions.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutItemMatchAny,
            this.layoutItemMatchWholeWord,
            this.layoutItemCaseSensitive});
         this.layoutGroupOptions.Location = new System.Drawing.Point(0, 26);
         this.layoutGroupOptions.Name = "layoutGroupOptions";
         this.layoutGroupOptions.Size = new System.Drawing.Size(368, 46);
         // 
         // layoutItemMatchAny
         // 
         this.layoutItemMatchAny.Control = this.chkMatchAny;
         this.layoutItemMatchAny.Location = new System.Drawing.Point(0, 0);
         this.layoutItemMatchAny.Name = "layoutItemMatchAny";
         this.layoutItemMatchAny.Size = new System.Drawing.Size(220, 23);
         this.layoutItemMatchAny.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemMatchAny.TextVisible = false;
         // 
         // layoutItemMatchWholeWord
         // 
         this.layoutItemMatchWholeWord.Control = this.chkMatchWholeWord;
         this.layoutItemMatchWholeWord.Location = new System.Drawing.Point(0, 23);
         this.layoutItemMatchWholeWord.Name = "layoutItemMatchWholeWord";
         this.layoutItemMatchWholeWord.Size = new System.Drawing.Size(368, 23);
         this.layoutItemMatchWholeWord.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemMatchWholeWord.TextVisible = false;
         // 
         // layoutItemCaseSensitive
         // 
         this.layoutItemCaseSensitive.Control = this.chkCaseSensitive;
         this.layoutItemCaseSensitive.Location = new System.Drawing.Point(220, 0);
         this.layoutItemCaseSensitive.Name = "layoutItemCaseSensitive";
         this.layoutItemCaseSensitive.Size = new System.Drawing.Size(148, 23);
         this.layoutItemCaseSensitive.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemCaseSensitive.TextVisible = false;
         // 
         // layoutItemButtonSearch
         // 
         this.layoutItemButtonSearch.Control = this.btnFind;
         this.layoutItemButtonSearch.Location = new System.Drawing.Point(193, 0);
         this.layoutItemButtonSearch.Name = "layoutItemButtonSearch";
         this.layoutItemButtonSearch.Size = new System.Drawing.Size(119, 26);
         this.layoutItemButtonSearch.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonSearch.TextVisible = false;
         // 
         // layoutItemButtonClear
         // 
         this.layoutItemButtonClear.Control = this.btnClear;
         this.layoutItemButtonClear.Location = new System.Drawing.Point(312, 0);
         this.layoutItemButtonClear.Name = "layoutItemButtonClear";
         this.layoutItemButtonClear.Size = new System.Drawing.Size(56, 26);
         this.layoutItemButtonClear.TextSize = new System.Drawing.Size(0, 0);
         this.layoutItemButtonClear.TextVisible = false;
         // 
         // layoutItemSearch
         // 
         this.layoutItemSearch.Control = this.tbSearch;
         this.layoutItemSearch.Location = new System.Drawing.Point(0, 0);
         this.layoutItemSearch.Name = "layoutItemSearch";
         this.layoutItemSearch.Size = new System.Drawing.Size(193, 26);
         this.layoutItemSearch.TextSize = new System.Drawing.Size(85, 13);
         // 
         // JournalSearchView
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.layoutControl);
         this.Name = "JournalSearchView";
         this.Size = new System.Drawing.Size(385, 71);
         ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutControl)).EndInit();
         this.layoutControl.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.tbSearch.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkCaseSensitive.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkMatchWholeWord.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.chkMatchAny.Properties)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupSearch)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutGroupOptions)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMatchAny)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemMatchWholeWord)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemCaseSensitive)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonSearch)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemButtonClear)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.layoutItemSearch)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private DevExpress.XtraLayout.LayoutControl layoutControl;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupSearch;
      private DevExpress.XtraEditors.CheckEdit chkMatchAny;
      private DevExpress.XtraLayout.LayoutControlGroup layoutGroupOptions;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMatchAny;
      private DevExpress.XtraEditors.CheckEdit chkMatchWholeWord;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemMatchWholeWord;
      private DevExpress.XtraEditors.SimpleButton btnClear;
      private DevExpress.XtraEditors.SimpleButton btnFind;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonSearch;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemButtonClear;
      private DevExpress.XtraEditors.CheckEdit chkCaseSensitive;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemCaseSensitive;
      private UxMRUEdit tbSearch;
      private DevExpress.XtraLayout.LayoutControlItem layoutItemSearch;
   }
}
