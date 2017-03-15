using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public interface ISimulationAnalysisPresenterContextMenuFactory : IContextMenuFactory<ISimulationAnalysisPresenter>
   {
   }

   public class SimulationAnalysisPresenterContextMenuFactory : ContextMenuFactory<ISimulationAnalysisPresenter>, ISimulationAnalysisPresenterContextMenuFactory
   {
      public SimulationAnalysisPresenterContextMenuFactory(IRepository<IContextMenuSpecificationFactory<ISimulationAnalysisPresenter>> contextMenuSpecFactoryRepository) :
         base(contextMenuSpecFactoryRepository)
      {
      }
   }
}