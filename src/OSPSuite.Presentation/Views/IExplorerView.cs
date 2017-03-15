using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;

namespace OSPSuite.Presentation.Views
{
   public interface IExplorerView : IView, IBatchUpdatable
   {
      IUxTreeView TreeView { get; }
      ITreeNode AddNode(ITreeNode nodeToAdd);
      bool Enabled { get; set; }
   }
}