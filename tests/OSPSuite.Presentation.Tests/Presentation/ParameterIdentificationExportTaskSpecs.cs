using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization.ParameterIdentificationExport;
using OSPSuite.Core.Serialization.SimModel.Services;
using OSPSuite.Core.Services;
using OSPSuite.Core.Services.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationExportTask : ContextSpecification<ParameterIdentificationExportTask>
   {
      protected IParameterIdentificationExporter _parameterIdentificationExporter;
      protected ILazyLoadTask _lazyLoadTask;
      protected ISimulationToModelCoreSimulationMapper _simulationToModelCoreSimulationMapper;
      protected ISimModelExporter _simModelExporter;
      protected ParameterIdentification _parameterIdentification;
      protected List<string> _createdDirectories;
      protected List<string> _deletedDirectories;
      private Func<string, string> _initialCreateDirectory;
      private Action<string, bool> _initialDeleteDirectory;
      protected IDialogCreator _dialogCreator;
      private Action<string, string> _initialStringWriteToFile;
      protected ICache<string, string> _writtenFileCache;
      protected IExportDataTableToExcelTask _exportToExcelTask;

      public override void GlobalContext()
      {
         _initialCreateDirectory = DirectoryHelper.CreateDirectory;
         _initialDeleteDirectory = DirectoryHelper.DeleteDirectory;
         _initialStringWriteToFile = FileHelper.WriteStringToFile;
         DirectoryHelper.CreateDirectory = path =>
         {
            _createdDirectories.Add(path);
            return path;
         };
         DirectoryHelper.DeleteDirectory = (path, recursive) => _deletedDirectories.Add(path);
         FileHelper.WriteStringToFile = (content, path) => _writtenFileCache.Add(path, content);
      }

      protected override void Context()
      {
         _deletedDirectories = new List<string>();
         _parameterIdentificationExporter = A.Fake<IParameterIdentificationExporter>();
         _lazyLoadTask = A.Fake<ILazyLoadTask>();
         _simulationToModelCoreSimulationMapper = A.Fake<ISimulationToModelCoreSimulationMapper>();
         _simModelExporter = A.Fake<ISimModelExporter>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _exportToExcelTask = A.Fake<IExportDataTableToExcelTask>();
         _parameterIdentification = A.Fake<ParameterIdentification>();
         _writtenFileCache = new Cache<string, string>();
         _parameterIdentification.Name = "name with spaces";
         _createdDirectories = new List<string>();
         sut = new ParameterIdentificationExportTask(_dialogCreator, _simModelExporter, _simulationToModelCoreSimulationMapper, _lazyLoadTask, _parameterIdentificationExporter, _exportToExcelTask);
      }

      public override void GlobalCleanup()
      {
         DirectoryHelper.CreateDirectory = _initialCreateDirectory;
         DirectoryHelper.DeleteDirectory = _initialDeleteDirectory;
         FileHelper.WriteStringToFile = _initialStringWriteToFile;
      }
   }

   public class When_exporting_a_parameter_identification_but_user_cancels_when_selecting_the_directory : concern_for_ParameterIdentificationExportTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(string.Empty);
      }

      protected override void Because()
      {
         sut.ExportToMatlab(_parameterIdentification).Wait();
      }

      [Observation]
      public void a_call_to_the_file_system_service_must_not_have_happened()
      {
         _createdDirectories.ShouldBeEmpty();
      }
   }

   public class When_exporting_a_parameter_identification_to_R_and_the_directory_already_exists : concern_for_ParameterIdentificationExportTask
   {
      private ISimulation _simulation;
      private IModelCoreSimulation _modelCoreSimulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
         _modelCoreSimulation = A.Fake<IModelCoreSimulation>();
         A.CallTo(() => _dialogCreator.AskForFolder(A<string>._, A<string>._, A<string>._)).Returns("a path");
         DirectoryHelper.DirectoryExists = path => true;
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, Captions.Delete, Captions.Cancel)).Returns(ViewResult.Yes);
         A.CallTo(() => _parameterIdentification.AllSimulations).Returns(new[] {_simulation});
         A.CallTo(() => _simulationToModelCoreSimulationMapper.MapFrom(_simulation, A<bool>._)).Returns(_modelCoreSimulation);
      }

      protected override void Because()
      {
         sut.ExportToR(_parameterIdentification).Wait();
      }

      [Observation]
      public void the_file_should_be_written_at_the_correct_location()
      {
         _writtenFileCache.Keys.Single().ShouldBeEqualTo("a path\\name with spaces\\name_with_spaces.r");
      }

      [Observation]
      public void the_file_should_have_been_written_to_the_file_system()
      {
         _writtenFileCache.Count.ShouldBeEqualTo(1);
         _writtenFileCache.Single().ShouldNotBeEmpty();
      }

      [Observation]
      public void the_model_core_exporter_is_used_to_export_the_simulation()
      {
         A.CallTo(() => _simModelExporter.Export(_modelCoreSimulation, A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void the_file_system_service_is_used_to_remove_the_directory_and_create_it_again()
      {
         _deletedDirectories.ShouldOnlyContain("a path\\name with spaces");
         _createdDirectories.ShouldOnlyContain("a path\\name with spaces");
      }

      [Observation]
      public void the_dialog_creator_must_be_used_to_inform_the_user()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, Captions.Delete, Captions.Cancel)).MustHaveHappened();
      }
   }

   public class When_exporting_a_parameter_identification_to_matlab_and_the_directory_already_exists : concern_for_ParameterIdentificationExportTask
   {
      private ISimulation _simulation;
      private IModelCoreSimulation _modelCoreSimulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
         _modelCoreSimulation = A.Fake<IModelCoreSimulation>();
         A.CallTo(() => _dialogCreator.AskForFolder(A<string>._, A<string>._, A<string>._)).Returns("a path");
         DirectoryHelper.DirectoryExists = path => true;
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, Captions.Delete, Captions.Cancel)).Returns(ViewResult.Yes);
         A.CallTo(() => _parameterIdentification.AllSimulations).Returns(new[] {_simulation});
         A.CallTo(() => _simulationToModelCoreSimulationMapper.MapFrom(_simulation, A<bool>._)).Returns(_modelCoreSimulation);
      }

      protected override void Because()
      {
         sut.ExportToMatlab(_parameterIdentification).Wait();
      }

      [Observation]
      public void the_model_core_exporter_is_used_to_export_the_simulation()
      {
         A.CallTo(() => _simModelExporter.Export(_modelCoreSimulation, A<string>._)).MustHaveHappened();
      }

      [Observation]
      public void the_file_system_service_is_used_to_remove_the_directory_and_create_it_again()
      {
         _deletedDirectories.ShouldOnlyContain("a path\\name with spaces");
         _createdDirectories.ShouldOnlyContain("a path\\name with spaces");
      }

      [Observation]
      public void the_dialog_creator_must_be_used_to_inform_the_user()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, Captions.Delete, Captions.Cancel)).MustHaveHappened();
      }
   }

   public class When_exporting_a_parameter_identification_to_xml : concern_for_ParameterIdentificationExportTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _dialogCreator.AskForFolder(A<string>._, A<string>._, A<string>._)).Returns("A path");
      }

      protected override void Because()
      {
         sut.ExportToMatlab(_parameterIdentification).Wait();
      }

      [Observation]
      public void the_file_should_be_written_at_the_correct_location()
      {
         _writtenFileCache.Keys.Single().ShouldBeEqualTo("A path\\name with spaces\\name_with_spaces.m");
      }

      [Observation]
      public void the_directory_must_be_created_by_the_file_system_service()
      {
         _createdDirectories.ShouldOnlyContain("A path\\name with spaces");
      }

      [Observation]
      public void the_lazy_load_task_should_be_used_to_load_an_identification_that_is_not_loaded()
      {
         A.CallTo(() => _lazyLoadTask.Load(_parameterIdentification)).MustHaveHappened();
      }

      [Observation]
      public void The_paramter_identification_exporter_is_used_to_export_the_object()
      {
         A.CallTo(() => _parameterIdentificationExporter.Export(_parameterIdentification, A<string>._)).MustHaveHappened();
      }
   }

   public class When_exporting_the_parameters_history_to_excel_and_the_user_cancels_the_action : concern_for_ParameterIdentificationExportTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(null);
      }

      protected override void Because()
      {
         sut.ExportParametersHistoryToExcel(new List<IdentificationParameterHistory>(), new List<float>());
      }

      [Observation]
      public void should_not_export_to_excel()
      {
         A.CallTo(() => _exportToExcelTask.ExportDataTableToExcel(A<DataTable>._, A<string>._, A<bool>._)).MustNotHaveHappened();
      }
   }

   public class When_exporting_the_parameters_history_to_excel : concern_for_ParameterIdentificationExportTask
   {
      private DataTable _dataTable;
      private List<IdentificationParameterHistory> _parametersHistory;
      private IDimension _dimension;
      private List<float> _errorHistory;
      private IdentificationParameterHistory _history1;
      private IdentificationParameterHistory _history2;

      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns("FILE");
         A.CallTo(() => _exportToExcelTask.ExportDataTableToExcel(A<DataTable>._, A<string>._, true))
            .Invokes(x => _dataTable = x.GetArgument<DataTable>(0));

         _dimension = DomainHelperForSpecs.LengthDimensionForSpecs();
         _parametersHistory = new List<IdentificationParameterHistory>();
         _errorHistory = new List<float>();
         var identificationParameter1 = A.Fake<IdentificationParameter>();
         identificationParameter1.Name = "A";
         A.CallTo(() => identificationParameter1.Dimension).Returns(_dimension);
         A.CallTo(() => identificationParameter1.DisplayUnit).Returns(_dimension.Units.ElementAt(1));

         var identificationParameter2 = A.Fake<IdentificationParameter>();
         identificationParameter2.Name = "B";
         A.CallTo(() => identificationParameter2.Dimension).Returns(_dimension);
         A.CallTo(() => identificationParameter2.DisplayUnit).Returns(_dimension.Units.ElementAt(1));

         _history1 = new IdentificationParameterHistory(identificationParameter1);
         _history2 = new IdentificationParameterHistory(identificationParameter2);
         _history1.AddValue(1);
         _history1.AddValue(2);
         _history1.AddValue(3);
         _history2.AddValue(4);
         _history2.AddValue(5);
         _history2.AddValue(6);
         _parametersHistory.Add(_history1);
         _parametersHistory.Add(_history2);

         _errorHistory.AddRange(new[] {7, 8, 9, 10f});
      }

      protected override void Because()
      {
         sut.ExportParametersHistoryToExcel(_parametersHistory, _errorHistory).Wait();
      }

      [Observation]
      public void should_export_the_expected_table_using_the_display_unit_and_the_error()
      {
         _dataTable.Columns.Count.ShouldBeEqualTo(3);
         _dataTable.Rows.Count.ShouldBeEqualTo(3);
         _dataTable.AllValuesInColumn<double>(_history1.DisplayName).ShouldOnlyContainInOrder(100d, 200d, 300d);
         _dataTable.AllValuesInColumn<double>(_history2.DisplayName).ShouldOnlyContainInOrder(400d, 500d, 600d);
         _dataTable.AllValuesInColumn<double>(Captions.ParameterIdentification.Error).ShouldOnlyContainInOrder(7d, 8d, 9d);
      }
   }
}