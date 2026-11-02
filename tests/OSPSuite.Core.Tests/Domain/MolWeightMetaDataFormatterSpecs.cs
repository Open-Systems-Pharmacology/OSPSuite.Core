using System.Globalization;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_MolWeightMetaDataFormatter : StaticContextSpecification
   {
      protected IDimension _molWeightDimension;
      protected string _result;

      protected override void Context()
      {
         //base unit kg/µmol, default unit g/mol as in the suite dimension definition
         _molWeightDimension = new Dimension(new BaseDimensionRepresentation(), Constants.Dimension.MOLECULAR_WEIGHT, "kg/µmol");
         _molWeightDimension.AddUnit("g/mol", 1e-9, 0, isDefault: true);
      }
   }

   public class When_formatting_the_mol_weight_of_a_parameter_for_the_importer : concern_for_MolWeightMetaDataFormatter
   {
      private IParameter _molWeightParameter;

      protected override void Context()
      {
         base.Context();
         _molWeightParameter = DomainHelperForSpecs.ConstantParameterWithValue(300e-9).WithDimension(_molWeightDimension);
      }

      protected override void Because()
      {
         _result = MolWeightMetaDataFormatter.MolWeightInDefaultUnitFor(_molWeightParameter);
      }

      [Observation]
      public void should_return_the_value_in_g_per_mol()
      {
         double.Parse(_result, CultureInfo.CurrentCulture).ShouldBeEqualTo(300, 1e-6);
      }
   }

   public class When_formatting_a_mol_weight_value_in_base_unit_for_the_importer : concern_for_MolWeightMetaDataFormatter
   {
      protected override void Because()
      {
         _result = MolWeightMetaDataFormatter.MolWeightInDefaultUnitFor(450e-9, _molWeightDimension);
      }

      [Observation]
      public void should_return_the_value_in_g_per_mol()
      {
         double.Parse(_result, CultureInfo.CurrentCulture).ShouldBeEqualTo(450, 1e-6);
      }
   }

   public class When_formatting_an_undefined_mol_weight_for_the_importer : concern_for_MolWeightMetaDataFormatter
   {
      [Observation]
      public void should_return_an_empty_string_for_a_missing_parameter()
      {
         MolWeightMetaDataFormatter.MolWeightInDefaultUnitFor(null).ShouldBeEqualTo(string.Empty);
      }

      [Observation]
      public void should_return_an_empty_string_for_a_missing_value()
      {
         MolWeightMetaDataFormatter.MolWeightInDefaultUnitFor(null, _molWeightDimension).ShouldBeEqualTo(string.Empty);
      }

      [Observation]
      public void should_return_an_empty_string_for_a_nan_value()
      {
         MolWeightMetaDataFormatter.MolWeightInDefaultUnitFor(double.NaN, _molWeightDimension).ShouldBeEqualTo(string.Empty);
      }

      [Observation]
      public void should_return_an_empty_string_for_a_missing_dimension()
      {
         MolWeightMetaDataFormatter.MolWeightInDefaultUnitFor(300e-9, null).ShouldBeEqualTo(string.Empty);
      }
   }
}
