using System.IO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Serializer.Xml;
using OSPSuite.SimModel;
using OSPSuite.Utility;

namespace OSPSuite.Core.Serialization.SimModel.Services
{
   internal class SimModelExporter : ISimModelExporter
   {
      private readonly ISimulationExportCreatorFactory _simulationExportCreatorFactory;
      private readonly IExportSerializer _exportSerializer;

      public SimModelExporter(ISimulationExportCreatorFactory simulationExportCreatorFactory, IExportSerializer exportSerializer)
      {
         _simulationExportCreatorFactory = simulationExportCreatorFactory;
         _exportSerializer = exportSerializer;
      }

      public string ExportSimModelXml(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode, SimulationExportCreatorMode simulationExportCreatorMode = SimulationExportCreatorMode.Default)
      {
         return _exportSerializer.Serialize(createSimulationExport(simulation, simModelExportMode, simulationExportCreatorMode));
      }

      public void ExportSimModelXml(IModelCoreSimulation simulation, string fileName, SimulationExportCreatorMode simulationExportCreatorMode = SimulationExportCreatorMode.Default)
      {
         var element = _exportSerializer.SerializeElement(createSimulationExport(simulation, SimModelExportMode.Full, simulationExportCreatorMode));
         XmlHelper.SaveXmlElementToFile(element, fileName);
      }

      public void ExportODEForMatlab(IModelCoreSimulation modelCoreSimulation, string outputFolder, FormulaExportMode formulaExportMode)
      {
         exportToCode(modelCoreSimulation, outputFolder, formulaExportMode, CodeExportLanguage.Matlab);
      }

      public void ExportODEForR(IModelCoreSimulation modelCoreSimulation, string outputFolder, FormulaExportMode formulaExportMode)
      {
         exportToCode(modelCoreSimulation, outputFolder, formulaExportMode, CodeExportLanguage.R);
      }

      public void ExportCppCode(IModelCoreSimulation modelCoreSimulation, string outputFolder, FormulaExportMode formulaExportMode)
      {
         exportToCode(modelCoreSimulation, outputFolder, formulaExportMode, CodeExportLanguage.Cpp);
      }

      private void exportToCode(IModelCoreSimulation modelCoreSimulation, string outputFolder, FormulaExportMode formulaExportMode, CodeExportLanguage codeExportLanguage)
      {
         if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

         var simModelXml = ExportSimModelXml(modelCoreSimulation, SimModelExportMode.Full, SimulationExportCreatorMode.Default);
         var simModelSimulation = new Simulation();

         //SimModel optionally caches XML used for loading a simulation as string.
         //This XML string was used e.g. by the old Matlab-/R-Toolbox when saving a simulation to XML.
         //C++ export also depends on the original XML string at the moment (not quite clear why).
         //Because per default XML is NOT cached, we need to set the KeepXML-option to true BEFORE loading a simulation.
         if (codeExportLanguage == CodeExportLanguage.Cpp)
            simModelSimulation.Options.KeepXMLNodeAsString = true;

         simModelSimulation.LoadFromXMLString(simModelXml);
         simModelSimulation.ExportToCode(outputFolder, codeExportLanguage, writeModeFrom(formulaExportMode));
      }

      private CodeExportMode writeModeFrom(FormulaExportMode formulaExportMode)
      {
         return EnumHelper.ParseValue<CodeExportMode>(formulaExportMode.ToString());
      }

      private SimulationExport createSimulationExport(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode, SimulationExportCreatorMode simulationExportCreatorMode)
      {
         var simulationExportCreator = _simulationExportCreatorFactory.Create();
         var simulationExport = simulationExportCreator.CreateExportFor(simulation.Model, simModelExportMode, simulationExportCreatorMode);
         simulationExport.AddSimulationConfiguration(simulation.SimulationSettings);
         return simulationExport;
      }
   }
}