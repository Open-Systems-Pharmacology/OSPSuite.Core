using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
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

   public class When_converting_a_double_array_from_molar_unit_to_mass_unit : concern_for_DimensionTask
   {
      protected override void Because()
      {
         //20 µmol/kg
         _result = sut.ConvertToUnit(Constants.Dimension.AMOUNT, "kg", new[] { 1d, 2d, 3d }, molWeight: 20);
      }

      [Observation]
      public void should_be_able_to_convert_the_array()
      {
         _result.ShouldOnlyContainInOrder(20, 40, 60d);
      }
   }


   public class When_converting_a_double_value_from_molar_unit_to_mass_unit : concern_for_DimensionTask
   {
      protected override void Because()
      {
         //50 µmol/kg
         _result = sut.ConvertToUnit(Constants.Dimension.AMOUNT, "kg", 10, molWeight: 50);
      }

      [Observation]
      public void should_be_able_to_convert_the_array()
      {
         _result.ShouldOnlyContainInOrder(500);
      }
   }

   public class When_converting_a_double_array_from_mass_unit_to_molar_unit_and_the_mol_weight_is_not_present : concern_for_DimensionTask
   {
      [Observation]
      public void should_throw_an_exception()
      {
        The.Action(() =>sut.ConvertToUnit(Constants.Dimension.MASS_AMOUNT, "µmol", new[] { 1d, 2d, 3d })).ShouldThrowAn<UnableToResolveParametersException>();
      }
   }

   public class When_converting_a_double_array_from_mass_unit_to_molar_unit : concern_for_DimensionTask
   {
      protected override void Because()
      {
         //20 µmol/kg
         _result = sut.ConvertToUnit(Constants.Dimension.MASS_AMOUNT, "µmol", new[] { 1d }, molWeight: 20);
      }

      [Observation]
      public void should_be_able_to_convert_the_array()
      {
         _result.ShouldOnlyContainInOrder(1/20d);
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
}