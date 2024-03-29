﻿using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IPathToPathElementsMapper : IMapper<IEntity, PathElements>
   {
      /// <summary>
      ///    Returns values for the fixed <see cref="PathElementId" /> to display the variable length path starting from
      ///    container
      /// </summary>
      /// <param name="rootContainer">root container for path </param>
      /// <param name="entityPath">variable length path (may begin with root container name)</param>
      /// <returns>values for the fixed  <see cref="PathElementId" /></returns>
      PathElements MapFrom(IContainer rootContainer, IReadOnlyList<string> entityPath);
   }

   public class PathToPathElementsMapper : IPathToPathElementsMapper
   {
      private readonly IEntityPathResolver _entityPathResolver;

      public PathToPathElementsMapper(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public PathElements MapFrom(IContainer rootContainer, IReadOnlyList<string> entityPath)
      {
         var containers = getContainers(rootContainer, entityPath);
         return CreatePathElementsFrom(containers, entityPath.LastOrDefault());
      }

      /// <summary>
      ///    returns list of container objects
      ///    at call from OutputSelection: root container is Simulation, path begins with Organism
      ///    at call from DataBrowser:  root container is Simulation, path begins with Simulation
      /// </summary>
      /// <param name="rootContainer"> root container </param>
      /// <param name="pathElements"> list of path elements  (may begin with root container name)</param>
      /// <returns> list of container objects </returns>
      private IReadOnlyList<IContainer> getContainers(IContainer rootContainer, IReadOnlyList<string> pathElements)
      {
         var containers = new List<IContainer>();
         if (rootContainer == null)
            return containers;

         if (pathElements.Count == 0)
         {
            containers.Add(rootContainer);
            return containers;
         }

         int firstPathElementToProcess = 0; //start index in for loop = first index of child of topContainer in pathElements
         int lastPathElementToProcess = pathElements.Count - 1;

         // include root container, if in Path
         if (string.Equals(rootContainer.Name, pathElements[0]))
         {
            containers.Add(rootContainer);
            firstPathElementToProcess = 1; // first pathElementId already processed
         }

         IContainer container = rootContainer;

         for (int i = firstPathElementToProcess; i < pathElements.Count; i++)
         {
            container = container.Container(pathElements[i]);

            if (container == null)
               continue;

            //Parameter and last path element. do not process the parameter (Distrubuted Parameter)
            if (container.IsAnImplementationOf<IParameter>() && i == lastPathElementToProcess)
               continue;

            //this is a standard container
            containers.Add(container);
         }

         return containers;
      }

      /// <summary>
      ///    Returns the <see cref="PathElements" /> corresponding to the container hiearchy represented by the
      ///    <paramref name="containers" />.
      /// </summary>
      /// <param name="containers">Containers hiearchy</param>
      /// <param name="name">name of the entity for which the <see cref="PathElements" /> should be created</param>
      protected virtual PathElements CreatePathElementsFrom(IReadOnlyList<IContainer> containers, string name)
      {
         var pathElements = new PathElements();
         var containerList = containers.ToList();

         if (!string.IsNullOrEmpty(name))
            pathElements[PathElementId.Name] = CreatePathElement(name);

         if (!containerList.Any())
            return pathElements;

         // add Simulation entry
         if (isRootContainer(containerList[0]))
         {
            var rootPathElement = CreatePathElement(containerList[0]);
            pathElements[PathElementId.Simulation] = rootPathElement;
            if (string.IsNullOrEmpty(rootPathElement.IconName))
               rootPathElement.IconName = IconNames.SIMULATION;

            containerList.RemoveAt(0);
         }

         // add Molecule entry first as top container might also be molecule container
         var moleculeContainer = containerList.LastOrDefault(item => item.ContainerType == ContainerType.Molecule);
         if (moleculeContainer != null)
         {
            pathElements[PathElementId.Molecule] = CreatePathElement(moleculeContainer);
            if (containerList.Last() == moleculeContainer)
               containerList.Remove(moleculeContainer);

            // same name in molecule and in name, this is the amount for the molecule
            if (string.Equals(pathElements[PathElementId.Molecule].DisplayName, name))
               pathElements[PathElementId.Name].DisplayName = Captions.AmountInContainer;
         }

         // add TopContainer entry
         if (containerList.Any())
         {
            pathElements[PathElementId.TopContainer] = CreatePathElement(containerList[0]);
            containerList.RemoveAt(0);
         }

         // add BottomCompartment entry
         // Problem: restrict to ContainerType == Compartment, to avoid Organ in BottomCompartment column e.g. for Organ Observers/Parameters
         var compartmentContainer = containerList.LastOrDefault(item => item.ContainerType == ContainerType.Compartment);
         if (compartmentContainer != null)
         {
            pathElements[PathElementId.BottomCompartment] = CreatePathElement(compartmentContainer);
            if (containerList.Last() == compartmentContainer)
               containerList.Remove(compartmentContainer);
         }

         if (containerList.Any())
            pathElements[PathElementId.Container] = CreateContainerPath(containerList);

         return pathElements;
      }

      protected virtual PathElement CreateContainerPath(IReadOnlyList<IContainer> containerList)
      {
         var containerNames = containerList.Select(DisplayNameFor).ToString(Constants.DISPLAY_PATH_SEPARATOR);
         return CreatePathElement(containerNames);
      }

      protected virtual string DisplayNameFor(IContainer container)
      {
         return container.Name;
      }

      private bool isRootContainer(IContainer container)
      {
         return container.IsAnImplementationOf<IRootContainer>() ||
                container.ContainerType == ContainerType.Simulation;
      }

      public virtual PathElements MapFrom(IEntity entity)
      {
         return entity == null
            ? new PathElements()
            : MapFrom(entity.RootContainer, _entityPathResolver.ObjectPathFor(entity, addSimulationName: true).ToList());
      }

      protected virtual PathElement CreatePathElement(IContainer container)
      {
         return CreatePathElement(DisplayNameFor(container), container?.Icon);
      }

      protected virtual PathElement CreatePathElement(string pathElementValue, string iconName = null)
      {
         var iconNameToUse = string.IsNullOrEmpty(iconName) ? pathElementValue.Replace(" ", string.Empty) : iconName;
         return new PathElement
         {
            DisplayName = pathElementValue,
            IconName = iconNameToUse
         };
      }
   }
}