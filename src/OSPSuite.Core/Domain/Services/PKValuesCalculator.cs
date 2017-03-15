using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Maths.Interpolations;

namespace OSPSuite.Core.Domain.Services
{
   public class PKValuesCalculator : IPKValuesCalculator
   {
      /// <summary>
      ///    3 intervals: TStart=>TD2, TLast=>TEnd and TStart=>TEnd, where TD2 is the second application and TLast the last
      ///    application
      /// </summary>

      //First interval is either the full range for a single dosing or the first dosing interval
      private const int FIRST_INTERVAL = 0;

      //Last but one interval (only relevant for multiple dosing)
      private const int LAST_MINUS_ONE_INTERVAL = 1;

      //Last interval is the last dosing interval  (only relevant for multiple dosing)
      private const int LAST_INTERVAL = 2;

      //This represents the time interval between simulation start and end
      private const int FULL_RANGE_INTERVAL = 3;

      public PKValues CalculatePK(IReadOnlyList<float> time, IReadOnlyList<float> concentration, PKCalculationOptions options = null)
      {
         //we use a cache to avoid conversion problem between double and float. 
         var pk = new Cache<string, double>();

         options = checkOptions(options);
         var allIntervals = allIntervalsFor(time, concentration, options).ToList();
         var isSingleDosing = allIntervals.Count == 1;

         allIntervals.Each(x => x.Calculate());
         if (isSingleDosing)
            setSingleDosingPKValues(pk, allIntervals[FIRST_INTERVAL], options.Dose);

         else if (allIntervals.Any())
            setMultipleDosingPKValues(pk, allIntervals, options);

         return pkValuesFrom(pk);
      }

      private void setMultipleDosingPKValues(ICache<string, double> pk, List<DosingInterval> allIntervals, PKCalculationOptions options)
      {
         var firstInterval = allIntervals[FIRST_INTERVAL];
         var lastMinusOneInterval = allIntervals[LAST_MINUS_ONE_INTERVAL];
         var lastInterval = allIntervals[LAST_INTERVAL];
         var fullInterval = allIntervals[FULL_RANGE_INTERVAL];

         setCmaxAndTmax(pk, firstInterval, options.FirstDose, Constants.PKParameters.C_max_t1_t2, Constants.PKParameters.Tmax_t1_t2);
         setValue(pk, Constants.PKParameters.Ctrough_t2, firstInterval.CTrough);
         setValueAndNormalize(pk, Constants.PKParameters.AUC_t1_t2, firstInterval.Auc, options.FirstDose);
         setValueAndNormalize(pk, Constants.PKParameters.AUC_inf_t1, firstInterval.AucInf, options.FirstDose);
         setFirstIntervalPKValues(pk, firstInterval, options.FirstDose);

         setValueAndNormalize(pk, Constants.PKParameters.AUC_tLast_minus_1_tLast, lastMinusOneInterval.Auc, options.LastMinusOneDose);

         setCmaxAndTmax(pk, lastInterval, options.LastDose, Constants.PKParameters.C_max_tLast_tEnd, Constants.PKParameters.Tmax_tLast_tEnd);
         setValue(pk, Constants.PKParameters.Ctrough_tLast, lastInterval.CTrough);
         setValue(pk, Constants.PKParameters.Thalf_tLast_tEnd, lastInterval.Thalf);
         setValueAndNormalize(pk, Constants.PKParameters.AUC_inf_tLast, lastInterval.AucInf, options.LastDose);

         setCmaxAndTmax(pk, fullInterval, options.Dose, Constants.PKParameters.C_max, Constants.PKParameters.Tmax);
      }

      private void setSingleDosingPKValues(ICache<string, double> pk, DosingInterval interval, double? dose)
      {
         setCmaxAndTmax(pk, interval, dose, Constants.PKParameters.C_max, Constants.PKParameters.Tmax);
         setValue(pk, Constants.PKParameters.C_tEnd, interval.CTrough);
         setValueAndNormalize(pk, Constants.PKParameters.AUC, interval.Auc, dose);
         setValueAndNormalize(pk, Constants.PKParameters.AUC_inf, interval.AucInf, dose);
         setFirstIntervalPKValues(pk, interval, dose);

         setValue(pk, Constants.PKParameters.FractionAucEndToInf, interval.FractionAucEndToInf);
      }

      private void setCmaxAndTmax(ICache<string, double> pk, DosingInterval interval, double? dose, string cmaxName, string tMaxName)
      {
         setValueAndNormalize(pk, cmaxName, interval.Cmax, dose);
         setValue(pk, tMaxName, interval.Tmax);
      }

      private void setFirstIntervalPKValues(ICache<string, double> pk, DosingInterval interval, double? dose)
      {
         setValue(pk, Constants.PKParameters.Thalf, interval.Thalf);
         setValue(pk, Constants.PKParameters.MRT, interval.Mrt);

         if (!dose.HasValue)
            return;

         setValue(pk, Constants.PKParameters.CL, interval.CL);
         setValue(pk, Constants.PKParameters.Vss, interval.Vss);
         setValue(pk, Constants.PKParameters.Vd, interval.Vd);
      }

