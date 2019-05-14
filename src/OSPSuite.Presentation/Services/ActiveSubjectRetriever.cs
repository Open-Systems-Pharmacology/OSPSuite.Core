using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.Main;

namespace OSPSuite.Presentation.Services
{
   public class ActiveSubjectRetriever : IActiveSubjectRetriever
   {
      private readonly IMainViewPresenter _mainViewPresenter;

      public ActiveSubjectRetriever(IMainViewPresenter mainViewPresenter)
      {
         _mainViewPresenter = mainViewPresenter;
      }

      public T Active<T>() where T : class
      {
         var activePresenter = _mainViewPresenter.ActivePresenter;
         return activePresenter?.Subject as T;
      }
   }
}