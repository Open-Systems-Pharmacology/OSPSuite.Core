using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class SensitivityAnalysisClassificationNodeContextMenu : ExplorerClassificationNodeContextMenu
   {
      public SensitivityAnalysisClassificationNodeContextMenu(ClassificationNode classificationNode, IExplorerPresenter presenter, IContainer container)
         : base(classificationNode, presenter, container)
      {
      }
   }

   public class SensitivityAnalysisGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      private readonly IContainer _container;

      public SensitivityAnalysisGroupingFolderTreeNodeContextMenuFactory(IContainer container) : base(ClassificationType.SensitiviyAnalysis)
      {
         _container = container;
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new SensitivityAnalysisClassificationNodeContextMenu(classificationNode, presenter, _container);
      }
   }
}