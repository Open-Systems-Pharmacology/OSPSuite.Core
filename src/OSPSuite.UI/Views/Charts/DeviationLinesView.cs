using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Charts
{
   public partial class DeviationLinesView : BaseModalView, IDeviationLinesView
   {
      private readonly ScreenBinder<FoldValueDTO> _screenBinder;
      private IDeviationLinesPresenter _presenter;
      public DeviationLinesView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<FoldValueDTO>();
         foldValueInputControlItem.TextVisible = false;
         foldValueTextLabelControl.Text = Captions.Chart.DeviationLines.SpecifyFoldValue.FormatForLabel();
         deviationLineDescriptionLabelControl.AsDescription();
         deviationLineDescriptionLabelControl.Text = Captions.Chart.DeviationLines.DeviationLineDescription.FormatForDescription();
         foldValueTextEdit.Select();
      }

      public void AttachPresenter(IDeviationLinesPresenter presenter)
      {
         _presenter = presenter;
      }

      
      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _screenBinder.Bind(x => x.FoldValue).To(foldValueTextEdit); 
         RegisterValidationFor(_screenBinder, NotifyViewChanged);
      }

      public void BindTo(FoldValueDTO foldValueDTO)
      {
         _screenBinder.BindToSource(foldValueDTO);
      }

      public override bool HasError => _screenBinder.HasError;
   }
}