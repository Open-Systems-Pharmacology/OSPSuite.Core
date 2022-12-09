using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IStartValuesBuildingBlock<T> : IBuildingBlock<T> where T : class, IStartValue
   {
      /// <summary>
      ///    Returns the start value registered for the given path. If not found, null is returned
      /// </summary>
      /// <param name="objectPath"></param>
      /// <returns></returns>
      T this[IObjectPath objectPath] { get; set; }

      /// <summary>
      ///    Id of the spatial structure used to create the parameter start values.
      /// </summary>
      string SpatialStructureId { get; set; }

      /// <summary>
      ///    Id of the molecule building block used to create the parameter start values
      /// </summary>
      string MoleculeBuildingBlockId { get; set; }

      /// <summary>
      ///    Checks whether the given building block is used as a reference
      /// </summary>
      /// <param name="buildingBlock">The building block being checked for reference</param>
      /// <returns>true if the building block is referred to by the start value building block</returns>
      bool Uses(IBuildingBlock buildingBlock);

      /// <summary>
      ///    Removes the start value with path <paramref name="objectPath" /> if available. Does nothing otherwise
      /// </summary>
      void Remove(IObjectPath objectPath);
   }

   public abstract class StartValueBuildingBlock<T> : PathAndValueEntityBuildingBlock<T>, IStartValuesBuildingBlock<T> where T : class, IStartValue
   {
      public string SpatialStructureId { get; set; }
      public string MoleculeBuildingBlockId { get; set; }

      public bool Uses(IBuildingBlock buildingBlock)
      {
         return refersTo(buildingBlock, SpatialStructureId)
                || refersTo(buildingBlock, MoleculeBuildingBlockId);
      }

      private bool refersTo(IBuildingBlock buildingBlock, string idToCheck)
      {
         return string.Equals(idToCheck, buildingBlock.Id);
      }

      protected StartValueBuildingBlock()
      {
         _allValues = new Cache<IObjectPath, T>(x => x.Path, x => null);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceStartValeBuidlingBlock = source as IStartValuesBuildingBlock<T>;
         if (sourceStartValeBuidlingBlock == null) return;

         MoleculeBuildingBlockId = sourceStartValeBuidlingBlock.MoleculeBuildingBlockId;
         SpatialStructureId = sourceStartValeBuidlingBlock.SpatialStructureId;
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _allValues.Each(msv => msv.AcceptVisitor(visitor));
      }
   }
}