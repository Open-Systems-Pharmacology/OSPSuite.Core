using OSPSuite.Utility.Container;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.FileLocker;
using DevExpress.XtraBars;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;
using OSPSuite.Starter.Services;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Views;
using OSPSuite.UI.Services;
using ApplicationSettings = OSPSuite.Starter.Services.ApplicationSettings;
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
            scan.IncludeNamespaceContainingType<LazyLoadTask>();
            scan.ExcludeType<Workspace>();
            scan.ExcludeType<ShellView>();
            scan.ExcludeType<ShellPresenter>();
            scan.ExcludeType<ProgressManager>();
            scan.ExcludeType<GroupRepository>();
            scan.ExcludeType<SimulationRepository>();
            scan.ExcludeType<DimensionRetriever>();
            scan.ExcludeType<ProjectRetriever>();
            scan.ExcludeType<ApplicationSettings>();
            scan.ExcludeType<HistoryManagerRetriever>();
         });

         container.Register<IToolTipCreator, ToolTipCreator>(LifeStyle.Singleton);
         container.Register<IProjectRetriever, ProjectRetriever>(LifeStyle.Singleton);
         container.Register<IWorkspace, Workspace>(LifeStyle.Singleton);
         container.Register<IFileLocker, FileLocker>(LifeStyle.Singleton);
         container.Register<IExceptionManager, ExceptionManager>(LifeStyle.Singleton);
         container.Register<IHistoryManagerRetriever, HistoryManagerRetriever>(LifeStyle.Singleton);
         container.Register<IProgressManager, ProgressManager>(LifeStyle.Singleton);
         container.Register<IGroupRepository, GroupRepository>(LifeStyle.Singleton);
         container.Register<IReactionDimensionRetriever, DimensionRetriever>(LifeStyle.Singleton);
         container.Register<ISimulationRepository, SimulationRepository>(LifeStyle.Singleton);
         container.Register<IShellPresenter, IMainViewPresenter, ShellPresenter>(LifeStyle.Singleton);
         container.Register<IShell, IShellView, IMainView, ShellView>(LifeStyle.Singleton);
         container.Register<IApplicationSettings, ApplicationSettings>(LifeStyle.Singleton); ;

         container.Register<IObjectIdResetter, ObjectIdResetter>();
         container.Register<ITreeNodeFactory, TreeNodeFactory>();
         container.Register<IDisplayNameProvider, DisplayNameProvider>();
         container.Register<IPathToPathElementsMapper, PathToPathElementsMapper>();
         container.Register<IDataColumnToPathElementsMapper, DataColumnToPathElementsMapper>();
         container.Register<IQuantityPathToQuantityDisplayPathMapper, QuantityPathToQuantityDisplayPathMapper>();

         container.Register<BarManager, BarManager>();
         container.Register<ModelHelperForSpecs, ModelHelperForSpecs>();
         container.Register<IDataNamingService, DataNamingServiceForSpecs >();

         container.RegisterFactory<IHeavyWorkPresenterFactory>();

         container.Register<TestEnvironment, TestEnvironment>();
      }
   }
}