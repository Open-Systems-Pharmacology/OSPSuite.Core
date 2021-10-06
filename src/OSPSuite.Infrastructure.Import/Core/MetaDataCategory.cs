using OSPSuite.Core.Domain;
using System;
using System.Collections.Generic;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class MetaDataCategory : IWithName
   {
      public string Name { get; set; }
      public string DisplayName { get; set; }
      public string Description { get; set; }
      public bool IsMandatory { get; set; }

      /// <summary>
      ///    List of valid values, if available
      /// </summary>
      /// <remarks>The key is the real value. The value of the dictionary is the display text of the value in the gui.</remarks>
      public Dictionary<string, string> ListOfValues { get; private set; }

      public bool ShouldListOfValuesBeIncluded { get; set; } = false;

      public bool AllowsManualInput { get; set; } = false;

      public bool SelectDefaultValue { get; set; } = false;

      public List<string> TopNames { get; } = new List<string>();

      /// <summary>
      ///    List of images, if available
      /// </summary>
      /// <remarks>The images should be added with a key equal to the value key in <see cref="ListOfValues" />.</remarks>
      public Dictionary<string, string> ListOfImages { get; private set; }

      /// <summary>
      ///    Is the list of values fixed or can the user enter values not in the list and the list is just a suggestion.
      /// </summary>
      public bool IsListOfValuesFixed { get; set; }

      public double? MinValue { get; set; }
      public bool MinValueAllowed { get; set; }
      public double? MaxValue { get; set; }
      public bool MaxValueAllowed { get; set; }

      /// <summary>
      ///    Type of meta data (string, double etc)
      /// </summary>
      public Type MetaDataType { get; set; }

      /// <summary>
      ///    This can be used to specify the maximum number of characters for string columns.
      /// </summary>
      public int MaxLength { get; set; }

      public object DefaultValue { get; set; }
      public MetaDataCategory()
      {
         ListOfValues = new Dictionary<string, string>();
         ListOfImages = new Dictionary<string, string>();
         Name = string.Empty;
         DisplayName = string.Empty;
         Description = string.Empty;
         IsMandatory = true;
         MinValueAllowed = true;
         MaxValueAllowed = true;
         MetaDataType = typeof(string);
         MaxLength = -1;
         DefaultValue = null;
      }
   }
}