      private void setValue(ICache<string, double> pk, string parameterName, double value)
      {
         pk[parameterName] = value;
      }

      private void setValueAndNormalize(ICache<string, double> pk, string parameterName, double value, double? dose)
      {
         setValue(pk, parameterName, value);
         pk[Constants.PKParameters.NormalizedName(parameterName)] = normalizedValue(value, dose);
      }

      private double normalizedValue(double value, double? dose)
      {
         if (!dose.HasValue)
            return double.NaN;

         return value / dose.Value;
      }

      private PKValues pkValuesFrom(ICache<string, double> pk)
      {
         var pkValues = new PKValues();
         foreach (var pkValue in  pk.KeyValues)
         {
            pkValues.AddValue(pkValue.Key, pkValue.Value.ConvertedTo<float>());
         }
         return pkValues;
      }

      private static PKCalculationOptions checkOptions(PKCalculationOptions options)
      {
         return options ?? new PKCalculationOptions();
      }

      public PKValues CalculatePK(DataColumn dataColumn, PKCalculationOptions options = null)
      {
         return CalculatePK(dataColumn.BaseGrid.Values, dataColumn.Values, options);
      }

      private IEnumerable<DosingInterval> allIntervalsFor(IReadOnlyList<float> time, IReadOnlyList<float> concentration, PKCalculationOptions options)
      {
         var timeValues = time.ToList();
         var concentrationValues = concentration.ToList();

         if (!timeValues.Any() || !concentrationValues.Any())
            yield break;

         var fullRange = new DosingInterval(timeValues, concentrationValues, options);

         //only one interval
         if (options.SingleDosing)
         {
            yield return fullRange;
            yield break;
         }

         var firstDosingStartIndex = ArrayHelper.ClosestIndexOf(timeValues, options.FirstDosingStartValue);
         var firstDosingEndIndex = ArrayHelper.ClosestIndexOf(timeValues, options.FirstDosingEndValue);
         var lastMinusOneDosingStartIndex = ArrayHelper.ClosestIndexOf(timeValues, options.LastMinusOneDosingStartValue);
         var lastDosingStartIndex = ArrayHelper.ClosestIndexOf(timeValues, options.LastDosingStartValue);
         var lastDosingEndIndex = ArrayHelper.ClosestIndexOf(timeValues, options.LastDosingEndValue);

         if (oneTimeIndexInvalid(firstDosingEndIndex, firstDosingStartIndex, lastDosingStartIndex, lastDosingEndIndex, lastMinusOneDosingStartIndex))
            yield break;

         yield return dosingInterval(timeValues, concentrationValues, firstDosingStartIndex, firstDosingEndIndex, options);
         //The end time of the last minus one dosing interval is the start time of the last interval
         yield return dosingInterval(timeValues, concentrationValues, lastMinusOneDosingStartIndex, lastDosingStartIndex, options);
         yield return dosingInterval(timeValues, concentrationValues, lastDosingStartIndex, lastDosingEndIndex, options);
         yield return fullRange;
      }

      private bool oneTimeIndexInvalid(params int[] indexes)
      {
         return indexes.Any(i => i < 0);
      }

      private DosingInterval dosingInterval(List<float> time, List<float> concentration, int startIndex, int endIndex, PKCalculationOptions options)
      {
         return new DosingInterval(ArrayHelper.TruncateArray(time, startIndex, endIndex), ArrayHelper.TruncateArray(concentration, startIndex, endIndex), options);
      }

      private static class ArrayHelper
      {
         public static List<float> TruncateArray(IReadOnlyList<float> array, int startIndex, int endIndex)
         {
            var truncate = new float[endIndex - startIndex + 1];
            for (int i = 0; i < truncate.Length; i++)
            {
               truncate[i] = array[i + startIndex];
            }
            return truncate.ToList();
         }

         public static IReadOnlyList<double> TimeStepsFrom(IReadOnlyList<float> time)
         {
            return derivedArray(time, (f1, f2) => f2 - f1);
         }

         public static IEnumerable<double> Multiply(IReadOnlyList<double> array1, IReadOnlyList<double> array2)
         {
            return array1.Select((item1, index) => item1 * array2[index]);
         }

         public static IEnumerable<float> Multiply(IReadOnlyList<float> array1, IReadOnlyList<float> array2)
         {
            return array1.Select((item1, index) => item1 * array2[index]);
         }

         public static IReadOnlyList<double> MeanValuesFrom(IReadOnlyList<float> conc)
         {
            return derivedArray(conc, (f1, f2) => 0.5 * (f1 + f2));
         }

         private static double[] derivedArray(IReadOnlyList<float> source, Func<float, float, double> operation)
         {
            var derived = new double[source.Count - 1];
            for (int i = 0; i < source.Count - 1; i++)
            {
               derived[i] = operation(source[i], source[i + 1]);
            }
            return derived;
         }

