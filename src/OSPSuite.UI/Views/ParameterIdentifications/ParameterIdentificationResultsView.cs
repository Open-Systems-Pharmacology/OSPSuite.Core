using System.Drawing;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.ParameterIdentifications
{
   public partial class ParameterIdentificationResultsView : BaseUserControl, IParameterIdentificationResultsView
   {
      private IParameterIdentificationResultsPresenter _presenter;
      private readonly LabelControl _lblInfo;
      public override string Caption => Captions.ParameterIdentification.Results;
      public override ApplicationIcon ApplicationIcon => ApplicationIcons.Results;

      public ParameterIdentificationResultsView()
      {
         InitializeComponent();
         _lblInfo = new LabelControl {Parent = this};
      }

      public void AttachPresenter(IParameterIdentificationResultsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void ShowResultsView(IView view)
      {
         showResult = true;
         panelResult.FillWith(view);
      }

      public void HideResultsView()
      {
         showResult = false;
      }

      private bool showResult
      {
         set
         {
            layoutControl.Visible = value;
            _lblInfo.Visible = !layoutControl.Visible;
         }
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         _lblInfo.Font = new Font(_lblInfo.Font.Name, 15.0f);
         _lblInfo.Text = Captions.ParameterIdentification.NoResultsAvailable;
         _lblInfo.AutoSizeMode = LabelAutoSizeMode.Vertical;
         _lblInfo.Width = 400;
         _lblInfo.AsDescription();
         _lblInfo.BackColor = Color.Transparent;
         _lblInfo.Top = 200;
         _lblInfo.Left = 200;
      }
   }
}