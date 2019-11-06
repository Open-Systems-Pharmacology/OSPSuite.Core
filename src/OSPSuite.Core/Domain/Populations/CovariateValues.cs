using System;
using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Populations
{
   public class CovariateValues : IWithName
   {
      public string Name { get; set; }
      public List<string> Values { get; set; } = new List<string>();


      [Obsolete("For serialization")]
      public CovariateValues()
      {
      }

      public CovariateValues(string name, List<string> values = null)
      {
         Name = name;
         Values = values ?? Values;
      }
      
      public void Add(string value)
      {
         Values.Add(value);
      }

      public string ValueAt(int index)
      {
         if (index < 0 || index >= Count)
            return Constants.UNKNOWN;

         return Values[index];
      }

      public int Count => Values.Count;

      public CovariateValues Clone()
      {
         return new CovariateValues(Name) {Values = new List<string>(Values)};
      }
      
      public void AddEmptyItems(int count, string defaultValue = Constants.UNKNOWN)
      {
         for (int i = 0; i < count; i++)
         {
            Add(defaultValue);
         }
      }

      public void Merge(CovariateValues covariateValues)
      {
         Values.AddRange(covariateValues.Values);
      }

   }
}