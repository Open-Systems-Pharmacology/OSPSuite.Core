﻿using System.Windows.Forms;
 using DevExpress.XtraEditors;
 using DevExpress.XtraTab;
using OSPSuite.Assets;
using OSPSuite.Core.Serialization;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.Utility.Container;

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
         sourceFileLayoutControlItem.Name = Captions.Importer.SourceLayout;
         previewLayoutControlItem.Name = Captions.Importer.PreviewLayout;
         columnMappingLayoutControlItem.Name = Captions.Importer.MappingName;
         nanLayoutControlItem.AdjustControlHeight(80);
      }

      public void ShowErrorMessage(string message)
      {
         XtraMessageBox.Show(this, message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         sourceFileLayoutControlItem.Name = Captions.Importer.SourceLayout;
         previewLayoutControlItem.Name = Captions.Importer.PreviewLayout;
         columnMappingLayoutControlItem.Name = Captions.Importer.MappingName;

         saveMappingBtn.Click += (s, a) => OnEvent(() =>
         {
            var fileDialog = new OpenFileDialog { Multiselect = false };
            fileDialog.Title = Captions.Importer.SaveConfiguration;
            fileDialog.Filter = Captions.Importer.SaveConfigurationFilter;

            if (fileDialog.ShowDialog() != DialogResult.OK)
               return;
            _presenter.SaveConfiguration(fileDialog.FileName);
         });
         saveMappingBtn.Text = Captions.Importer.SaveConfiguration;
         saveMappingBtnLayoutControlItem.AdjustButtonSize();
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
   }
}