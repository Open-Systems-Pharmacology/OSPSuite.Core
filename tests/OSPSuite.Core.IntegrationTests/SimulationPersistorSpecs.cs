using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Serialization.Exchange;
using OSPSuite.Core.Serialization.Xml;
using OSPSuite.Core.Serializers;
using OSPSuite.Helpers;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core
{
   internal abstract class concern_for_SimulationPersistor : ModelingXmlSerializerWithModelBaseSpecs
   {
      protected ISimulationPersistor _simulationPersistor;
      protected string _filePath;
      protected IPKMLPersistor _pkmlPersistor;
      protected SerializationContext _context;
      protected IOSPSuiteXmlSerializerRepository _serializerRepository;

      protected override void Context()
      {
         base.Context();
         _simulationPersistor = IoC.Resolve<ISimulationPersistor>();
         _pkmlPersistor = IoC.Resolve<IPKMLPersistor>();
         _serializerRepository = IoC.Resolve<IOSPSuiteXmlSerializerRepository>();
         _context = NewDeserializationContext();
         _filePath = FileHelper.GenerateTemporaryFileName();
      }

      public override void Cleanup()
      {
         base.Cleanup();
         FileHelper.DeleteFile(_filePath);
      }
   }

   internal class When_serializing_and_deserializing_without_files : concern_for_SimulationPersistor
   {
      [Observation]
      public void the_simulation_can_be_serialized_and_deserialized()
      {
         var serializer = _serializerRepository.SerializerFor<SimulationTransfer>();
         var simulationTransfer = serializer.Deserialize<SimulationTransfer>(
            XElement.Parse(_simulationPersistor.Serialize(new SimulationTransfer
               {
                  Simulation = _simulation
               })), 
            _context);

         ReferenceEquals(simulationTransfer.Simulation, _simulation).ShouldBeFalse();
         AssertForSpecs.AreEqual(simulationTransfer.Simulation, _simulation);
      }
   }

   internal class When_serializing_and_deserializing_an_entity_with_illegal_xml_characters : concern_for_SimulationPersistor
   {
      [Observation]
      public void should_return_a_simulation_transfer_containing_a_valid_simulation()
      {
         var x1 = new SimulationTransfer { Simulation = _simulation };
         var illegalXmlString = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes("F1").Concat(new byte[] { 0x1F }).ToArray());
         x1.Favorites.Add(illegalXmlString);

         _simulationPersistor.Save(x1, _filePath);
         File.Exists(_filePath).ShouldBeTrue();
         var deserializationObjectBaseRepository = IoC.Resolve<IWithIdRepository>();

         var x2 = _simulationPersistor.Load(_filePath, deserializationObjectBaseRepository);
         x2.Favorites.First().ShouldBeEqualTo(illegalXmlString);
      }
   }

   internal class When_deserializing_a_valid_simulation_file_containing_some_licenses : concern_for_SimulationPersistor
   {
      [Observation]
      public void should_return_a_simulation_transfer_containing_a_valid_simulation()
      {
         var x1 = new SimulationTransfer { Simulation = _simulation };
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

   internal class When_deserializing_a_pkml_file_that_does_not_contain_a_simulation : concern_for_SimulationPersistor
   {
      [Observation]
      public void should_throw_an_exception()
      {
         _pkmlPersistor.SaveToPKML(_simulationBuilder.SpatialStructureAndMergeBehaviors[0].spatialStructure, _filePath);
         File.Exists(_filePath).ShouldBeTrue();

         var deserializationObjectBaseRepository = IoC.Resolve<IWithIdRepository>();

         The.Action(() => _simulationPersistor.Load(_filePath, deserializationObjectBaseRepository))
            .ShouldThrowAn<OSPSuiteException>();
      }
   }

   internal class When_deserializing_a_simulation_transfer_containing_observed_data_and_mapping : concern_for_SimulationPersistor
   {
      private SimulationTransfer _simulationTransfer;
      private DataRepository _obsData;
      private OutputMapping _outputMapping;
      private OutputMappings _outputMappings;
      private SimulationTransfer _loadedSimulation;

      protected override void Context()
      {
         base.Context();
         _obsData = DomainHelperForSpecs.ObservedData();
         _outputMapping = new OutputMapping
         {
            WeightedObservedData = new WeightedObservedData(_obsData),
            OutputSelection = new SimulationQuantitySelection(_simulation, new QuantitySelection("A|B|C", QuantityType.Complex))
         };
         _outputMappings = new OutputMappings { _outputMapping };

         _simulationTransfer = new SimulationTransfer
         {
            Simulation = _simulation,
            AllObservedData = new List<DataRepository> { _obsData },
            OutputMappings = _outputMappings
         };
      }

      protected override void Because()
      {
         _simulationPersistor.Save(_simulationTransfer, _filePath);
         _loadedSimulation = _simulationPersistor.Load(_filePath);
      }

      [Observation]
      public void should_be_able_to_deserialize_the_output_mappings()
      {
         _loadedSimulation.ShouldNotBeNull();
         _loadedSimulation.OutputMappings.Count().ShouldBeEqualTo(1);
         var outputMapping = _loadedSimulation.OutputMappings.ElementAt(0);
         outputMapping.Simulation.ShouldBeEqualTo(_simulation);
         _obsData.Name.ShouldBeEqualTo(outputMapping.WeightedObservedData.ObservedData.Name);
      }
   }
}