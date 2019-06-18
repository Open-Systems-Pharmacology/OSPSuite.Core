using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Core.Domain
{
   public static class GenericRules
   {
      public static IBusinessRule NonEmptyRule<T>(Expression<Func<T, string>> property, string error = Validation.ValueIsRequired)
      {
         return CreateRule.For<T>()
            .Property(property)
            .WithRule((o, v) => v.StringIsNotEmpty())
            .WithError(error);
      }

      public static IBusinessRule FileExists<T>(Expression<Func<T, string>> property)
      {
         return CreateRule.For<T>()
            .Property(property)
            .WithRule((item, file) => FileHelper.FileExists(file))
            .WithError((item, file) => Validation.FileDoesNotExist(file));
      }

      public static IBusinessRule NotNull<T, U>(Expression<Func<T, U>> property, string error = Validation.ValueIsRequired) where U : class
      {
         return CreateRule.For<T>()
            .Property(property)
            .WithRule((o, u) => u != null)
            .WithError(error);
      }
   }

   public static class ScaleDivisorRules
   {
      public static IBusinessRule ScaleDivisorStrictlyPositive
      {
         get
         {
            return CreateRule.For<IWithScaleDivisor>()
               .Property(x => x.ScaleDivisor)
               .WithRule((m, scaleDivisor) => scaleDivisor > 0)
               .WithError(Error.ScaleFactorShouldBeGreaterThanZero);
         }
      }
   }

   public static class EntityRules
   {
      private static readonly IList<IBusinessRule> _allEntityRules = new List<IBusinessRule>
      {
         NotEmptyName
      };

      public static IBusinessRule NotEmptyName
      {
         get { return GenericRules.NonEmptyRule<IEntity>(x => x.Name, Validation.NameIsRequired); }
      }

      public static IEnumerable<IBusinessRule> All()
      {
         return _allEntityRules;
      }
   }
}