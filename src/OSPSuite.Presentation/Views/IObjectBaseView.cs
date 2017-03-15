using System.Drawing;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IObjectBaseView : IModalView<IObjectBasePresenter>
   {
      string NameDescription { get; set; }
      Icon Icon { get; set; }
      void BindTo(ObjectBaseDTO dto);
      bool DescriptionVisible { get; set; }
      bool NameVisible { get; set; }
      bool NameEditable { get; set; }
      bool NameDescriptionVisible { get; set; }
   }
}