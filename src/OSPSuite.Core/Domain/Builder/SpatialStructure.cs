using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public interface ISpatialStructure : IBuildingBlock<IContainer>
   {
      /// <summary>
      ///    Container-structure with subcontainers and parameters (e.g. one container but for special modelling situation like
      ///    mother/featus model, 2 root containers would be used)
      /// </summary>
      IEnumerable<IContainer> TopContainers { get; }

      /// <summary>
      ///    Add container in the top hiearchy level of the spatial structure
      /// </summary>
      void AddTopContainer(IContainer container);

      /// <summary>
      ///    Remove container from the top hiearchy level of the spatial structure
      /// </summary>
      void RemoveTopContainer(IContainer container);

      /// <summary>
      ///    All neighborhoods defined for the spatial structure
      /// </summary>
      IEnumerable<INeighborhoodBuilder> Neighborhoods { get; }

      /// <summary>
      ///    Container which contains all NeighborhoodBuilder
      /// </summary>
      IContainer NeighborhoodsContainer { get; set; }

      /// <summary>
      ///    Add neighborhood to spatial structure
      /// </summary>
      void AddNeighborhood(INeighborhoodBuilder neighborhoodBuilder);

      void RemoveNeighborhood(INeighborhoodBuilder neighborhoodBuilder);

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
   }

   public class SpatialStructure : BuildingBlock, ISpatialStructure
   {
      private readonly IList<IContainer> _allTopContainers;
      public IContainer GlobalMoleculeDependentProperties { get; set; }
      public IContainer NeighborhoodsContainer { get; set; }

      public SpatialStructure()
      {
         Icon = IconNames.SPATIAL_STRUCTURE;
         _allTopContainers = new List<IContainer>();
      }

      public IEnumerable<IContainer> TopContainers
      {
         get { return _allTopContainers; }
      }

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

      public IEnumerable<INeighborhoodBuilder> Neighborhoods
      {
         get
         {
            if (NeighborhoodsContainer == null)
               return Enumerable.Empty<INeighborhoodBuilder>();

            return NeighborhoodsContainer.GetChildren<INeighborhoodBuilder>();
         }
      }

      public void AddNeighborhood(INeighborhoodBuilder neighborhoodBuilder)
      {
         NeighborhoodsContainer.Add(neighborhoodBuilder);
      }

      public void RemoveNeighborhood(INeighborhoodBuilder neighborhoodBuilder)
      {
         NeighborhoodsContainer.RemoveChild(neighborhoodBuilder);
      }

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

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var sourceSpatialStructure = source as ISpatialStructure;
         if (sourceSpatialStructure == null) return;

         sourceSpatialStructure.TopContainers.Each(c => AddTopContainer(cloneManager.Clone(c)));
         NeighborhoodsContainer = cloneManager.Clone(sourceSpatialStructure.NeighborhoodsContainer);
         foreach (var neighborhoodBuilder in Neighborhoods)
         {
            var sourceNeighborhood = neighborhoodBuilderSourceFor(neighborhoodBuilder, sourceSpatialStructure.Neighborhoods);
            updateNeighborsReferences(sourceNeighborhood, neighborhoodBuilder);
         }

         GlobalMoleculeDependentProperties = cloneManager.Clone(sourceSpatialStructure.GlobalMoleculeDependentProperties);
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         if (GlobalMoleculeDependentProperties != null)
            GlobalMoleculeDependentProperties.AcceptVisitor(visitor);

         TopContainers.Each(container => container.AcceptVisitor(visitor));

         if (NeighborhoodsContainer != null)
            NeighborhoodsContainer.AcceptVisitor(visitor);
      }

      /// <summary>
      ///    Copy the references for first and second Neighbor from <paramref name="sourcecNeighborhoodBuilder" /> to
      ///    <paramref name="targetNeighborhoodBuilder" />.
      /// </summary>
      /// <param name="sourcecNeighborhoodBuilder">The source neighborhood builder to copy neighbors from.</param>
      /// <param name="targetNeighborhoodBuilder">The neighborhood builder to copy neighbors to.</param>
      private void updateNeighborsReferences(INeighborhoodBuilder sourcecNeighborhoodBuilder, INeighborhoodBuilder targetNeighborhoodBuilder)
      {
         var objectPathFactory = new ObjectPathFactory(new AliasCreator());
         var firstPath = objectPathFactory.CreateAbsoluteObjectPath(sourcecNeighborhoodBuilder.FirstNeighbor);
         var secondPath = objectPathFactory.CreateAbsoluteObjectPath(sourcecNeighborhoodBuilder.SecondNeighbor);

         foreach (var topContainer in TopContainers)
         {
            var firstNeighbor = firstPath.Resolve<IContainer>(topContainer);
            var secondNeighbor = secondPath.Resolve<IContainer>(topContainer);

            if (firstNeighbor != null)
               targetNeighborhoodBuilder.FirstNeighbor = firstNeighbor;

            if (secondNeighbor != null)
               targetNeighborhoodBuilder.SecondNeighbor = secondNeighbor;

            //both neighbors set. early exit
            if (targetNeighborhoodBuilder.FirstNeighbor != null && targetNeighborhoodBuilder.SecondNeighbor != null)
               return;
         }
      }

      private INeighborhoodBuilder neighborhoodBuilderSourceFor(INeighborhoodBuilder neighborhoodBuilder, IEnumerable<INeighborhoodBuilder> sourceNeighborhoods)
      {
         return sourceNeighborhoods.FindByName(neighborhoodBuilder.Name);
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

      public void Add(IContainer container)
      {
         _allTopContainers.Add(container);
      }

      public void Remove(IContainer container)
      {
         _allTopContainers.Remove(container);
      }
   }
}