namespace OSPSuite.Core.Domain
{
   public class CoreOriginData : BaseOriginData
   {
      public string Species { set; get; }

      public CoreOriginData Clone()
      {
         var coreOriginData = new CoreOriginData();
         base.UpdateProperties(coreOriginData);
         coreOriginData.Species = Species;
         return coreOriginData;
      }
   }
}