         public static int ClosestIndexOf(List<float> list, float? value)
         {
            if (value == null)
               return -1;

            return closestIndexOf(list, value.Value);
         }

         private static int closestIndexOf(List<float> list, float value)
         {
            int index = list.BinarySearch(value);
            //already exist return
            if (index >= 0)
               return index;

            //does not exist in the list. We find the index of the point immedialy after this value
            var closestIndex = ~index;
            return closestIndex < list.Count ? closestIndex : -1;
         }
      }

      private class DosingInterval
      {
         private readonly PKCalculationOptions _options;
         private readonly IReadOnlyList<double> _timeSteps;
         private readonly PolyFit _polyFit;
         private double _intercept;
         private double _lambda;

         private readonly List<float> _time;
         private readonly List<float> _concentration;
         public float Cmax { get; private set; }
         public float Tmax { get; private set; }
         public float CTrough { get; private set; }
         public double Auc { get; private set; }
         public double Aucm { get; private set; }
         public double AucInf { get; private set; }
         public double AucTendInf { get; private set; }
         public double Mrt { get; private set; }

         public double FractionAucEndToInf => AucTendInf / AucInf;
         public double Thalf => Math.Log(2) / _lambda;
         public double Vss => CL * Mrt;
         public double Vd => CL / _lambda;

         public DosingInterval(List<float> time, List<float> conc, PKCalculationOptions options)
         {
            _options = options;
            _time = time;
            _concentration = conc;
            _timeSteps = ArrayHelper.TimeStepsFrom(time);
            _polyFit = new PolyFit();
         }

         public void Calculate()
         {
            Cmax = _concentration.Max();
            Tmax = _time[_concentration.IndexOf(Cmax)];
            CTrough = _concentration.Last();
            Auc = calculateAuc();
            calculateAucInf();

            Aucm = calculateAucm();
            Mrt = calculateMrt();
         }

         private void calculateAucInf()
         {
            var fit = straightLineFit();
            _intercept = fit.Item1;
            _lambda = fit.Item2;

            //valid cases are lambda > 0 otherwise value are INF
            if (_lambda < 0)
               return;

            // add the last part of the auc interpolated to infinity (- because slope <0)
            AucTendInf = _intercept / _lambda * Math.Exp(-_lambda * _time.Last());
            AucInf = Auc + AucTendInf;
         }

         private double calculateAuc()
         {
            var meanValues = ArrayHelper.MeanValuesFrom(_concentration);
            return ArrayHelper.Multiply(_timeSteps, meanValues).Sum();
         }

         private double calculateAucm()
         {
            //First momentum
            var xy = ArrayHelper.Multiply(_time, _concentration).ToArray();
            var meanValuesMomentum = ArrayHelper.MeanValuesFrom(xy);
            return ArrayHelper.Multiply(_timeSteps, meanValuesMomentum).Sum();
         }

         private double calculateMrt()
         {
            // curve should be decreasing!
            if (AucInf == 0 || _lambda <= 0)
               return double.NaN;

            //calculates the moment between t_end and infinity as the integral of the last 10% fit
            // integral_tEnd_Infinity ( t * Exp(slope * t + intercept) * dt) with slope <0!
            var tEnd = _time.Last();
            var cLast = _intercept * Math.Exp(-_lambda * tEnd);
            var aucmInf = cLast / _lambda * (tEnd + 1 / _lambda);

            var mrt = (Aucm + aucmInf) / AucInf;

            if (_options.InfusionTime.HasValue)
               mrt -= _options.InfusionTime.Value / 2;

            return mrt;
         }

         public double CL
         {
            get
            {
               if (_options.Dose.HasValue)
                  return _options.Dose.Value / AucInf;

               return double.NaN;
            }
         }

         /// <summary>
         ///    Creates a polynomial fit of order 1 for the last 10% of the data.
         ///    and returns the two coefficient (c_0 and C_1 of the fit)
         /// </summary>
         private Tuple<double, double> straightLineFit()
         {
            var errorResult = new Tuple<double, double>(0, 0);

            int numOfPoints = (int) Math.Floor(_time.Count / 10.0);
            //the number of point is zero. We return a slope of 0. this should never happen
            if (numOfPoints == 0)
               return errorResult;

            var x = new double[numOfPoints];
            var y = new double[numOfPoints];

            for (int i = 0; i < numOfPoints; i++)
            {
               int index = _time.Count - i - 1;
               float conc_i = _concentration[index];

               if (float.IsNaN(conc_i) || conc_i < 0)
                  return errorResult;

               x[i] = _time[index];
               y[i] = Math.Log(conc_i);
            }

            try
            {
               var res = _polyFit.Polyfit(x, y, 1);
               return new Tuple<double, double>(Math.Exp(res[0]), -res[1]);
            }
            catch (Exception)
            {
               //in that case we return and do not throw as something went wrong in the calculation
               return errorResult;
            }
         }
      }
   }
}