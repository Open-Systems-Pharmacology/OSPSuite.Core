using System;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   public partial class SensitivityAnalysisFeedbackView : BaseToggleableView, ISensitivityAnalysisFeedbackView
   {
      private readonly LabelControl _lblInfo;

      public SensitivityAnalysisFeedbackView()
      {
         InitializeComponent();
         _lblInfo = new LabelControl { Parent = this };
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.SensitivityAnalysis.FeedbackView;
         Icon = ApplicationIcons.SensitivityAnalysisVisualFeedback;

         _lblInfo.AsFullViewText(Captions.SensitivityAnalysis.NoSensitivityAnalysisRunning);
         _lblInfo.Left = 5;

         showFeedback = false;

         progressBarControl.Properties.Minimum = 0;
         progressBarControl.Properties.ShowTitle = true;
         layoutControlItemProgressBar.TextLocation = Locations.Bottom;
         MaximizeBox = false;
      }

      private bool showFeedback
      {
         set
         {
            layoutControl.Visible = value;
            _lblInfo.Visible = !layoutControl.Visible;
         }
      }

      protected override void OnSizeChanged(EventArgs e)
      {
         base.OnSizeChanged(e);
         if (_lblInfo == null) return;
         _lblInfo.Width = Width - _lblInfo.Left;
         _lblInfo.Top = 0;
      }

      public void AttachPresenter(ISensitivityAnalysisFeedbackPresenter presenter)
      {
         _presenter = presenter;
      }

      public void ShowFeedback()
      {
         showFeedback = true;
      }

      public void UpdateProgress(int progressAmount, int totalAmount)
      {
         progressBarControl.Properties.Maximum = totalAmount;
         progressBarControl.Position = progressAmount;
         layoutControlItemProgressBar.Text = Captions.SensitivityAnalysis.SensitivityProgress(progressAmount, totalAmount);
      }

      public void ResetFeedback()
      {
         layoutControlItemProgressBar.Text = Captions.SensitivityAnalysis.SensitivityHasNotBeenUpdated;
         progressBarControl.Position = 0;
      }
   }
}
