using System.Windows.Forms;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Importer
{
   public partial class ErrorTypeColumnEditorView : BaseUserControl, IErrorTypeColumnEditorView
   {
      private IErrorTypeColumnEditorPresenter _presenter;

      public ErrorTypeColumnEditorView()
      {
         InitializeComponent();
         errorTypeColumnToggleSwitch.IsOnChanged += (s, a) => OnEvent(onIsOnChanged);
         errorTypeColumnToggleLayoutControlItem.Text = Captions.Importer.ImportErrorTypeFromColumn;
      }

      public void AttachPresenter(IErrorTypeColumnEditorPresenter presenter)
      {
         _presenter = presenter;
      }

      private void onIsOnChanged()
      {
         updateVisibility();
         _presenter.ErrorTypeFromColumnChanged();
      }

      private void updateVisibility()
      {
         var errorTypeFromColumn = errorTypeColumnToggleSwitch.IsOn;
         errorTypeColumnDescriptionLayoutControlItem.Visibility = errorTypeFromColumn ? LayoutVisibility.Never : LayoutVisibility.Always;
         errorTypeColumnLayoutControlItem.Visibility = errorTypeFromColumn ? LayoutVisibility.Always : LayoutVisibility.Never;
      }

      public void FillErrorTypeColumnSelector(IView view)
      {
         errorTypeColumnPanelControl.FillWith(view);
      }

      public void SetErrorTypeColumnToggle(bool errorTypeFromColumn)
      {
         errorTypeColumnToggleSwitch.IsOn = errorTypeFromColumn;
         updateVisibility();
      }

      public bool IsErrorTypeColumnToggleOn()
      {
         return errorTypeColumnToggleSwitch.IsOn;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         errorTypeColumnDescriptionLabelControl.Text = Captions.Importer.ErrorTypeColumnDescription;
         errorTypeColumnPanelControl.Dock = DockStyle.Fill;
         errorTypeColumnLayoutControlItem.Text = Captions.Importer.Column.FormatForLabel();
      }
   }
}
