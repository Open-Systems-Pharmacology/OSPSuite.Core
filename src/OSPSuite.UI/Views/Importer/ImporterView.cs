using DevExpress.XtraTab;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImporterView : BaseUserControl, IImporterView
   {
      private IImporterPresenter _presenter;
      private XtraTabPage _previewTabPage;
      private const int DATA_PAGE_INDEX = 0;
      private const int DIMENSION_PAGE_INDEX = 1;
      private const int CONFIRMATION_PAGE_INDEX = 2;

      public ImporterView()
      {
         InitializeComponent();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         sourceFileLayoutControlGroup.Text = Captions.Importer.SourceLayout;
         previewLayoutControlItem.Name = Captions.Importer.PreviewLayout;
         mappingLayoutControlGroup.Text = Captions.Importer.MappingSettings;


         saveMappingBtn.Click += (o, e) => OnEvent(_presenter.SaveConfiguration);
         applyMappingBtn.Click += (o, e) => OnEvent(_presenter.LoadConfigurationWithoutImporting);
         resetMappingBasedOnCurrentSheetBtn.Click += (o, e) => OnEvent(_presenter.ResetMappingBasedOnCurrentSheet);
         clearMappingBtn.Click += (o, e) => OnEvent(_presenter.ClearMapping);

         saveMappingBtn.InitWithImage(ApplicationIcons.Save, Captions.Importer.SaveConfiguration);
         applyMappingBtn.InitWithImage(ApplicationIcons.Load, Captions.Importer.ApplyConfiguration);
         resetMappingBasedOnCurrentSheetBtn.InitWithImage(ApplicationIcons.Refresh, Captions.Importer.ResetMapping);
         clearMappingBtn.InitWithImage(ApplicationIcons.RedCross, Captions.Importer.ClearMapping);
         
         rootLayoutControl.BeginUpdate();
         saveMappingBtnLayoutControlItem.AdjustLargeButtonSize();
         applyMappingLayoutControlItem.AdjustLargeButtonSize();
         resetMappingBasedOnCurrentSheetLayoutControlItem.AdjustLongButtonSize();
         clearMappingLayoutControlItem.AdjustLongButtonSize();
         sourceFileLayoutControlItem.AdjustControlHeight(70);
         rootLayoutControl.EndUpdate();

         resetMappingBasedOnCurrentSheetBtn.ToolTip = Captions.Importer.ResetMappingToolTip;
         clearMappingBtn.ToolTip = Captions.Importer.ClearMappingToolTip;
      }

      public void AttachPresenter(IImporterPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddImporterView(IImporterDataView importerDataView)
      {
         previewXtraTabControl.AddPageFrom(importerDataView, DATA_PAGE_INDEX);
      }

      public void AddSourceFileControl(ISourceFileControl sourceFileControl)
      {
         sourceFilePanelControl.FillWith(sourceFileControl);
      }

      public void AddColumnMappingControl(IColumnMappingView columnMappingView)
      {
         columnMappingPanelControl.FillWith(columnMappingView);
      }

      public void AddPreviewView(IImportPreviewView previewView)
      {
         _previewTabPage = previewXtraTabControl.AddPageFrom(previewView, CONFIRMATION_PAGE_INDEX);
      }

      public void EnablePreviewView()
      {
         if (_previewTabPage.PageEnabled)
            return;

         _previewTabPage.PageEnabled = true;
         _previewTabPage.PageVisible = true;
         previewXtraTabControl.SelectedTabPage = _previewTabPage;
      }

      public void DisableConfirmationView()
      {
         _previewTabPage.PageEnabled = false;
         _previewTabPage.PageVisible = false;
      }

      public void AddNanView(INanView nanView)
      {
         nanPanelControl.FillWith(nanView);
      }
   }
}