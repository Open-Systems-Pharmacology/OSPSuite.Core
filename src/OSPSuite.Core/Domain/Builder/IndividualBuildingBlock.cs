using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class IndividualBuildingBlock : PathAndValueEntityBuildingBlockSourcedFromPKSimBuildingBlock<IndividualParameter>
   {
      public CoreOriginData OriginData { get; set; }

      public string Species => OriginData.Species;

      public double Age => OriginData.Age?.Value ?? 0;

      public double Weight => OriginData.Weight.Value;

      public double Height => OriginData.Height?.Value ?? 0;

      public string Comment => OriginData.Comment;

      public ValueOrigin ValueOrigin => OriginData.ValueOrigin;

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceIndividual = source as IndividualBuildingBlock;
         if (sourceIndividual == null) 
            return;

         OriginData = sourceIndividual.OriginData.Clone();
      }
   }
}