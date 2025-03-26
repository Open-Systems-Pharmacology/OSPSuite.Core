using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Helpers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants.Dimension;
using static OSPSuite.Core.Domain.Constants.PKParameters;

namespace OSPSuite.Core
{
   public abstract class concern_for_PKValuesCalculator : ContextForIntegration<IPKValuesCalculator>
   {
      protected IPKParameterRepository _pkParameterRepository;

      protected override void Context()
      {
         _pkParameterRepository = IoC.Resolve<IPKParameterRepository>();
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
            TotalDrugMassPerBodyWeight = 10,
         };
         _pkOptions.AddInterval(new DosingInterval { StartValue = 0, EndValue = 8, DrugMassPerBodyWeight = _firstDose });
         _pkOptions.AddInterval(new DosingInterval { StartValue = 8, EndValue = 16, DrugMassPerBodyWeight = _oneMinusLastDose });
         _pkOptions.AddInterval(new DosingInterval { StartValue = 16, EndValue = 48, DrugMassPerBodyWeight = _lastDose });
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_multipleDosingColumn, _pkOptions);
      }

      [Observation]
      public void should_return_parameters_that_are_available_in_the_pk_parameter_repository()
      {
         var errorList = new List<string>();

         _pk.Values.Keys.Each(x =>
         {
            if (_pkParameterRepository.FindByName(x) == null)
               errorList.Add(x);
         });

         errorList.Count.ShouldBeEqualTo(0, errorList.ToString(", "));
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[C_max].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[C_max_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[C_max], _pkOptions.TotalDrugMassPerBodyWeight), 1e-2);
         _pk[C_max_tD1_tD2].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[C_max_tD1_tD2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[C_max_tD1_tD2], _firstDose), 1e-2);
         _pk[C_max_tDLast_tDEnd].Value.ShouldBeEqualTo(16.72404671f, 1e-2);
         _pk[C_max_tDLast_tDEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[C_max_tDLast_tDEnd], _lastDose), 1e-2);
         _pk[Tmax].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Tmax_tD1_tD2].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Tmax_tDLast_tDEnd].Value.ShouldBeEqualTo(16.25f, 1e-2);
         _pk[AUC_tD1_tD2].Value.ShouldBeEqualTo(37.6964700029334f, 1e-2);
         _pk[AUC_tD1_tD2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[AUC_tD1_tD2], _firstDose), 1e-2);
         _pk[AUC_tDLast_minus_1_tDLast].Value.ShouldBeEqualTo(53.4314169287681f, 1e-2);
         _pk[AUC_tDLast_minus_1_tDLast_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[AUC_tDLast_minus_1_tDLast], _oneMinusLastDose), 1e-2);
         _pk[AUC_inf_tD1].Value.ShouldBeEqualTo(80.7110566815556f, 1e-2);
         _pk[AUC_inf_tD1_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[AUC_inf_tD1], _firstDose), 1e-2);
         _pk[MRT].Value.ShouldBeEqualTo(13.6275422361186f, 0.05);
         _pk[Thalf].Value.ShouldBeEqualTo(10.2978867324386f, 0.05);
         _pk[Thalf_tDLast_tEnd].Value.ShouldBeEqualTo(10.8489508986824f, 1e-2);
         _pk[Ctrough_tD2].Value.ShouldBeEqualTo(2.89605998992919f, 1e-2);
         _pk[Ctrough_tDLast].Value.ShouldBeEqualTo(1.210245967f, 1e-2);
         _pk[AUC_inf_tDLast].Value.ShouldBeEqualTo(152.2143635f, 1e-2);
         _pk[AUC_inf_tLast_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[AUC_inf_tDLast], _lastDose), 1e-2);
         _pk[AUC_tEnd].Value.ShouldBeEqualTo(224.402f, 1e-2);
         _pk[AUC_tEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[AUC_tEnd], _pkOptions.TotalDrugMassPerBodyWeight), 1e-2);
         _pk[C_tEnd].Value.ShouldBeEqualTo(1.210246f, 1e-2);
      }

      [Observation]
      public void should_return_nan_for_values_that_should_not_be_calculated()
      {
         _pk.HasValueFor(AUC_inf).ShouldBeFalse();
         _pk.HasValueFor(AUC_inf_norm).ShouldBeFalse();
         _pk.HasValueFor(Vss).ShouldBeFalse();
         _pk.HasValueFor(CL).ShouldBeFalse();
      }
   }

   public class When_calculating_the_pk_parameters_for_a_multiple_dosing_application_using_also_dynamic_parameters : concern_for_PKValuesCalculator
   {
      private DataColumn _multipleDosingColumn;
      private PKCalculationOptions _pkOptions;
      private PKValues _pk;
      protected readonly double _firstDose = 4;
      protected readonly double _oneMinusLastDose = 4;
      protected readonly double _lastDose = 2;
      private IReadOnlyList<UserDefinedPKParameter> _allDynamicPkParameters;
      private UserDefinedPKParameter _cmax_tD1_tD2;
      private UserDefinedPKParameter _tmax_tD1_tD2;
      private UserDefinedPKParameter _cmax_t1_t2;
      private UserDefinedPKParameter _cmax_t1_t2_offset;
      private UserDefinedPKParameter _cmax_t1_offset_no_end;
      private UserDefinedPKParameter _cmax_tD1_tD2_DOSE_BW;
      private UserDefinedPKParameter _tThreshold;
      private UserDefinedPKParameter _tThreshold_last;
      private UserDefinedPKParameter _cmax_tD1_tD2_DOSE_BW_auto;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _multipleDosingColumn = DataColumnLoader.GetDataColumnFrom("MultipleDosing_0_8_16");

         _pkOptions = new PKCalculationOptions
         {
            TotalDrugMassPerBodyWeight = 10,
         };
         _pkOptions.AddInterval(new DosingInterval { StartValue = 0, EndValue = 8, DrugMassPerBodyWeight = _firstDose });
         _pkOptions.AddInterval(new DosingInterval { StartValue = 8, EndValue = 16, DrugMassPerBodyWeight = _oneMinusLastDose });
         _pkOptions.AddInterval(new DosingInterval { StartValue = 16, EndValue = 48, DrugMassPerBodyWeight = _lastDose });

         _cmax_tD1_tD2 = new UserDefinedPKParameter { StartApplicationIndex = 0, EndApplicationIndex = 1, StandardPKParameter = StandardPKParameter.C_max, Name = "MyCmaxD1D2" };
         _cmax_tD1_tD2_DOSE_BW = new UserDefinedPKParameter { StartApplicationIndex = 0, EndApplicationIndex = 1, StandardPKParameter = StandardPKParameter.C_max, Name = "MyCmaxD1D2_Normalized", NormalizationFactor = _firstDose };
         _cmax_tD1_tD2_DOSE_BW_auto = new UserDefinedPKParameter { StartApplicationIndex = 0, EndApplicationIndex = 1, StandardPKParameter = StandardPKParameter.C_max_norm, Name = "MyCmaxD1D2_Normalized_auto" };

         _tmax_tD1_tD2 = new UserDefinedPKParameter { StartApplicationIndex = 0, EndApplicationIndex = 1, StandardPKParameter = StandardPKParameter.t_max, Name = "MyTmaxD1D2" };

         _cmax_t1_t2 = new UserDefinedPKParameter { StartTime = 0, EndTime = 8, StandardPKParameter = StandardPKParameter.C_max, Name = "MyCmaxT1T2" };
         _cmax_t1_t2_offset = new UserDefinedPKParameter { StartTime = 0, StartTimeOffset = 16, EndTime = 48, StandardPKParameter = StandardPKParameter.C_max, Name = "MyCmaxT1T2offset" };
         _cmax_t1_offset_no_end = new UserDefinedPKParameter { StartTime = 0, StartTimeOffset = 16, StandardPKParameter = StandardPKParameter.C_max, Name = "MyCmaxT1offset_no_end" };
         _tThreshold = new UserDefinedPKParameter { StartApplicationIndex = 0, StandardPKParameter = StandardPKParameter.Tthreshold, Name = "Threshold", ConcentrationThreshold = 4 };
         _tThreshold_last = new UserDefinedPKParameter { StartApplicationIndex = 2, StandardPKParameter = StandardPKParameter.Tthreshold, Name = "Threshold_last", ConcentrationThreshold = 5 };


         _allDynamicPkParameters = new[] { _cmax_tD1_tD2, _tmax_tD1_tD2, _cmax_t1_t2, _cmax_t1_t2_offset, _cmax_t1_offset_no_end, _cmax_tD1_tD2_DOSE_BW, _tThreshold, _tThreshold_last, _cmax_tD1_tD2_DOSE_BW_auto };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_multipleDosingColumn, _pkOptions, _allDynamicPkParameters);
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[C_max].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[C_max_tD1_tD2].Value.ShouldBeEqualTo(_pk[_cmax_tD1_tD2.Name].Value, 1e-2);
         _pk[_cmax_tD1_tD2_DOSE_BW.Name].Value.ShouldBeEqualTo((float)(_pk[_cmax_tD1_tD2.Name].Value / _firstDose), 1e-2);
         _pk[_cmax_tD1_tD2_DOSE_BW_auto.Name].Value.ShouldBeEqualTo(_pk[_cmax_tD1_tD2_DOSE_BW.Name].Value, 1e-2);
         _pk[C_max_tD1_tD2].Value.ShouldBeEqualTo(_pk[_cmax_t1_t2.Name].Value, 1e-2);
         _pk[Tmax_tD1_tD2].Value.ShouldBeEqualTo(_pk[_tmax_tD1_tD2.Name].Value, 1e-2);
         _pk[C_max_tDLast_tDEnd].Value.ShouldBeEqualTo(_pk[_cmax_t1_t2_offset.Name].Value, 1e-2);
         _pk[C_max_tDLast_tDEnd].Value.ShouldBeEqualTo(_pk[_cmax_t1_offset_no_end.Name].Value, 1e-2);
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
            TotalDrugMassPerBodyWeight = 10,
         };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_singleDosing, _pkOptions);
      }

      [Observation]
      public void should_return_parameters_that_are_available_in_the_pk_parameter_repository()
      {
         var errorList = new List<string>();

         _pk.Values.Keys.Each(x =>
         {
            if (_pkParameterRepository.FindByName(x) == null)
               errorList.Add(x);
         });

         errorList.Count.ShouldBeEqualTo(0, errorList.ToString(", "));
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[C_max].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[C_max_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[C_max], _pkOptions.TotalDrugMassPerBodyWeight), 1e-2);
         _pk[Tmax].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[AUC_tEnd].Value.ShouldBeEqualTo(37.6964700029334f, 1e-2);
         _pk[AUC_tEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[AUC_tEnd], _pkOptions.TotalDrugMassPerBodyWeight), 1e-2);
         _pk[AUC_inf].Value.ShouldBeEqualTo(80.7110566815556f, 1e-2);
         _pk[AUC_inf_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[AUC_inf], _pkOptions.TotalDrugMassPerBodyWeight), 1e-2);
         _pk[Vss].Value.ShouldBeEqualTo(1.68843561172615f, 1e-2);
         _pk[CL].Value.ShouldBeEqualTo(0.123898761968324f, 1e-2);
         _pk[C_tEnd].Value.ShouldBeEqualTo(2.89605998992919f, 1e-2);
         _pk[MRT].Value.ShouldBeEqualTo(13.6275422361186f, 0.03);
         _pk[Thalf].Value.ShouldBeEqualTo(10.2978867324386f, 0.03);
      }

      [Observation]
      public void should_return_nan_for_values_that_should_not_be_calculated()
      {
         _pk.HasValueFor(C_max_tD1_tD2).ShouldBeFalse();
         _pk.HasValueFor(C_max_tD1_tD2_norm).ShouldBeFalse();
         _pk.HasValueFor(Tmax_tD1_tD2).ShouldBeFalse();
         _pk.HasValueFor(C_max_tDLast_tDEnd).ShouldBeFalse();
         _pk.HasValueFor(C_max_tDLast_tDEnd_norm).ShouldBeFalse();
         _pk.HasValueFor(Tmax_tDLast_tDEnd).ShouldBeFalse();
         _pk.HasValueFor(Ctrough_tD2).ShouldBeFalse();
         _pk.HasValueFor(Ctrough_tDLast).ShouldBeFalse();
         _pk.HasValueFor(AUC_tD1_tD2).ShouldBeFalse();
         _pk.HasValueFor(AUC_tD1_tD2_norm).ShouldBeFalse();
         _pk.HasValueFor(AUC_tDLast_minus_1_tDLast).ShouldBeFalse();
         _pk.HasValueFor(AUC_tDLast_minus_1_tDLast_norm).ShouldBeFalse();
         _pk.HasValueFor(AUC_inf_tD1).ShouldBeFalse();
         _pk.HasValueFor(AUC_inf_tD1_norm).ShouldBeFalse();
         _pk.HasValueFor(Thalf_tDLast_tEnd).ShouldBeFalse();
         _pk.HasValueFor(AUC_inf_tDLast).ShouldBeFalse();
         _pk.HasValueFor(AUC_inf_tLast_norm).ShouldBeFalse();
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
            TotalDrugMassPerBodyWeight = 10,
         };

         _pkOptions.AddInterval(new DosingInterval { StartValue = 0, EndValue = 8.1f, DrugMassPerBodyWeight = _firstDose });
         _pkOptions.AddInterval(new DosingInterval { StartValue = 8.1f, EndValue = 16f, DrugMassPerBodyWeight = _oneMinusLastDose });
         _pkOptions.AddInterval(new DosingInterval { StartValue = 16, EndValue = 48, DrugMassPerBodyWeight = _lastDose });
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_multipleDosingColumn, _pkOptions);
      }

      [Observation]
      public void should_return_the_expected_parameter_values()
      {
         _pk[C_max].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[C_max_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[C_max], _pkOptions.TotalDrugMassPerBodyWeight), 1e-2);
         _pk[C_max_tD1_tD2].Value.ShouldBeEqualTo(23.07205582f, 1e-2);
         _pk[C_max_tD1_tD2_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[C_max_tD1_tD2], _firstDose), 1e-2);
         _pk[C_max_tDLast_tDEnd].Value.ShouldBeEqualTo(16.72404671f, 1e-2);
         _pk[C_max_tDLast_tDEnd_norm].Value.ShouldBeEqualTo(NormalizeValue(_pk[C_max_tDLast_tDEnd], _lastDose), 1e-2);
         _pk[Tmax].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Tmax_tD1_tD2].Value.ShouldBeEqualTo(0.05000000f, 1e-2);
         _pk[Tmax_tDLast_tDEnd].Value.ShouldBeEqualTo(16.25f, 1e-2);
      }

      [Observation]
      public void should_return_nan_for_values_that_should_not_be_calculated()
      {
         _pk.HasValueFor(AUC_inf).ShouldBeFalse();
         _pk.HasValueFor(AUC_inf_norm).ShouldBeFalse();
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
         _baseGrid = new BaseGrid("BaseGrid", NO_DIMENSION) { Values = Array.Empty<float>() };
         _emptyColumns = new DataColumn("TEST", NO_DIMENSION, _baseGrid) { Values = Array.Empty<float>() };
         _pkOptions = new PKCalculationOptions
         {
            TotalDrugMassPerBodyWeight = 10,
         };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_emptyColumns, _pkOptions);
      }

      [Observation]
      public void should_return_an_empty_pk_calculation()
      {
         _pk.HasValueFor(C_max).ShouldBeFalse();
      }
   }

   public class When_calculating_pk_values_for_a_column_with_few_values : concern_for_PKValuesCalculator
   {
      private PKValues _pk;
      private DataColumn _emptyColumns;
      private PKCalculationOptions _pkOptions;
      private BaseGrid _baseGrid;

      protected override void Context()
      {
         base.Context();
         _baseGrid = new BaseGrid("BaseGrid", NO_DIMENSION) { Values = new[] { 0f, 1f, 2f } };
         _emptyColumns = new DataColumn("TEST", NO_DIMENSION, _baseGrid) { Values = new[] { 3f, 2f, 0.1f } };
         _pkOptions = new PKCalculationOptions
         {
            TotalDrugMassPerBodyWeight = 10,
         };
      }

      protected override void Because()
      {
         _pk = sut.CalculatePK(_emptyColumns, _pkOptions);
      }

      [Observation]
      public void should_return_an_empty_pk_calculation()
      {
         _pk.ValueFor(Thalf).ShouldNotBeEqualTo(float.PositiveInfinity);
      }
   }
}