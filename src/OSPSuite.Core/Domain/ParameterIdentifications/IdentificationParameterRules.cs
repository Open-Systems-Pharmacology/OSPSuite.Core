using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   internal static class IdentificationParameterRules
   {
      public static IEnumerable<IBusinessRule> All
      {
         get
         {
            yield return nameNotEmpty;
            yield return minLessThanMax;
            yield return maxGreaterThanMin;
            yield return startValueBetweenMinAndMax;
            yield return minStrictBiggerThanZeroForLogScalling;
            yield return nameUnique;
            yield return minimumConsistentWithLinkedParameters;
            yield return maximumConsistentWithLinkedParameters;
         }
      }

      private static IBusinessRule nameNotEmpty
      {
         get { return GenericRules.NonEmptyRule<IdentificationParameter>(x => x.Name); }
      }

      private static IBusinessRule minimumConsistentWithLinkedParameters
      {
         get
         {
            return CreateRule.For<IdentificationParameter>()
               .Property(item => item.MinValue)
               .WithRule((parameter, d) => isValidExtreme(parameter, d, isMinValueConsistent))
               .WithError((param, value) => messagesForInconsistentExtremes(allInconsistentExtremesFor(param, value, isMinValueConsistent),
                  Rules.Parameters.MinimumMustBeGreaterThanOrEqualTo, Rules.Parameters.MinimumMustBeGreaterThan,
                  parameter => parameter.MinIsAllowed, parameter => parameter.MinValue));
         }
      }

      private static IBusinessRule maximumConsistentWithLinkedParameters
      {
         get
         {
            return CreateRule.For<IdentificationParameter>()
               .Property(item => item.MaxValue)
               .WithRule((parameter, d) => isValidExtreme(parameter, d, isMaxValueConsistent))
               .WithError((param, value) => messagesForInconsistentExtremes(allInconsistentExtremesFor(param, value, isMaxValueConsistent),
                  Rules.Parameters.MaximumMustBeLessThanOrEqualTo, Rules.Parameters.MaximumMustBeLessThan,
                  parameter => parameter.MaxIsAllowed, parameter => parameter.MaxValue));
         }
      }

      private static bool isMaxValueConsistent(IParameter parameter, double newMaximum, bool useAsFactor)
      {
         if (parameter?.MaxValue == null)
            return true;

         if (useAsFactor)
            newMaximum = newMaximum * parameter.Value;

         if (parameter.MaxIsAllowed)
            return newMaximum <= parameter.MaxValue.Value;

         return newMaximum < parameter.MaxValue.Value;
      }

      private static bool isMinValueConsistent(IParameter parameter, double newMinimum, bool useAsFactor)
      {
         if (parameter?.MinValue == null)
            return true;

         if (useAsFactor)
            newMinimum = newMinimum * parameter.Value;

         if (parameter.MinIsAllowed)
            return newMinimum >= parameter.MinValue.Value;

         return newMinimum > parameter.MinValue.Value;
      }

      private static bool isValidExtreme(IdentificationParameter identificationParameter, double value, Func<IParameter, double, bool, bool> consistencyChecker)
      {
         return !allInconsistentExtremesFor(identificationParameter, value, consistencyChecker).Any();
      }

      private static IEnumerable<ParameterSelection> allInconsistentExtremesFor(IdentificationParameter identificationParameter, double value, Func<IParameter, double, bool, bool> consistencyChecker)
      {
         return identificationParameter.AllLinkedParameters.Where(x => x.IsValid && !consistencyChecker(x.Parameter, value, identificationParameter.UseAsFactor));
      }

      private static string messagesForInconsistentExtremes(IEnumerable<ParameterSelection> allInconsistentExtremes,
         Func<double, string, string> extremeAllowedFunc, Func<double, string, string> extremeNotAllowedFunc,
         Func<IParameter, bool> extremeIsAllowed, Func<IParameter, double?> valueFunc)
      {
         return allInconsistentExtremes.Select(x =>
         {
            var value = valueFunc(x.Parameter);
            if (value.HasValue)
               return messageForInconsistentExtreme(extremeIsAllowed(x.Parameter), value.Value, x.FullQuantityPath, extremeAllowedFunc, extremeNotAllowedFunc);

            return string.Empty;
         }).Where(x => !string.IsNullOrEmpty(x)).ToString(Environment.NewLine);
      }

      private static string messageForInconsistentExtreme(bool extremeIsAllowed, double extremeValue, string fullQuantityPath, Func<double, string, string> extremeAllowedFunc, Func<double, string, string> extremeNotAllowedFunc)
      {
         if (extremeIsAllowed)
            return extremeAllowedFunc(extremeValue, fullQuantityPath);
         return extremeNotAllowedFunc(extremeValue, fullQuantityPath);
      }

      private static IBusinessRule minLessThanMax
      {
         get
         {
            return CreateRule.For<IdentificationParameter>()
               .Property(item => item.MinValue)
               .WithRule((x, value) => value < x.MaxValue)
               .WithError((param, value) => Rules.Parameters.MinLessThanMax);
         }
      }

      private static IBusinessRule maxGreaterThanMin
      {
         get
         {
            return CreateRule.For<IdentificationParameter>()
               .Property(item => item.MaxValue)
               .WithRule((x, value) => x.MinValue < value)
               .WithError((param, value) => Rules.Parameters.MaxGreaterThanMin);
         }
      }

      private static IBusinessRule startValueBetweenMinAndMax
      {
         get
         {
            return CreateRule.For<IdentificationParameter>()
               .Property(item => item.StartValue)
               .WithRule((x, value) => x.MinValue <= value && x.MaxValue >= value)
               .WithError((param, value) => Rules.Parameters.ValueShouldBeBetweenMinAndMax);
         }
      }

      private static IBusinessRule minStrictBiggerThanZeroForLogScalling
      {
         get
         {
            return CreateRule.For<IdentificationParameter>()
               .Property(item => item.MinValue)
               .WithRule((x, value) => validateScaling(value, x.Scaling))
               .WithError((param, value) => Rules.Parameters.MinShouldBeStrictlyGreaterThanZeroForLogScale);
         }
      }

      private static bool validateScaling(double minValue, Scalings scaling)
      {
         return scaling != Scalings.Log || minValue > 0;
      }

      private static IBusinessRule nameUnique
      {
         get
         {
            return CreateRule.For<IdentificationParameter>()
               .Property(x => x.Name)
               .WithRule((currentIdentificationParameter, name) =>
               {
                  var parameterIdentification = currentIdentificationParameter.ParameterIdentification;

                  var otherIdentificationParameter = parameterIdentification?.IdentificationParameterByName(name);
                  if (otherIdentificationParameter == null)
                     return true;

                  return ReferenceEquals(otherIdentificationParameter, currentIdentificationParameter);
               })
               .WithError((field, name) => Error.NameAlreadyExistsInContainerType(name, ObjectTypes.IdentificationParameter));
         }
      }
   }
}