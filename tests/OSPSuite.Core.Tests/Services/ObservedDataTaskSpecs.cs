using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using OSPSuite.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_ObservedDataTask : ContextSpecification<IObservedDataTask>
   {
      protected IDialogCreator _dialogCreator;
      protected IOSPSuiteExecutionContext _context;
      protected IDataRepositoryExportTask _dataRepositoryTask;
      protected IContainerTask _containerTask;
      protected IObjectTypeResolver _objectTypeResolver;
      protected IConfirmationManager _confirmationManager;
      protected IOutputMappingMatchingTask _outputMappingMatchingTask;
      protected DataRepository _obsData1;
      protected DataRepository _obsData2;
      protected List<IUsesObservedData> _allUserOfObservedData = new List<IUsesObservedData>();

      protected override void Context()
      {
         _dialogCreator = A.Fake<IDialogCreator>();
         _context = A.Fake<IOSPSuiteExecutionContext>();
         _dataRepositoryTask = A.Fake<IDataRepositoryExportTask>();
         _containerTask = A.Fake<IContainerTask>();
         _objectTypeResolver = A.Fake<IObjectTypeResolver>();
         _confirmationManager = A.Fake<IConfirmationManager>();
         _outputMappingMatchingTask = A.Fake<IOutputMappingMatchingTask>();

         sut = new ObservedDataTaskForSpecs(_dialogCreator, _context, _dataRepositoryTask, _containerTask, _objectTypeResolver, _confirmationManager, _outputMappingMatchingTask);

         _obsData1 = DomainHelperForSpecs.ObservedData("OBS1");
         _obsData2 = DomainHelperForSpecs.ObservedData("OBS2");

         A.CallTo(() => _context.Project.AllUsersOfObservedData).Returns(_allUserOfObservedData);
      }
   }

   public class When_the_observed_data_task_is_deleting_some_observed_data_that_are_used_in_different_analyzable_of_the_project : concern_for_ObservedDataTask
   {
      private IUsesObservedData _userOfObservedData1;
      private IUsesObservedData _userOfObservedData2;

      protected override void Context()
      {
         base.Context();
         _userOfObservedData1 = A.Fake<IUsesObservedData>().WithName("USER_OF_DATA_1");
         _userOfObservedData2 = A.Fake<IUsesObservedData>().WithName("USER_OF_DATA_2");
         A.CallTo(() => _objectTypeResolver.TypeFor(_userOfObservedData1)).Returns("THE USER TYPE 1");
         A.CallTo(() => _objectTypeResolver.TypeFor(_userOfObservedData2)).Returns("THE USER TYPE 2");
         _allUserOfObservedData.Add(_userOfObservedData1);
         _allUserOfObservedData.Add(_userOfObservedData2);
         A.CallTo(() => _userOfObservedData1.UsesObservedData(_obsData1)).Returns(true);
         A.CallTo(() => _userOfObservedData2.UsesObservedData(_obsData1)).Returns(true);
         A.CallTo(() => _userOfObservedData1.UsesObservedData(_obsData2)).Returns(true);
      }

      [Observation]
      public void should_throw_an_exception_notifying_the_user_that_no_observed_data_can_be_deleted()
      {
         The.Action(() => sut.Delete(new[] { _obsData1, _obsData2 })).ShouldThrowAn<CannotDeleteObservedDataException>();
      }
   }

   public class When_the_observed_data_task_is_deleting_some_observed_data_and_only_a_subset_of_which_can_be_deleted : concern_for_ObservedDataTask
   {
      private IUsesObservedData _userOfObservedData1;
      private IUsesObservedData _userOfObservedData2;
      private string _message;

      protected override void Context()
      {
         base.Context();
         _userOfObservedData1 = A.Fake<IUsesObservedData>().WithName("USER_OF_DATA_1");
         _userOfObservedData2 = A.Fake<IUsesObservedData>().WithName("USER_OF_DATA_2");
         A.CallTo(() => _objectTypeResolver.TypeFor(_userOfObservedData1)).Returns("THE USER TYPE 1");
         A.CallTo(() => _objectTypeResolver.TypeFor(_userOfObservedData2)).Returns("THE USER TYPE 2");
         _allUserOfObservedData.Add(_userOfObservedData1);
         _allUserOfObservedData.Add(_userOfObservedData2);
         A.CallTo(() => _userOfObservedData1.UsesObservedData(_obsData1)).Returns(true);
         A.CallTo(() => _userOfObservedData2.UsesObservedData(_obsData1)).Returns(true);
         A.CallTo(() => _userOfObservedData1.UsesObservedData(_obsData2)).Returns(false);

         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).Invokes(x => _message = x.GetArgument<string>(0));
      }

      protected override void Because()
      {
         sut.Delete(new[] { _obsData1, _obsData2 });
      }

      [Observation]
      public void should_show_a_message_to_the_user_letting_him_know_that_some_used_observed_data_cannot_be_deleted()
      {
         _message.Contains(_userOfObservedData1.Name).ShouldBeTrue();
         _message.Contains(_userOfObservedData2.Name).ShouldBeTrue();
         _message.Contains(_obsData1.Name).ShouldBeTrue();
         _message.Contains(_obsData2.Name).ShouldBeFalse();
      }
   }

   public class When_the_observed_data_task_is_deleting_empty_observed_data : concern_for_ObservedDataTask
   {
      [Observation]
      public void should_return_true()
      {
         sut.Delete(new List<DataRepository>()).ShouldBeTrue();
      }
   }

   public class When_the_observed_data_task_is_deleting_some_observed_data_and_the_silent_mode_is_activated : concern_for_ObservedDataTask
   {
      private IUsesObservedData _userOfObservedData1;
      private IUsesObservedData _userOfObservedData2;

      protected override void Context()
      {
         base.Context();
         _userOfObservedData1 = A.Fake<IUsesObservedData>().WithName("USER_OF_DATA_1");
         _userOfObservedData2 = A.Fake<IUsesObservedData>().WithName("USER_OF_DATA_2");
         A.CallTo(() => _objectTypeResolver.TypeFor(_userOfObservedData1)).Returns("THE USER TYPE 1");
         A.CallTo(() => _objectTypeResolver.TypeFor(_userOfObservedData2)).Returns("THE USER TYPE 2");
         _allUserOfObservedData.Add(_userOfObservedData1);
         _allUserOfObservedData.Add(_userOfObservedData2);
         A.CallTo(() => _userOfObservedData1.UsesObservedData(_obsData1)).Returns(true);
         A.CallTo(() => _userOfObservedData2.UsesObservedData(_obsData1)).Returns(true);
         A.CallTo(() => _userOfObservedData1.UsesObservedData(_obsData2)).Returns(false);
      }

      protected override void Because()
      {
         sut.Delete(new[] { _obsData1, _obsData2 }, silent: true);
      }

      [Observation]
      public void should_not_prompt_the_user_to_confirm()
      {
         A.CallTo(() => _dialogCreator.MessageBoxYesNo(A<string>._, ViewResult.Yes)).MustNotHaveHappened();
      }
   }

   public class When_the_observed_data_task_exported_to_excel : concern_for_ObservedDataTask
   {
      private float _lloq;

      protected override void Context()
      {
         base.Context();
         _lloq = 1.2f;
         _obsData1.Columns.Last().DataInfo.LLOQ = _lloq;
         A.CallTo(() => _dialogCreator.AskForFileToSave(A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored, A<string>.Ignored)).Returns("export filename");
      }

      protected override void Because()
      {
         sut.Export(_obsData1);
      }

      [Observation]
      public void should_export_included_lloq_Columns()
      {
         _ = A.CallTo(() => _dataRepositoryTask.ExportToExcel(
               A<IEnumerable<DataColumn>>.That.Matches(observedData => observedDataIncludesLLOQColumn(observedData)),
               A<string>._,
               A<bool>.That.IsEqualTo(true),
               A<DataColumnExportOptions>._))
            .MustHaveHappened();
      }

      private bool observedDataIncludesLLOQColumn(IEnumerable<DataColumn> observedData)
      {
         var dataColumns = observedData as DataColumn[] ?? observedData.ToArray();

         dataColumns.Count().ShouldBeEqualTo(3);
         dataColumns.Count(c => c.Name.StartsWith("LLOQ")).ShouldBeEqualTo(1);
         dataColumns.Single(od => od.Name.StartsWith("LLOQ")).Values[0].ShouldBeEqualTo(_lloq);
         return true;
      }
   }

   public class When_adding_observed_data_to_a_simulation : concern_for_ObservedDataTask
   {
      private ISimulation _simulation;
      private DataRepository _alreadyUsedObservedData;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
         _alreadyUsedObservedData = _obsData2;
         A.CallTo(() => _simulation.UsesObservedData(_obsData1)).Returns(false);
         A.CallTo(() => _simulation.UsesObservedData(_alreadyUsedObservedData)).Returns(true);
      }

      protected override void Because()
      {
         sut.AddObservedDataToAnalysable(new[] {_obsData1, _alreadyUsedObservedData}, _simulation, showData: true);
      }

      [Observation]
      public void should_register_the_observed_data_not_already_used_by_the_simulation()
      {
         A.CallTo(() => _simulation.AddUsedObservedData(_obsData1)).MustHaveHappened();
         A.CallTo(() => _simulation.AddUsedObservedData(_alreadyUsedObservedData)).MustNotHaveHappened();
      }

      [Observation]
      public void should_add_matching_output_mappings_only_for_the_newly_used_observed_data()
      {
         A.CallTo(() => _outputMappingMatchingTask.AddMatchingOutputMapping(_obsData1, _simulation)).MustHaveHappened();
         A.CallTo(() => _outputMappingMatchingTask.AddMatchingOutputMapping(_alreadyUsedObservedData, _simulation)).MustNotHaveHappened();
      }

      [Observation]
      public void should_mark_the_simulation_as_changed()
      {
         A.CallToSet(() => _simulation.HasChanged).To(true).MustHaveHappened();
      }

      [Observation]
      public void should_notify_that_observed_data_was_added_to_the_simulation()
      {
         A.CallTo(() => _context.PublishEvent(A<ObservedDataAddedToAnalysableEvent>.That.Matches(x => x.ShowData && x.ObservedData.Contains(_obsData1)))).MustHaveHappened();
      }
   }

   public class When_adding_observed_data_already_used_by_the_simulation : concern_for_ObservedDataTask
   {
      private ISimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>();
         A.CallTo(() => _simulation.UsesObservedData(_obsData1)).Returns(true);
      }

      protected override void Because()
      {
         sut.AddObservedDataToAnalysable(new[] {_obsData1}, _simulation);
      }

      [Observation]
      public void should_not_register_the_observed_data_again()
      {
         A.CallTo(() => _simulation.AddUsedObservedData(A<DataRepository>._)).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_notify_any_change()
      {
         A.CallTo(() => _context.PublishEvent(A<ObservedDataAddedToAnalysableEvent>._)).MustNotHaveHappened();
      }
   }

   public abstract class concern_for_ObservedDataTask_updating_mol_weight : concern_for_ObservedDataTask
   {
      protected const double MOL_WEIGHT_STORED_WITH_DATA = 100;
      protected const double MOL_WEIGHT_OF_PROJECT_MOLECULE = 250;
      protected DataColumn _observationColumn;
      protected ObservedDataTaskForSpecs _observedDataTask;

      protected override void Context()
      {
         base.Context();
         _observedDataTask = (ObservedDataTaskForSpecs) sut;
         _observedDataTask.ProjectMolecules["Drug"] = MOL_WEIGHT_OF_PROJECT_MOLECULE;
         _observationColumn = _obsData1.AllButBaseGrid().Single();
         _observationColumn.DataInfo.MolWeight = MOL_WEIGHT_STORED_WITH_DATA;
      }

      protected void AddMoleculeMetaData(string moleculeName)
      {
         _obsData1.ExtendedProperties.Add(new ExtendedProperty<string> {Name = Constants.ObservedData.MOLECULE, Value = moleculeName});
      }

      protected override void Because()
      {
         sut.UpdateMolWeight(_obsData1);
      }
   }

   public class When_updating_the_mol_weight_of_observed_data_whose_molecule_is_defined_in_the_project : concern_for_ObservedDataTask_updating_mol_weight
   {
      protected override void Context()
      {
         base.Context();
         AddMoleculeMetaData("Drug");
      }

      [Observation]
      public void should_take_the_mol_weight_of_the_project_molecule_for_the_observation_columns()
      {
         _observationColumn.DataInfo.MolWeight.ShouldBeEqualTo(MOL_WEIGHT_OF_PROJECT_MOLECULE);
      }

      [Observation]
      public void should_not_set_a_mol_weight_on_the_base_grid()
      {
         _obsData1.BaseGrid.DataInfo.MolWeight.ShouldBeNull();
      }
   }

   public class When_updating_the_mol_weight_of_observed_data_whose_molecule_is_not_defined_in_the_project : concern_for_ObservedDataTask_updating_mol_weight
   {
      protected override void Context()
      {
         base.Context();
         AddMoleculeMetaData("Unknown molecule");
      }

      [Observation]
      public void should_keep_the_mol_weight_stored_with_the_observed_data()
      {
         _observationColumn.DataInfo.MolWeight.ShouldBeEqualTo(MOL_WEIGHT_STORED_WITH_DATA);
      }
   }

   public class When_updating_the_mol_weight_of_observed_data_without_molecule_metadata : concern_for_ObservedDataTask_updating_mol_weight
   {
      [Observation]
      public void should_keep_the_mol_weight_stored_with_the_observed_data()
      {
         _observationColumn.DataInfo.MolWeight.ShouldBeEqualTo(MOL_WEIGHT_STORED_WITH_DATA);
      }

      [Observation]
      public void should_not_look_for_a_project_molecule()
      {
         _observedDataTask.RequestedMoleculeNames.ShouldBeEmpty();
      }
   }

   public class ObservedDataTaskForSpecs : ObservedDataTask
   {
      /// <summary>
      ///    Molecules of the fake project with their mol weight, keyed by name
      /// </summary>
      public Dictionary<string, double> ProjectMolecules { get; } = new Dictionary<string, double>();

      public List<string> RequestedMoleculeNames { get; } = new List<string>();

      public ObservedDataTaskForSpecs(IDialogCreator dialogCreator, IOSPSuiteExecutionContext executionContext, IDataRepositoryExportTask dataRepositoryTask, IContainerTask containerTask, IObjectTypeResolver objectTypeResolver, IConfirmationManager confirmationManager, IOutputMappingMatchingTask outputMappingMatchingTask) : base(dialogCreator, executionContext, dataRepositoryTask,
         containerTask, objectTypeResolver, confirmationManager, outputMappingMatchingTask)
      {
      }

      public override void Rename(DataRepository observedData)
      {
      }

      protected override double? MolWeightFor(string moleculeName)
      {
         RequestedMoleculeNames.Add(moleculeName);
         return ProjectMolecules.TryGetValue(moleculeName, out var molWeight) ? molWeight : (double?) null;
      }
   }
}