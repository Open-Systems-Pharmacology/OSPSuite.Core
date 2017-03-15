using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class OutputResiduals : IEnumerable<Residual>
   {
      public virtual DataRepository ObservedData { get; }
      private readonly List<Residual> _residuals = new List<Residual>();
      public virtual string FullOutputPath { get; private set; }
      public virtual IReadOnlyList<Residual> Residuals => _residuals;
      public virtual string ObservedDataName => ObservedData.Name;

      [Obsolete("For serialization")]
      public OutputResiduals()
      {
      }

      public OutputResiduals(string fullOutputPath, DataRepository observedData, IReadOnlyList<Residual> residuals)
      {
         ObservedData = observedData;
         FullOutputPath = fullOutputPath;
         _residuals.AddRange(residuals);
      }

      public void Add(Residual residual)
      {
         _residuals.Add(residual);
      }

      public IEnumerator<Residual> GetEnumerator()
      {
         return _residuals.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public void UpdateFullOutputPath(string oldValue, string newValue)
      {
         var newPath = new ObjectPath(FullOutputPath.ToPathArray());
         newPath.Replace(oldValue, newValue);
         FullOutputPath = newPath;
      }

      public virtual bool IsActive => _residuals.Any(x => x.Weight > 0);
   }
}