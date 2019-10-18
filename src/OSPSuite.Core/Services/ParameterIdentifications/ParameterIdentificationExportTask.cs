using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Serialization.ParameterIdentificationExport;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services.ParameterIdentifications
{
   public interface IParameterIdentificationExportTask
   {
      Task ExportToMatlab(ParameterIdentification parameterIdentification);
      Task ExportToR(ParameterIdentification parameterIdentification);
      Task ExportParametersHistoryToExcel(IReadOnlyCollection<IdentificationParameterHistory> parametersHistory, IReadOnlyList<float> errorHistory);
   }

   public class ParameterIdentificationExportTask : IParameterIdentificationExportTask
   {
      private const string PARAMETER_IDENTIFICATION_EXPORT_TEMPLATE_FOR_R = "ParameterIdentificationExportTemplate.r";
      private const string PARAMETER_IDENTIFICATION_EXPORT_TEMPLATE_FOR_MATLAB = "ParameterIdentificationExportTemplate.m";

      private const string PI_FILE_NAME = "@PI_FILE_NAME";
      private const string SIM_FILE_NAMES = "@SIM_FILE_NAMES";

      private readonly IDialogCreator _dialogCreator;
      private readonly ISimModelExporter _simModelExporter;
      private readonly ISimulationToModelCoreSimulationMapper _simulationToModelCoreSimulationMapper;
      private readonly ILazyLoadTask _lazyLoadTask;
      private readonly IParameterIdentificationExporter _parameterIdentificationExporter;
      private readonly IExportDataTableToExcelTask _exportToExcelTask;

      public ParameterIdentificationExportTask(IDialogCreator dialogCreator, ISimModelExporter simModelExporter, ISimulationToModelCoreSimulationMapper simulationToModelCoreSimulationMapper,
         ILazyLoadTask lazyLoadTask, IParameterIdentificationExporter parameterIdentificationExporter, IExportDataTableToExcelTask exportToExcelTask)
      {
         _dialogCreator = dialogCreator;
         _simModelExporter = simModelExporter;
         _simulationToModelCoreSimulationMapper = simulationToModelCoreSimulationMapper;
         _lazyLoadTask = lazyLoadTask;
         _parameterIdentificationExporter = parameterIdentificationExporter;
         _exportToExcelTask = exportToExcelTask;
      }

      public Task ExportToMatlab(ParameterIdentification parameterIdentification)
      {
         return export(parameterIdentification, PARAMETER_IDENTIFICATION_EXPORT_TEMPLATE_FOR_MATLAB, Constants.Filter.MATLAB_EXTENSION);
      }

      public Task ExportToR(ParameterIdentification parameterIdentification)
      {
         return export(parameterIdentification, PARAMETER_IDENTIFICATION_EXPORT_TEMPLATE_FOR_R, Constants.Filter.R_EXTENSION);
      }

      public Task ExportParametersHistoryToExcel(IReadOnlyCollection<IdentificationParameterHistory> parameterHistory, IReadOnlyList<float> errorHistory)
      {
         var fileName = _dialogCreator.AskForFileToSave(Captions.ParameterIdentification.SelectFileForParametersHistoryExport, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT);
         if (string.IsNullOrEmpty(fileName))
            return Task.FromResult(false); 

         return Task.Run(() =>
         {
            var dataTable = createParametersHistoryTableFrom(parameterHistory, errorHistory);
            _exportToExcelTask.ExportDataTableToExcel(dataTable, fileName, openExcel: true);
         });
      }

      private DataTable createParametersHistoryTableFrom(IReadOnlyCollection<IdentificationParameterHistory> parametersHistory, IReadOnlyList<float> totalErrorHistory)
      {
         var dataTable = new DataTable(Captions.ParameterIdentification.ParametersHistory);
         var allNames = parametersHistory.Select(x => x.DisplayName).ToList();
         dataTable.AddColumns<double>(allNames);
         dataTable.AddColumn<double>(Captions.ParameterIdentification.Error);

         if (parametersHistory.Count == 0)
            return dataTable;

         var numberOfValues = parametersHistory.ElementAt(0).Values.Count;
         var totalErrorValues = totalErrorHistory.Count;

         //use min to ensure that we are not taken by surprise if one array has been changed since we are using references
         var numberOfElements = Math.Min(numberOfValues, totalErrorValues);
         for (int i = 0; i < numberOfElements; i++)
         {
            var row = dataTable.NewRow();
            foreach (var parameterHistory in parametersHistory)
            {
               row[parameterHistory.DisplayName] = parameterHistory.DisplayValueAt(i);
            }
            row[Captions.ParameterIdentification.Error] = totalErrorHistory[i];
            dataTable.Rows.Add(row);
         }
         return dataTable;
      }

      private Task export(ParameterIdentification parameterIdentification, string templatePath, string exportFileExtension)
      {
         var exportDirectory = getExportDirectory(parameterIdentification);
         exportFor(parameterIdentification, exportDirectory, templatePath, exportFileExtension);
         return exportParameterIdentificationContent(parameterIdentification, exportDirectory);
      }

      private void exportFor(ParameterIdentification parameterIdentification, string exportDirectory, string templatePath, string exportFileExtension)
      {
         var template = getTemplateText(templatePath);
         var outputString = replaceSimulationsInTemplate(parameterIdentification.AllSimulations.Select(objectWithName => createFileNameFor(objectWithName, Constants.Filter.XML_EXTENSION)), template);
         outputString = replaceParameterIdentificationNameInTemplate(createFileNameFor(parameterIdentification, Constants.Filter.XML_EXTENSION), outputString);

         FileHelper.WriteStringToFile(outputString, createFilePathFor(parameterIdentification, exportDirectory, exportFileExtension, replaceSpaces: true));
      }

      private Task exportParameterIdentificationContent(ParameterIdentification parameterIdentification, string exportDirectory)
      {
         return Task.Run(() =>
         {
            if (string.IsNullOrEmpty(exportDirectory))
               return;

            _lazyLoadTask.Load(parameterIdentification);

            parameterIdentification.AllSimulations.Each(simulation => exportToDirectory(simulation, exportDirectory));
            exportToDirectory(parameterIdentification, exportDirectory);
         });
      }

      private string replaceParameterIdentificationNameInTemplate(string parameterIdentificationName, string template)
      {
         return template.Replace(PI_FILE_NAME, $"'{parameterIdentificationName}'");
      }

      private string replaceSimulationsInTemplate(IEnumerable<string> simulationNames, string template)
      {
         return template.Replace(SIM_FILE_NAMES, simulationNames.ToString(", ", "'"));
      }

      private string getTemplateText(string resourceName)
      {
         var assembly = Assembly.GetExecutingAssembly();
         //it is assumed that the resource are located in the same namespace as this service
         var currentNamespace = GetType().Namespace;
         var resourceFullPath =  $"{currentNamespace}.{resourceName}";
         using (var stream = assembly.GetManifestResourceStream(resourceFullPath))
         {
            if (stream == null)
               throw new ArgumentException(Error.CannotFindResource(resourceFullPath));

            using (var reader = new StreamReader(stream))
            {
               return reader.ReadToEnd();
            }
         }
      }

      private void exportToDirectory(ParameterIdentification parameterIdentification, string exportDirectory)
      {
         _parameterIdentificationExporter.Export(parameterIdentification, createFilePathFor(parameterIdentification, exportDirectory, Constants.Filter.XML_EXTENSION));
      }

      private void exportToDirectory(ISimulation simulation, string exportDirectory)
      {
         var fileName = createFilePathFor(simulation, exportDirectory, Constants.Filter.XML_EXTENSION);
         var modelCoreSimulation = _simulationToModelCoreSimulationMapper.MapFrom(simulation, shouldCloneModel:false);
         _simModelExporter.Export(modelCoreSimulation, fileName);
      }

      private string createFilePathFor(IWithName objectWithName, string exportDirectory, string extension, bool replaceSpaces = false)
      {
         return Path.Combine(exportDirectory, createFileNameFor(objectWithName, extension, replaceSpaces));
      }

      private static string createFileNameFor(IWithName objectWithName, string extension, bool replaceSpaces = false)
      {
         var baseFileName = $"{objectWithName.Name}{extension}";
         return !replaceSpaces ? baseFileName : baseFileName.Replace(" ", "_");
      }

      private string getExportDirectory(ParameterIdentification parameterIdentification)
      {
         var path = _dialogCreator.AskForFolder(Captions.ParameterIdentification.SelectDirectoryForParameterIdentificationExport, Constants.DirectoryKey.SIM_MODEL_XML);
         if (string.IsNullOrEmpty(path))
            return path;

         var newDirectoryName = parameterIdentification.Name;
         path = Path.Combine(path, newDirectoryName);

         if (!DirectoryHelper.DirectoryExists(path))
            return DirectoryHelper.CreateDirectory(path);

         if (_dialogCreator.MessageBoxYesNo(Captions.DoYouWantToDeleteDirectory(newDirectoryName), Captions.Delete, Captions.Cancel) == ViewResult.No)
            return string.Empty;

         DirectoryHelper.DeleteDirectory(path, true);
         return DirectoryHelper.CreateDirectory(path);
      }
   }
}