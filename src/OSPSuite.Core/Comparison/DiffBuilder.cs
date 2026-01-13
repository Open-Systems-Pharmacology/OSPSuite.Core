using System;
using System.Linq.Expressions;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Core.Comparison
{
   public interface IDiffBuilder : ISpecification<Type>
   {
      void CompareObjects(object object1, object object2, ComparerSettings comparerSettings, DiffReport report, object commonAncestor);
   }

   public interface IDiffBuilder<in TObject> : IDiffBuilder where TObject : class
   {
      void Compare(IComparison<TObject> comparison);
   }

   public abstract class DiffBuilder<TObject> : IDiffBuilder<TObject> where TObject : class
   {
      private readonly UnitFormatter _numericFormatter;

      protected DiffBuilder()
      {
         _numericFormatter = new UnitFormatter();
      }

      public void CompareObjects(object object1, object object2, ComparerSettings comparerSettings, DiffReport report, object commonAncestor)
      {
         var comparison = new Comparison<TObject>(
            object1.DowncastTo<TObject>(),
            object2.DowncastTo<TObject>(),
            comparerSettings,
            report,
            commonAncestor
         );

         Compare(comparison);
      }

      public abstract void Compare(IComparison<TObject> comparison);

      internal void CompareValues<TInput, TOutput>(Func<TInput, TOutput> funcEvaluation, Expression<Func<TInput, TOutput>> propertyNameExpression, IComparison<TInput> comparison) where TInput : class
      {
         CompareValues(funcEvaluation, NameFrom(propertyNameExpression), comparison);
      }

      protected void CompareValues<TInput, TOutput>(Func<TInput, TOutput> funcEvaluation, Expression<Func<TInput, TOutput>> propertyNameExpression, IComparison<TInput> comparison, Func<TOutput, TOutput, bool> areEquals) where TInput : class
      {
         CompareValues(funcEvaluation, NameFrom(propertyNameExpression), comparison, areEquals);
      }

      protected void CompareStringValues<TInput>(Func<TInput, string> funcEvaluation, Expression<Func<TInput, string>> propertyNameExpression, IComparison<TInput> comparison) where TInput : class
      {
         CompareStringValues(funcEvaluation, NameFrom(propertyNameExpression), comparison);
      }

      protected void CompareNullableDoubleValues<TInput>(Func<TInput, double?> funcEvaluation, Expression<Func<TInput, double?>> propertyNameExpression, IComparison<TInput> comparison, Func<TInput, Unit> displayUnitFunc = null) where TInput : class
      {
         CompareNullableDoubleValues(funcEvaluation, NameFrom(propertyNameExpression), comparison, displayUnitFunc);
      }

      protected void CompareDoubleValues<TInput>(Func<TInput, double> funcEvaluation, Expression<Func<TInput, double>> propertyNameExpression, IComparison<TInput> comparison, Func<TInput, Unit> displayUnitFunc = null) where TInput : class
      {
         CompareDoubleValues(funcEvaluation, NameFrom(propertyNameExpression), comparison, displayUnitFunc);
      }

      protected void CompareValues<TInput, TOutput>(Func<TInput, TOutput> funcEvaluation, Expression<Func<TInput, TOutput>> propertyNameExpression, IComparison<TInput> comparison,
         Func<TOutput, TOutput, bool> areEqual, Func<TInput, TOutput, string> formatter) where TInput : class
      {
         CompareValues(funcEvaluation, NameFrom(propertyNameExpression), comparison, areEqual, formatter);
      }

      protected string NameFrom<TInput, TOutput>(Expression<Func<TInput, TOutput>> propertyNameExpression)
      {
         return propertyNameExpression.Name().SplitToUpperCase();
      }

      protected void CompareValues<TInput, TOutput>(Func<TInput, TOutput> funcEvaluation, string propertyName, IComparison<TInput> comparison) where TInput : class
      {
         CompareValues(funcEvaluation, propertyName, comparison, (x1, x2) => Equals(x1, x2), (input, output) => defaultFormatter(input, output));
      }

      protected void CompareValues<TInput, TOutput>(Func<TInput, TOutput> funcEvaluation, string propertyName, IComparison<TInput> comparison, Func<TOutput, TOutput, bool> areEquals) where TInput : class
      {
         CompareValues(funcEvaluation, propertyName, comparison, areEquals, (input, output) => defaultFormatter(input, output));
      }

      protected void CompareStringValues<TInput>(Func<TInput, string> funcEvaluation, string propertyName, IComparison<TInput> comparison) where TInput : class
      {
         CompareValues(funcEvaluation, propertyName, comparison, areStringEquals, defaultFormatter);
      }

      protected void CompareNullableDoubleValues<TInput>(Func<TInput, double?> funcEvaluation, string propertyName, IComparison<TInput> comparison, Func<TInput, Unit> displayUnitFunc = null) where TInput : class
      {
         CompareDoubleValues(x => funcEvaluation(x).GetValueOrDefault(double.NaN), propertyName, comparison, displayUnitFunc);
      }

      protected void CompareDoubleValues<TInput>(Func<TInput, double> funcEvaluation, string propertyName, IComparison<TInput> comparison, Func<TInput, Unit> displayUnitFunc = null) where TInput : class
      {
         try
         {
            // Determine the display unit to use for both values (based on object1)
            var standardDisplayUnit = comparison.Object1 is IWithDimension && displayUnitFunc != null ? displayUnitFunc(comparison.Object1) : null;

            string ConsistentUnitFormatter(TInput input, double value)
            {
               if (!(input is IWithDimension withDimension) || standardDisplayUnit == null || withDimension.Dimension == null)
                  return _numericFormatter.Format(value);

               // Convert the value to object1's unit for consistent display
               var valueInStandardUnit = withDimension.ConvertToUnit(value, standardDisplayUnit);
               return _numericFormatter.Format(valueInStandardUnit, standardDisplayUnit);
            }

            CompareValues(
               funcEvaluation,
               propertyName,
               comparison,
               (x, y) => ValueComparer.AreValuesEqual(x, y, comparison.Settings.RelativeTolerance),
               ConsistentUnitFormatter
            );
         }
         catch (OSPSuiteException)
         {
            //in that case formula could not be evaluated for the given objects (can happen in pksim with parameters within alternatives)
         }
      }

      /// <summary>
      ///    Check if the value are equals for the given property and the two objects.
      ///    First implementation was using an expression but the call to compile was way to long comparing to the actual
      ///    comparison
      /// </summary>
      /// <typeparam name="TInput">Input type</typeparam>
      /// <typeparam name="TOutput">Output type</typeparam>
      /// <param name="funcEvaluation">
      ///    The actual function being evaluated. We could compile the property name expression but
      ///    this would have an impact on performance
      /// </param>
      /// <param name="propertyName">Name of property being compared</param>
      /// <param name="comparison">Comparison</param>
      /// <param name="areEquals">Method used to compare the two values of type TOutput</param>
      /// <param name="formatter">Formatter used to format the output value</param>
      protected void CompareValues<TInput, TOutput>(Func<TInput, TOutput> funcEvaluation, string propertyName, IComparison<TInput> comparison, Func<TOutput, TOutput, bool> areEquals, Func<TInput, TOutput, string> formatter) where TInput : class
      {
         var object1 = comparison.Object1;
         var object2 = comparison.Object2;

         var value1 = funcEvaluation(object1);
         var value2 = funcEvaluation(object2);

         if (areEquals(value1, value2))
            return;

         var formattedValue1 = formatter(object1, value1);
         var formattedValue2 = formatter(object2, value2);


         var diffItem = new PropertyValueDiffItem
         {
            Object1 = comparison.Object1,
            Object2 = comparison.Object2,
            CommonAncestor = comparison.CommonAncestor,
            PropertyName = propertyName,
            FormattedValue1 = formattedValue1,
            FormattedValue2 = formattedValue2,
            Description = Captions.Diff.PropertyDiffers(propertyName, formattedValue1, formattedValue2)
         };

         comparison.Add(diffItem);
      }

      private string defaultFormatter(object input, object output)
      {
         return output?.ToString() ?? string.Empty;
      }

      private string numericFormatter<TInput>(TInput input, double value, Func<TInput, Unit> unitRetrieverFunc)
      {
         var withDimension = input as IWithDimension;
         if (withDimension == null)
            return _numericFormatter.Format(value);

         if (unitRetrieverFunc == null || withDimension.Dimension == null)
            return _numericFormatter.Format(value);

         var displayUnit = unitRetrieverFunc(input);
         var displayValue = withDimension.Dimension.BaseUnitValueToUnitValue(displayUnit, value);
         return _numericFormatter.Format(displayValue, displayUnit);
      }

      private bool areStringEquals(string x, string y)
      {
         if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y))
            return true;

         if (string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y))
            return false;

         return string.Equals(x, y);
      }

      public virtual bool IsSatisfiedBy(Type type)
      {
         return type.IsAnImplementationOf<TObject>();
      }
   }
}