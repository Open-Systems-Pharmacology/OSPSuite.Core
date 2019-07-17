using System;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_PKValuesCalculator : ContextSpecification<IPKValuesCalculator>
   {
      protected override void Context()
      {
         sut = new PKValuesCalculator();
      }

      protected float NormalizeValue(double? value, double? norm)
      {
         return Convert.ToSingle(value.Value / norm.Value);
      }

   }

   public class When_calculating_the_pk_parameters_for_a_multipe_dosing_application : concern_for_PKValuesCalculator
   {
      private DataColumn _multipleDosingColumn;
      private PKCalculationOptions _pkOptions;
      private PKValues _pk;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _multipleDosingColumn = DataColumnLoader.GetDataColumnFrom("MultipleDosing_0_8_16");

         _pkOptions = new PKCalculationOptions
         {
            FirstDosingStartValue = 0,
            FirstDosingEndValue = 8,
            LastMinusOneDosingStartValue = 8,
            LastDosingStartValue = 16,
            LastDosingEndValue = 48,
            Dose = 10,
            FirstDose = 4,
            LastMinusOneDose = 4,
            LastDose = 2
         };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_multipleDosingColumn, _pkOptions);
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[Constants.PKParameters.C_max].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[Constants.PKParameters.C_max_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max], _pkOptions.Dose), 1e-2);
         _pk[Constants.PKParameters.C_max_t1_t2].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[Constants.PKParameters.C_max_t1_t2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_t1_t2], _pkOptions.FirstDose), 1e-2);
         _pk[Constants.PKParameters.C_max_tLast_tEnd].Value.ShouldBeEqualTo(16.72404671f, 1e-2);
         _pk[Constants.PKParameters.C_max_tLast_tEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_tLast_tEnd], _pkOptions.LastDose), 1e-2);
         _pk[Constants.PKParameters.Tmax].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.Tmax_t1_t2].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.Tmax_tLast_tEnd].Value.ShouldBeEqualTo(16.25f, 1e-2);
         _pk[Constants.PKParameters.AUC_t1_t2].Value.ShouldBeEqualTo(37.6964700029334f, 1e-2);
         _pk[Constants.PKParameters.AUC_t1_t2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_t1_t2], _pkOptions.FirstDose), 1e-2);
         _pk[Constants.PKParameters.AUC_tLast_minus_1_tLast].Value.ShouldBeEqualTo(53.4314169287681f, 1e-2);
         _pk[Constants.PKParameters.AUC_tLast_minus_1_tLast_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_tLast_minus_1_tLast], _pkOptions.LastMinusOneDose), 1e-2);
         _pk[Constants.PKParameters.AUC_inf_t1].Value.ShouldBeEqualTo(80.7110566815556f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_t1_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_inf_t1], _pkOptions.FirstDose), 1e-2);
         _pk[Constants.PKParameters.Vss].Value.ShouldBeEqualTo(1.68843561172615f, 1e-2);
         _pk[Constants.PKParameters.CL].Value.ShouldBeEqualTo(0.123898761968324f, 1e-2);
         _pk[Constants.PKParameters.MRT].Value.ShouldBeEqualTo(13.6275422361186f, 0.05);
         _pk[Constants.PKParameters.Thalf].Value.ShouldBeEqualTo(10.2978867324386f, 0.05);
         _pk[Constants.PKParameters.Thalf_tLast_tEnd].Value.ShouldBeEqualTo(10.8489508986824f, 1e-2);
         _pk[Constants.PKParameters.Ctrough_t2].Value.ShouldBeEqualTo(2.89605998992919f, 1e-2);
         _pk[Constants.PKParameters.Ctrough_tLast].Value.ShouldBeEqualTo(1.210245967f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_tLast].Value.ShouldBeEqualTo(152.2143635f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_tLast_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_inf_tLast], _pkOptions.LastDose), 1e-2);
      }

   
      [Observation]
      public void should_return_nan_for_values_that_should_not_be_calculated()
      {
         _pk.HasValueFor(Constants.PKParameters.AUC_inf).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.C_tEnd).ShouldBeFalse();
      }
   }

   public class When_calculating_the_pk_parameters_for_a_single_dosing_application : concern_for_PKValuesCalculator
   {
      private DataColumn _singleDosing;
      private PKCalculationOptions _pkOptions;
      private PKValues _pk;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _singleDosing = DataColumnLoader.GetDataColumnFrom("SingleDosing_CL");

         _pkOptions = new PKCalculationOptions
         {
            Dose = 10,
         };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_singleDosing, _pkOptions);
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[Constants.PKParameters.C_max].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[Constants.PKParameters.C_max_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max], _pkOptions.Dose), 1e-2);
         _pk[Constants.PKParameters.Tmax].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.AUC].Value.ShouldBeEqualTo(37.6964700029334f, 1e-2);
         _pk[Constants.PKParameters.AUC_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC], _pkOptions.Dose), 1e-2);
         _pk[Constants.PKParameters.AUC_inf].Value.ShouldBeEqualTo(80.7110566815556f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_inf], _pkOptions.Dose), 1e-2);
         _pk[Constants.PKParameters.Vss].Value.ShouldBeEqualTo(1.68843561172615f, 1e-2);
         _pk[Constants.PKParameters.CL].Value.ShouldBeEqualTo(0.123898761968324f, 1e-2);
         _pk[Constants.PKParameters.C_tEnd].Value.ShouldBeEqualTo(2.89605998992919f, 1e-2);
         _pk[Constants.PKParameters.MRT].Value.ShouldBeEqualTo(13.6275422361186f, 0.03);
         _pk[Constants.PKParameters.Thalf].Value.ShouldBeEqualTo(10.2978867324386f, 0.03);
      }

      [Observation]
      public void should_return_nan_for_values_that_should_not_be_calculated()
      {
         _pk.HasValueFor(Constants.PKParameters.C_max_t1_t2).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.C_max_t1_t2_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.Tmax_t1_t2).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.C_max_tLast_tEnd).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.C_max_tLast_tEnd_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.Tmax_tLast_tEnd).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.Ctrough_t2).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.Ctrough_tLast).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_t1_t2).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_t1_t2_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_tLast_minus_1_tLast).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_tLast_minus_1_tLast_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_t1).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_t1_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.Thalf_tLast_tEnd).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_tLast).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_tLast_norm).ShouldBeFalse();
      }
   }

   public class When_calculating_the_pk_parameters_for_a_multipe_dosing_application_with_approximated_time : concern_for_PKValuesCalculator
   {
      private DataColumn _multipleDosingColumn;
      private PKCalculationOptions _pkOptions;
      private PKValues _pk;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _multipleDosingColumn = DataColumnLoader.GetDataColumnFrom("MultipleDosing_0_8_16");

         _pkOptions = new PKCalculationOptions
         {
            FirstDosingStartValue = 0,
            FirstDosingEndValue = 8.1f,
            LastMinusOneDosingStartValue = 8.1f,
            LastDosingStartValue = 16,
            LastDosingEndValue = 48,
            Dose = 10,
            FirstDose = 5,
            LastMinusOneDose = 2,
            LastDose = 3
         };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_multipleDosingColumn, _pkOptions);
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[Constants.PKParameters.C_max].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[Constants.PKParameters.C_max_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max],_pkOptions.Dose), 1e-2);
         _pk[Constants.PKParameters.C_max_t1_t2].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[Constants.PKParameters.C_max_t1_t2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_t1_t2], _pkOptions.FirstDose), 1e-2);
         _pk[Constants.PKParameters.C_max_tLast_tEnd].Value.ShouldBeEqualTo(16.72404671f, 1e-2);
         _pk[Constants.PKParameters.C_max_tLast_tEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_tLast_tEnd], _pkOptions.LastDose), 1e-2);
         _pk[Constants.PKParameters.Tmax].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.Tmax_t1_t2].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.Tmax_tLast_tEnd].Value.ShouldBeEqualTo(16.25f, 1e-2);
      }

      [Observation]
      public void should_return_nan_for_values_that_should_not_be_calculated()
      {
         _pk.HasValueFor(Constants.PKParameters.AUC_inf).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.C_tEnd).ShouldBeFalse();
      }
   }

   public class When_calculating_pk_values_for_a_column_with_no_values : concern_for_PKValuesCalculator
   {
      private PKValues _pk;
      private DataColumn _emptyColums;
      private PKCalculationOptions _pkOptions;
      private BaseGrid _baseGrid;

      protected override void Context()
      {
         base.Context();
         _baseGrid = new BaseGrid("BaseGrid", Constants.Dimension.NO_DIMENSION) {Values = new float[0]};
         _emptyColums = new DataColumn("TEST", Constants.Dimension.NO_DIMENSION, _baseGrid) {Values = new float[0]};
         _pkOptions = new PKCalculationOptions
         {
            Dose = 10,
         };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_emptyColums, _pkOptions);
      }

      [Observation]
      public void should_return_an_empty_pk_calculation()
      {
         _pk.HasValueFor(Constants.PKParameters.C_max).ShouldBeFalse();
      }
   }
}