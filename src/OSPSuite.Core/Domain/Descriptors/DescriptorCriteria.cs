using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Descriptors
{
   public enum CriteriaOperator
   {
      And,
      Or
   }

   public class DescriptorCriteria : List<ITagCondition>, ISpecification<IEntity>, ISpecification<EntityDescriptor>
   {
      //Default behavior
      public CriteriaOperator Operator { get; set; } = CriteriaOperator.And;

      public override string ToString()
      {
         var operation = Operator == CriteriaOperator.And ? Constants.AND : Constants.OR;
         return this.ToString(operation.ToUpper(), " ").Trim(' ');
      }

      public bool IsSatisfiedBy(EntityDescriptor entityDescriptor)
      {
         //Empty criteria does not fulfill 
         if (!this.Any())
            return false;

         if (Operator == CriteriaOperator.And)
            return this.All(x => x.IsSatisfiedBy(entityDescriptor));

         return this.Any(x => x.IsSatisfiedBy(entityDescriptor));
      }

      public virtual bool IsSatisfiedBy(IEntity entity)
      {
         return IsSatisfiedBy(new EntityDescriptor(entity));
      }

      public bool Equals(DescriptorCriteria other)
      {
         if (other == null) return false;
         if (Count != other.Count())
            return false;

         if (Operator != other.Operator)
            return false;

         for (int i = 0; i < Count; i++)
            if (!this[i].Equals(other.ElementAt(i)))
               return false;

         return true;
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         return Equals(obj as DescriptorCriteria);
      }

      /// <summary>
      ///    Removes all tag conditions for the given <paramref name="tag" />
      /// </summary>
      /// <typeparam name="T">
      ///    This allows to filter the type being removed. To remove all conditions for the given type, use T =
      ///    ITagCondition
      /// </typeparam>
      /// <param name="tag">Tag to remove</param>
      public void RemoveByTag<T>(string tag) where T : class, ITagCondition
      {
         var conditionsToRemove = (from conditions in this
               let tagConditions = conditions as T
               where tagConditions != null
               where tagConditions.Tag.Equals(tag)
               select conditions)
            .ToList();

         conditionsToRemove.Each(condition => Remove(condition));
      }

      public DescriptorCriteria Clone()
      {
         var clone = new DescriptorCriteria();
         this.Each(x => clone.Add(x.CloneCondition()));
         clone.Operator = Operator;
         return clone;
      }

      public override int GetHashCode()
      {
         //http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
         unchecked // Overflow is fine, just wrap
         {
            int hash = 17;
            //ok to use operator here
            hash = (hash * 23) + Operator.GetHashCode();
            foreach (var descriptor in this)
            {
               hash = (hash * 23) + descriptor.GetHashCode();
            }

            return hash;
         }
      }
   }
}