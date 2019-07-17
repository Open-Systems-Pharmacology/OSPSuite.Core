using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterStartValue : ContextSpecification<ParameterStartValue>
   {
      protected override void Context()
      {
         sut = new ParameterStartValue {ContainerPath = new ObjectPath("Path1", "Path2"), Name = "Name"};
      }
   }

   public class when_instantiating_new_ParameterStartValue : concern_for_ParameterStartValue
   {
      [Observation]
      public void name_should_be_last_element_in_ObjectPath()
      {
         sut.ParameterName.ShouldBeEqualTo("Name");
      }

      [Observation]
      public void container_path_should_be_equal_to_all_but_last_element()
      {
         sut.ContainerPath.ShouldOnlyContainInOrder("Path1", "Path2");
      }

      [Observation]
      public void parameter_path_should_be_equal_to_container_path_plus_parameter_name()
      {
         sut.Path.ShouldOnlyContainInOrder("Path1", "Path2", "Name");
      }
   }

   public class when_creating_parameter_start_value_with_empty_path : concern_for_ParameterStartValue
   {
      protected override void Context()
      {
         sut = new ParameterStartValue();
      }

      [Observation]
      public void container_path_should_be_empty()
      {
         sut.ContainerPath.ShouldBeEqualTo(ObjectPath.Empty);
      }

      [Observation]
      public void name_should_be_null()
      {
         sut.ParameterName.ShouldBeEqualTo(ObjectPath.Empty.ToString());
      }
   }

   public class when_setting_parameter_name : concern_for_ParameterStartValue
   {
      protected override void Because()
      {
         sut.Name = "Name2";
      }

      [Observation]
      public void parameter_name_should_be_updated()
      {
         sut.ParameterName.ShouldBeEqualTo("Name2");
      }

      [Observation]
      public void container_path_is_not_affected()
      {
         sut.ContainerPath.ShouldOnlyContainInOrder("Path1", "Path2");
      }

      [Observation]
      public void parameter_path_should_reflect_new_name()
      {
         sut.Path.ShouldOnlyContainInOrder("Path1", "Path2", "Name2");
      }
   }

   public abstract class equivalency_should_test_false : equivalent_parameter_start_values_setup
   {
      [Observation]
      public void should_not_be_equivalent()
      {
         _result.ShouldBeFalse();
      }
   }

   public class when_comparing_psv_with_different_container_path : equivalency_should_test_false
   {
      protected override void Context()
      {
         base.Context();
         sut.Path = new ObjectPath("Z", "B", "C");
      }
   }

   public class when_comparing_psv_with_different_name : equivalency_should_test_false
   {
      protected override void Context()
      {
         base.Context();
         sut.Name = "NewName";
      }
   }

   public class when_comparing_psv_with_different_null_start_values : equivalency_should_test_false
   {
      protected override void Context()
      {
         base.Context();
         sut.StartValue = null;
      }
   }

   public class when_comparing_psv_with_different_start_values : equivalency_should_test_false
   {
      protected override void Context()
      {
         base.Context();
         sut.StartValue = _comparable.StartValue + 1.0;
      }
   }

   public class when_comparing_psv_with_different_formula : equivalency_should_test_false
   {
      protected override void Context()
      {
         base.Context();
         sut.Formula = new ExplicitFormula("MA");
      }
   }

   public class when_comparing_psv_with_different_dimension : equivalency_should_test_false
   {
      protected override void Context()
      {
         base.Context();
         sut.Dimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Concentration);
      }
   }

   public class when_comparing_psv_with_different_icon : equivalency_should_test_false
   {
      protected override void Context()
      {
         base.Context();
         sut.Icon = "notIcon";
      }
   }

   public class when_comparing_psv_with_different_description : equivalency_should_test_false
   {
      protected override void Context()
      {
         base.Context();
         sut.Description = null;
      }
   }

   public abstract class equivalent_parameter_start_values_setup : concern_for_ParameterStartValue
   {
      protected ParameterStartValue _comparable;
      protected bool _result;
      protected override void Context()
      {
         sut = new ParameterStartValue();
         _comparable = new ParameterStartValue();

         sut.Path = new ObjectPath("A", "B", "Name");
         _comparable.Path = sut.Path.Clone<IObjectPath>();

         sut.StartValue = 1.0;
         _comparable.StartValue = sut.StartValue;

         sut.Formula = new ExplicitFormula("MV");
         _comparable.Formula = new ExplicitFormula("MV");

         sut.Dimension = DimensionFactoryForSpecs.Factory.Dimension(DimensionFactoryForSpecs.DimensionNames.Mass);
         _comparable.Dimension = sut.Dimension;

         sut.DisplayUnit = sut.Dimension.DefaultUnit;
         _comparable.DisplayUnit = sut.DisplayUnit;

         sut.Icon = "string";
         _comparable.Icon = "string";


         sut.Description = "Description";
         _comparable.Description = "Description";
      }

      protected override void Because()
      {
         _result = sut.IsEquivalentTo(_comparable);
      }
   }

   public class when_comparing_equivalent_parameter_start_values : equivalent_parameter_start_values_setup
   {
      [Observation]
      public void equivalency_should_be_true()
      {
         _result.ShouldBeTrue();
      }
   }

   public class when_comparing_reference_equal_start_values : when_comparing_equivalent_parameter_start_values
   {
      protected override void Because()
      {
         _result = sut.IsEquivalentTo(sut);
      }
   }

   public class when_comparing_empty_start_values : concern_for_ParameterStartValue
   {
      protected ParameterStartValue _comparable;
      protected bool _result;

      protected override void Context()
      {
         sut = new ParameterStartValue();
         _comparable = new ParameterStartValue();
      }

      protected override void Because()
      {
         _result = sut.IsEquivalentTo(_comparable);
      }

      [Observation]
      public void start_values_should_be_equal()
      {
         _result.ShouldBeTrue();
      }
   }
}