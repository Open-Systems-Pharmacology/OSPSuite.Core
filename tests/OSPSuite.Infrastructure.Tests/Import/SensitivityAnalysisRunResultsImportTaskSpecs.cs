using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Core.Importer.Services;
using OSPSuite.Utility.Events;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_SensitivityAnalysisRunResultsImportTask : ContextSpecificationAsync<ISensitivityAnalysisRunResultsImportTask>
   {
      protected IPKParameterSensitivitiesImporter _pkParameterSensitivitiesImporter;
      private IEntitiesInSimulationRetriever _quantityRetriever;
      protected SensitivityAnalysisRunResultsImport _results;
      protected readonly List<PKParameterSensitivity> _pkParameterSensitivityList1 = new List<PKParameterSensitivity>();
      protected readonly List<PKParameterSensitivity> _pkParameterSensitivityList2 = new List<PKParameterSensitivity>();
      private const string _file1 = "File1";
      private const string _file2 = "File2";
      protected IModelCoreSimulation _simulation;
      protected List<string> _files;
      protected CancellationToken _cancellationToken;
      protected readonly PathCache<IQuantity> _allQuantities = new PathCache<IQuantity>(new EntityPathResolverForSpecs());
      private IProgressManager _progressManager;

      public override async Task GlobalContext()
      {
         await base.GlobalContext();
         _allQuantities.Add("Organism|Liver|Int|Drug", new Parameter());
      }

      protected override Task Context()
      {
         _pkParameterSensitivitiesImporter = A.Fake<IPKParameterSensitivitiesImporter>();
         _quantityRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         _progressManager = A.Fake<IProgressManager>();
         _simulation = A.Fake<IModelCoreSimulation>();
         _files = new List<string> {_file1, _file2};
         _cancellationToken = new CancellationToken();
         sut = new SensitivityAnalysisRunResultsImportTask(_quantityRetriever, _pkParameterSensitivitiesImporter, _progressManager);
         A.CallTo(_quantityRetriever).WithReturnType<PathCache<IQuantity>>().Returns(_allQuantities);
         A.CallTo(() => _pkParameterSensitivitiesImporter.ImportFrom(_file1, _simulation, A<IImportLogger>._)).Returns(_pkParameterSensitivityList1);
         A.CallTo(() => _pkParameterSensitivitiesImporter.ImportFrom(_file2, _simulation, A<IImportLogger>._)).Returns(_pkParameterSensitivityList2);

         return _completed;
      }

      protected PKParameterSensitivity CreatePKParameterSensitivity(string parameterName, string quantityPath = "Organism|Liver|Int|Drug", string pkParameterName = "AUC")
      {
         return new PKParameterSensitivity
         {
            ParameterName = parameterName,
            QuantityPath = quantityPath,
            PKParameterName = pkParameterName,
         };
      }
   }

   public class When_importing_the_pk_parameter_sensitivity_defined_in_a_valid_set_of_files : concern_for_SensitivityAnalysisRunResultsImportTask
   {
      protected override async Task Context()
      {
         await base.Context();
         _pkParameterSensitivityList1.Add(CreatePKParameterSensitivity("P1"));
         _pkParameterSensitivityList1.Add(CreatePKParameterSensitivity("P2"));
         _pkParameterSensitivityList2.Add(CreatePKParameterSensitivity("P3"));
         _pkParameterSensitivityList2.Add(CreatePKParameterSensitivity("P4"));
      }

      protected override async Task Because()
      {
         _results = await sut.ImportResults(_simulation, _files, _cancellationToken);
      }

      [Observation]
      public void should_return_the_pk_parameter_sensitivity_containing_all_results_defined_in_the_files()
      {
         _results.SensitivityAnalysisRunResult.AllPKParameterSensitivities.Count.ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_return_a_valid_status()
      {
         _results.HasError.ShouldBeFalse();
      }
   }

   public class When_importing_the_pk_parameter_sensitivity_defined_in_a_set_of_file_containing_duplicate_parameter_names : concern_for_SensitivityAnalysisRunResultsImportTask
   {
      protected override async Task Context()
      {
         await base.Context();
         _pkParameterSensitivityList1.Add(CreatePKParameterSensitivity("P1"));
         _pkParameterSensitivityList1.Add(CreatePKParameterSensitivity("P2"));
         _pkParameterSensitivityList2.Add(CreatePKParameterSensitivity("P2"));
         _pkParameterSensitivityList2.Add(CreatePKParameterSensitivity("P4"));
      }

      protected override async Task Because()
      {
         _results = await sut.ImportResults(_simulation, _files, _cancellationToken);
      }

      [Observation]
      public void should_return_an_invalid_status()
      {
         _results.HasError.ShouldBeTrue();
      }
   }

   public class When_importing_the_pk_parameter_sensitivity_defined_in_a_set_of_files_with_unknown_quantity_path : concern_for_SensitivityAnalysisRunResultsImportTask
   {
      protected override async Task Context()
      {
         await base.Context();
         _allQuantities.Add("KNOWN_PATH", new Parameter());

         _pkParameterSensitivityList1.Add(CreatePKParameterSensitivity("P1", "KNOWN_PATH"));
         _pkParameterSensitivityList1.Add(CreatePKParameterSensitivity("P2", "UNKNOWN_PATH"));
      }

      protected override async Task Because()
      {
         _results = await sut.ImportResults(_simulation, _files, _cancellationToken);
      }

      [Observation]
      public void should_return_an_invalid_status()
      {
         _results.HasError.ShouldBeTrue();
      }
   }
}