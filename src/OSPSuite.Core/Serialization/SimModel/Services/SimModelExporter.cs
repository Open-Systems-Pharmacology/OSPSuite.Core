//TODO SIMMODEL



//using System.IO;
//using OSPSuite.Core.Domain;
//using OSPSuite.Core.Serialization.SimModel.DTO;
//using OSPSuite.Serializer.Xml;
//using OSPSuite.Utility;
//
//namespace OSPSuite.Core.Serialization.SimModel.Services
//{
//   internal class SimModelExporter : ISimModelExporter
//   {
//      private readonly ISimulationExportCreatorFactory _simulationExportCreatorFactory;
//      private readonly IExportSerializer _exportSerializer;
//
//      public SimModelExporter(ISimulationExportCreatorFactory simulationExportCreatorFactory, IExportSerializer exportSerializer)
//      {
//         _simulationExportCreatorFactory = simulationExportCreatorFactory;
//         _exportSerializer = exportSerializer;
//      }
//
//      public string Export(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode)
//      {
//         return _exportSerializer.Serialize(createSimulationExport(simulation, simModelExportMode));
//      }
//
//      public void Export(IModelCoreSimulation simulation, string fileName)
//      {
//         var element = _exportSerializer.SerializeElement(createSimulationExport(simulation, SimModelExportMode.Full));
//         XmlHelper.SaveXmlElementToFile(element, fileName);
//      }
//
//      public void ExportODEForMatlab(IModelCoreSimulation modelCoreSimulation, string outputFolder, MatlabFormulaExportMode formulaExportMode)
//      {
//         if (!Directory.Exists(outputFolder))
//            Directory.CreateDirectory(outputFolder);
//
//         var simModelXml = Export(modelCoreSimulation, SimModelExportMode.Full);
//         var simModelSimulation = new Simulation();
//
//         simModelSimulation.LoadFromXMLString(simModelXml);
//         simModelSimulation.WriteMatlabCode(outputFolder, writeModeFrom(formulaExportMode), ParameterNamesWriteMode.FullyQualified);
//      }
//
//      private MatlabCodeWriteMode writeModeFrom(MatlabFormulaExportMode formulaExportMode)
//      {
//         return EnumHelper.ParseValue<MatlabCodeWriteMode>(formulaExportMode.ToString());
//      }
//
//      private SimulationExport createSimulationExport(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode)
//      {
//         var simulationExportCreator = _simulationExportCreatorFactory.Create();
//         var simulationExport = simulationExportCreator.CreateExportFor(simulation.Model, simModelExportMode);
//         simulationExport.AddSimulationConfiguration(simulation.BuildConfiguration.SimulationSettings);
//         return simulationExport;
//      }
//   }
//}