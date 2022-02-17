using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   public interface ITestResizableView: IResizableView
   {
      void BindTo(IEnumerable<QuantitySelectionDTO> quantitySelectionDTOs);
   }

   public partial class TestResizableView : BaseGridViewOnlyUserControl, ITestResizableView
   {
      private readonly PathElementsBinder<QuantitySelectionDTO> _pathElementsBinder;
      private readonly GridViewBinder<QuantitySelectionDTO> _gridViewBinder;

      public TestResizableView(PathElementsBinder<QuantitySelectionDTO> pathElementsBinder)
      {
         _pathElementsBinder = pathElementsBinder;
         _gridViewBinder = new GridViewBinder<QuantitySelectionDTO>(gridView);
         InitializeComponent();
      }

      public override void InitializeBinding()
      {
         base.InitializeBinding();
         _pathElementsBinder.InitializeBinding(_gridViewBinder);
         var colDimension = _gridViewBinder.AutoBind(x => x.DimensionDisplayName)
            .WithCaption(Captions.Dimension)
            .AsReadOnly();


      }

      public void BindTo(IEnumerable<QuantitySelectionDTO> quantitySelectionDTOs)
      {
         _gridViewBinder.BindToSource(quantitySelectionDTOs);
         AdjustHeight();
      }
   }
}