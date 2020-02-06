using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Presenters
{
   public interface IHeavyWorkPresenterFactory
   {
      IHeavyWorkPresenter Create();
   }

   internal class HeavyWorkPresenterFactory : DynamicFactory<IHeavyWorkPresenter>, IHeavyWorkPresenterFactory
   {
      public HeavyWorkPresenterFactory(IContainer container) : base(container)
      {
      }
   }
}