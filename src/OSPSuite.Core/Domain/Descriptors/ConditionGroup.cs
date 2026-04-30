using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Descriptors;

public class ConditionGroup : DescriptorCriteria, ITagCondition
{
   //condition groups have no tag of their own; expose empty so RemoveByTag never matches
   public string Tag => string.Empty;

   public string Condition => $"({base.ToString()})";

   public ITagCondition CloneCondition()
   {
      return cloneCondition();
   }

   private ConditionGroup cloneCondition()
   {
      var clone = new ConditionGroup { Operator = Operator };
      this.Each(c => clone.Add(c.CloneCondition()));
      return clone;
   }

   public override DescriptorCriteria Clone() => cloneCondition();

   public override string ToString() => Condition;
}
