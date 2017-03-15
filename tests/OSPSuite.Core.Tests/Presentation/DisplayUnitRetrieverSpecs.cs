using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_DisplayUnitRetriever : ContextSpecification<IDisplayUnitRetriever>
   {
      private IPresentationUserSettings _userSettings;
      private IProjectRetriever _projectRetriever;
      private DisplayUnitsManager _userSettingsDisplayUnits;
      private DisplayUnitsManager _projectDisplayUnits;
      protected Unit _userSettingsUnit;
      protected Unit _projectSettingsUnit;
      protected IDimension _dimension;
      protected Unit _defaultDimensionUnit;

      protected override void Context()
      {
         _userSettings = A.Fake<IPresentationUserSettings>();
         _projectRetriever = A.Fake<IProjectRetriever>();
         _userSettingsDisplayUnits = A.Fake<DisplayUnitsManager>();
         _projectDisplayUnits = A.Fake<DisplayUnitsManager>();
         A.CallTo(() => _userSettings.DisplayUnits).Returns(_userSettingsDisplayUnits);
         A.CallTo(() => _projectRetriever.CurrentProject.DisplayUnits).Returns(_projectDisplayUnits);
         sut = new DisplayUnitRetriever(_projectRetriever, _userSettings);
         _dimension = A.Fake<IDimension>();
         _defaultDimensionUnit = A.Fake<Unit>();
         _dimension.DefaultUnit = _defaultDimensionUnit;
         A.CallTo(() => _userSettingsDisplayUnits.DisplayUnitFor(_dimension)).Returns(_userSettingsUnit);
         A.CallTo(() => _projectDisplayUnits.DisplayUnitFor(_dimension)).Returns(_projectSettingsUnit);
      }
   }

   public class When_retrieving_the_display_unit_for_a_dimension_that_has_no_predefined_settings_in_project_or_user_settings : concern_for_DisplayUnitRetriever
   {
      [Observation]
      public void should_return_the_display_unit_defined_in_the_dimension()
      {
         sut.PreferredUnitFor(_dimension).ShouldBeEqualTo(_defaultDimensionUnit);
      }
   }

   public class When_retrieving_the_display_unit_for_an_object_with_display_unit_that_has_no_predefined_settings_in_project_or_user_settings : concern_for_DisplayUnitRetriever
   {
      private IWithDisplayUnit _withDisplayUnit;
      private Unit _usedDisplayUnit;

      protected override void Context()
      {
         base.Context();
         _withDisplayUnit = A.Fake<IWithDisplayUnit>().WithDimension(_dimension);
         _usedDisplayUnit = A.Fake<Unit>();
      }

      [Observation]
      public void should_return_the_display_unit_defined_in_the_dimension_if_the_display_unit_is_null()
      {
         _withDisplayUnit.DisplayUnit = null;
         sut.PreferredUnitFor(_withDisplayUnit).ShouldBeEqualTo(_defaultDimensionUnit);
      }

      [Observation]
      public void should_return_the_existing_display_unit_defined_in_the_object_otherwise()
      {
         _withDisplayUnit.DisplayUnit = _usedDisplayUnit;
         A.CallTo(() => _dimension.HasUnit(_usedDisplayUnit)).Returns(true);
         sut.PreferredUnitFor(_withDisplayUnit).ShouldBeEqualTo(_usedDisplayUnit);
      }
   }

   public class When_retrieving_the_display_unit_for_an_object_with_display_unit_that_has_no_predefined_settings_in_project_or_user_settings_and_does_not_exist_in_the_dimension : concern_for_DisplayUnitRetriever
   {
      private IWithDisplayUnit _withDisplayUnit;
      private Unit _usedDisplayUnit;

      protected override void Context()
      {
         base.Context();
         _withDisplayUnit = A.Fake<IWithDisplayUnit>().WithDimension(_dimension);
         _usedDisplayUnit = A.Fake<Unit>();
         A.CallTo(() => _usedDisplayUnit.Name).Returns("DOES NOT EXIST");
         A.CallTo(() => _dimension.HasUnit(_usedDisplayUnit)).Returns(false);
         _withDisplayUnit.DisplayUnit = _usedDisplayUnit;
      }

      [Observation]
      public void should_return_the_existing_display_unit_defined_in_the_object_otherwise()
      {
         sut.PreferredUnitFor(_withDisplayUnit).ShouldBeEqualTo(_defaultDimensionUnit);
      }
   }


   public class When_retrieving_the_display_unit_for_a_dimension_that_has_a_predefined_settings_in_project_and_user_settings : concern_for_DisplayUnitRetriever
   {
      protected override void Context()
      {
         _projectSettingsUnit = A.Fake<Unit>();
         _userSettingsUnit = A.Fake<Unit>();
         base.Context();
      }

      [Observation]
      public void should_return_the_display_unit_defined_in_the_project()
      {
         sut.PreferredUnitFor(_dimension).ShouldBeEqualTo(_projectSettingsUnit);
      }
   }

   public class When_retrieving_the_display_unit_for_a_dimension_that_has_a_predefined_settings_in_the_user_settings_but_not_in_the_project : concern_for_DisplayUnitRetriever
   {
      protected override void Context()
      {
         _userSettingsUnit = A.Fake<Unit>();
         base.Context();
      }

      [Observation]
      public void should_return_the_display_unit_defined_in_the_project()
      {
         sut.PreferredUnitFor(_dimension).ShouldBeEqualTo(_userSettingsUnit);
      }
   }
}