using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;
using Process = System.Diagnostics.Process;

namespace OSPSuite.UI.Views
{
   public partial class ExceptionView : XtraForm, IExceptionView
   {
      private string _supportEmail;
      private string _assemblyInfo;
      private const int _maxEmailSize = 1964;
      private const string _couldNotCopyToClipboard = "Unable to copy the information to the clipboard.";
      public object MainView { private get; set; }

      public ExceptionView()
      {
         InitializeComponent();
         initializeResources();
         btnSendEmail.Click += (o, e) => sendEmail();
         btnCopyToClipboard.Click += (o, e) => copyToClipboard();
      }

      private void initializeResources()
      {
         layoutItemException.TextVisible = false;
         layoutItemFullException.TextVisible = false;
         tbException.Properties.ReadOnly = true;
         tbFullException.Properties.ReadOnly = true;
         lblDescription.AutoSizeMode = LabelAutoSizeMode.Vertical;
         lblDescription.AllowHtmlString = true;
         MinimizeBox = false;
         MaximizeBox = false;
         btnSendEmail.Text = "Send Email...";
         btnCopyToClipboard.Text = "Copy to Clipboard";
         btnOk.Text = "Close";
         layoutItemOk.AdjustButtonSize();
         layoutItemCopyToClipbord.AdjustButtonSize();
         layoutItemSendEmail.AdjustButtonSize();
         Description = Captions.ExceptionViewDescription(Constants.SUPPORT_EMAIL);
      }

      public string Description
      {
         set
         {
            layoutItemDescription.Visibility = LayoutVisibilityConvertor.FromBoolean(!string.IsNullOrEmpty(value));
            lblDescription.Text = value;
         }
      }

      public void Initialize(string caption, ApplicationIcon icon, string emailSubject, string supportEmail)
      {
         Initialize(caption, icon, emailSubject, supportEmail, "Exception", "Stack Trace");
      }

      public void Initialize(string caption, ApplicationIcon icon, string emailSubject, string supportEmail, string exceptionDisplay, string fullStackTraceDisplay)
      {
         Text = caption;
         Icon = icon;
         layoutGroupException.Text = exceptionDisplay;
         layoutGroupStackTraceException.Text = fullStackTraceDisplay;
         _assemblyInfo = emailSubject;
         _supportEmail = supportEmail;
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
         Clipboard.SetText(fullContent());
      }

      private string fullContent()
      {
         return string.Format("{0}\n\nStack trace:\n{1}", ExceptionMessage, FullStackTrace);
      }

      private void showException(Exception ex)
      {
         showException(ex.FullMessage());
      }

      private void showException(string message)
      {
         XtraMessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      private void sendEmail()
      {
         try
         {

            //0A makes new line available in outlook!
            var fullMessage = fullContent().Replace(Environment.NewLine, "%0A");

            if(fullMessage.Length > _maxEmailSize)
            {
               fullMessage = string.Format("Please copy the error content using the '{0}' button.", btnCopyToClipboard.Text);
            }

            var psi = new ProcessStartInfo(string.Format("mailto:{0}?subject={1}&body={2}", _supportEmail, _assemblyInfo, fullMessage));
            Process.Start(psi);
         }
         catch (Exception ex)
         {
            showException(ex);
         }
      }

      public string ExceptionMessage
      {
         set { tbException.Text = value; }
         private get { return tbException.Text; }
      }

      public string FullStackTrace
      {
         set { tbFullException.Text = value; }
         private get { return tbFullException.Text; }
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

      public void Display(Exception exception)
      {
         ExceptionMessage = exception.FullMessage();
         FullStackTrace = exception.FullStackTrace();
         Display();
      }
   }
}