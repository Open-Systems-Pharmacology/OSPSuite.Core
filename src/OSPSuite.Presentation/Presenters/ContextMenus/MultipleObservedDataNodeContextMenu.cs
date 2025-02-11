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
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Presentation.Presenters.ContextMenus
{
   public class MultipleObservedDataNodeContextMenu : ContextMenu<IReadOnlyList<ClassifiableObservedData>, IOSPSuiteExecutionContext>
   {
      public MultipleObservedDataNodeContextMenu(IReadOnlyList<ClassifiableObservedData> observedDataList, IOSPSuiteExecutionContext executionContext, IContainer container)
         : base(observedDataList, executionContext, container)
      {
      }

      protected override IEnumerable<IMenuBarItem> AllMenuItemsFor(IReadOnlyList<ClassifiableObservedData> observedDataList, IOSPSuiteExecutionContext executionContext)
      {
         var observedData = observedDataList.Select(x => x.Repository).ToList();
         yield return CreateMenuButton.WithCaption(MenuNames.EditMetaData)
            .WithCommandFor<EditMultipleMetaDataUICommand, IEnumerable<DataRepository>>(observedData, _container)
            .WithIcon(ApplicationIcons.Edit)
            .AsGroupStarter();

         yield return ObjectBaseCommonContextMenuItems.AddToJournal(observedData, _container);

         yield return CreateMenuButton.WithCaption(MenuNames.Delete)
            .WithCommandFor<DeleteSelectedObservedDataUICommand, IEnumerable<DataRepository>>(observedData, _container)
            .WithIcon(ApplicationIcons.Delete)
            .AsGroupStarter();
      }
   }

   public class MultipleObservedDataNodeContextMenuFactory : MultipleNodeContextMenuFactory<ClassifiableObservedData>
   {
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IContainer _container;

      public MultipleObservedDataNodeContextMenuFactory(IOSPSuiteExecutionContext executionContext, IContainer container)
      {
         _executionContext = executionContext;
         _container = container;
      }

      protected override IContextMenu CreateFor(IReadOnlyList<ClassifiableObservedData> observedDataList, IPresenterWithContextMenu<IReadOnlyList<ITreeNode>> presenter)
      {
         return new MultipleObservedDataNodeContextMenu(observedDataList, _executionContext, _container);
      }
   }
}