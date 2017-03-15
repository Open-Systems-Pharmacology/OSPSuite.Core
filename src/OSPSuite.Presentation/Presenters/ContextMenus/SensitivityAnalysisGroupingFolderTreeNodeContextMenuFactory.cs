using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Nodes;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   internal class SensitivityAnalysisClassificationNodeContextMenu : ClassificationNodeContextMenu<IExplorerPresenter>
   {
      public SensitivityAnalysisClassificationNodeContextMenu(ClassificationNode classificationNode, IExplorerPresenter presenter)
         : base(classificationNode, presenter)
      {
      }
   }

   public class SensitivityAnalysisGroupingFolderTreeNodeContextMenuFactory : ClassificationNodeContextMenuFactory
   {
      public SensitivityAnalysisGroupingFolderTreeNodeContextMenuFactory() : base(ClassificationType.SensitiviyAnalysis)
      {
      }

      protected override IContextMenu CreateFor(ClassificationNode classificationNode, IExplorerPresenter presenter)
      {
         return new SensitivityAnalysisClassificationNodeContextMenu(classificationNode, presenter);
      }
   }
}