using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Descriptors;

public class CompoundCondition : DescriptorCriteria, ITagCondition
{
   //compound conditions have no tag of their own; expose empty so RemoveByTag never matches
   public string Tag => string.Empty;

   public string Condition => $"({base.ToString()})";

   public ITagCondition CloneCondition()
   {
      return cloneCondition();
   }

   private CompoundCondition cloneCondition()
   {
      var clone = new CompoundCondition { Operator = Operator };
      this.Each(c => clone.Add(c.CloneCondition()));
      return clone;
   }

   public override DescriptorCriteria Clone() => cloneCondition();

   public override string ToString() => Condition;
}