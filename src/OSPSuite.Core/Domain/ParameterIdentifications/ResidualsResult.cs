using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class ResidualsResult
   {
      private readonly List<OutputResiduals> _allOutputResiduals = new List<OutputResiduals>();

      public IReadOnlyCollection<OutputResiduals> AllOutputResiduals => _allOutputResiduals;

      /// <summary>
      ///    Cached total error. Null if not computed yet
      /// </summary>
      private double? _totalError;

      public bool ExceptionOccured { get; set; }
      public string ExceptionMessage { get; set; }

      public virtual void AddOutputResiduals(string fullOutputPath, DataRepository observedData, IReadOnlyList<Residual> residuals)
      {
         _allOutputResiduals.Add(new OutputResiduals(fullOutputPath, observedData, residuals));
      }

      public virtual IReadOnlyList<OutputResiduals> AllOutputResidualsFor(string fullOutputPath)
      {
         return _allOutputResiduals.FindAll(x => string.Equals(x.FullOutputPath, fullOutputPath));
      }

      public virtual void AddOutputResiduals(OutputResiduals outputResiduals)
      {
         _allOutputResiduals.Add(outputResiduals);
      }

      public virtual IReadOnlyList<Residual> AllResiduals => allResiduals(x => true);
      public virtual IReadOnlyList<Residual> AllResidualsWithWeightsStrictBiggerZero => allResiduals(x => x.Weight > 0);

      private IReadOnlyList<Residual> allResiduals(Func<Residual, bool> query)
      {
         return _allOutputResiduals.SelectMany(x => x.Residuals).Where(query).ToList();
      }

      public double SumResidual2
      {
         get
         {
            if (ExceptionOccured)
               return double.PositiveInfinity;

            return _allOutputResiduals.Sum(x => x.Residuals.Sum(v => v.Value * v.Value));
         }
      }

      public virtual double TotalError
      {
         get
         {
            if (ExceptionOccured)
               return double.PositiveInfinity;

            if (_totalError == null)
               _totalError = Math.Sqrt(SumResidual2);

            return _totalError.Value;
         }
      }

      public void RemoveResidual(OutputResiduals residual)
      {
         _allOutputResiduals.Remove(residual);
      }
   }
}