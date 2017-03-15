using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Services;
using OSPSuite.UI.Views;

namespace OSPSuite.Starter.Views
{
   public partial class ExplorerForTestView : BaseExplorerView, IExplorerForTestView
   {
      public ExplorerForTestView(IImageListRetriever imageListRetriever) : base(imageListRetriever)
      {
         InitializeComponent();
      }

      public void AttachPresenter(IExplorerForTestPresenter presenter)
      {
         base.AttachPresenter(presenter);
      }
   }
}
