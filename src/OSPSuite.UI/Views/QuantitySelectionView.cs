using System.Drawing;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors.Controls;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class QuantitySelectionView : BaseUserControl, IQuantitySelectionView
   {
      private IQuantitySelectionPresenter _presenter;

      public QuantitySelectionView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IQuantitySelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         btnDeselectAll.Click += (o, e) => OnEvent(_presenter.DeselectAll);
      }

      public string Info
      {
         set => txtInfo.Text = value;
         get => txtInfo.Text;
      }

      public string InfoError
      {
         set => errorProvider.SetError(txtInfo, value);
         get => errorProvider.GetError(txtInfo);
      }

      public bool DeselectAllEnabled
      {
         get => btnDeselectAll.Enabled;
         set => btnDeselectAll.Enabled = value;
      }

      public string Description
      {
         get => lblDescription.Text;
         set => lblDescription.Text = value;
      }

      public void SetQuantityListView(IView view)
      {
         panelQuantities.FillWith(view);
      }

      public void SetSelectedQuantityListView(IView view)
      {
         panelSelectedQuantities.FillWith(view);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();

         txtInfo.Properties.AllowFocused = false;
         txtInfo.Enabled = false;
         txtInfo.Properties.ReadOnly = true;
         txtInfo.Properties.Appearance.ForeColor = SystemColors.WindowText;
         txtInfo.Properties.Appearance.Options.UseForeColor = true;
         txtInfo.Properties.BorderStyle = BorderStyles.NoBorder;
         lblDescription.AsDescription();
         layoutItemDeselectAll.AdjustButtonSize();
         btnDeselectAll.Text = Captions.DeselectAll;
      }
   }
}