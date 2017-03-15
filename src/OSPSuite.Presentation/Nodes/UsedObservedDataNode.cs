using OSPSuite.Utility;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Presentation.Nodes
{
   public class UsedObservedDataNode : AbstractNode<UsedObservedData>
   {
      private readonly DataRepository _observedData;
      private readonly string _id;

      public UsedObservedDataNode(UsedObservedData usedObservedData, DataRepository observedData)
         : base(usedObservedData)
      {
         _observedData = observedData;
         Tag = usedObservedData;
         UpdateText();
         //new id should not be the one from the data repository=>hence a new id
         _id = ShortGuid.NewGuid();
      }

      public override string Id
      {
         get { return _id; }
      }

      protected override void UpdateText()
      {
         Text = _observedData.Name;
      }
   }
}