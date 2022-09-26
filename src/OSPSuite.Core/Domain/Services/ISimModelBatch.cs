using System;
using System.Collections.Generic;
using OSPSuite.SimModel;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimModelBatch : IDisposable
   {
      void InitializeWith(IModelCoreSimulation modelCoreSimulation, IReadOnlyList<string> variableParameterPaths,
         IReadOnlyList<string> variableMoleculePaths, bool calculateSensitivities = false, string simulationResultsName = null);

      void InitializeForSensitivity();

      SimulationRunResults RunSimulation();

      void UpdateParameterValue(string parameterPath, double value);
      void UpdateParameterValues(IReadOnlyList<double> parameterValues);

      void UpdateInitialValue(string moleculePath, double value);
      void UpdateInitialValues(IReadOnlyList<double> initialValues);

      void StopSimulation();
      double[] SensitivityValuesFor(string outputPath, string parameterPath);

      /// <summary>
      ///    Path of parameters that will be effectively varied. (we can assume here that all parameters effectively exist in the
      ///    simulation)
      /// </summary>
      IReadOnlyList<string> VariableParameterPaths { get; }

      /// <summary>
      ///    Path of molecules that will be effectively varied. (we can assume here that all molecules effectively exist in the
      ///    simulation)
      /// </summary>
      IReadOnlyList<string> VariableMoleculePaths { get; }

      /// <summary>
      /// Export SimModel simulation as C++ code for compilation
      /// </summary>
      /// <param name="outputFolder">Folder where C++ code files will be created</param>
      /// <param name="exportMode">Formula or Values mode. Formula-Mode can be applied only if the SimModel simulation was NOT yet finalized</param>
      /// <param name="modelName">Name of the model to be exported. If left empty: model will be named "Standard".
      ///   If not empty: must be a valid file name AND a valid C++ identifier, otherwise the export will crash
      /// </param>
      void ExportToCPPCode(string outputFolder, CodeExportMode exportMode, string modelName = "");

      //SimModel optionally caches XML used for loading a simulation as string.
      //This XML string was used e.g. by the old Matlab-/R-Toolbox when saving a simulation to XML.
      //C++ export also depends on the original XML string at the moment (not quite clear why).
      //Because per default XML is NOT cached, we need to set the KeepXML-option to true BEFORE loading a simulation.
      bool KeepXMLNodeInSimModelSimulation { get; set; }
   }
}