using System.IO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Helpers;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Serializers;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimulationPersistor : ModellingXmlSerializerWithModelBaseSpecs
   {
      protected ISimulationPersistor _simulationPersistor;
      protected string _filePath;
      protected IPKMLPersistor _pkmlPersistor;

      protected override void Context()
      {
         base.Context();
         _simulationPersistor = IoC.Resolve<ISimulationPersistor>();
         _pkmlPersistor = IoC.Resolve<IPKMLPersistor>();
         _filePath = FileHelper.GenerateTemporaryFileName();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_filePath);
      }
   }

   public class When_deserializing_a_valid_simulation_file_containing_some_licenses : concern_for_SimulationPersistor
   {
      [Observation]
      public void should_return_a_simulation_transfer_containing_a_valid_simulation_and_license()
      {
         var x1 = new SimulationTransfer {Simulation = _simulation};
         x1.Favorites.Add("F1");
         x1.Favorites.Add("F2");
         _simulationPersistor.Save(x1, _filePath);
         File.Exists(_filePath).ShouldBeTrue();

         var deserializationObjectBaseRepository = IoC.Resolve<IWithIdRepository>();

         var x2 = _simulationPersistor.Load(_filePath, deserializationObjectBaseRepository);

         AssertForSpecs.AreEqualSimulationTransfer(x1, x2);

         x2.Id.ShouldBeEqualTo(x1.Id);
      }
   }

   public class When_deserializing_a_pkml_file_that_does_not_contain_a_simulation : concern_for_SimulationPersistor
   {
      [Observation]
      public void should_throw_an_exception()
      {
         _pkmlPersistor.SaveToPKML(_simulation.BuildConfiguration.MoleculeStartValues, _filePath);
         File.Exists(_filePath).ShouldBeTrue();

         var deserializationObjectBaseRepository = IoC.Resolve<IWithIdRepository>();

         The.Action(() => _simulationPersistor.Load(_filePath, deserializationObjectBaseRepository))
            .ShouldThrowAn<OSPSuiteException>();
      }
   }
}