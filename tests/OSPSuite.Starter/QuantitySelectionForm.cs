using DevExpress.XtraEditors;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.UI.Extensions;

namespace OSPSuite.Starter
{
   public partial class QuantitySelectionForm : XtraForm
   {
      private readonly IQuantitySelectionPresenter _presenter;

      public QuantitySelectionForm(IQuantitySelectionPresenter presenter)
      {
         _presenter = presenter;
         InitializeComponent();
         panelControl1.FillWith(presenter.View);

      }


      public void Initialize(IContainer container)
      {
         _presenter.Edit(container); 
         
      }
   }
}