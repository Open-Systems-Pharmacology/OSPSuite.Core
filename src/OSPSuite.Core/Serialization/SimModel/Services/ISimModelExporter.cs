using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.SimModel.Services
{
   public enum SimModelExportMode
   {
      /// <summary>
      ///    Xml will be generated with any piece of information available : e.g. Path, units etc..
      /// </summary>
      Full,

      /// <summary>
      ///    Only required information for SimModel will be generated.
      /// </summary>
      Optimized
   }

   public enum FormulaExportMode
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
      string ExportSimModelXml(IModelCoreSimulation modelCoreSimulation, SimModelExportMode simModelExportMode, SimulationExportCreatorMode simulationExportCreatorMode = SimulationExportCreatorMode.Default);

      /// <summary>
      ///    Creates a SimModel representation of the given <paramref name="modelCoreSimulation" /> and save it to the file
      ///    given as parameter using the full export
      /// </summary>
      void ExportSimModelXml(IModelCoreSimulation modelCoreSimulation, string fileName, SimulationExportCreatorMode simulationExportCreatorMode = SimulationExportCreatorMode.Default);

      /// <summary>
      ///    Export ODE system of the current simulation as pure matlab code
      /// </summary>
      /// <param name="modelCoreSimulation">Simulation to be exported</param>
      /// <param name="outputFolder">Folder where matlab files will be stored</param>
      /// <param name="formulaExportMode">Defines if formulas should be replaced by values where possible</param>
      void ExportODEForMatlab(IModelCoreSimulation modelCoreSimulation, string outputFolder, FormulaExportMode formulaExportMode);

      /// <summary>
      ///    Export ODE system of the current simulation as pure R code
      /// </summary>
      /// <param name="modelCoreSimulation">Simulation to be exported</param>
      /// <param name="outputFolder">Folder where matlab files will be stored</param>
      /// <param name="formulaExportMode">Defines if formulas should be replaced by values where possible</param>
      void ExportODEForR(IModelCoreSimulation modelCoreSimulation, string outputFolder, FormulaExportMode formulaExportMode);

      /// <summary>
      ///    Export the simulation as C++ code
      /// </summary>
      /// <param name="modelCoreSimulation">Simulation to be exported</param>
      /// <param name="outputFolder">Folder where matlab files will be stored</param>
      /// <param name="formulaExportMode">Defines if formulas should be replaced by values where possible</param>
      void ExportCppCode(IModelCoreSimulation modelCoreSimulation, string outputFolder, FormulaExportMode formulaExportMode);
   }
}