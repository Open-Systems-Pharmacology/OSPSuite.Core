﻿using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Exceptions;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_EditObservedDataTask : ContextSpecification<IObservedDataMetaDataTask>
   {
      private IApplicationController _applicationController;
      private IOSPSuiteExecutionContext _context;
      private IDimensionFactory _dimensionFactory;
      protected IParameterIdentificationTask _parameterIdentificationTask;

      protected override void Context()
      {
         _applicationController = A.Fake<IApplicationController>();
         _context = A.Fake<IOSPSuiteExecutionContext>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _parameterIdentificationTask = A.Fake<IParameterIdentificationTask>();

         sut = new ObservedDataMetaDataTask(_context, _applicationController, _dimensionFactory, _parameterIdentificationTask);
      }
   }


   public abstract class When_executing_metadata_commands_on_repository : concern_for_EditObservedDataTask
   {
      protected ICommand _result;
      protected List<DataRepository> _list;
      protected string _expectedCommandType;
      protected string _expectedDescription;

      protected override void Context()
      {
         base.Context();
         _list = new List<DataRepository> {new DataRepository(), new DataRepository()};
      }

      [Observation]
      public void returned_command_should_be_pksimmacrocommand()
      {
         _result.ShouldBeAnInstanceOf<IMacroCommand>();
      }

      [Observation]
      public void returned_command_should_contain_a_command_per_repository()
      {
         var command = _result as IMacroCommand;

         // ReSharper disable once PossibleNullReferenceException - allow this to fail if returned command is wrong type
         command.Count.ShouldBeEqualTo(_list.Count);
      }

      [Observation]
      public void description_has_appropriate_information()
      {
         _result.Description.ShouldBeEqualTo(_expectedDescription);
      }

      [Observation]
      public void should_have_set_the_expected_type()
      {
         _result.CommandType.ShouldBeEqualTo(_expectedCommandType);
      }

      [Observation]
      public void object_type_should_be_observed_data()
      {
         _result.ObjectType.ShouldBeEqualTo(ObjectTypes.ObservedData);
      }
   }

   public class When_executing_metadata_remove_command_on_repsoitory : When_executing_metadata_commands_on_repository
   {
      protected override void Context()
      {
         base.Context();
         _expectedCommandType = Command.CommandTypeDelete;
         _expectedDescription = Command.MetaDataRemovedFromDataRepositories;
      }

      protected override void Because()
      {
         _result = sut.RemoveMetaData(_list, new MetaDataKeyValue {Key = "Key", Value = "Value"});
      }
   }

   public class When_executing_metadata_change_command_on_repository : When_executing_metadata_commands_on_repository
   {
      protected override void Context()
      {
         base.Context();
         _expectedCommandType = Command.CommandTypeEdit;
         _expectedDescription = Command.MetaDataModifiedInDataRepositories;
      }

      protected override void Because()
      {
         _result = sut.ChangeMetaData(_list, new MetaDataChanged());
      }
   }

   public class When_executing_metadata_add_command_on_repository : When_executing_metadata_commands_on_repository
   {
      protected override void Context()
      {
         base.Context();
         _expectedCommandType = Command.CommandTypeAdd;
         _expectedDescription = Command.MetaDataAddedToDataRepositories;
      }

      protected override void Because()
      {
         _result = sut.AddMetaData(_list, new MetaDataKeyValue {Key = "Key", Value = "Value"});
      }
   }

   public class When_executing_the_update_molecular_weight_on_repositories : When_executing_metadata_commands_on_repository
   {
      private DataRepository _observedData;
      private DataColumn _column;

      protected override void Context()
      {
         base.Context();
         _expectedCommandType = Command.CommandTypeEdit;
         _expectedDescription = Command.MolecularWeightModifiedInDataRepositories;
         var baseGrid = new BaseGrid("Time", "Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _column = new DataColumn("Col", "Col", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), baseGrid) ;

         _observedData = new DataRepository { _column };

         _list = new List<DataRepository>{_observedData};
      }

      protected override void Because()
      {
         _result = sut.UpdateMolWeight(_list, 20, 50);
      }

      [Observation]
      public void should_have_set_the_mol_weight_as_expected()
      {
         _column.DataInfo.MolWeight.ShouldBeEqualTo(50);
      }
   }
}