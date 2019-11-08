using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.Presenters
{
   public interface IHeavyWorkPresenterFactory
   {
      IHeavyWorkPresenter Create();
   }

   internal class HeavyWorkPresenterFactory : DynamicFactory<IHeavyWorkPresenter>, IHeavyWorkPresenterFactory
   {
   }
}