using OSPSuite.Assets;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Domain
{
   public interface IModelCoreSimulation : IObjectBase, IWithCreationMetaData, IWithModel
   {
      IBuildConfiguration BuildConfiguration { get; }
      DataRepository Results { get; set; }
   }

   public class ModelCoreSimulation : ObjectBase, IModelCoreSimulation
   {
      public IModel Model { get; set; }
      public IBuildConfiguration BuildConfiguration { get; set; }
      public virtual DataRepository Results { get; set; }
      public CreationMetaData Creation { get; set; }

      public ModelCoreSimulation()
      {
         Creation = new CreationMetaData();
         Icon = IconNames.SIMULATION;
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);

         if (Model != null)
            Model.AcceptVisitor(visitor);

         if (BuildConfiguration != null)
            BuildConfiguration.AcceptVisitor(visitor);

         if (!Results.IsNull())
            Results.AcceptVisitor(visitor);
      }
   }
}