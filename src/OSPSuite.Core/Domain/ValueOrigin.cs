using System;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class ValueOrigin
   {
      /// <summary>
      /// Indicates if a parameter value is a default value (e.g. coming from the PK-Sim database) or if the value was entered or modified by the user.
      /// Default value is <c>false</c>
      /// </summary>
      public bool Default { get; set; } = false;

      /// <summary>
      ///    Source of the value
      /// </summary>
      public ValueOriginSource Source { get; set; } = ValueOriginSources.Undefined;

      /// <summary>
      ///    Determination method of the value
      /// </summary>
      public ValueOriginDeterminationMethod Method { get; set; } = ValueOriginDeterminationMethods.Undefined;

      /// <summary>
      ///    Optional description explaining the quantity value
      /// </summary>
      public string Description { get; set; }

      public ValueOrigin Clone()
      {
         var clone = new ValueOrigin();
         clone.UpdateFrom(this);
         return clone;
      }

      public void UpdateFrom(ValueOrigin valueOrigin)
      {
         if (valueOrigin == null)
            return;

         Source = valueOrigin.Source;
         Method = valueOrigin.Method;
         Description = valueOrigin.Description;
         Default = valueOrigin.Default;
      }

      public string Display => defaultDisplay(this);

      public override string ToString() => Display;

      //TEMP to ensure that we can test the best display text from the app
      public static Func<ValueOrigin, string> DisplayFunc = defaultDisplay;

      private static string defaultDisplay(ValueOrigin valueOrigin)
      {
         if (valueOrigin.Source == null && string.IsNullOrEmpty(valueOrigin.Description))
            return Captions.ValueOrigins.Undefined;

         return new[]
         {
            valueOrigin.Source?.Display, valueOrigin.Description
         }.Where(x => !string.IsNullOrWhiteSpace(x)).ToString("-");
      }
   }
}