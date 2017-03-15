using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.UICommands;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class MultipleObservedDataNodeContextMenu : ContextMenu<IReadOnlyList<ClassifiableObservedData>, IOSPSuiteExecutionContext>
   {
      public MultipleObservedDataNodeContextMenu(IReadOnlyList<ClassifiableObservedData> observedDataList, IOSPSuiteExecutionContext executionContext)
         : base(observedDataList, executionContext)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<ClassifiableObservedData> observedDataList, IOSPSuiteExecutionContext executionContext)
      {
         var observedData = observedDataList.Select(x => x.Repository).ToList();
         yield return CreateMenuButton.WithCaption(MenuNames.EditMetaData)
            .WithCommandFor<EditMultipleMetaDataUICommand, IEnumerable<DataRepository>>(observedData)
            .WithIcon(ApplicationIcons.Edit)
            .AsGroupStarter();

         yield return ObjectBaseCommonContextMenuItems.AddToJournal(observedData);

         yield return CreateMenuButton.WithCaption(MenuNames.Delete)
            .WithCommandFor<DeleteSelectedObservedDataUICommand, IEnumerable<DataRepository>>(observedData)
            .WithIcon(ApplicationIcons.Delete)
            .AsGroupStarter();
      }
   }

   public class MultipleObservedDataNodeContextMenuFactory : MultipleNodeContextMenuFactory<ClassifiableObservedData>
   {
      private readonly IOSPSuiteExecutionContext _executionContext;

      public MultipleObservedDataNodeContextMenuFactory(IOSPSuiteExecutionContext executionContext)
      {
         _executionContext = executionContext;
      }

      protected override IContextMenu CreateFor(IReadOnlyList<ClassifiableObservedData> observedDataList, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleObservedDataNodeContextMenu(observedDataList, _executionContext);
      }
   }
}