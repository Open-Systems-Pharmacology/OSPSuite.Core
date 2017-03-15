using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class EditDisplayUnitsView : BaseModalView, IEditDisplayUnitsView
   {
      public EditDisplayUnitsView()
      {
         InitializeComponent();
      }

      public void AttachPresenter(IEditDisplayUnitsPresenter presenter)
      {
      }

      public void AddUnitsView(IView view)
      {
         panelUnits.FillWith(view);
      }
   }
}