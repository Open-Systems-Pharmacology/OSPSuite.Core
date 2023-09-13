using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public class Neighborhood : Container, INeighborhoodBase
   {
      public IContainer FirstNeighbor { get; set; }

      public IContainer SecondNeighbor { get; set; }

      public Neighborhood()
      {
         ContainerType = ContainerType.Neighborhood;
      }

      /// <summary>
      ///    Return one neighbor in the neighborhood satisfying the criteria.
      ///    <para></para>
      ///    If both first and second neighbors fulfill the criteria, exception is thrown.
      ///    <para></para>
      ///    If none of the neighbors fulfills the criteria, null is returned
      /// </summary>
      /// <exception cref="BothNeighborsSatisfyingCriteriaException">Thrown </exception>
      public IContainer GetNeighborSatisfying(DescriptorCriteria criteria)
      {
         var isSatisfiedByFirstNeighbor = criteria.IsSatisfiedBy(FirstNeighbor);
         var isSatisfiedBySecondNeighbor = criteria.IsSatisfiedBy(SecondNeighbor);

         if (isSatisfiedByFirstNeighbor && isSatisfiedBySecondNeighbor)
            throw new BothNeighborsSatisfyingCriteriaException(this);

         if (isSatisfiedByFirstNeighbor)
            return FirstNeighbor;

         if (isSatisfiedBySecondNeighbor)
            return SecondNeighbor;

         return null;
      }

      /// <summary>
      ///    return true either if the first neighbor satisfies the <paramref name="criteriaForOneNeighbor" /> and the second
      ///    neighbor satisfies the  <paramref name="criteriaForTheOtherNeighbor" />
      ///    or if the first neighbor satisfies the <paramref name="criteriaForTheOtherNeighbor" /> and the second neighbor
      ///    satisfies the <paramref name="criteriaForOneNeighbor" />.
      ///    Otherwise false
      /// </summary>
      /// <param name="criteriaForOneNeighbor">One criteria to be satisfied by at least one neighbor</param>
      /// <param name="criteriaForTheOtherNeighbor">Another criteria to be satisfied by at least one neighbor</param>
      /// <returns></returns>
      public bool Satisfies(DescriptorCriteria criteriaForOneNeighbor, DescriptorCriteria criteriaForTheOtherNeighbor)
      {
         return (criteriaForOneNeighbor.IsSatisfiedBy(FirstNeighbor) &&
                 criteriaForTheOtherNeighbor.IsSatisfiedBy(SecondNeighbor)) ||
                (criteriaForOneNeighbor.IsSatisfiedBy(SecondNeighbor) &&
                 criteriaForTheOtherNeighbor.IsSatisfiedBy(FirstNeighbor));
      }

      public bool StrictlySatisfies(DescriptorCriteria criteriaFoFirstNeighbor, DescriptorCriteria criteriaForTheSecondNeighbor)
      {
         return criteriaFoFirstNeighbor.IsSatisfiedBy(FirstNeighbor) &&
                criteriaForTheSecondNeighbor.IsSatisfiedBy(SecondNeighbor);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcNeighborhood = source as Neighborhood;
         if (srcNeighborhood == null) return;

         //First/Second neighbor should NOT be cloned
         //Instead, some Model-Finalizer must be called
      }

      public bool IsDefined => FirstNeighbor != null && SecondNeighbor != null;
   }
}