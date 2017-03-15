using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.FileLocker;
using DevExpress.XtraBars;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Views;
using OSPSuite.UI.Services;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Starter
{
   internal class TestRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<TestRegister>();

            scan.IncludeNamespaceContainingType<JournalTestPresenter>();
            scan.IncludeNamespaceContainingType<JournalTestView>();
            scan.IncludeNamespaceContainingType<ProjectRetriever>();
            scan.ExcludeType<Workspace>();
            scan.ExcludeType<ProgressManager>();
            scan.ExcludeType<GroupRepository>();
            scan.ExcludeType<DimensionRetriever>();
            scan.ExcludeType<ProjectRetriever>();
         });

         container.Register<IToolTipCreator, ToolTipCreator>(LifeStyle.Singleton);
         container.Register<IProjectRetriever, ProjectRetriever>(LifeStyle.Singleton);
         container.Register<IWorkspace, Workspace>(LifeStyle.Singleton);
         container.Register<IFileLocker, FileLocker>(LifeStyle.Singleton);
         container.Register<IExceptionManager, SimpleExceptionManager>(LifeStyle.Singleton);
         container.Register<IProgressManager, ProgressManager>(LifeStyle.Singleton);
         container.Register<IGroupRepository, GroupRepository>(LifeStyle.Singleton);
         container.Register<IReactionDimensionRetriever, DimensionRetriever>(LifeStyle.Singleton);
         
         container.Register<IDisplayNameProvider, DisplayNameProvider>();
         container.Register<IPathToPathElementsMapper, PathToPathElementsMapper>();
         container.Register<IDataColumnToPathElementsMapper, DataColumnToPathElementsMapper>();

         container.Register<BarManager, BarManager>();
         container.Register<ModelHelperForSpecs, ModelHelperForSpecs>();
         container.Register<IShell, ChartTestProgramForm>();

         container.RegisterFactory<IHeavyWorkPresenterFactory>();
      }
   }
}