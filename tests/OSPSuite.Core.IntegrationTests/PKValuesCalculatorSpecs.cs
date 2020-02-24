using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
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

   public class When_calculating_the_pk_parameters_for_a_multiple_dosing_application : concern_for_PKValuesCalculator
   {
      private DataColumn _multipleDosingColumn;
      private PKCalculationOptions _pkOptions;
      private PKValues _pk;
      private readonly double _firstDose = 4;
      private readonly double _oneMinusLastDose = 4;
      private readonly double _lastDose = 2;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _multipleDosingColumn = DataColumnLoader.GetDataColumnFrom("MultipleDosing_0_8_16");

         _pkOptions = new PKCalculationOptions
         {
            Dose = 10,
         };
         _pkOptions.AddInterval(new DosingInterval {StartValue = 0, EndValue = 8, Dose = _firstDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 8, EndValue = 16, Dose = _oneMinusLastDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 16, EndValue = 48, Dose = _lastDose});
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
         _pk[Constants.PKParameters.C_max_t1_t2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_t1_t2], _firstDose), 1e-2);
         _pk[Constants.PKParameters.C_max_tLast_tEnd].Value.ShouldBeEqualTo(16.72404671f, 1e-2);
         _pk[Constants.PKParameters.C_max_tLast_tEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_tLast_tEnd], _lastDose), 1e-2);
         _pk[Constants.PKParameters.Tmax].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.Tmax_t1_t2].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.Tmax_tLast_tEnd].Value.ShouldBeEqualTo(16.25f, 1e-2);
         _pk[Constants.PKParameters.AUC_t1_t2].Value.ShouldBeEqualTo(37.6964700029334f, 1e-2);
         _pk[Constants.PKParameters.AUC_t1_t2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_t1_t2], _firstDose), 1e-2);
         _pk[Constants.PKParameters.AUC_tLast_minus_1_tLast].Value.ShouldBeEqualTo(53.4314169287681f, 1e-2);
         _pk[Constants.PKParameters.AUC_tLast_minus_1_tLast_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_tLast_minus_1_tLast], _oneMinusLastDose), 1e-2);
         _pk[Constants.PKParameters.AUC_inf_t1].Value.ShouldBeEqualTo(80.7110566815556f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_t1_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_inf_t1], _firstDose), 1e-2);
         _pk[Constants.PKParameters.Vss].Value.ShouldBeEqualTo(1.68843561172615f, 1e-2);
         _pk[Constants.PKParameters.CL].Value.ShouldBeEqualTo(0.123898761968324f, 1e-2);
         _pk[Constants.PKParameters.MRT].Value.ShouldBeEqualTo(13.6275422361186f, 0.05);
         _pk[Constants.PKParameters.Thalf].Value.ShouldBeEqualTo(10.2978867324386f, 0.05);
         _pk[Constants.PKParameters.Thalf_tLast_tEnd].Value.ShouldBeEqualTo(10.8489508986824f, 1e-2);
         _pk[Constants.PKParameters.Ctrough_t2].Value.ShouldBeEqualTo(2.89605998992919f, 1e-2);
         _pk[Constants.PKParameters.Ctrough_tLast].Value.ShouldBeEqualTo(1.210245967f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_tLast].Value.ShouldBeEqualTo(152.2143635f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_tLast_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_inf_tLast], _lastDose), 1e-2);
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

   public class When_calculating_the_pk_parameters_for_a_multiple_dosing_application_using_also_dynamic_parameters : concern_for_PKValuesCalculator
   {
      private DataColumn _multipleDosingColumn;
      private PKCalculationOptions _pkOptions;
      private PKValues _pk;
      private readonly double _firstDose = 4;
      private readonly double _oneMinusLastDose = 4;
      private readonly double _lastDose = 2;
      private IReadOnlyList<DynamicPKParameter> _allDynamicPkParameters;
      private DynamicPKParameter _cmax_tD1_tD2;
      private DynamicPKParameter _tmax_tD1_tD2;
      private DynamicPKParameter _cmax_t1_t2;
      private DynamicPKParameter _cmax_t1_t2_offset;
      private DynamicPKParameter _cmax_t1_offset_no_end;
      private DynamicPKParameter _cmax_tD1_tD2_DOSE_BW;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _multipleDosingColumn = DataColumnLoader.GetDataColumnFrom("MultipleDosing_0_8_16");

         _pkOptions = new PKCalculationOptions
         {
            Dose = 10,
         };
         _pkOptions.AddInterval(new DosingInterval {StartValue = 0, EndValue = 8, Dose = _firstDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 8, EndValue = 16, Dose = _oneMinusLastDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 16, EndValue = 48, Dose = _lastDose});

         _cmax_tD1_tD2 = new DynamicPKParameter {StartApplication = 1, EndApplication = 2, StandardPKParameter = StandardPKParameter.Cmax, Name = "MyCmaxD1D2"};
         _cmax_tD1_tD2_DOSE_BW = new DynamicPKParameter {StartApplication = 1, EndApplication = 2, StandardPKParameter = StandardPKParameter.Cmax, Name = "MyCmaxD1D2_Normalized", DoseForNormalization = 10};
         _tmax_tD1_tD2 = new DynamicPKParameter {StartApplication = 1, EndApplication = 2, StandardPKParameter = StandardPKParameter.Tmax, Name = "MyTmaxD1D2"};

         _cmax_t1_t2 = new DynamicPKParameter {StartTime = 0, EndTime = 8, StandardPKParameter = StandardPKParameter.Cmax, Name = "MyCmaxT1T2"};
         _cmax_t1_t2_offset = new DynamicPKParameter {StartTime = 0, StartTimeOffset = 16, EndTime = 48, StandardPKParameter = StandardPKParameter.Cmax, Name = "MyCmaxT1T2offset"};
         _cmax_t1_offset_no_end = new DynamicPKParameter {StartTime = 0, StartTimeOffset = 16, StandardPKParameter = StandardPKParameter.Cmax, Name = "MyCmaxT1offset_no_end"};

         _allDynamicPkParameters = new[] {_cmax_tD1_tD2, _tmax_tD1_tD2, _cmax_t1_t2, _cmax_t1_t2_offset, _cmax_t1_offset_no_end, _cmax_tD1_tD2_DOSE_BW };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_multipleDosingColumn, _pkOptions, _allDynamicPkParameters);
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[Constants.PKParameters.C_max_t1_t2].Value.ShouldBeEqualTo(_pk[_cmax_tD1_tD2.Name].Value, 1e-2);
         _pk[_cmax_tD1_tD2_DOSE_BW.Name].Value.ShouldBeEqualTo(_pk[_cmax_tD1_tD2.Name].Value/10, 1e-2);
         _pk[Constants.PKParameters.C_max_t1_t2].Value.ShouldBeEqualTo(_pk[_cmax_t1_t2.Name].Value, 1e-2);
         _pk[Constants.PKParameters.Tmax_t1_t2].Value.ShouldBeEqualTo(_pk[_tmax_tD1_tD2.Name].Value, 1e-2);
         _pk[Constants.PKParameters.C_max_tLast_tEnd].Value.ShouldBeEqualTo(_pk[_cmax_t1_t2_offset.Name].Value, 1e-2);
         _pk[Constants.PKParameters.C_max_tLast_tEnd].Value.ShouldBeEqualTo(_pk[_cmax_t1_offset_no_end.Name].Value, 1e-2);
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

   public class When_calculating_the_pk_parameters_for_a_multiple_dosing_application_with_approximated_time : concern_for_PKValuesCalculator
   {
      private DataColumn _multipleDosingColumn;
      private PKCalculationOptions _pkOptions;
      private PKValues _pk;
      private readonly double _firstDose = 5;
      private readonly double _oneMinusLastDose = 2;
      private readonly double _lastDose = 3;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _multipleDosingColumn = DataColumnLoader.GetDataColumnFrom("MultipleDosing_0_8_16");

         _pkOptions = new PKCalculationOptions
         {
            Dose = 10,
         };

         _pkOptions.AddInterval(new DosingInterval {StartValue = 0, EndValue = 8.1f, Dose = _firstDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 8.1f, EndValue = 16f, Dose = _oneMinusLastDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 16, EndValue = 48, Dose = _lastDose});
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
         _pk[Constants.PKParameters.C_max_t1_t2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_t1_t2], _firstDose), 1e-2);
         _pk[Constants.PKParameters.C_max_tLast_tEnd].Value.ShouldBeEqualTo(16.72404671f, 1e-2);
         _pk[Constants.PKParameters.C_max_tLast_tEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_tLast_tEnd], _lastDose), 1e-2);
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
      private DataColumn _emptyColumns;
      private PKCalculationOptions _pkOptions;
      private BaseGrid _baseGrid;

      protected override void Context()
      {
         base.Context();
         _baseGrid = new BaseGrid("BaseGrid", Constants.Dimension.NO_DIMENSION) {Values = new float[0]};
         _emptyColumns = new DataColumn("TEST", Constants.Dimension.NO_DIMENSION, _baseGrid) {Values = new float[0]};
         _pkOptions = new PKCalculationOptions
         {
            Dose = 10,
         };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_emptyColumns, _pkOptions);
      }

      [Observation]
      public void should_return_an_empty_pk_calculation()
      {
         _pk.HasValueFor(Constants.PKParameters.C_max).ShouldBeFalse();
      }
   }
}