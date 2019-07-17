using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_DisplayUnitsPresenter : ContextSpecification<IDisplayUnitsPresenter>
   {
      private IDimensionFactory _dimensionFactory;
      private IDisplayUnitsView _view;
      protected DisplayUnitsManager _displayUnitManager;
      private List<IDimension> _allDimensions;
      protected IDimension _dimension1;
      protected IDimension _dimension2;
      protected IDimension _dimension3;
      protected IEnumerable<DefaultUnitMapDTO> _allDTOs;
      protected IDialogCreator _dialogCreator;
      protected ISerializationTask _serializationTask;
      private ICloneManagerForModel _cloneManager;

      protected override void Context()
      {
         _view = A.Fake<IDisplayUnitsView>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _serializationTask = A.Fake<ISerializationTask>();
         _cloneManager = A.Fake<ICloneManagerForModel>();

         sut = new DisplayUnitsPresenter(_view, _dimensionFactory, _dialogCreator, _serializationTask, _cloneManager);

         _displayUnitManager = new DisplayUnitsManager();
         _allDimensions = new List<IDimension>();
         A.CallTo(() => _dimensionFactory.Dimensions).Returns(_allDimensions);
         _dimension1 = A.Fake<IDimension>();
         _dimension2 = A.Fake<IDimension>();
         _dimension3 = A.Fake<IDimension>();
         _allDimensions.Add(_dimension1);
         _allDimensions.Add(_dimension2);
         _allDimensions.Add(_dimension3);

         A.CallTo(() => _view.BindTo(A<IEnumerable<DefaultUnitMapDTO>>._))
            .Invokes(x => _allDTOs = x.GetArgument<IEnumerable<DefaultUnitMapDTO>>(0));
      }
   }

   public class When_editing_the_default_units_and_the_user_accepts_the_action : concern_for_DisplayUnitsPresenter
   {
      protected override void Context()
      {
         base.Context();
         _displayUnitManager.AddDisplayUnit(new DisplayUnitMap {Dimension = _dimension1, DisplayUnit = _dimension1.DefaultUnit});
         _displayUnitManager.AddDisplayUnit(new DisplayUnitMap {Dimension = _dimension2, DisplayUnit = _dimension2.DefaultUnit});
      }

      protected override void Because()
      {
         sut.Edit(_displayUnitManager);
      }

      [Observation]
      public void should_bind_the_available_default_unit_map_to_the_view()
      {
         _allDTOs.Count().ShouldBeEqualTo(2);
      }
   }

   public class When_retrieving_the_dimensions_available_for_a_given_default_unit_map : concern_for_DisplayUnitsPresenter
   {
      private DefaultUnitMapDTO _defaultUnitMapDTO;

      protected override void Context()
      {
         base.Context();
         _displayUnitManager.AddDisplayUnit(new DisplayUnitMap {Dimension = _dimension1, DisplayUnit = _dimension1.DefaultUnit});
         var defaultUnitMap = new DisplayUnitMap {Dimension = _dimension2, DisplayUnit = _dimension2.DefaultUnit};
         _displayUnitManager.AddDisplayUnit(defaultUnitMap);
         _defaultUnitMapDTO = new DefaultUnitMapDTO(defaultUnitMap);
         sut.Edit(_displayUnitManager);
      }

      [Observation]
      public void should_return_all_the_dimensions_defined_in_the_project_but_the_one_already_mapped()
      {
         sut.AllPossibleDimensionsFor(_defaultUnitMapDTO).ShouldOnlyContain(_dimension2, _dimension3);
      }
   }

   public class When_the_user_decides_to_add_a_new_default_unit_map : concern_for_DisplayUnitsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.Edit(_displayUnitManager);
      }

      protected override void Because()
      {
         sut.AddDefaultUnit();
      }

      [Observation]
      public void should_add_a_new_default_unit_mapping_and_display_it_on_the_view()
      {
         _displayUnitManager.AllDisplayUnits.Any().ShouldBeTrue();
         _allDTOs.Count().ShouldBeEqualTo(1);
      }
   }

   public class When_the_user_removes_a_default_unit_map : concern_for_DisplayUnitsPresenter
   {
      protected override void Context()
      {
         base.Context();
         _displayUnitManager.AddDisplayUnit(new DisplayUnitMap {Dimension = _dimension1, DisplayUnit = _dimension1.DefaultUnit});
         sut.Edit(_displayUnitManager);
      }

      protected override void Because()
      {
         sut.RemoveDefaultUnit(_allDTOs.First());
      }

      [Observation]
      public void should_remvove_it_from_the_list_of_managed_default_units()
      {
         _displayUnitManager.AllDisplayUnits.Count().ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_remove_if_from_the_view()
      {
         _allDTOs.ShouldBeEmpty();
      }
   }

   public class When_saving_the_current_units_to_a_file : concern_for_DisplayUnitsPresenter
   {
      private string _fileName;

      protected override void Context()
      {
         base.Context();
         _fileName = "file";
         A.CallTo(() => _dialogCreator.AskForFileToSave(Captions.SaveUnitsToFile, Constants.Filter.UNITS_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, null, null)).Returns(_fileName);
         sut.Edit(_displayUnitManager);
      }

      protected override void Because()
      {
         sut.SaveUnitsToFile();
      }

      [Observation]
      public void should_save_the_units_into_a_file_selected_by_the_user()
      {
         A.CallTo(() => _serializationTask.SaveModelPart(_displayUnitManager, _fileName)).MustHaveHappened();
      }
   }

   public class When_loading_the_current_units_from_a_file : concern_for_DisplayUnitsPresenter
   {
      private string _fileName;
      private DisplayUnitsManager _newDisplayUnitManager;

      protected override void Context()
      {
         base.Context();
         _fileName = "file";
         A.CallTo(() => _dialogCreator.AskForFileToOpen(Captions.LoadUnitsFromFile, Constants.Filter.UNITS_FILE_FILTER, Constants.DirectoryKey.MODEL_PART, null, null)).Returns(_fileName);
         sut.Edit(_displayUnitManager);
         _newDisplayUnitManager = new DisplayUnitsManager();
         _newDisplayUnitManager.AddDisplayUnit(new DisplayUnitMap {Dimension = _dimension1, DisplayUnit = _dimension1.DefaultUnit});
         A.CallTo(() => _serializationTask.Load<DisplayUnitsManager>(_fileName, false)).Returns(_newDisplayUnitManager);
      }

      protected override void Because()
      {
         sut.LoadUnitsFromFile();
      }

      [Observation]
      public void should_ask_the_user_to_select_a_file_containing_the_unit_to_load_and_display_the_units_in_the_view()
      {
         _displayUnitManager.AllDisplayUnits.Count().ShouldBeEqualTo(1);
         _displayUnitManager.AllDisplayUnits.ElementAt(0).Dimension.ShouldBeEqualTo(_dimension1);
         _displayUnitManager.AllDisplayUnits.ElementAt(0).DisplayUnit.ShouldBeEqualTo(_dimension1.DefaultUnit);
      }
   }
}