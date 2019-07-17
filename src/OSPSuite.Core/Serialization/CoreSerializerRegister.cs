using OSPSuite.Utility.Container;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.ParameterIdentificationExport;
using OSPSuite.Core.Serialization.ParameterIdentificationExport.Serializer;
using OSPSuite.Core.Serialization.SimModel.Serializer;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Serialization.Xml;

namespace OSPSuite.Core.Serialization
{
   public class CoreSerializerRegister : Register
   {
      public override void RegisterInContainer(IContainer container)
      {
         //Xml serializer repository should be treated as singleton since repository initialization is a slow process
         container.Register<IUnitSystemXmlSerializerRepository, UnitSystemXmlSerializerRepository>(LifeStyle.Singleton);
         container.Register<IOSPSuiteXmlSerializerRepository, OSPSuiteXmlSerializerRepository>(LifeStyle.Singleton);
         container.Register<ISimModelSerializerRepository, SimModelSerializerRepository>(LifeStyle.Singleton);
         container.Register<IParameterIdentificationExportSerializerRepository, ParameterIdentificationExportSerializerRepository>(LifeStyle.Singleton);

         container.Register<IPKMLPersistor, PKMLPersistor>();
         container.Register<IDataPersistor, DataPersistor>();
         container.Register<ISimulationPersistor, SimulationPersistor>();
         container.Register<IDimensionFactoryPersistor, DimensionFactoryPersistor>();
         container.Register<IGroupRepositoryPersistor, GroupRepositoryPersistor>();

         //REGISTER DOMAIN OBJECTS
         container.AddScanner(scan =>
         {
            scan.AssemblyContainingType<CoreSerializerRegister>();

            //Sim Model services
            scan.IncludeNamespaceContainingType<ISimModelExporter>();
            scan.IncludeType<ParameterIdentificationExporter>();
         });
      }

      /// <summary>
      ///    Calls the PerformMapping method of all underlying serializer repository
      /// </summary>
      public void PerformMappingForSerializerIn(IContainer container)
      {
         container.Resolve<IUnitSystemXmlSerializerRepository>().PerformMapping();
         container.Resolve<IOSPSuiteXmlSerializerRepository>().PerformMapping();
      }
   }
}