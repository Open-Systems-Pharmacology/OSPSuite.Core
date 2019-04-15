using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public static class ContainerExtensions
   {
      public static IParameter Parameter(this IContainer container, string parameterName)
      {
         if (container == null)
            return null;

         return container.GetSingleChildByName<IParameter>(parameterName);
      }

      public static IEnumerable<IParameter> AllParameters(this IContainer container)
      {
         return container.AllParameters(x => true);
      }

      public static IEnumerable<IParameter> AllParameters(this IContainer container, Func<IParameter, bool> predicate)
      {
         if (container == null)
            return Enumerable.Empty<IParameter>();

         return container.GetChildren<IParameter>().Where(predicate);
      }

      /// <summary>
      ///    Set the mode of the container
      /// </summary>
      /// <param name="container">Container for which the mode should be set</param>
      /// <param name="containerMode">Mode to set</param>
      public static TContainer WithMode<TContainer>(this TContainer container, ContainerMode containerMode) where TContainer : IContainer
      {
         container.Mode = containerMode;
         return container;
      }

      /// <summary>
      ///    Set the ContainerType of the container
      /// </summary>
      /// <param name="container">Container for which the ContainerType should be set</param>
      /// <param name="containerType">ContainerType to set</param>
      public static TContainer WithContainerType<TContainer>(this TContainer container, ContainerType containerType) where TContainer : IContainer
      {
         container.ContainerType = containerType;
         return container;
      }

      /// <summary>
      ///    Return all the  neighborhoods defined in the given neighborhood container for which the
      ///    <paramref
      ///       name="criteriaForFirstContainer" />
      ///    and <paramref name="criteriaForSecondContainer" /> are strictly met
      /// </summary>
      /// <param name="neighborhood">parent neighborhood container</param>
      /// <param name="criteriaForFirstContainer">criteria that should be met by the first container of the neighborhood</param>
      /// <param name="criteriaForSecondContainer">criteria that should be met by the second container of the neighborhood</param>
      internal static IEnumerable<INeighborhood> AllNeighboorhoodsFor(this IContainer neighborhood, DescriptorCriteria criteriaForFirstContainer, DescriptorCriteria criteriaForSecondContainer)
      {
         return neighborhood.GetChildren<INeighborhood>(n => n.StrictlySatisfies(criteriaForFirstContainer, criteriaForSecondContainer));
      }

      /// <summary>
      ///    Returns <c>true</c> if the container has a child with the given name otherwise, <c>false</c>
      /// </summary>
      public static bool ContainsName(this IContainer container, string name)
      {
         return container.GetSingleChildByName(name) != null;
      }

      /// <summary>
      ///    Determines whether the Container contains all specified names.
      /// </summary>
      /// <returns>
      ///    <c>true</c> if the specified child names contains names; otherwise, <c>false</c>
      /// </returns>
      public static bool ContainsNames(this IContainer container, IEnumerable<string> childrenNames)
      {
         var allNames = container.Children.Select(x => x.Name).ToList();
         return childrenNames.All(allNames.Contains);
      }

      /// <summary>
      ///    Returns the child with the given name if it exists otherwise null
      /// </summary>
      public static IEntity GetSingleChildByName(this IContainer container, string name)
      {
         return container.Children.FirstOrDefault(item => string.Equals(item.Name, name));
      }

      /// <summary>
      ///    Returns the child of type <typeparamref name="T" /> with the given name if it exists otherwise null
      /// </summary>
      public static T GetSingleChildByName<T>(this IContainer container, string name) where T : class, IEntity
      {
         return container.GetSingleChildByName(name) as T;
      }

      public static T GetSingleChild<T>(this IContainer container, Func<T, bool> predicate) where T : class, IEntity
      {
         return container.GetChildren(predicate).SingleOrDefault();
      }

      public static IContainer Container(this IContainer container, string containerName)
      {
         return container?.GetSingleChildByName<IContainer>(containerName);
      }

      /// <summary>
      ///    Retrieves an entity of type T define with the given path relative to the container.
      ///    Returns null if the entity is not found at this location
      ///    <example>
      ///       cont.EntityAt{IObserver}("liver", "cell", "peripheral venous blood") will return the observer
      ///       peripheral venous blood if defined at the given location, null otherwise
      ///    </example>
      /// </summary>
      /// <returns></returns>
      public static T EntityAt<T>(this IContainer container, params string[] path) where T : class, IEntity
      {
         if (!path.Any())
            return null;

         var current = container;
         //-1 since last entry is the actual entity to return
         for (int i = 0; i < path.Length - 1; i++)
         {
            current = current.Container(path[i]);
         }

         return current?.GetSingleChildByName<T>(path.Last());
      }

      /// <summary>
      ///    Returns the name of all direct children defined in the container. If the container is null, an empty enumeration is
      ///    returned
      /// </summary>
      public static IReadOnlyCollection<string> AllChildrenNames(this IContainer container)
      {
         if (container == null)
            return new List<string>();

         return container.Children.Select(param => param.Name).ToList();
      }

      /// <summary>
      ///    Adds each entity in the <paramref name="children" /> using the AddChild method
      /// </summary>
      public static void AddChildren(this IContainer container, params IEntity[] children)
      {
         AddChildren(container, children.AsEnumerable());
      }

      public static IContainer WithChildren(this IContainer container, IEnumerable<IEntity> children)
      {
         container.AddChildren(children);
         return container;
      }

      public static IContainer WithChild(this IContainer container, IEntity child)
      {
         container.Add(child);
         return container;
      }

      /// <summary>
      ///    Adds each entity in the <paramref name="children" /> using the AddChild method
      /// </summary>
      public static void AddChildren(this IContainer container, IEnumerable<IEntity> children)
      {
         children.Each(container.Add);
      }

      public static TChild GetSingleChild<TChild>(this IContainer container) where TChild : class, IEntity
      {
         if (container == null)
            return null;

         return container.GetSingleChild<TChild>(x => true);
      }
   }
}