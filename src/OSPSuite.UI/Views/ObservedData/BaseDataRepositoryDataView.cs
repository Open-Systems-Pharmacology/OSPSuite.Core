using System.Data;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.Extensions;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.ObservedData;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;
using OSPSuite.UI.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Views.ObservedData
{
   public partial class BaseDataRepositoryDataView<TView, TPresenter> : BaseUserControl, IBaseDataRepositoryDataView<TPresenter> where TPresenter : IBaseDataRepositoryDataPresenter<TView> where TView : IView
   {
      private readonly IToolTipCreator _tooltipCreator;
      protected TPresenter _presenter;

      public BaseDataRepositoryDataView(IToolTipCreator tooltipCreator)
      {
         _tooltipCreator = tooltipCreator;
         InitializeComponent();
         gridView.AllowsFiltering = false;
         gridView.OptionsCustomization.AllowQuickHideColumns = false;
      }

      public virtual void BindTo(DataTable dataTable)
      {
         gridControl.DataSource = dataTable;
         gridView.PopulateColumns();
         updateColumnFormatting();
      }

      private void updateColumnFormatting()
      {
         gridView.Columns.Each(col =>
         {
            col.DisplayFormat.AddDefaultFormattingFor<float>();
            col.RealColumnEdit.ConfigureWith(typeof(float));
            col.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
         });
      }

      public void AttachPresenter(TPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeBinding()
      {
         gridView.RowStyle += (sender, args) => OnEvent(() => highlightRowsBelowLLOQ(args));
         gridControl.ToolTipController = new ToolTipController();
         gridControl.ToolTipController.GetActiveObjectInfo += (o, e) => OnEvent(() => createToolTip(e));
      }

      private void createToolTip(ToolTipControllerGetActiveObjectInfoEventArgs e)
      {
         var hi = gridView.CalcHitInfo(e.ControlMousePosition);
         if (hi == null) return;

         if (hi.InDataRow)
         {
            tooltipForRow(e, hi.RowHandle);
         }
      }

      private void tooltipForRow(ToolTipControllerGetActiveObjectInfoEventArgs e, int rowHandle)
      {
         var observedDataRowIndex = gridView.GetDataSourceRowIndex(rowHandle);
         if (_presenter.AnyObservationInThisRowIsBelowLLOQ(observedDataRowIndex))
         {
            e.Info = new ToolTipControlInfo(rowHandle, string.Empty)
            {
               SuperTip = _tooltipCreator.CreateToolTip(_presenter.ToolTipTextForRow(observedDataRowIndex), Captions.LLOQ),
               ToolTipType = ToolTipType.SuperTip
            };
         }
      }

      private void highlightRowsBelowLLOQ(RowStyleEventArgs e)
      {
         if (e.RowHandle < 0 || e.RowHandle >= gridView.RowCount) return;

         var sourceRow = gridView.GetDataSourceRowIndex(e.RowHandle);

         var table = gridControl.DataSource as DataTable;

         if (table == null) return;

         var color = _presenter.BackgroundColorForRow(sourceRow);
         gridView.AdjustAppearance(e, color);
      }

      public override ApplicationIcon ApplicationIcon => ApplicationIcons.Parameters;

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnAddData.InitWithImage(ApplicationIcons.Create, text: Captions.AddDataPoint);
         layoutItemAddData.AdjustLargeButtonSize();
      }
   }
}