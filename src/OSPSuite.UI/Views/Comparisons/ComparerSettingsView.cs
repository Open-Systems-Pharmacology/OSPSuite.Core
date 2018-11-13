using OSPSuite.Assets;
using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.Core.Comparison;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Comparisons;
using OSPSuite.Presentation.Views.Comparisons;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Comparisons
{
   public partial class ComparerSettingsView : BaseUserControl, IComparerSettingsView
   {
      private IComparerSettingsPresenter _presenter;
      private readonly ScreenBinder<ComparerSettings> _screenBinder;

      public ComparerSettingsView()
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<ComparerSettings>();
      }

      public void AttachPresenter(IComparerSettingsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindTo(ComparerSettings comparerSettings)
      {
         _screenBinder.BindToSource(comparerSettings);
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.RelativeTolerance)
            .To(tbRelativeTolerance);

         _screenBinder.Bind(x => x.OnlyComputingRelevant)
            .To(chkOnlyComputeModelRelevantProperties)
            .WithCaption(Captions.Comparisons.OnlyComputeModelRelevantProperties);

         _screenBinder.Bind(x => x.CompareHiddenEntities)
            .To(chkCompareHiddenEntities)
            .WithCaption(Captions.Comparisons.CompareHiddenEntities);

         _screenBinder.Bind(x => x.ShowValueOrigin)
            .To(chkShowValueOrigin)
            .WithCaption(Captions.Comparisons.ShowValueOriginForChangedValues);
     
         _screenBinder.Bind(x => x.FormulaComparison)
            .To(cbFormulaComparisonMode)
            .WithValues(x => _presenter.FormulaComparisonValues)
            .AndDisplays(_presenter.FormulaComparisonDisplayValueFor);

         RegisterValidationFor(_screenBinder);
      }

      public virtual void SaveChanges()
      {
         //this releases any control that has focus and thus forces binding
         ActiveControl = null;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemRelativeTolerance.Text = Captions.Comparisons.RelativeTolerance.FormatForLabel();
         lblRelativeToleranceDescription.AsDescription();
         lblRelativeToleranceDescription.Text = Captions.Comparisons.RelativeToleranceDescription.FormatForDescription();

         layoutItemFormulaComparisonMode.Text = Captions.Comparisons.FormulaComparisonMode.FormatForLabel();
         lblFormulaComparisonModeDescription.AsDescription();
         lblFormulaComparisonModeDescription.Text = Captions.Comparisons.FormulaComparisonModeDescription.FormatForDescription();
      }
   }
}