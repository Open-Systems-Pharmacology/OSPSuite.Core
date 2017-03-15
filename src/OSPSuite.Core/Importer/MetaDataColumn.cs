using System.Collections.Generic;
using System.Data;
using OSPSuite.Assets;

namespace OSPSuite.Core.Importer
{
   public class MetaDataColumn : DataColumn
   {
      public new MetaDataTable Table
      {
         get { return (MetaDataTable) base.Table; }
      }

      public bool Required
      {
         get { return !AllowDBNull; }
         set { AllowDBNull = !value; }
      }

      public double? MinValue { get; set; }
      public bool MinValueAllowed { get; set; }
      public double? MaxValue { get; set; }
      public bool MaxValueAllowed { get; set; }

      public bool IsValueValid(double valueToCheck)
      {
         if (MinValue == null && MaxValue == null) return true;

         var retVal = true;
         if (MinValue != null && MinValueAllowed) retVal &= (valueToCheck >= MinValue);
         if (MinValue != null && !MinValueAllowed) retVal &= (valueToCheck > MinValue);
         if (MaxValue != null && MaxValueAllowed) retVal &= (valueToCheck <= MaxValue);
         if (MaxValue != null && !MaxValueAllowed) retVal &= (valueToCheck < MaxValue);
         return retVal;
      }

      /// <summary>
      ///    This property is a list of string values which shows the list of possible column values.
      /// </summary>
      /// <remarks>The key is the real value. The value of the dictionary is the display text of the value in the gui.</remarks>
      public Dictionary<string, string> ListOfValues { get; set; }

      /// <summary>
      ///    List of images, if available
      /// </summary>
      /// <remarks>The images should be added with a key equal to the value key in <see cref="ListOfValues" />.</remarks>
      public Dictionary<string, ApplicationIcon> ListOfImages { get; set; }

      /// <summary>
      ///    Is the list of values fixed or can the user enter values not in the list and the list is just a suggestion.
      /// </summary>
      public bool IsListOfValuesFixed { get; set; }

      public string Description { get; set; }

      public string DisplayName
      {
         get { return Caption; }
         set { Caption = value; }
      }

      private bool _disposed;

      public bool IsColumnUsedForGrouping { set; get; }

      protected override void Dispose(bool disposing)
      {
         if (!_disposed)
         {
            try
            {
               _disposed = true;
            }
            finally
            {
               base.Dispose(disposing);
            }
         }
      }
   }
}