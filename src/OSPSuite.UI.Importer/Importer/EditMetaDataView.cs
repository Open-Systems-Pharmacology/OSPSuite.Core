using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Importer
{
   public partial class EditMetaDataView : XtraForm
   {
      private readonly MetaDataEditControl _metaDataControl;

      public EditMetaDataView(MetaDataTable data)
      {
         InitializeComponent();

         Text = Captions.Importer.PleaseEnterMetaDataInformation;
         btnOK.Click += onOkClick;
         btnCopy.Click += onCopyClick;

         int rowIndex = data.Rows.Count > 0 ? 0 : -1;
         _metaDataControl = new MetaDataEditControl(data, rowIndex, false);
         panelControl.Controls.Add(_metaDataControl);
         _metaDataControl.Dock = DockStyle.Fill;

         _metaDataControl.OnIsDataValidChanged += onIsDataValidChanged;
         enableButtons();
         if (ParentForm != null) 
            Icon = ParentForm.Icon;
      }

      /// <summary>
      /// Handler for event OnCopyMetaData.
      /// </summary>
      public delegate void CopyMetaDataHandler(object sender, EventArgs e);

      /// <summary>
      /// Event raised when a user clicks on the copy button.
      /// </summary>
      public event CopyMetaDataHandler OnCopyMetaData;

      /// <summary>
      /// The text of the view can be changed.
      /// </summary>
      public override sealed string Text
      {
         get { return base.Text; }
         set { base.Text = value; }
      }

      /// <summary>
      /// Method reacting on data changes of the underlying control.
      /// </summary>
      void onIsDataValidChanged(object sender, EventArgs e)
      {
         enableButtons();
      }

      /// <summary>
      /// Method toggling buttons.
      /// </summary>
      private void enableButtons()
      {
         btnOK.Enabled = _metaDataControl.IsDataValid;
         btnCopy.Enabled = btnOK.Enabled;
      }

      /// <summary>
      /// Method which accepts the entered data if the user clicks on OK button.
      /// </summary>
      void onOkClick(object sender, EventArgs e)
      {
         this.DoWithinExceptionHandler(_metaDataControl.AcceptDataChanges);
      }

      /// <summary>
      /// Method which accepts the entered data if the user clicks on Copy (Apply to others) button.
      /// </summary>
      void onCopyClick(object sender, EventArgs e)
      {
         this.DoWithinExceptionHandler(_metaDataControl.AcceptDataChanges);

         if (OnCopyMetaData != null)
            OnCopyMetaData(this, new EventArgs());
      }

      private void onCancelClick(object sender, EventArgs e)
      {
         this.DoWithinExceptionHandler(_metaDataControl.RejectDataChanges);
      }

      private void cleanMemory()
      {
         _metaDataControl.Dispose();
         CleanUpHelper.ReleaseControls(Controls);
         Controls.Clear();
      }

   }
}