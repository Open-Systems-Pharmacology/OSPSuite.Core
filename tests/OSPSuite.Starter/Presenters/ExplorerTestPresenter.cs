using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Starter.Views;

namespace OSPSuite.Starter.Presenters
{
   public interface IExplorerTestPresenter : IPresenter<IExplorerTestView>
   {
   }

   public class ExplorerTestPresenter : AbstractPresenter<IExplorerTestView, IExplorerTestPresenter>, IExplorerTestPresenter
   {
      private readonly IExplorerForTestPresenter _explorerForTestPresenter;

      public ExplorerTestPresenter(IExplorerTestView view, IExplorerForTestPresenter explorerForTestPresenter) : base(view)
      {
         _explorerForTestPresenter = explorerForTestPresenter;
         _explorerForTestPresenter.Initialize();
         _explorerForTestPresenter.AddNode(new ObjectWithIdAndNameNode<Parameter>(new Parameter().WithId("1").WithName("Para1")));
         _explorerForTestPresenter.AddNode(new ObjectWithIdAndNameNode<Parameter>(new Parameter().WithId("2").WithName("Para2")));
      }
   }
}