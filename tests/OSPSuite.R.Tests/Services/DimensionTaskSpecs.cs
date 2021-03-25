using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_DimensionTask : ContextForIntegration<IDimensionTask>
   {
      protected double[] _result;

      protected override void Context()
      {
         sut = Api.GetDimensionTask();
      }
   }

   public class When_converting_a_double_array_from_mass_unit_to_mass_unit : concern_for_DimensionTask
   {
      protected override void Because()
      {
         _result = sut.ConvertToUnit(Constants.Dimension.MASS_AMOUNT, "g", new[] {1d, 2d, 3d});
      }

      [Observation]
      public void should_be_able_to_convert_the_array()
      {
         _result.ShouldOnlyContainInOrder(1000, 2000, 3000d);
      }
   }

   public class When_converting_a_double_array_from_mass_unit_to_mass_unit_using_a_micro_unit : concern_for_DimensionTask
   {
      private double[] _result_u;
      private double[] _result_mc;

      protected override void Because()
      {
         _result = sut.ConvertToUnit(Constants.Dimension.MASS_AMOUNT, "µg", new[] {1e-3, 2e-3});
         _result_u = sut.ConvertToUnit(Constants.Dimension.MASS_AMOUNT, "ug", new[] {1e-3, 2e-3});
         _result_mc = sut.ConvertToUnit(Constants.Dimension.MASS_AMOUNT, "mcg", new[] {1e-3, 2e-3});
      }

      [Observation]
      public void should_be_able_to_convert_the_array()
      {
         _result.ShouldOnlyContainInOrder(1000000, 2000000);
         _result_u.ShouldOnlyContainInOrder(1000000, 2000000);
         _result_mc.ShouldOnlyContainInOrder(1000000, 2000000);
      }
   }

   public class When_converting_a_double_array_from_molar_unit_to_mass_unit : concern_for_DimensionTask
   {
      protected override void Because()
      {
         //20 µmol/kg
         _result = sut.ConvertToUnit(Constants.Dimension.MOLAR_AMOUNT, "kg", new[] {1d, 2d, 3d}, molWeight: 20);
      }

      [Observation]
      public void should_be_able_to_convert_the_array()
      {
         _result.ShouldOnlyContainInOrder(20, 40, 60d);
      }
   }

   public class When_returning_the_mu_symbol_to_be_used_for_conversion : concern_for_DimensionTask
   {
      [Observation]
      public void should_return_the_expected_symbol()
      {
         sut.MuSymbol.ShouldBeEqualTo("µ");
         //also verifies that we are using the value defined in the dimension files
         sut.MuSymbol.ShouldBeEqualTo(sut.DimensionByName(Constants.Dimension.MOLAR_AMOUNT).BaseUnit.Name.Substring(0, 1));
      }
   }

   public class When_converting_a_double_value_from_molar_unit_to_mass_unit : concern_for_DimensionTask
   {
      protected override void Because()
      {
         //50 kg/mol
         _result = sut.ConvertToUnit(Constants.Dimension.MOLAR_AMOUNT, "kg", 10, molWeight: 50);
      }

      [Observation]
      public void should_be_able_to_convert_the_array()
      {
         _result.ShouldOnlyContainInOrder(500);
      }
   }

   public class When_converting_a_double_value_from_mass_unit_to_molar_unit : concern_for_DimensionTask
   {
      protected override void Because()
      {
         //50 kg/mol
         _result = sut.ConvertToBaseUnit(Constants.Dimension.MOLAR_AMOUNT, "kg", 500, molWeight: 50);
      }

      [Observation]
      public void should_be_able_to_convert_the_array()
      {
         _result.ShouldOnlyContainInOrder(10);
      }
   }

   public class When_converting_a_double_value_from_mg_to_kg_unit : concern_for_DimensionTask
   {
      protected override void Because()
      {
         _result = sut.ConvertToBaseUnit(Constants.Dimension.MASS_AMOUNT, "g", new[] {1, 2, 3.0});
      }

      [Observation]
      public void should_be_able_to_convert_the_array()
      {
         _result.ShouldOnlyContainInOrder(0.001, 0.002, 0.003);
      }
   }

   public class When_converting_a_double_array_from_mass_unit_to_molar_unit_and_the_mol_weight_is_not_present : concern_for_DimensionTask
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.ConvertToUnit(Constants.Dimension.MASS_AMOUNT, "µmol", new[] {1d, 2d, 3d})).ShouldThrowAn<UnableToResolveParametersException>();
      }
   }

   public class When_converting_a_double_array_from_mass_unit_to_molar_unit : concern_for_DimensionTask
   {
      protected override void Because()
      {
         //20 µmol/kg
         _result = sut.ConvertToUnit(Constants.Dimension.MASS_AMOUNT, "µmol", new[] {1d}, molWeight: 20);
      }

      [Observation]
      public void should_be_able_to_convert_the_array()
      {
         _result.ShouldOnlyContainInOrder(1 / 20d);
      }
   }

   public class When_retrieving_the_name_of_all_dimensions : concern_for_DimensionTask
   {
      private string[] _dimensionNames;

      protected override void Because()
      {
         _dimensionNames = sut.AllAvailableDimensionNames();
      }

      [Observation]
      public void should_return_the_dimensions_names_sorted()
      {
         _dimensionNames[0].StartsWith("A").ShouldBeTrue();
      }
   }

   public class When_retrieving_all_unit_names_defined_for_a_given_dimension : concern_for_DimensionTask
   {
      private string[] _unitNames;

      protected override void Because()
      {
         _unitNames = sut.AllAvailableUnitNamesFor("Mass");
      }

      [Observation]
      public void should_return_the_expected_units()
      {
         _unitNames.ShouldOnlyContain("kg", "g", "mg", "µg", "ng", "pg");
      }
   }

   public class When_retrieving_all_unit_names_defined_for_a_dimension_that_does_not_exist : concern_for_DimensionTask
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.AllAvailableUnitNamesFor("DOES_NOT_EXIST")).ShouldThrowAn<KeyNotFoundException>();
      }
   }

   public class When_retrieving_the_base_for_a_given_dimension : concern_for_DimensionTask
   {
      [Observation]
      public void should_return_the_expected_unit()
      {
         sut.BaseUnitFor("Mass").ShouldBeEqualTo("kg");
      }
   }

   public class When_retrieving_the_base_unit_for_an_unknown_dimension : concern_for_DimensionTask
   {
      [Observation]
      public void should_throw_an_exception()
      {
         The.Action(() => sut.BaseUnitFor("DOES_NOT_EXIST")).ShouldThrowAn<KeyNotFoundException>();
      }
   }

   public class When_checking_if_a_dimension_has_a_unit : concern_for_DimensionTask
   {
      [Observation]
      public void should_return_the_true_if_the_dimension_has_unit()
      {
         sut.HasUnit("Mass", "kg").ShouldBeTrue();
      }

      [Observation]
      public void should_return_the_false_if_the_dimension_does_not_have_the_unit()
      {
         sut.HasUnit("Mass", "cm").ShouldBeFalse();
      }

      [Observation]
      public void should_throw_an_exception_if_the_dimension_is_not_found()
      {
         The.Action(() => sut.HasUnit("TOTO", "cm")).ShouldThrowAn<KeyNotFoundException>();
      }
   }

   public class When_retrieving_the_dimensions_for_standard_pk_parameters : concern_for_DimensionTask
   {
      [Observation]
      public void should_return_the_expected_dimension()
      {
         sut.DimensionForStandardPKParameter(StandardPKParameter.Unknown).Name.ShouldBeEqualTo(Constants.Dimension.NO_DIMENSION.Name);
         sut.DimensionForStandardPKParameter(StandardPKParameter.C_max).Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_CONCENTRATION);
         sut.DimensionForStandardPKParameter(StandardPKParameter.C_max_norm).Name.ShouldBeEqualTo(Constants.Dimension.MASS_CONCENTRATION);
         sut.DimensionForStandardPKParameter(StandardPKParameter.C_min).Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_CONCENTRATION);
         sut.DimensionForStandardPKParameter(StandardPKParameter.C_min_norm).Name.ShouldBeEqualTo(Constants.Dimension.MASS_CONCENTRATION);
         sut.DimensionForStandardPKParameter(StandardPKParameter.t_max).Name.ShouldBeEqualTo(Constants.Dimension.TIME);
         sut.DimensionForStandardPKParameter(StandardPKParameter.t_min).Name.ShouldBeEqualTo(Constants.Dimension.TIME);
         sut.DimensionForStandardPKParameter(StandardPKParameter.C_trough).Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_CONCENTRATION);
         sut.DimensionForStandardPKParameter(StandardPKParameter.C_trough_norm).Name.ShouldBeEqualTo(Constants.Dimension.MASS_CONCENTRATION);
         sut.DimensionForStandardPKParameter(StandardPKParameter.AUC_tEnd).Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AUC);
         sut.DimensionForStandardPKParameter(StandardPKParameter.AUC_tEnd_norm).Name.ShouldBeEqualTo(Constants.Dimension.MASS_AUC);
         sut.DimensionForStandardPKParameter(StandardPKParameter.AUCM_tEnd).Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AUCM);
         sut.DimensionForStandardPKParameter(StandardPKParameter.AUC_inf).Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AUC);
         sut.DimensionForStandardPKParameter(StandardPKParameter.AUC_inf_norm).Name.ShouldBeEqualTo(Constants.Dimension.MASS_AUC);
         sut.DimensionForStandardPKParameter(StandardPKParameter.AUC_tEnd_inf).Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_AUC);
         sut.DimensionForStandardPKParameter(StandardPKParameter.AUC_tEnd_inf_norm).Name.ShouldBeEqualTo(Constants.Dimension.MASS_AUC);
         sut.DimensionForStandardPKParameter(StandardPKParameter.MRT).Name.ShouldBeEqualTo(Constants.Dimension.TIME);
         sut.DimensionForStandardPKParameter(StandardPKParameter.FractionAucEndToInf).Name.ShouldBeEqualTo(Constants.Dimension.FRACTION);
         sut.DimensionForStandardPKParameter(StandardPKParameter.Thalf).Name.ShouldBeEqualTo(Constants.Dimension.TIME);
         sut.DimensionForStandardPKParameter(StandardPKParameter.Vss).Name.ShouldBeEqualTo(Constants.Dimension.VOLUME_PER_BODY_WEIGHT);
         sut.DimensionForStandardPKParameter(StandardPKParameter.Vd).Name.ShouldBeEqualTo(Constants.Dimension.VOLUME_PER_BODY_WEIGHT);
         sut.DimensionForStandardPKParameter(StandardPKParameter.Tthreshold).Name.ShouldBeEqualTo(Constants.Dimension.TIME);
      }
   }
}