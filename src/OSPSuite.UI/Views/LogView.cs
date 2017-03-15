using System.Collections.Generic;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   public partial class LogView : BaseUserControl, ILogView
   {
      private ILogPresenter _presenter;
      private readonly ScreenBinder<MessageStatusFilterDTO> _screenBinder;

      public LogView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<MessageStatusFilterDTO>();
      }

      public void AttachPresenter(ILogPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddLog(string log)
      {
         var lines = new List<string>(tbLog.Lines) {log};
         tbLog.Lines = lines.ToArray();
         tbLog.SelectionStart = tbLog.Text.Length;
         tbLog.ScrollToCaret();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.Error).To(chkError);
         _screenBinder.Bind(x => x.Debug).To(chkDebug);
         _screenBinder.Bind(x => x.Info).To(chkInfo);
         _screenBinder.Bind(x => x.Warning).To(chkWarning);
      }

      public void ClearLog()
      {
         tbLog.ResetText();
      }

      public void BindTo(MessageStatusFilterDTO statusFilter)
      {
         _screenBinder.BindToSource(statusFilter);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         chkDebug.Text = "Debug";
         chkError.Text = "Error";
         chkInfo.Text = "Info";
         chkWarning.Text = "Warming";
      }
   }
}