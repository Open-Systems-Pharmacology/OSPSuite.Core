//TODO SIMMODEL

//using System;
//using System.IO;
//using System.Xml;
//using OSPSuite.BDDHelper;
//using OSPSuite.BDDHelper.Extensions;
//using OSPSuite.Utility;
//using OSPSuite.Utility.Container;
//using OSPSuite.Utility.Xml;
//using NUnit.Framework;
//using SimModelNET;
//using OSPSuite.Core.Domain;
//using OSPSuite.Core.Domain.Services;
//using OSPSuite.Core.Serialization.SimModel;
//using OSPSuite.Core.Serialization.SimModel.Serializer;
//using OSPSuite.Core.Serialization.SimModel.Services;
//using OSPSuite.Engine.Serialization;
//using OSPSuite.Helpers;
//
//namespace OSPSuite.Core
//{
//   public abstract class concern_for_SimModelExporter : ContextForIntegration<ISimModelExporter>
//   {
//      private SimModelSerializerRepository _simModelXmlSerializerRepository;
//      protected SolverSettings _solverSettings;
//      protected IXmlValidator _xmlValidator;
//      protected IObjectPathFactory _objectPathFactory;
//      private ISimulationExportCreatorFactory _simulationExporterCreatorFactory;
//
//      public override void GlobalContext()
//      {
//         base.GlobalContext();
//         _simModelXmlSerializerRepository = new SimModelSerializerRepository();
//         var schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.SimModel.xsd");
//         _xmlValidator = new XmlValidator(schemaPath, SimModelSchemaConstants.Namespace);
//      }
//
//      protected override void Context()
//      {
//         _objectPathFactory = IoC.Resolve<IObjectPathFactory>();
//         var solverSettingsFactory = IoC.Resolve<ISolverSettingsFactory>();
//         _solverSettings = solverSettingsFactory.CreateCVODE();
//         _simulationExporterCreatorFactory = IoC.Resolve<ISimulationExportCreatorFactory>();
//         sut = new SimModelExporter(_simulationExporterCreatorFactory, new ExportSerializer(_simModelXmlSerializerRepository));
//      }
//   }
//
//   public class When_the_sim_model_serializer_is_serializing_a_container_to_an_xml_string_for_sim_model : concern_for_SimModelExporter
//   {
//      private IModelCoreSimulation _simulation;
//      private string _xmlString;
//
//      protected override void Context()
//      {
//         base.Context();
//         _simulation = IoC.Resolve<SimulationHelperForSpecs>().CreateSimulation();
//         _simulation.BuildConfiguration.SimulationSettings.OutputSchema.AddTimePoint(10);
//      }
//
//      protected override void Because()
//      {
//         _xmlString = sut.Export(_simulation, SimModelExportMode.Full);
//      }
//
//      [Observation]
//      public void should_return_a_valid_xml_string_that_is_valid_according_the_the_sim_model_schema()
//      {
//         var result = _xmlValidator.Validate(_xmlString);
//         Assert.IsTrue(result.Success, result.FullMessageLog);
//         var doc = new XmlDocument();
//         doc.LoadXml(_xmlString);
//         doc.Save("SimModelExample.xml");
//      }
//
//      [Observation]
//      public void should_export_molecule_amounts_as_nonnegative()
//      {
//         var doc = new XmlDocument();
//         doc.LoadXml(_xmlString);
//
//         foreach (XmlNode node in doc.GetElementsByTagName("VariableList")[0])
//         {
//            node.Attributes.GetNamedItem("negativeValuesAllowed").Value.ShouldBeEqualTo("0");
//         }
//      }
//   }
//
//   public class When_serializing_a_simulation_to_a_file : concern_for_SimModelExporter
//   {
//      private IModelCoreSimulation _simulation;
//      private string _tempFile;
//
//      protected override void Context()
//      {
//         base.Context();
//         _simulation = IoC.Resolve<SimulationHelperForSpecs>().CreateSimulation();
//         _tempFile = FileHelper.GenerateTemporaryFileName();
//      }
//
//      protected override void Because()
//      {
//         sut.Export(_simulation, _tempFile);
//      }
//
//      [Observation]
//      public void should_create_a_file_containing_the_sim_model_representation()
//      {
//         FileHelper.FileExists(_tempFile).ShouldBeTrue();
//      }
//
//      public override void Cleanup()
//      {
//         base.Cleanup();
//         FileHelper.DeleteFile(_tempFile);
//      }
//   }
//
//   public class When_exporting_a_simulation_as_ode_system : concern_for_SimModelExporter
//   {
//      private IModelCoreSimulation _simulation;
//      private string _tempFolderFormulas;
//      private string _tempFolderValues;
//
//      protected override void Context()
//      {
//         base.Context();
//         _simulation = IoC.Resolve<SimulationHelperForSpecs>().CreateSimulation();
//         _tempFolderFormulas = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
//         _tempFolderValues = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
//
//         if (!XMLSchemaCache.IsInitialized)
//         {
//            var schemaFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OSPSuite.SimModel.xsd");
//            XMLSchemaCache.InitializeFromFile(schemaFile);
//         }
//      }
//
//      private void checkMatlabFiles(string path)
//      {
//         var matlabFileNames = new[]
//            {
//               "ODEoptions.m",
//               "ODEInitialValues.m",
//               "ODERHSFunction.m",
//               "ODEMain.m",
//               "PerformSwitches.m"
//            };
//
//         foreach (var matlabFileName in matlabFileNames)
//         {
//            FileHelper.FileExists(Path.Combine(path, matlabFileName)).ShouldBeTrue();
//         }
//      }
//
//      protected override void Because()
//      {
//         sut.ExportODEForMatlab(_simulation, _tempFolderFormulas, MatlabFormulaExportMode.Formula);
//         sut.ExportODEForMatlab(_simulation, _tempFolderValues, MatlabFormulaExportMode.Values);
//      }
//
//      [Observation]
//      public void should_export_ode_to_matlab_in_formula_mode_and_in_value_mode()
//      {
//         checkMatlabFiles(_tempFolderFormulas);
//         checkMatlabFiles(_tempFolderValues);
//      }
//
//      public override void Cleanup()
//      {
//         base.Cleanup();
//         Directory.Delete(_tempFolderFormulas, true);
//         Directory.Delete(_tempFolderValues, true);
//      }
//   }
//}