using System.Collections;
using System.Collections.Generic;
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

   public abstract class StartValueBuildingBlock<T> : BuildingBlock, IStartValuesBuildingBlock<T> where T : class, IStartValue
   {
      private readonly ICache<IObjectPath, T> _allStartValues;

      public string SpatialStructureId { get; set; }
      public string MoleculeBuildingBlockId { get; set; }

      public bool Uses(IBuildingBlock buildingBlock)
      {
         return (refersTo(buildingBlock, SpatialStructureId))
                || (refersTo(buildingBlock, MoleculeBuildingBlockId));
      }

      public void Remove(IObjectPath objectPath)
      {
         Remove(this[objectPath]);
      }

      private bool refersTo(IBuildingBlock buildingBlock, string idToCheck)
      {
         return string.Equals(idToCheck, buildingBlock.Id);
      }

      protected StartValueBuildingBlock()
      {
         _allStartValues = new Cache<IObjectPath, T>(x => x.Path, x => null);
      }

      public T this[IObjectPath objectPath]
      {
         get { return _allStartValues[objectPath]; }
         set { _allStartValues[objectPath] = value; }
      }

      public void Add(T startValue)
      {
         _allStartValues.Add(startValue);
      }

      public void Remove(T startValue)
      {
         if (startValue == null) return;
         _allStartValues.Remove(startValue.Path);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceStartValeBuidlingBlock = source as IStartValuesBuildingBlock<T>;
         if (sourceStartValeBuidlingBlock == null) return;

         sourceStartValeBuidlingBlock.Each(startValue => Add(cloneManager.Clone(startValue)));
         MoleculeBuildingBlockId = sourceStartValeBuidlingBlock.MoleculeBuildingBlockId;
         SpatialStructureId = sourceStartValeBuidlingBlock.SpatialStructureId;
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _allStartValues.Each(msv => msv.AcceptVisitor(visitor));
      }

      public IEnumerator<T> GetEnumerator()
      {
         return _allStartValues.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }
}