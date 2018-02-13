using DevExpress.XtraBars;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters.Comparisons;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Comparisons;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views.Comparisons
{
   public partial class MainComparisonView : BaseUserControl, IMainComparisonView
   {
      private IMainComparisonPresenter _presenter;

      public MainComparisonView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         barManager.Images = imageListRetriever.AllImages16x16;
      }

      public void AttachPresenter(IMainComparisonPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddSettingsView(IView view)
      {
         panelSettings.FillWith(view);
      }

      public void AddComparisonView(IView view)
      {
         panelComparison.FillWith(view);
      }

      public bool SettingsVisible
      {
         get => layoutItemSettings.Visible;
         set
         {
            layoutGroupSettings.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            splitterItem.Visibility = layoutGroupSettings.Visibility;
            btnSettings.Caption = value ? Captions.Comparisons.HideSettings : Captions.Comparisons.ShowSettings;
         }
      }

      public void UpdateButtonsEnableState(bool enabled)
      {
         btnRunComparison.Enabled = enabled;
         btnExportToExcel.Enabled = enabled;
      }

      public override void InitializeBinding()
      {
         btnExportToExcel.ItemClick += (o, e) => OnEvent(_presenter.ExportToExcel);
         btnRunComparison.ItemClick += (o, e) => OnEvent(_presenter.RunComparison);
         btnSettings.ItemClick += (o, e) => toggleSettingsVisibility();
      }

      private void toggleSettingsVisibility()
      {
         SettingsVisible = !SettingsVisible;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemComparison.TextVisible = false;
         layoutItemSettings.TextVisible = false;

         initializeButton(btnSettings, ApplicationIcons.Settings, Captions.Comparisons.ShowSettings);
         initializeButton(btnRunComparison, ApplicationIcons.Run, Captions.Comparisons.RunComparison);
         initializeButton(btnExportToExcel, ApplicationIcons.Excel, Captions.Comparisons.ExportToExcel);
      }

      private void initializeButton(BarButtonItem button, ApplicationIcon icon, string caption)
      {
         button.Caption = caption;
         button.ImageIndex = ApplicationIcons.IconIndex(icon);
         button.PaintStyle = BarItemPaintStyle.CaptionGlyph;
      }
   }
}