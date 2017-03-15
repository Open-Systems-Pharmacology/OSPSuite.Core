using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Format;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Domain
{
   public class ParameterRules
   {
      private static readonly IList<IBusinessRule> _allParameterRules = new List<IBusinessRule>
         {
            valueSmallerThanMax,
            valueBiggerThanMin,
            minIsAllowed,
            maxIsAllowed,
         };

      private static IBusinessRule valueSmallerThanMax
      {
         get
         {
            return CreateRule.For<IParameter>()
               .Property(item => item.Value)
               .WithRule((param, value) => !param.MaxValue.HasValue || value <= param.MaxValue)
               .WithError((param, value) => ParameterMessages.ValueSmallerThanMax(param));
         }
      }

      private static IBusinessRule valueBiggerThanMin
      {
         get
         {
            return CreateRule.For<IParameter>()
               .Property(item => item.Value)
               .WithRule((param, value) => !param.MinValue.HasValue || value >= param.MinValue)
               .WithError((param, value) => ParameterMessages.ValueBiggerThanMin(param));
         }
      }

      private static IBusinessRule minIsAllowed
      {
         get
         {
            return CreateRule.For<IParameter>()
               .Property(item => item.Value)
               .WithRule((param, value) => (value != param.MinValue || param.MinIsAllowed))
               .WithError((param, value) => ParameterMessages.ValueStrictBiggerThanMin(param));
         }
      }

      private static IBusinessRule maxIsAllowed
      {
         get
         {
            return CreateRule.For<IParameter>()
               .Property(item => item.Value)
               .WithRule((param, value) => (value != param.MaxValue || param.MaxIsAllowed))
               .WithError((param, value) => ParameterMessages.ValueStrictSmallerThanMax(param));
         }
      }

      public static IEnumerable<IBusinessRule> All()
      {
         return _allParameterRules;
      }
   }

   internal static class ParameterMessages
   {
      private static readonly NumericFormatter<double> _numericFormatter = new NumericFormatter<double>(NumericFormatterOptions.Instance);

      public static string ValueBiggerThanMin(IParameter parameter)
      {
         return Validation.ValueBiggerThanMin(parameter.Name,
                                                                  displayFor(parameter, parameter.MinValue),
                                                                  parameter.DisplayUnit.ToString());
      }

      public static string ValueSmallerThanMax(IParameter parameter)
      {
         return Validation.ValueSmallerThanMax(parameter.Name,
                                                                   displayFor(parameter, parameter.MaxValue),
                                                                   parameter.DisplayUnit.ToString());
      }

      public static string ValueStrictBiggerThanMin(IParameter parameter)
      {
         return Validation.ValueStrictBiggerThanMin(parameter.Name,
                                                                        displayFor(parameter, parameter.MinValue),
                                                                        parameter.DisplayUnit.ToString());
      }

      public static string ValueStrictSmallerThanMax(IParameter parameter)
      {
         return Validation.ValueStrictSmallerThanMax(parameter.Name,
                                                                         displayFor(parameter, parameter.MaxValue),
                                                                         parameter.DisplayUnit.ToString());
      }


      private static string displayFor(double value)
      {
         return _numericFormatter.Format(value);
      }

      private static string displayFor(IParameter parameter, double? value)
      {
         return displayFor(parameter.ConvertToDisplayUnit(value));
      }
   }
}