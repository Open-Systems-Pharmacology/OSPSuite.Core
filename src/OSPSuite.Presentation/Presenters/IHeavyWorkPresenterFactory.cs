using OSPSuite.Core.Services;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation.Presenters
{
   public interface IHeavyWorkPresenterFactory
   {
      IHeavyWorkPresenter Create(bool supportsCancellation = false);
   }

   public class HeavyWorkPresenterFactory : DynamicFactory<IHeavyWorkPresenter>, IHeavyWorkPresenterFactory
   {
      public HeavyWorkPresenterFactory(IContainer container) : base(container) { }

      public IHeavyWorkPresenter Create(bool supportsCancellation = false)
      {
         if (supportsCancellation)
            return Create<HeavyWorkCancellablePresenter>();  
         return Create<HeavyWorkPresenter>();  
      }
   }
}