using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTab;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ImporterView : BaseUserControl, IImporterView
   {
      private IImporterPresenter _presenter;
      private XtraTabPage _confirmationTabPage;
      private const int DATA_PAGE_INDEX = 0;
      private const int CONFIRMATION_PAGE_INDEX = 1;

      public ImporterView()
      {
         InitializeComponent();
         nanLayoutControlItem.AdjustControlHeight(80);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         sourceFileLayoutControlItem.Name = Captions.Importer.SourceLayout;
         previewLayoutControlItem.Name = Captions.Importer.PreviewLayout;
         columnMappingLayoutControlItem.Name = Captions.Importer.MappingSettings;

         saveMappingBtn.Click += (s, a) => OnEvent(() =>
         {
            _presenter.SaveConfiguration();
         });
         applyMappingBtn.Click += (s, a) => OnEvent(() =>
         {
            _presenter.LoadConfigurationWithoutImporting();
         });
         saveMappingBtn.InitWithImage(ApplicationIcons.Save, Captions.Importer.SaveConfiguration);
         applyMappingBtn.InitWithImage(ApplicationIcons.Load, Captions.Importer.ApplyConfiguration);
         saveMappingBtnLayoutControlItem.AdjustLargeButtonSize();
         applyMappingLayoutControlItem.AdjustLargeButtonSize();
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

      public void AddConfirmationView(IImportConfirmationView confirmationView)
      {
         _confirmationTabPage = previewXtraTabControl.AddPageFrom(confirmationView, CONFIRMATION_PAGE_INDEX);
      }

      public void EnableConfirmationView()
      {
         if (_confirmationTabPage.PageEnabled)
            return;

         _confirmationTabPage.PageEnabled = true;
         _confirmationTabPage.PageVisible = true;
         previewXtraTabControl.SelectedTabPage = _confirmationTabPage;
      }

      public void DisableConfirmationView()
      {
         _confirmationTabPage.PageEnabled = false;
         _confirmationTabPage.PageVisible = false;
      }

      public void AddNanView(INanView nanView)
      {
         nanPanelControl.FillWith(nanView);
      }
      public void HideExtraErrors()
      {
         _labelExtraErrors.Text = "";
         layoutControlItemExtraError.ContentVisible = false;
      }
   }
}