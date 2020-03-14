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
            DrugMassPerBodyWeight = 10,
         };
         _pkOptions.AddInterval(new DosingInterval {StartValue = 0, EndValue = 8, DrugMassPerBodyWeight = _firstDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 8, EndValue = 16, DrugMassPerBodyWeight = _oneMinusLastDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 16, EndValue = 48, DrugMassPerBodyWeight = _lastDose});
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_multipleDosingColumn, _pkOptions);
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[Constants.PKParameters.C_max].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[Constants.PKParameters.C_max_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max], _pkOptions.DrugMassPerBodyWeight), 1e-2);
         _pk[Constants.PKParameters.C_max_tD1_tD2].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[Constants.PKParameters.C_max_tD1_tD2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_tD1_tD2], _firstDose), 1e-2);
         _pk[Constants.PKParameters.C_max_tDLast_tDEnd].Value.ShouldBeEqualTo(16.72404671f, 1e-2);
         _pk[Constants.PKParameters.C_max_tDLast_tDEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_tDLast_tDEnd], _lastDose), 1e-2);
         _pk[Constants.PKParameters.Tmax].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.Tmax_tD1_tD2].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.Tmax_tDLast_tDEnd].Value.ShouldBeEqualTo(16.25f, 1e-2);
         _pk[Constants.PKParameters.AUC_tD1_tD2].Value.ShouldBeEqualTo(37.6964700029334f, 1e-2);
         _pk[Constants.PKParameters.AUC_tD1_tD2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_tD1_tD2], _firstDose), 1e-2);
         _pk[Constants.PKParameters.AUC_tDLast_minus_1_tDLast].Value.ShouldBeEqualTo(53.4314169287681f, 1e-2);
         _pk[Constants.PKParameters.AUC_tDLast_minus_1_tDLast_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_tDLast_minus_1_tDLast], _oneMinusLastDose), 1e-2);
         _pk[Constants.PKParameters.AUC_inf_tD1].Value.ShouldBeEqualTo(80.7110566815556f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_tD1_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_inf_tD1], _firstDose), 1e-2);
         _pk[Constants.PKParameters.Vss].Value.ShouldBeEqualTo(1.68843561172615f, 1e-2);
         _pk[Constants.PKParameters.CL].Value.ShouldBeEqualTo(0.123898761968324f, 1e-2);
         _pk[Constants.PKParameters.MRT].Value.ShouldBeEqualTo(13.6275422361186f, 0.05);
         _pk[Constants.PKParameters.Thalf].Value.ShouldBeEqualTo(10.2978867324386f, 0.05);
         _pk[Constants.PKParameters.Thalf_tDLast_tEnd].Value.ShouldBeEqualTo(10.8489508986824f, 1e-2);
         _pk[Constants.PKParameters.Ctrough_tD2].Value.ShouldBeEqualTo(2.89605998992919f, 1e-2);
         _pk[Constants.PKParameters.Ctrough_tDLast].Value.ShouldBeEqualTo(1.210245967f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_tDLast].Value.ShouldBeEqualTo(152.2143635f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_tLast_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_inf_tDLast], _lastDose), 1e-2);
      }

      [Observation]
      public void should_return_nan_for_values_that_should_not_be_calculated()
      {
         _pk.HasValueFor(Constants.PKParameters.AUC_inf).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_tEnd).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_tEnd_norm).ShouldBeFalse();
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
      private IReadOnlyList<UserDefinedPKParameter> _allDynamicPkParameters;
      private UserDefinedPKParameter _cmax_tD1_tD2;
      private UserDefinedPKParameter _tmax_tD1_tD2;
      private UserDefinedPKParameter _cmax_t1_t2;
      private UserDefinedPKParameter _cmax_t1_t2_offset;
      private UserDefinedPKParameter _cmax_t1_offset_no_end;
      private UserDefinedPKParameter _cmax_tD1_tD2_DOSE_BW;
      private UserDefinedPKParameter _tThreshold;
      private UserDefinedPKParameter _tThreshold_last;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _multipleDosingColumn = DataColumnLoader.GetDataColumnFrom("MultipleDosing_0_8_16");

         _pkOptions = new PKCalculationOptions
         {
            DrugMassPerBodyWeight = 10,
         };
         _pkOptions.AddInterval(new DosingInterval {StartValue = 0, EndValue = 8, DrugMassPerBodyWeight = _firstDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 8, EndValue = 16, DrugMassPerBodyWeight = _oneMinusLastDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 16, EndValue = 48, DrugMassPerBodyWeight = _lastDose});

         _cmax_tD1_tD2 = new UserDefinedPKParameter {StartApplicationIndex = 0, EndApplicationIndex = 1, StandardPKParameter = StandardPKParameter.Cmax, Name = "MyCmaxD1D2"};
         _cmax_tD1_tD2_DOSE_BW = new UserDefinedPKParameter {StartApplicationIndex = 0, EndApplicationIndex = 1, StandardPKParameter = StandardPKParameter.Cmax, Name = "MyCmaxD1D2_Normalized", NormalizationFactor = 10};
         _tmax_tD1_tD2 = new UserDefinedPKParameter {StartApplicationIndex = 0, EndApplicationIndex = 1, StandardPKParameter = StandardPKParameter.Tmax, Name = "MyTmaxD1D2"};

         _cmax_t1_t2 = new UserDefinedPKParameter {StartTime = 0, EndTime = 8, StandardPKParameter = StandardPKParameter.Cmax, Name = "MyCmaxT1T2"};
         _cmax_t1_t2_offset = new UserDefinedPKParameter {StartTime = 0, StartTimeOffset = 16, EndTime = 48, StandardPKParameter = StandardPKParameter.Cmax, Name = "MyCmaxT1T2offset"};
         _cmax_t1_offset_no_end = new UserDefinedPKParameter {StartTime = 0, StartTimeOffset = 16, StandardPKParameter = StandardPKParameter.Cmax, Name = "MyCmaxT1offset_no_end"};
         _tThreshold= new UserDefinedPKParameter { StartApplicationIndex = 0,  StandardPKParameter = StandardPKParameter.Tthreshold, Name = "Threshold", ConcentrationThreshold = 4};
         _tThreshold_last = new UserDefinedPKParameter { StartApplicationIndex = 2,  StandardPKParameter = StandardPKParameter.Tthreshold, Name = "Threshold_last", ConcentrationThreshold = 5};


         _allDynamicPkParameters = new[] {_cmax_tD1_tD2, _tmax_tD1_tD2, _cmax_t1_t2, _cmax_t1_t2_offset, _cmax_t1_offset_no_end, _cmax_tD1_tD2_DOSE_BW, _tThreshold, _tThreshold_last };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_multipleDosingColumn, _pkOptions, _allDynamicPkParameters);
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[Constants.PKParameters.C_max].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[Constants.PKParameters.C_max_tD1_tD2].Value.ShouldBeEqualTo(_pk[_cmax_tD1_tD2.Name].Value, 1e-2);
         _pk[_cmax_tD1_tD2_DOSE_BW.Name].Value.ShouldBeEqualTo(_pk[_cmax_tD1_tD2.Name].Value/10, 1e-2);
         _pk[Constants.PKParameters.C_max_tD1_tD2].Value.ShouldBeEqualTo(_pk[_cmax_t1_t2.Name].Value, 1e-2);
         _pk[Constants.PKParameters.Tmax_tD1_tD2].Value.ShouldBeEqualTo(_pk[_tmax_tD1_tD2.Name].Value, 1e-2);
         _pk[Constants.PKParameters.C_max_tDLast_tDEnd].Value.ShouldBeEqualTo(_pk[_cmax_t1_t2_offset.Name].Value, 1e-2);
         _pk[Constants.PKParameters.C_max_tDLast_tDEnd].Value.ShouldBeEqualTo(_pk[_cmax_t1_offset_no_end.Name].Value, 1e-2);
         _pk[_tThreshold.Name].Value.ShouldBeEqualTo(3.75f);
         _pk[_tThreshold_last.Name].Value.ShouldBeEqualTo(26);
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
            DrugMassPerBodyWeight = 10,
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
         _pk[Constants.PKParameters.C_max_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max], _pkOptions.DrugMassPerBodyWeight), 1e-2);
         _pk[Constants.PKParameters.Tmax].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.AUC_tEnd].Value.ShouldBeEqualTo(37.6964700029334f, 1e-2);
         _pk[Constants.PKParameters.AUC_tEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_tEnd], _pkOptions.DrugMassPerBodyWeight), 1e-2);
         _pk[Constants.PKParameters.AUC_inf].Value.ShouldBeEqualTo(80.7110566815556f, 1e-2);
         _pk[Constants.PKParameters.AUC_inf_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.AUC_inf], _pkOptions.DrugMassPerBodyWeight), 1e-2);
         _pk[Constants.PKParameters.Vss].Value.ShouldBeEqualTo(1.68843561172615f, 1e-2);
         _pk[Constants.PKParameters.CL].Value.ShouldBeEqualTo(0.123898761968324f, 1e-2);
         _pk[Constants.PKParameters.C_tEnd].Value.ShouldBeEqualTo(2.89605998992919f, 1e-2);
         _pk[Constants.PKParameters.MRT].Value.ShouldBeEqualTo(13.6275422361186f, 0.03);
         _pk[Constants.PKParameters.Thalf].Value.ShouldBeEqualTo(10.2978867324386f, 0.03);
      }

      [Observation]
      public void should_return_nan_for_values_that_should_not_be_calculated()
      {
         _pk.HasValueFor(Constants.PKParameters.C_max_tD1_tD2).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.C_max_tD1_tD2_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.Tmax_tD1_tD2).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.C_max_tDLast_tDEnd).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.C_max_tDLast_tDEnd_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.Tmax_tDLast_tDEnd).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.Ctrough_tD2).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.Ctrough_tDLast).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_tD1_tD2).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_tD1_tD2_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_tDLast_minus_1_tDLast).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_tDLast_minus_1_tDLast_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_tD1).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_tD1_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.Thalf_tDLast_tEnd).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_tDLast).ShouldBeFalse();
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
            DrugMassPerBodyWeight = 10,
         };

         _pkOptions.AddInterval(new DosingInterval {StartValue = 0, EndValue = 8.1f, DrugMassPerBodyWeight = _firstDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 8.1f, EndValue = 16f, DrugMassPerBodyWeight = _oneMinusLastDose});
         _pkOptions.AddInterval(new DosingInterval {StartValue = 16, EndValue = 48, DrugMassPerBodyWeight = _lastDose});
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_multipleDosingColumn, _pkOptions);
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[Constants.PKParameters.C_max].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[Constants.PKParameters.C_max_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max], _pkOptions.DrugMassPerBodyWeight), 1e-2);
         _pk[Constants.PKParameters.C_max_tD1_tD2].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[Constants.PKParameters.C_max_tD1_tD2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_tD1_tD2], _firstDose), 1e-2);
         _pk[Constants.PKParameters.C_max_tDLast_tDEnd].Value.ShouldBeEqualTo(16.72404671f, 1e-2);
         _pk[Constants.PKParameters.C_max_tDLast_tDEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[Constants.PKParameters.C_max_tDLast_tDEnd], _lastDose), 1e-2);
         _pk[Constants.PKParameters.Tmax].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.Tmax_tD1_tD2].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Constants.PKParameters.Tmax_tDLast_tDEnd].Value.ShouldBeEqualTo(16.25f, 1e-2);
      }

      [Observation]
      public void should_return_nan_for_values_that_should_not_be_calculated()
      {
         _pk.HasValueFor(Constants.PKParameters.AUC_inf).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_inf_norm).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_tEnd).ShouldBeFalse();
         _pk.HasValueFor(Constants.PKParameters.AUC_tEnd_norm).ShouldBeFalse();
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
            DrugMassPerBodyWeight = 10,
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