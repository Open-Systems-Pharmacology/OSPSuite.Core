using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IEditValueOriginView : IView<IEditValueOriginPresenter>, IResizableView
   {
      void BindTo(ValueOriginDTO valueOriginDTO);
      bool ShowCaption { get; set; }
   }
}