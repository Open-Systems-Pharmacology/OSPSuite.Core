using System.IO;
using SimModelNET;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Serialization.SimModel.DTO;
using OSPSuite.Serializer.Xml;
using OSPSuite.Utility;

namespace OSPSuite.Core.Serialization.SimModel.Services
{
   public enum SimModelExportMode
   {
      /// <summary>
      ///    Xml will be generated with any piece of information available : e.g. Path, units etc..
      /// </summary>
      Full,

      /// <summary>
      ///    Only required information for SimModel will be genrated.
      /// </summary>
      Optimized
   }

   public enum MatlabFormulaExportMode
   {
      Formula,
      Values
   }

   public interface ISimModelExporter
   {
      /// <summary>
      ///    Returns a SimModel representation of the given <paramref name="modelCoreSimulation" /> using the
      ///    <paramref name="simModelExportMode" />
      /// </summary>
      string Export(IModelCoreSimulation modelCoreSimulation, SimModelExportMode simModelExportMode);

      /// <summary>
      ///    Creates a SimModel representation of the given <paramref name="modelCoreSimulation" /> and save it to the file
      ///    given as parameter using the full export
      /// </summary>
      void Export(IModelCoreSimulation modelCoreSimulation, string fileName);

      /// <summary>
      ///    Export ODE system of the current simulation as pure matlab code
      /// </summary>
      /// <param name="modelCoreSimulation">Simulation to be exported</param>
      /// <param name="outputFolder">Folder where matlab files will be stored</param>
      /// <param name="formulaExportMode">Defines if formulas should be replaced by values where possible</param>
      void ExportODEForMatlab(IModelCoreSimulation modelCoreSimulation, string outputFolder, MatlabFormulaExportMode formulaExportMode);
   }

   internal class SimModelExporter : ISimModelExporter
   {
      private readonly ICreateExportModelVisitor _createExportModelVisitor;
      private readonly IExportSerializer _exportSerializer;

      public SimModelExporter(ICreateExportModelVisitor createExportModelVisitor, IExportSerializer exportSerializer)
      {
         _createExportModelVisitor = createExportModelVisitor;
         _exportSerializer = exportSerializer;
      }

      public string Export(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode)
      {
         return _exportSerializer.Serialize(createSimulationExport(simulation, simModelExportMode));
      }

      public void Export(IModelCoreSimulation simulation, string fileName)
      {
         var element = _exportSerializer.SerializeElement(createSimulationExport(simulation, SimModelExportMode.Full));
         XmlHelper.SaveXmlElementToFile(element, fileName);
      }

      public void ExportODEForMatlab(IModelCoreSimulation modelCoreSimulation, string outputFolder, MatlabFormulaExportMode formulaExportMode)
      {
         if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

         var simModelXml = Export(modelCoreSimulation, SimModelExportMode.Full);
         var simModelSimulation = new Simulation();

         simModelSimulation.LoadFromXMLString(simModelXml);
         simModelSimulation.WriteMatlabCode(outputFolder, writeModeFrom(formulaExportMode) , ParameterNamesWriteMode.FullyQualified);
      }

      private MatlabCodeWriteMode writeModeFrom(MatlabFormulaExportMode formulaExportMode)
      {
         return EnumHelper.ParseValue<MatlabCodeWriteMode>(formulaExportMode.ToString());
      }

      private SimulationExport createSimulationExport(IModelCoreSimulation simulation, SimModelExportMode simModelExportMode)
      {
         var simulationExport = _createExportModelVisitor.CreateExportFor(simulation.Model, simModelExportMode);
         simulationExport.AddSimulationConfiguration(simulation.BuildConfiguration.SimulationSettings);
         return simulationExport;
      }
   }
}