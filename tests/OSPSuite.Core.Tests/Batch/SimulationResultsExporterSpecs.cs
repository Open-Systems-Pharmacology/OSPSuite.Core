using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Batch;
using OSPSuite.Core.Batch.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Infrastructure.Services;
using OSPSuite.Utility;

namespace OSPSuite.Batch
{
   public abstract class concern_for_SimulationResultsExporter : ContextSpecification<ISimulationResultsExporter>
   {
      protected IDataRepositoryTask _dataRepositoryTask;
      protected IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;
      protected ISimulationResultsToBatchSimulationExportMapper _batchSimulationExportMapper;
      protected ISimulation _simulation;
      protected DataRepository _results;
      protected string _fileName;

      protected override void Context()
      {
         _dataRepositoryTask = A.Fake<IDataRepositoryTask>();
         _quantityDisplayPathMapper = A.Fake<IQuantityPathToQuantityDisplayPathMapper>();
         _batchSimulationExportMapper = A.Fake<ISimulationResultsToBatchSimulationExportMapper>();
         sut = new SimulationResultsExporter(_dataRepositoryTask, _quantityDisplayPathMapper, _batchSimulationExportMapper);

         _simulation = A.Fake<ISimulation>();
         _results = new DataRepository();
         _fileName = FileHelper.GenerateTemporaryFileName();
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.DeleteFile(_fileName);
      }
   }

   public class When_exporting_simulation_results_to_csv : concern_for_SimulationResultsExporter
   {
      private DataColumnExportOptions _exportOptions;
      private DataColumn _dataColumn;

      protected override void Context()
      {
         base.Context();

         A.CallTo(() => _dataRepositoryTask.ExportToCsvAsync(A<IEnumerable<DataColumn>>._, _fileName, A<DataColumnExportOptions>._))
            .Invokes(x => _exportOptions = x.GetArgument<DataColumnExportOptions>(2));

         _dataColumn = DomainHelperForSpecs.ObservedData().FirstDataColumn();
         _dataColumn.Name = "TOTO";

         A.CallTo(() => _quantityDisplayPathMapper.DisplayPathAsStringFor(_simulation, _dataColumn,false)).Returns("NEW NAME");
      }

      protected override void Because()
      {
         sut.ExportToCsvAsync(_simulation, _results, _fileName).Wait();
      }

      [Observation]
      public void should_export_the_value_unformatted_and_in_core_unit()
      {
         _exportOptions.UseDisplayUnit.ShouldBeFalse();
      }
      
      [Observation]
      public void should_use_the_quantity_display_path_mapper_to_create_the_name_of_the_column()
      {
         _exportOptions.ColumnNameRetriever(_dataColumn).ShouldBeEqualTo("NEW NAME");
      }
   }

   public class When_exporting_simulation_results_to_json : concern_for_SimulationResultsExporter
   {
      private BatchSimulationExport _batchSimulationExport;

      protected override void Context()
      {
         base.Context();
         _batchSimulationExport = new BatchSimulationExport {Name = "Sim"};
         A.CallTo(() => _batchSimulationExportMapper.MapFrom(_simulation, _results)).Returns(_batchSimulationExport);
      }

      [Observation]
      public void should_create_a_batch_simulation_export_object_and_export_it_to_the_file()
      {
         sut.ExportToJsonAsync(_simulation, _results, _fileName).Wait();
         FileHelper.FileExists(_fileName).ShouldBeTrue();
      }
   }
}