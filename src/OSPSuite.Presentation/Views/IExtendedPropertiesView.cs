using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IExtendedPropertiesView : IView<IExtendedPropertiesPresenter>, IResizableView
   {
      void BindTo(ExtendedProperties algorithmProperties);
      bool ReadOnly { set; }
   }
}