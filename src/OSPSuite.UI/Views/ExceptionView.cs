using System;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class ExceptionView : XtraForm, IExceptionView
   {
      private string _issueTrackerUrl;
      private string _cliboardContent;
      private const string _couldNotCopyToClipboard = "Unable to copy the information to the clipboard.";
      public object MainView { private get; set; }

      public ExceptionView()
      {
         InitializeComponent();
         initializeResources();
         btnCopyToClipboard.Click += (o, e) => copyToClipboard();
      }

      private void initializeResources()
      {
         layoutItemException.TextVisible = false;
         layoutItemFullException.TextVisible = false;
         tbException.Properties.ReadOnly = true;
         tbFullException.Properties.ReadOnly = true;

         MinimizeBox = false;
         MaximizeBox = false;
         btnCopyToClipboard.Text = Captions.CopyToClipboard;
         btnClose.Text = Captions.CloseButton;
         layoutItemOk.AdjustButtonSize();
         layoutItemCopyToClipbord.AdjustButtonSize();
         layoutGroupException.Text = Captions.Exception;
         layoutGroupStackTraceException.Text = Captions.StackTrace;
         issueTrackerLink.OpenLink += (o, e) => goToIssueTracker(e);
         ActiveControl = btnClose;
         labelDescription.AsDescription();
      }

      private void goToIssueTracker(OpenLinkEventArgs e)
      {
         e.EditValue = _issueTrackerUrl;
      }

      public string Description
      {
         set
         {
            layoutItemDescription.Visibility = LayoutVisibilityConvertor.FromBoolean(!string.IsNullOrEmpty(value));
            labelDescription.Text = value;
         }
      }

      public void Initialize(string caption, ApplicationIcon icon, string issueTrackerUrl, string productName)
      {
         Text = caption;
         Icon = icon;
         _issueTrackerUrl = issueTrackerUrl;
         Description = Captions.ExceptionViewDescription(issueTrackerUrl, html:false);
         issueTrackerLink.Text = Captions.IssueTrackerLinkFor(productName);
      }

      private void copyToClipboard()
      {
         try
         {
            invokeOnSTAThread(copyToClipboardOnUIThread);
         }
         catch (Exception)
         {
            showException(_couldNotCopyToClipboard);
         }
      }

      private void invokeOnSTAThread(ThreadStart method)
      {
         if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
         {
            Thread thread = new Thread(method);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
         }
         else
         {
            method();
         }
      }

      private void copyToClipboardOnUIThread()
      {
         Clipboard.SetText(_cliboardContent);
      }

      private void showException(string message)
      {
         XtraMessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      public string ExceptionMessage
      {
         set => tbException.Text = value;
      }

      public string FullStackTrace
      {
         set => tbFullException.Text = value;
      }

      public void Display()
      {
         if (MainView == null)
         {
            ShowDialog();
            return;
         }

         showDialogWithOwner();
      }

      private void showDialogWithOwner()
      {
         try
         {
            ShowDialog((Form) MainView);
         }
         catch (Exception)
         {
            ShowDialog();
         }
      }

      public void Display(string message, string stackTrace, string clipboardContent)
      {
         ExceptionMessage = message;
         FullStackTrace = stackTrace;
         _cliboardContent = clipboardContent;
         Display();
      }
   }
}