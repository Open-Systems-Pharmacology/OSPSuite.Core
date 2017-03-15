using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.DataBinding.DevExpress.XtraGrid;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Controls;

namespace OSPSuite.UI.Views
{
   public partial class EditOutputSelectionsView : BaseUserControl, IEditOutputSelectionsView
   {
      private readonly GridViewBinder<QuantitySelection> _gridViewBinder;

      public EditOutputSelectionsView()
      {
         InitializeComponent();
         _gridViewBinder = new GridViewBinder<QuantitySelection>(gridView);
         gridView.AllowsFiltering = false;
      }

      public void AttachPresenter(IEditOutputSelectionsPresenter presenter)
      {
      }

      public override void InitializeBinding()
      {
         _gridViewBinder.Bind(x => x.Path).AsReadOnly();
         _gridViewBinder.Bind(x => x.QuantityType).AsReadOnly();
      }

      public void BindTo(IEnumerable<QuantitySelection> allOutputs)
      {
         _gridViewBinder.BindToSource(allOutputs);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Caption = Captions.OutputSelections;
      }
   }
}