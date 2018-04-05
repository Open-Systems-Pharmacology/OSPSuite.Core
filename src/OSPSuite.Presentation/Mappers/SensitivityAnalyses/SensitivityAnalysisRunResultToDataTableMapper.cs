using System.Data;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Presentation.Mappers.SensitivityAnalyses
{
   public interface ISensitivityAnalysisRunResultToDataTableMapper
   {
      DataTable MapFrom(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunResult sensitivityAnalysisRunResult);
   }

   public class SensitivityAnalysisRunResultToDataTableMapper : ISensitivityAnalysisRunResultToDataTableMapper
   {
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;
      private readonly IPKParameterRepository _pkParameterRepository;
      private readonly IEntityPathResolver _entityPathResolver;

      public SensitivityAnalysisRunResultToDataTableMapper(IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper, IPKParameterRepository pkParameterRepository, IEntityPathResolver entityPathResolver)
      {
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
         _pkParameterRepository = pkParameterRepository;
         _entityPathResolver = entityPathResolver;
      }

      public DataTable MapFrom(SensitivityAnalysis sensitivityAnalysis, SensitivityAnalysisRunResult sensitivityAnalysisRunResult)
      {
         var dataTable = new DataTable(Captions.SensitivityAnalysis.Results);
         dataTable.AddColumns<string>(Captions.SensitivityAnalysis.Output, Captions.SensitivityAnalysis.PKParameterName, Captions.SensitivityAnalysis.ParameterName, Captions.SensitivityAnalysis.ParameterDisplayPath, Captions.SensitivityAnalysis.PKParameterDescription, Captions.SensitivityAnalysis.ParameterPath);
         dataTable.AddColumn<double>(Captions.SensitivityAnalysis.Value);
         sensitivityAnalysisRunResult.AllPKParameterSensitivities.Each(x => addParameterSensitivity(x, dataTable, sensitivityAnalysis));
         return dataTable;
      }

      private void addParameterSensitivity(PKParameterSensitivity parameterSensitivity, DataTable dataTable, SensitivityAnalysis sensitivityAnalysis)
      {
         var row = dataTable.NewRow();
         var sensitivityParameter = sensitivityAnalysis.SensitivityParameterByName(parameterSensitivity.ParameterName);
         row[Captions.SensitivityAnalysis.Output] = parameterSensitivity.QuantityPath;
         row[Captions.SensitivityAnalysis.PKParameterName] = _pkParameterRepository.DisplayNameFor(parameterSensitivity.PKParameterName);
         row[Captions.SensitivityAnalysis.PKParameterDescription] = _pkParameterRepository.DescriptionFor(parameterSensitivity.PKParameterName);
         row[Captions.SensitivityAnalysis.ParameterName] = parameterSensitivity.ParameterName;
         row[Captions.SensitivityAnalysis.ParameterDisplayPath] = parameterDisplayPathFor(sensitivityParameter);
         row[Captions.SensitivityAnalysis.ParameterPath] = parameterFullPathFor(sensitivityParameter);
         row[Captions.SensitivityAnalysis.Value] = parameterSensitivity.Value;
         dataTable.Rows.Add(row);
      }

      private string parameterFullPathFor(SensitivityParameter sensitivityParameter)
      {
         if (sensitivityParameter?.Parameter == null)
            return string.Empty;

         return _entityPathResolver.FullPathFor(sensitivityParameter.Parameter);
      }

      private string parameterDisplayPathFor(SensitivityParameter sensitivityParameter)
      {
         if (sensitivityParameter?.Parameter == null)
            return string.Empty;

         return _quantityDisplayPathMapper.DisplayPathAsStringFor(sensitivityParameter.Parameter, addSimulationName: true);
      }
   }
}