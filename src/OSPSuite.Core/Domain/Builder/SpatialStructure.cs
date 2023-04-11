using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain.Builder
{
   public interface ISpatialStructure : IBuildingBlock, IEnumerable<IContainer>
   {
      /// <summary>
      ///    Container-structure with subcontainers and parameters (e.g. one container but for special modeling situation like
      ///    mother/featus model, 2 root containers would be used)
      /// </summary>
      IReadOnlyList<IContainer> TopContainers { get; }

      /// <summary>
      ///    Add container in the top hierarchy level of the spatial structure
      /// </summary>
      void AddTopContainer(IContainer container);

      /// <summary>
      ///    Remove container from the top hierarchy level of the spatial structure
      /// </summary>
      void RemoveTopContainer(IContainer container);

      /// <summary>
      ///    All neighborhoods defined for the spatial structure
      /// </summary>
      IReadOnlyList<NeighborhoodBuilder> Neighborhoods { get; }

      /// <summary>
      ///    Container which contains all NeighborhoodBuilder
      /// </summary>
      IContainer NeighborhoodsContainer { get; set; }

      /// <summary>
      ///    Add neighborhood to spatial structure
      /// </summary>
      void AddNeighborhood(NeighborhoodBuilder neighborhoodBuilder);

      void RemoveNeighborhood(NeighborhoodBuilder neighborhoodBuilder);

      /// <summary>
      ///    Molecule-dependent properties, which must be defined only
      ///    <para></para>
      ///    once for the whole spatial structure (neighborhood-independent)
      ///    <para></para>
      ///    Examples: K_rbc (part. coeff. plasma to red blood cells), B2P, ...
      /// </summary>
      IContainer GlobalMoleculeDependentProperties { get; set; }

      /// <summary>
      ///    Returns all physical containers starting at "Root"
      /// </summary>
      IEnumerable<IContainer> PhysicalContainers { get; }

      IReadOnlyList<NeighborhoodBuilder> AllNeighborhoodBuildersConnectedWith(ObjectPath containerPath);

      /// <summary>
      ///    Ensures that all references to containers are resolved.
      /// </summary>
      void ResolveReferencesInNeighborhoods();
   }

   public class SpatialStructure : BuildingBlock, ISpatialStructure
   {
      private readonly List<IContainer> _allTopContainers;
      public IContainer GlobalMoleculeDependentProperties { get; set; }
      public IContainer NeighborhoodsContainer { get; set; }

      public SpatialStructure()
      {
         Icon = IconNames.SPATIAL_STRUCTURE;
         _allTopContainers = new List<IContainer>();
      }

      public IReadOnlyList<IContainer> TopContainers => _allTopContainers;


      public void Add(IContainer topContainer) => AddTopContainer(topContainer);

      public void Remove(IContainer topContainer) => RemoveTopContainer(topContainer);

      public void AddTopContainer(IContainer container)
      {
         // Ensure that parent Container is null , may occur when adding already existing Container as Top Container (Load/Copy&Paste)
         container.ParentContainer = null;
         _allTopContainers.Add(container);
      }

      public void RemoveTopContainer(IContainer container)
      {
         _allTopContainers.Remove(container);
      }

      public IReadOnlyList<NeighborhoodBuilder> Neighborhoods
      {
         get
         {
            if (NeighborhoodsContainer == null)
               return Array.Empty<NeighborhoodBuilder>();

            return NeighborhoodsContainer.GetChildren<NeighborhoodBuilder>().ToList();
         }
      }

      public void AddNeighborhood(NeighborhoodBuilder neighborhoodBuilder) => NeighborhoodsContainer.Add(neighborhoodBuilder);

      public void RemoveNeighborhood(NeighborhoodBuilder neighborhoodBuilder) => NeighborhoodsContainer.RemoveChild(neighborhoodBuilder);

      public IEnumerable<IContainer> PhysicalContainers
      {
         get
         {
            var allPhysicals = from root in _allTopContainers
               from physicalContainer in root.GetAllChildren<IContainer>(c => c.Mode == ContainerMode.Physical)
               select physicalContainer;

            foreach (var container in allPhysicals)
               yield return container;

            foreach (var rootContainer in _allTopContainers.Where(x => x.Mode == ContainerMode.Physical))
               yield return rootContainer;
         }
      }

      public IReadOnlyList<NeighborhoodBuilder> AllNeighborhoodBuildersConnectedWith(ObjectPath containerPath) =>
         Neighborhoods.Where(x => x.IsConnectedTo(containerPath)).ToList();

      public void ResolveReferencesInNeighborhoods()
      {
         Neighborhoods.Each(x => x.ResolveReference(this));
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceSpatialStructure = source as ISpatialStructure;
         if (sourceSpatialStructure == null) return;

         sourceSpatialStructure.TopContainers.Each(c => AddTopContainer(cloneManager.Clone(c)));
         NeighborhoodsContainer = cloneManager.Clone(sourceSpatialStructure.NeighborhoodsContainer);
         GlobalMoleculeDependentProperties = cloneManager.Clone(sourceSpatialStructure.GlobalMoleculeDependentProperties);

         //once all neighborhood have been updated, we can safely update the references
         ResolveReferencesInNeighborhoods();
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         GlobalMoleculeDependentProperties?.AcceptVisitor(visitor);
         TopContainers.Each(x => x.AcceptVisitor(visitor));
         NeighborhoodsContainer?.AcceptVisitor(visitor);
      }

      public IEnumerator<IContainer> GetEnumerator()
      {
         foreach (var topContainer in TopContainers)
         {
            yield return topContainer;
         }

         if (GlobalMoleculeDependentProperties != null)
            yield return GlobalMoleculeDependentProperties;

         if (NeighborhoodsContainer != null)
            yield return NeighborhoodsContainer;
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

    
   }
}