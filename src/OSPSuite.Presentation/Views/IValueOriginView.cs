using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IValueOriginView : IView<IValueOriginPresenter>
   {
      void BindTo(ValueOrigin valueOrigin);
      void Save();
   }
}