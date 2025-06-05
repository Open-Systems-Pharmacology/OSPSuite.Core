using System.Collections;
using System.Collections.Generic;

namespace OSPSuite.Core.Domain
{
   public class QualificationPlan : ObjectBase, IEnumerable<IQualificationStep>
   {
      private readonly List<IQualificationStep> _allQualificationSteps = new List<IQualificationStep>();

      public void Add(IQualificationStep qualificationStep)
      {
         _allQualificationSteps.Add(qualificationStep);
      }

      public IReadOnlyList<IQualificationStep> Steps => _allQualificationSteps;

      public IEnumerator<IQualificationStep> GetEnumerator() => _allQualificationSteps.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }

   public interface IQualificationStep
   {
      string Display { get; }
   }
}