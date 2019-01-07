using System.Data;
using DevExpress.Data.PivotGrid;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraPivotGrid;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.SensitivityAnalyses
{
   public partial class SensitivityAnalysisResultsView : BaseUserControl, ISensitivityAnalysisResultsView
   {
      private readonly IToolTipCreator _toolTipCreator;
      private ISensitivityAnalysisResultsPresenter _presenter;
      private readonly LabelControl _lblInfo;
      private PivotGridField _pkParameterField;
      public override string Caption => Captions.SensitivityAnalysis.Results;
      public override ApplicationIcon ApplicationIcon => ApplicationIcons.Results;

      public SensitivityAnalysisResultsView(IImageListRetriever imageListRetriever, IToolTipCreator toolTipCreator)
      {
         _toolTipCreator = toolTipCreator;
         InitializeComponent();
         _lblInfo = new LabelControl {Parent = this};
         pivotGrid.ToolTipController = new ToolTipController();

         configureFields();

         pivotGrid.OptionsView.ShowColumnHeaders = true;
         pivotGrid.OptionsView.ShowRowHeaders = true;

         var toolTipController = new ToolTipController();
         toolTipController.Initialize(imageListRetriever);
         toolTipController.GetActiveObjectInfo += (o, e) => OnEvent(onToolTipControllerGetActiveObjectInfo, e);
         pivotGrid.ToolTipController = toolTipController;
      }

      private void onToolTipControllerGetActiveObjectInfo(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         if (e.SelectedControl != pivotGrid) return;

         var hi = pivotGrid.CalcHitInfo(e.ControlMousePosition);
         if (hi.HitTest != PivotGridHitTest.Value)
            return;

         var ds = hi.ValueInfo.CreateDrillDownDataSource();

         if (hi.ValueInfo.Field == pivotGrid.ParameterField)
            e.Info = toolTipForParameter(ds);

         if (hi.ValueInfo.Field == _pkParameterField)
            e.Info = toolTipForPKParameter(ds);
      }

      private ToolTipControlInfo toolTipForPKParameter(PivotDrillDownDataSource ds)
      {
         return toolTipFor(ds, Captions.SensitivityAnalysis.PKParameterName, Captions.SensitivityAnalysis.PKParameterDescription, ApplicationIcons.PKAnalysis);
      }

      private ToolTipControlInfo toolTipForParameter(PivotDrillDownDataSource ds)
      {
         return toolTipFor(ds, Captions.SensitivityAnalysis.ParameterName, Captions.SensitivityAnalysis.ParameterDisplayPath, ApplicationIcons.Parameter);
      }

      private ToolTipControlInfo toolTipFor(PivotDrillDownDataSource ds, string nameField, string descriptionField, ApplicationIcon icon)
      {
         var name = ds.StringValue(nameField);
         var description = ds.StringValue(descriptionField);
         var superToolTip = _toolTipCreator.CreateToolTip(description, name, icon);
         return _toolTipCreator.ToolTipControlInfoFor(name, superToolTip);
      }

      private void configureFields()
      {
         var ouputField = pivotGrid.CreateColumnAreaNamed(Captions.SensitivityAnalysis.Output);
         pivotGrid.AddField(ouputField);

         _pkParameterField = pivotGrid.CreateColumnAreaNamed(Captions.SensitivityAnalysis.PKParameterName);
         pivotGrid.AddField(_pkParameterField);

         var parameterField = pivotGrid.CreateRowAreaNamed(Captions.SensitivityAnalysis.ParameterName);
         pivotGrid.AddParameterField(parameterField);
         var valueField = pivotGrid.CreateDataAreaNamed(Captions.SensitivityAnalysis.Value);
         valueField.SummaryType = PivotSummaryType.Custom;
         pivotGrid.AddValueField(valueField);
      }

      public void AttachPresenter(ISensitivityAnalysisResultsPresenter presenter)
      {
         _presenter = presenter;
      }

      public void HideResultsView()
      {
         showResult = false;
      }

      public void BindTo(DataTable dataTable)
      {
         showResult = true;
         pivotGrid.DataSource = dataTable;
      }

      public DataTable GetSummaryData()
      {
         return pivotGrid.GetCellsSummary();
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
         _lblInfo.AsFullViewText(Captions.SensitivityAnalysis.NoResultsAvailable);
         layoutItemParameters.TextVisible = false;
         btnExportToExcel.InitWithImage(ApplicationIcons.Excel, text: Captions.SensitivityAnalysis.ExportPKAnalysesSentitivityToExcel);
         layoutItemExportToExcel.AdjustLargeButtonSize();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         btnExportToExcel.Click += (o, e) => OnEvent(_presenter.ExportToExcel);
      }
   }
}