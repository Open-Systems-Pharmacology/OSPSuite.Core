using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Mappers
{
   public abstract class concern_for_QuantityToOriginalQuantityValueMapper : ContextSpecification<QuantityToOriginalQuantityValueMapper>
   {
      protected IEntityPathResolver _entityPathResolver;
      protected IContainer _root;

      protected override void Context()
      {
         _entityPathResolver = new EntityPathResolverForSpecs();
         _root = new Container().WithName("Root");
         sut = new QuantityToOriginalQuantityValueMapper(_entityPathResolver);
      }
   }

   public class When_mapping_a_parameter_to_an_original_quantity_value : concern_for_QuantityToOriginalQuantityValueMapper
   {
      private IParameter _parameter;
      private OriginalQuantityValue _result;

      protected override void Context()
      {
         base.Context();
         _parameter = new Parameter
         {
            Name = "P",
            Formula = new ExplicitFormula("1"),
            Dimension = DimensionFactoryForSpecs.LengthDimension,
         };
         _parameter.Value = 2;
         _parameter.DisplayUnit = _parameter.Dimension.Unit("cm");
         _parameter.UpdateValueOriginFrom(new ValueOrigin { Source = ValueOriginSources.Internet });
         _root.Add(_parameter);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_parameter);
      }

      [Observation]
      public void should_map_the_path_value_dimension_display_unit_and_value_origin_of_the_parameter()
      {
         _result.Path.ShouldBeEqualTo("Root|P");
         _result.Value.ShouldBeEqualTo(2);
         _result.Dimension.ShouldBeEqualTo(_parameter.Dimension);
         _result.DisplayUnit.ShouldBeEqualTo(_parameter.DisplayUnit);
         _result.ValueOrigin.ShouldBeEqualTo(_parameter.ValueOrigin);
         _result.Type.ShouldBeEqualTo(OriginalQuantityValue.Types.Quantity);
      }
   }

   public class When_mapping_the_scale_divisor_of_a_molecule_amount_to_an_original_quantity_value : concern_for_QuantityToOriginalQuantityValueMapper
   {
      private MoleculeAmount _moleculeAmount;
      private OriginalQuantityValue _result;

      protected override void Context()
      {
         base.Context();
         _moleculeAmount = new MoleculeAmount { Name = "Drug", ScaleDivisor = 0.5 };
         _root.Add(_moleculeAmount);
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_moleculeAmount);
      }

      [Observation]
      public void should_map_the_path_and_the_scale_divisor_without_dimension()
      {
         _result.Path.ShouldBeEqualTo("Root|Drug");
         _result.Value.ShouldBeEqualTo(0.5);
         _result.Dimension.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION);
         _result.DisplayUnit.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION.DefaultUnit);
         _result.Type.ShouldBeEqualTo(OriginalQuantityValue.Types.ScaleDivisor);
      }
   }
}
