using System;
using System.Drawing;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationFeedbackView : BaseToggleableView, IParameterIdentificationFeedbackView
   {
      private readonly LabelControl _lblInfo;
      private readonly ScreenBinder<IParameterIdentificationFeedbackPresenter> _screenBinder;

      public ParameterIdentificationFeedbackView()
      {
         InitializeComponent();
         _lblInfo = new LabelControl {Parent = this};
         _screenBinder = new ScreenBinder<IParameterIdentificationFeedbackPresenter>();
      }

      public void AttachPresenter(IParameterIdentificationFeedbackPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();

         _screenBinder.Bind(x => x.ShouldRefreshFeedback)
            .To(chkRefreshFeedback)
            .WithCaption(Captions.ParameterIdentification.RefreshFeedback);
      }

      public void NoFeedbackAvailable()
      {
         showFeedback = false;
      }

      public void ShowFeedbackView(IView view)
      {
         showFeedback = true;
         panelContent.FillWith(view);
      }

      public void BindToProperties()
      {
         _screenBinder.BindToSource(feedbackPresenter);
      }

      private bool showFeedback
      {
         set
         {
            layoutControl.Visible = value;
            _lblInfo.Visible = !layoutControl.Visible;
         }
      }

      private IParameterIdentificationFeedbackPresenter feedbackPresenter => _presenter.DowncastTo<IParameterIdentificationFeedbackPresenter>();

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.ParameterIdentification.FeedbackView;
         Icon = ApplicationIcons.ParameterIdentificationVisualFeedback;

         _lblInfo.Font = new Font(_lblInfo.Font.Name, 15.0f);
         _lblInfo.Text = Captions.ParameterIdentification.NoParameterIdentificationRunning;
         _lblInfo.AutoSizeMode = LabelAutoSizeMode.Vertical;
         _lblInfo.AsDescription();
         _lblInfo.BackColor = Color.Transparent;
         _lblInfo.Left = 200;
      }

      protected override void OnSizeChanged(EventArgs e)
      {
         base.OnSizeChanged(e);
         if (_lblInfo == null) return;
         _lblInfo.Width = Width - _lblInfo.Left;
         _lblInfo.Top = Height / 2;
      }
   }
}