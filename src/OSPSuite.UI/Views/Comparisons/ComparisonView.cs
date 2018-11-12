using System.Collections.Generic;
using System.Drawing;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout.Utils;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters.Comparisons;
using OSPSuite.Presentation.Views.Comparisons;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views.Comparisons
{
   public partial class ComparisonView : BaseUserControl, IComparisonView
   {
      private readonly PathElementsBinder<DiffItemDTO> _pathElementsBinder;
      private readonly GridViewBinder<DiffItemDTO> _gridViewBinder;
      private IGridViewColumn _colValue1;
      private IGridViewColumn _colValue2;

      public ComparisonView(PathElementsBinder<DiffItemDTO> pathElementsBinder)
      {
         _pathElementsBinder = pathElementsBinder;
         InitializeComponent();
         gridView.ShouldUseColorForDisabledCell = false;
         _gridViewBinder = new GridViewBinder<DiffItemDTO>(gridView);
         gridView.RowCellStyle += updateRowCellStyle;
      }

      public void AttachPresenter(IComparisonPresenter presenter)
      {
         //nothing to do for now
      }

      public override void InitializeBinding()
      {
         _pathElementsBinder.InitializeBinding(_gridViewBinder);
         _gridViewBinder.Bind(x => x.ObjectName)
            .WithCaption(Captions.Comparisons.ObjectName)
            .AsReadOnly();

         _gridViewBinder.Bind(x => x.Property)
            .WithCaption(Captions.Comparisons.Property)
            .AsReadOnly();

         _colValue1 = _gridViewBinder.Bind(x => x.Value1).AsReadOnly();
         _colValue2 = _gridViewBinder.Bind(x => x.Value2).AsReadOnly();
      }

      private void updateRowCellStyle(object sender, RowCellStyleEventArgs e)
      {
         var dto = _gridViewBinder.ElementAt(e.RowHandle);
         if (dto == null) return;

         if (dto.ItemIsMissing)
            gridView.AdjustAppearance(e, Colors.ADDED_OR_MISSING);
         else
            e.CombineAppearance(gridView.Appearance.Row);
      }

      public void BindTo(IEnumerable<DiffItemDTO> diffItemsDTOs)
      {
         _gridViewBinder.BindToSource(diffItemsDTOs);
      }

      public string LeftCaption
      {
         get { return _colValue1.Caption; }
         set { _colValue1.Caption = value; }
      }

      public string RightCaption
      {
         get { return _colValue2.Caption; }
         set { _colValue2.Caption = value; }
      }

      public bool DifferenceTableVisible
      {
         get { return layoutItemGrid.Visible; }
         set
         {
            layoutItemGrid.Visibility = LayoutVisibilityConvertor.FromBoolean(value);
            layoutGroupNoDifferences.Visibility = LayoutVisibilityConvertor.FromBoolean(!value);
         }
      }

      public void SetVisibility(PathElement pathElement, bool visible)
      {
         _pathElementsBinder.SetVisibility(pathElement, visible);
      }

      public void ClearBinding()
      {
         _gridViewBinder.DeleteBinding();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         layoutItemGrid.TextVisible = false;
         lblNoDifference.AsDescription();
         lblNoDifference.Text = Captions.Diff.NoDifferenceFound.FormatForDescription();
         lblNoDifference.Font = new Font(lblNoDifference.Font.FontFamily, 14);
         lblNoDifference.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
      }
   }
}