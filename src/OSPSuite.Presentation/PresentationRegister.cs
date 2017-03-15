using OSPSuite.Core;
using OSPSuite.Presentation.Charts;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Journal;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Services;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.Presentation.Settings;
using OSPSuite.Presentation.UICommands;
using OSPSuite.Utility.Container;

namespace OSPSuite.Presentation
{
   public class PresenterRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<PresenterRegister>();

            //register views and presenters
            scan.IncludeNamespaceContainingType<IPresenter>();
            scan.IncludeType<RegionResolver>();
            scan.IncludeType<ChartFromTemplateService>();

            //register services
            scan.IncludeNamespaceContainingType<IPresentationSettingsTask>();

            //register mappers
            scan.IncludeNamespaceContainingType<DiffItemToDiffItemDTOMapper>();

            //Register all UI Commands
            scan.IncludeNamespaceContainingType<ShowHelpCommand>();

            //Exclude these implementations that are specific to each application
            scan.ExcludeType<PathToPathElementsMapper>();
            scan.ExcludeType<QuantityPathToQuantityDisplayPathMapper>();
            scan.ExcludeType<DisplayNameProvider>();

            scan.ExcludeType<ExceptionManager>();

            //should be registered as singleton
            scan.ExcludeType<JournalPageEditorFormPresenter>();
            scan.ExcludeType<ChartLayoutTemplateRepository>();
            scan.ExcludeType<HistoryBrowserConfiguration>();
            scan.ExcludeType<ParameterIdentificationFeedbackPresenter>();

            //specific app registration
            scan.ExcludeType<DataColumnToPathElementsMapper>();
         });

         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<PresenterRegister>();
            //Commands
            scan.IncludeNamespaceContainingType<GarbageCollectionCommand>();

            //Context Menus
            scan.IncludeNamespaceContainingType<IContextMenu>();
            scan.WithConvention(new ConcreteTypeRegistrationConvention());
         });

         registerSingleStartPresenters(container);

         //OPEN TYPES
         container.Register(typeof(ISubPresenterItemManager<>), typeof(SubPresenterItemManager<>));
         container.Register(typeof(IParameterToParameterDTOInContainerMapper<>), typeof(ParameterToParameterDTOInContainerMapper<>));
         container.Register(typeof(ICloneObjectBasePresenter<>), typeof(CloneObjectBasePresenter<>));

         //SINGLETON
         container.Register<IJournalPageEditorFormPresenter, JournalPageEditorFormPresenter>(LifeStyle.Singleton);
         container.Register<IChartLayoutTemplateRepository, ChartLayoutTemplateRepository>(LifeStyle.Singleton);
         container.Register<IParameterIdentificationFeedbackPresenter, ParameterIdentificationFeedbackPresenter>(LifeStyle.Singleton);
         container.Register<IHistoryBrowserConfiguration, HistoryBrowserConfiguration>(LifeStyle.Singleton);
         container.Register<DirectoryMapSettings, DirectoryMapSettings>(LifeStyle.Singleton);

         //Special registration
         container.Register<ChartEditorAndDisplaySettings, ChartEditorAndDisplaySettings>();
         container.Register<ChartEditorSettings, ChartEditorSettings>();
         container.Register<ChartPresenterContext, ChartPresenterContext>();
      }

      private static void registerSingleStartPresenters(IContainer container)
      {
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<PresenterRegister>();
            scan.IncludeNamespaceContainingType<IPresenter>();
            scan.WithConvention(new RegisterTypeConvention<ISingleStartPresenter>(registerWithDefaultConvention: false));
         });
      }
   }
}