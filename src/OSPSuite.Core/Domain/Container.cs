using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public interface IContainer : IEntity, IEnumerable<IEntity>
   {
      ContainerType ContainerType { set; get; }
      ContainerMode Mode { get; set; }

      /// <summary>
      ///    Returns the children of the Container
      /// </summary>
      IEnumerable<IEntity> Children { get; }


      /// <summary>
      ///    Add the given child to the container
      /// </summary>
      /// <exception cref="CircularReferenceException">is thrown if the container is a descendant of the child</exception>
      /// <exception cref="NotUniqueNameException">is thrown if a child already exists with the given name in the container</exception>
      /// <param name="newChild"> Child to add </param>
      void Add(IEntity newChild);

      /// <summary>
      ///    Remove the entity from the children of the container
      /// </summary>
      void RemoveChild(IEntity childToRemove);

      /// <summary>
      ///    Remove children (only direct children will be removed, this will not be done recursively)
      /// </summary>
      void RemoveChildren();

      /// <summary>
      ///    Returns all children recursively of the container of type <typeparamref name="T" />
      /// </summary>
      IReadOnlyList<T> GetAllChildren<T>() where T : class, IEntity;

      /// <summary>
      ///    Returns all children recursively of the container satisfying the predicate and of type <typeparamref name="T" />
      /// </summary>
      IReadOnlyList<T> GetAllChildren<T>(Func<T, bool> predicate) where T : class, IEntity;

      /// <summary>
      ///    Returns all children of type <typeparamref name="T" />
      /// </summary>
      IEnumerable<T> GetChildren<T>() where T : class, IEntity;

      /// <summary>
      ///    Returns all children of type <typeparamref name="T" /> satisfying the given predicate
      /// </summary>
      IEnumerable<T> GetChildren<T>(Func<T, bool> predicate) where T : class, IEntity;

      /// <summary>
      ///    returns the neighbors from the container connected by the given neigborhoods.
      /// </summary>
      IEnumerable<IContainer> GetNeighborsFrom(IEnumerable<INeighborhood> neighborhoods);

      /// <summary>
      ///    returns the neighborhoods connecting the container with its neighbors.
      /// </summary>
      /// <param name="neighborhoods"> The possible neighborhoods. </param>
      IEnumerable<INeighborhood> GetNeighborhoods(IEnumerable<INeighborhood> neighborhoods);

      /// <summary>
      ///    Returns all children containers defined and the container itself, if the container is form type
      ///    <typeparamref
      ///       name="TContainer" />
      /// </summary>
      IReadOnlyList<TContainer> GetAllContainersAndSelf<TContainer>() where TContainer : class, IContainer;

      /// <summary>
      ///    Returns all children containers defined in the container satisfying the predicate and the container itself, if the container is form type
      ///    <typeparamref
      ///       name="TContainer" />
      ///    and satisfies the predicate
      /// </summary>
      IReadOnlyList<TContainer> GetAllContainersAndSelf<TContainer>(Func<TContainer, bool> predicate) where TContainer : class, IContainer;
   }

   public class Container : Entity, IContainer
   {
      private readonly IList<IEntity> _children;
      private ContainerMode _mode;
      private ContainerType _containerType;

      public Container()
      {
         _children = new List<IEntity>();
         Mode = ContainerMode.Logical;
      }

      public virtual IEnumerable<IEntity> Children
      {
         get { return _children; }
      }

      public virtual void Add(IEntity newChild)
      {
         if (newChild == null)
            return;

         // check for circular container reference
         if (newChild is IContainer && IsDescendant((IContainer) newChild))
            throw new CircularReferenceException(newChild.Name, Name);

         if (this.ContainsName(newChild.Name))
         {
            var oldChild = this.GetSingleChildByName(newChild.Name);

            // if newChild already belongs to this container, do nothing
            if (!ReferenceEquals(oldChild, newChild))
               throw new NotUniqueNameException(newChild.Name, Name);
         }
         else
         {
            _children.Add(newChild);
            newChild.ParentContainer = this;
            OnChanged();
         }
      }

      public ContainerMode Mode
      {
         get { return _mode; }
         set
         {
            _mode = value;
            OnPropertyChanged(() => Mode);
         }
      }

      public ContainerType ContainerType
      {
         get { return _containerType; }
         set
         {
            _containerType = value;
            OnPropertyChanged(() => ContainerType);
         }
      }

      public virtual void RemoveChildren()
      {
         _children.Clear();
         OnChanged();
      }

      public virtual IReadOnlyList<T> GetAllChildren<T>(Func<T, bool> predicate) where T : class, IEntity
      {
         var allChildren = GetChildren(predicate).ToList();
         var containerChildren = GetChildren<IContainer>().ToList();

         foreach (var containerChild in containerChildren)
         {
            allChildren.AddRange(containerChild.GetAllChildren(predicate));
         }
         return allChildren;
      }

      /// <summary>
      ///    Returns all children recursively of the container of type <typeparamref name="T" />
      /// </summary>
      public virtual IReadOnlyList<T> GetAllChildren<T>() where T : class, IEntity
      {
         return GetAllChildren<T>(child => true);
      }

      public virtual void RemoveChild(IEntity childToRemove)
      {
         if (!_children.Contains(childToRemove)) return;
         var deleted = _children.Remove(childToRemove);
         if (!deleted)
            throw new UnableToRemoveChildException(childToRemove, this);

         OnChanged();
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         //uses a temporary list in case the visitor adds child in the iterated collection
         _children.ToList().Each(child => child.AcceptVisitor(visitor));
      }

      /// <summary>
      ///    Returns all children of type <typeparamref name="T" />
      /// </summary>
      public virtual IEnumerable<T> GetChildren<T>() where T : class, IEntity
      {
         return GetChildren<T>(x => true);
      }

      public virtual IEnumerable<T> GetChildren<T>(Func<T, bool> predicate) where T : class, IEntity
      {
         return from childEntity in Children
                let castChild = childEntity as T
                where castChild != null
                where predicate(castChild)
                select castChild;
      }

      public virtual IEnumerable<IContainer> GetNeighborsFrom(IEnumerable<INeighborhood> neighborhoods)
      {
         var allNeighborhoods = neighborhoods.ToList();
         var first = from neighborhood in GetNeighborhoods(allNeighborhoods)
                     where neighborhood.FirstNeighbor != this
                     select neighborhood.FirstNeighbor;

         var second = from neighborhood in GetNeighborhoods(allNeighborhoods)
                      where neighborhood.SecondNeighbor != this
                      select neighborhood.SecondNeighbor;
         return first.Union(second);
      }

      public virtual IEnumerable<INeighborhood> GetNeighborhoods(IEnumerable<INeighborhood> neighborhoods)
      {
         var allNeighborhoods = neighborhoods.ToList();
         var first = from neighborhood in allNeighborhoods
                     where neighborhood.FirstNeighbor == this
                     select neighborhood;

         var second = from neighborhood in allNeighborhoods
                      where neighborhood.SecondNeighbor == this
                      select neighborhood;
         return first.Union(second);
      }

      public IReadOnlyList<TContainer> GetAllContainersAndSelf<TContainer>() where TContainer : class, IContainer
      {
         return GetAllContainersAndSelf<TContainer>(x => true);
      }

      public IReadOnlyList<TContainer> GetAllContainersAndSelf<TContainer>(Func<TContainer, bool> predicate) where TContainer : class, IContainer
      {
         var allChildren = GetAllChildren(predicate).ToList();
         var container = this as TContainer;
         if (container != null && predicate(container))
            allChildren.Add(container);

         return allChildren;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var container = source as IContainer;
         if (container == null) return;

         Mode = container.Mode;
         ContainerType = container.ContainerType;
      }

      public IEnumerator<IEntity> GetEnumerator()
      {
         return Children.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }

   public enum ContainerType
   {
      Other,
      Simulation,
      Model,
      Organism,
      Organ,
      Compartment,
      Application,
      Event,
      EventGroup,
      Neighborhood,
      Molecule,
      Reaction,
      Formulation,
      Transport
   }
}