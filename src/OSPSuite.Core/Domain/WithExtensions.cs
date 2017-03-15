using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public static class WithExtensions
   {
      public static T WithDimension<T>(this T withDimension, IDimension dimension) where T : IWithDimension
      {
         withDimension.Dimension = dimension;
         return withDimension;
      }

      public static T WithValue<T>(this T withValue, double value) where T : IWithValue
      {
         withValue.Value = value;
         return withValue;
      }

      public static T WithId<T>(this T withId, string id) where T : IWithId
      {
         withId.Id = id;
         return withId;
      }

      public static T WithName<T>(this T withName, string name) where T : IWithName
      {
         withName.Name = name;
         return withName;
      }

      public static T WithDescription<T>(this T withDescription, string description) where T : IWithDescription
      {
         withDescription.Description = description;
         return withDescription;
      }

      public static T WithDisplayUnit<T>(this T withDisplayUnit, Unit displayUnit) where T : IWithDisplayUnit
      {
         withDisplayUnit.DisplayUnit=displayUnit;
         return withDisplayUnit;
      }

      public static bool IsNamed(this IWithName withName, string name)
      {
         if (withName == null) return false;
         return string.Equals(withName.Name, name);
      }

      public static bool NameIsOneOf(this IWithName withName, IEnumerable<string> names)
      {
         return names.Any(withName.IsNamed);
      }

      public static bool NameIsOneOf(this IWithName withName, params string[] names)
      {
         return withName.NameIsOneOf(names.ToList());
      }
   }
}