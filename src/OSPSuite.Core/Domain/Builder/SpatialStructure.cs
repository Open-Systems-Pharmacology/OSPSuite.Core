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
   public class SpatialStructure : BuildingBlock, IEnumerable<IContainer>
   {
      private readonly List<IContainer> _allTopContainers;

      /// <summary>
      ///    Molecule-dependent properties, which must be defined only
      ///    <para></para>
      ///    once for the whole spatial structure (neighborhood-independent)
      ///    <para></para>
      ///    Examples: K_rbc (part. coeff. plasma to red blood cells), B2P, ...
      /// </summary>
      public IContainer GlobalMoleculeDependentProperties { get; set; }

      /// <summary>
      ///    Container which contains all NeighborhoodBuilder
      /// </summary>
      public IContainer NeighborhoodsContainer { get; set; }

      public SpatialStructure()
      {
         Icon = IconNames.SPATIAL_STRUCTURE;
         _allTopContainers = new List<IContainer>();
      }

      /// <summary>
      ///    Container-structure with sub-containers and parameters (e.g. one container but for special modeling situation like
      ///    mother/fetus model, 2 root containers would be used)
      /// </summary>
      public IReadOnlyList<IContainer> TopContainers => _allTopContainers;

      public void Add(IContainer topContainer) => AddTopContainer(topContainer);

      public void Remove(IContainer topContainer) => RemoveTopContainer(topContainer);

      /// <summary>
      ///    Add container in the top hierarchy level of the spatial structure
      /// </summary>
      public void AddTopContainer(IContainer container)
      {
         // Ensure that parent Container is null , may occur when adding already existing Container as Top Container (Load/Copy&Paste)
         container.ParentContainer = null;
         _allTopContainers.Add(container);
      }

      /// <summary>
      ///    Remove container from the top hierarchy level of the spatial structure
      /// </summary>
      public void RemoveTopContainer(IContainer container)
      {
         _allTopContainers.Remove(container);
      }

      /// <summary>
      ///    All neighborhoods defined for the spatial structure
      /// </summary>
      public IReadOnlyList<NeighborhoodBuilder> Neighborhoods
      {
         get
         {
            if (NeighborhoodsContainer == null)
               return Array.Empty<NeighborhoodBuilder>();

            return NeighborhoodsContainer.GetChildren<NeighborhoodBuilder>().ToList();
         }
      }

      /// <summary>
      ///    Add neighborhood to spatial structure
      /// </summary>
      public void AddNeighborhood(NeighborhoodBuilder neighborhoodBuilder) => NeighborhoodsContainer.Add(neighborhoodBuilder);

      public void RemoveNeighborhood(NeighborhoodBuilder neighborhoodBuilder) => NeighborhoodsContainer.RemoveChild(neighborhoodBuilder);

      /// <summary>
      ///    Returns all physical containers starting at "Root"
      /// </summary>

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

      /// <summary>
      ///    Ensures that all references to containers are resolved.
      /// </summary>
      public void ResolveReferencesInNeighborhoods()
      {
         Neighborhoods.Each(x => x.ResolveReference(this));
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceSpatialStructure = source as SpatialStructure;
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