using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SolverSettingsFactory : ContextSpecification<ISolverSettingsFactory>
   {
      protected IObjectBaseFactory _objectBaseFactory;
      protected IParameterFactory _parameterFactory;

      protected override void Context()
      {
         _objectBaseFactory = A.Fake<IObjectBaseFactory>();
         _parameterFactory = A.Fake<IParameterFactory>();
         sut = new SolverSettingsFactory(_objectBaseFactory, _parameterFactory);
      }
   }

   public class When_creating_a_new_set_of_solver_settings_from_the_solver_settings_factory : concern_for_SolverSettingsFactory
   {
      private SolverSettings _solverSettings;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _objectBaseFactory.Create<SolverSettings>()).Returns(new SolverSettings());
         A.CallTo(() => _parameterFactory.CreateParameter(A<string>._, A<double>._, A<IDimension>._, A<string>._, A<IFormula>._,A<Unit>._))
            .ReturnsLazily(x=>new Parameter().WithName(x.GetArgument<string>(0)));
      }

      protected override void Because()
      {
         _solverSettings = sut.CreateCVODE();
      }

      [Observation]
      public void should_return_a_solver_setting_object_having_the_epxected_solver_name()
      {
         _solverSettings.Name.ShouldBeEqualTo(Constants.CVODES);
      }

      [Observation]
      public void should_have_initialized_the_default_solver_settings_parameter_as_default()
      {
         _solverSettings.AllParameters().Each(x => x.IsDefault.ShouldBeTrue());
      }

      [Observation]
      public void should_have_initialized_the_default_solver_settings_as_not_variable()
      {
         _solverSettings.AllParameters().Each(x => x.CanBeVaried.ShouldBeFalse());
      }

      [Observation]
      public void should_have_initialized_the_default_solver_settings_as_not_variable_in_population()
      {
         _solverSettings.AllParameters().Each(x => x.CanBeVariedInPopulation.ShouldBeFalse());
      }

      [Observation]
      public void should_have_initialized_the_default_solver_settings_as_visible_and_editable()
      {
         _solverSettings.AllParameters().Each(x => x.Visible.ShouldBeTrue());
         _solverSettings.AllParameters().Each(x => x.Editable.ShouldBeTrue());
      }
   }
}