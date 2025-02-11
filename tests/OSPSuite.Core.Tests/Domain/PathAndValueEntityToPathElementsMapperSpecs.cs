using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core.Domain
{
   public class concern_for_PathAndValueEntityToPathElementsMapper : ContextSpecification<PathAndValueEntityToPathElementsMapper>
   {
      protected const string _topContainer = "TopContainer";
      protected const string _container1 = "Container1";
      protected const string _container2 = "Container2";
      protected const string _bottomCompartment = "BottomCompartment";
      protected const string _moleculeName = "MoleculeName";
      protected const string _parameterName = "ParameterName";

      protected override void Context()
      {
         sut = new PathAndValueEntityToPathElementsMapper();
      }
   }

   public class When_mapping_reference_concentration : concern_for_PathAndValueEntityToPathElementsMapper
   {
      private PathElements _result;
      private ExpressionParameter _expressionParameter;

      protected override void Context()
      {
         base.Context();
         _expressionParameter = new ExpressionParameter
         {
            Path = new ObjectPath(_moleculeName, _parameterName)
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_expressionParameter);
      }

      [Observation]
      public void the_result_should_map_PathElementId_correctly()
      {
         _result[PathElementId.Name].DisplayName.ShouldBeEqualTo(_parameterName);
         _result[PathElementId.Molecule].DisplayName.ShouldBeEqualTo(_moleculeName); ;
      }
   }

   public class When_mapping_initial_conditions_to_path_elements : concern_for_PathAndValueEntityToPathElementsMapper
   {
      private PathElements _result;
      private InitialCondition _msv;

      protected override void Context()
      {
         base.Context();
         _msv = new InitialCondition
         {
            Path = new ObjectPath(_topContainer, _container1, _container2, _bottomCompartment, _moleculeName)
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_msv);
      }

      [Observation]
      public void the_result_should_map_PathElementId_correctly()
      {
         _result[PathElementId.Name].DisplayName.ShouldBeEqualTo(_moleculeName);
         _result[PathElementId.Molecule].DisplayName.ShouldBeEqualTo(_moleculeName);
         _result[PathElementId.BottomCompartment].DisplayName.ShouldBeEqualTo(_bottomCompartment);
         _result[PathElementId.TopContainer].DisplayName.ShouldBeEqualTo(_topContainer);
         _result[PathElementId.Container].DisplayName.ShouldBeEqualTo($"{_container1}{Constants.DISPLAY_PATH_SEPARATOR}{_container2}");
      }
   }

   public class When_mapping_expression_parameter_to_path_elements : concern_for_PathAndValueEntityToPathElementsMapper
   {
      private PathElements _result;
      private ExpressionParameter _individualParameter;

      protected override void Context()
      {
         base.Context();
         _individualParameter = new ExpressionParameter
         {
            Path = new ObjectPath(_topContainer, _container1, _container2, _bottomCompartment, _moleculeName, _parameterName)
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_individualParameter);
      }

      [Observation]
      public void the_result_should_map_PathElementId_correctly()
      {
         _result[PathElementId.Name].DisplayName.ShouldBeEqualTo(_parameterName);
         _result[PathElementId.Molecule].DisplayName.ShouldBeEqualTo(_moleculeName);
         _result[PathElementId.BottomCompartment].DisplayName.ShouldBeEqualTo(_bottomCompartment);
         _result[PathElementId.TopContainer].DisplayName.ShouldBeEqualTo(_topContainer);
         _result[PathElementId.Container].DisplayName.ShouldBeEqualTo($"{_container1}{Constants.DISPLAY_PATH_SEPARATOR}{_container2}");
      }
   }

   public class When_mapping_individual_parameter_to_path_elements : concern_for_PathAndValueEntityToPathElementsMapper
   {
      private PathElements _result;
      private IndividualParameter _individualParameter;

      protected override void Context()
      {
         base.Context();
         _individualParameter = new IndividualParameter
         {
            Path = new ObjectPath(_topContainer, _container1, _container2, _bottomCompartment, _parameterName)
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_individualParameter);
      }

      [Observation]
      public void the_result_should_map_PathElementId_correctly()
      {
         _result[PathElementId.Name].DisplayName.ShouldBeEqualTo(_parameterName);
         _result[PathElementId.Molecule].DisplayName.ShouldBeNullOrEmpty();
         _result[PathElementId.BottomCompartment].DisplayName.ShouldBeEqualTo(_bottomCompartment);
         _result[PathElementId.TopContainer].DisplayName.ShouldBeEqualTo(_topContainer);
         _result[PathElementId.Container].DisplayName.ShouldBeEqualTo($"{_container1}{Constants.DISPLAY_PATH_SEPARATOR}{_container2}");
      }
   }

   public class When_mapping_parameter_values_to_path_elements : concern_for_PathAndValueEntityToPathElementsMapper
   {
      private PathElements _result;
      private ParameterValue _psv;

      protected override void Context()
      {
         base.Context();
         _psv = new ParameterValue
         {
            Path = new ObjectPath(_topContainer, _container1, _container2, _bottomCompartment, _parameterName)
         };
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_psv);
      }

      [Observation]
      public void the_result_should_map_PathElementId_correctly()
      {
         _result[PathElementId.Name].DisplayName.ShouldBeEqualTo(_parameterName);
         _result[PathElementId.Molecule].DisplayName.ShouldBeNullOrEmpty();
         _result[PathElementId.BottomCompartment].DisplayName.ShouldBeEqualTo(_bottomCompartment);
         _result[PathElementId.TopContainer].DisplayName.ShouldBeEqualTo(_topContainer);
         _result[PathElementId.Container].DisplayName.ShouldBeEqualTo($"{_container1}{Constants.DISPLAY_PATH_SEPARATOR}{_container2}");
      }
   }
}