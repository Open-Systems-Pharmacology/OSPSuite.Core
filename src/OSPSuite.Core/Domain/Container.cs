using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   public interface IContainer : IEntity, IEnumerable<IEntity>
   {
      ContainerType ContainerType { set; get; }

      /// <summary>
      ///    Returns the <see cref="ContainerType" /> as a string
      /// </summary>
      string ContainerTypeAsString { get; }

      ContainerMode Mode { get; set; }

      /// <summary>
      ///    Returns the children of the Container
      /// </summary>
      IReadOnlyList<IEntity> Children { get; }

      /// <summary>
      ///    Returns the path to the parent container.
      ///    It should only be set if the container has no parent in the hierarchy. Otherwise, it will be null
      /// </summary>
      ObjectPath ParentPath { get; set; }

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
      ///    returns the neighbors from the container connected by the given neighborhoods.
      /// </summary>
      IReadOnlyList<IContainer> GetNeighborsFrom(IReadOnlyList<Neighborhood> neighborhoods);

      /// <summary>
      ///    returns the neighborhoods connecting the container with its neighbors.
      /// </summary>
      /// <param name="neighborhoods"> The possible neighborhoods. </param>
      IReadOnlyList<Neighborhood> GetNeighborhoods(IReadOnlyList<Neighborhood> neighborhoods);

      /// <summary>
      ///    Returns all children containers defined and the container itself, if the container is form type
      ///    <typeparamref
      ///       name="TContainer" />
      /// </summary>
      IReadOnlyList<TContainer> GetAllContainersAndSelf<TContainer>() where TContainer : class, IContainer;

      /// <summary>
      ///    Returns all children containers defined in the container satisfying the predicate and the container itself, if the
      ///    container is form type
      ///    <typeparamref
      ///       name="TContainer" />
      ///    and satisfies the predicate
      /// </summary>
      IReadOnlyList<TContainer> GetAllContainersAndSelf<TContainer>(Func<TContainer, bool> predicate) where TContainer : class, IContainer;

      /// <summary>
      ///    Returns all children recursively of the container of type <typeparamref name="TEntity" /> including the container
      ///    itself if it satisfies the type
      /// </summary>
      IReadOnlyList<TEntity> GetAllChildrenAndSelf<TEntity>() where TEntity : class, IEntity;

      /// <summary>
      ///    Returns all children recursively of the container of type <typeparamref name="TEntity" /> including the container
      ///    itself if it satisfies the type and predicate
      /// </summary>
      IReadOnlyList<TEntity> GetAllChildrenAndSelf<TEntity>(Func<TEntity, bool> predicate) where TEntity : class, IEntity;
   }

   public class Container : Entity, IContainer
   {
      private readonly List<IEntity> _children = new List<IEntity>();

      private ContainerMode _mode = ContainerMode.Logical;

      private ContainerType _containerType;

      //Path to parent container is null by default. In this case, it will be evaluated from container structure
      private ObjectPath _parentPath;

      public virtual IReadOnlyList<IEntity> Children => _children;

      public virtual void Add(IEntity newChild)
      {
         if (newChild == null)
            return;

         // check for circular container reference
         if (newChild is IContainer child && IsDescendant(child))
            throw new CircularReferenceException(child.Name, Name);

         if (this.ContainsName(newChild.Name))
         {
            var oldChild = this.GetSingleChildByName(newChild.Name);

            // if newChild already belongs to this container, do nothing
            if (!ReferenceEquals(oldChild, newChild))
               throw new NotUniqueNameException(newChild.Name, Name);

            return;
         }

         if (newChild is IContainer childContainer)
            childContainer.ParentPath = null;

         _children.Add(newChild);
         newChild.ParentContainer = this;

         OnChanged();
      }

      public string ContainerTypeAsString => ContainerType.ToString();

      public ContainerMode Mode
      {
         get => _mode;
         set => SetProperty(ref _mode, value);
      }

      public ObjectPath ParentPath
      {
         get => ParentContainer == null ? _parentPath : null;
         set => SetProperty(ref _parentPath, value);
      }

      public ContainerType ContainerType
      {
         get => _containerType;
         set => SetProperty(ref _containerType, value);
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
      public virtual IReadOnlyList<T> GetAllChildren<T>() where T : class, IEntity => GetAllChildren<T>(child => true);

      public virtual void RemoveChild(IEntity childToRemove)
      {
         if (!_children.Contains(childToRemove))
            return;

         var deleted = _children.Remove(childToRemove);
         if (!deleted)
            throw new UnableToRemoveChildException(childToRemove, this);

         childToRemove.ParentContainer = null;

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
      public virtual IEnumerable<T> GetChildren<T>() where T : class, IEntity => GetChildren<T>(x => true);

      public virtual IEnumerable<T> GetChildren<T>(Func<T, bool> predicate) where T : class, IEntity
      {
         return from childEntity in Children
            let castChild = childEntity as T
            where castChild != null
            where predicate(castChild)
            select castChild;
      }

      public virtual IReadOnlyList<IContainer> GetNeighborsFrom(IReadOnlyList<Neighborhood> neighborhoods)
      {
         var first = from neighborhood in GetNeighborhoods(neighborhoods)
            where neighborhood.FirstNeighbor != this
            select neighborhood.FirstNeighbor;

         var second = from neighborhood in GetNeighborhoods(neighborhoods)
            where neighborhood.SecondNeighbor != this
            select neighborhood.SecondNeighbor;

         return first.Union(second).ToList();
      }

      public virtual IReadOnlyList<Neighborhood> GetNeighborhoods(IReadOnlyList<Neighborhood> neighborhoods)
      {
         var first = from neighborhood in neighborhoods
            where neighborhood.FirstNeighbor == this
            select neighborhood;

         var second = from neighborhood in neighborhoods
            where neighborhood.SecondNeighbor == this
            select neighborhood;

         return first.Union(second).ToList();
      }

      public IReadOnlyList<TContainer> GetAllContainersAndSelf<TContainer>() where TContainer : class, IContainer => GetAllContainersAndSelf<TContainer>(x => true);

      public IReadOnlyList<TContainer> GetAllContainersAndSelf<TContainer>(Func<TContainer, bool> predicate) where TContainer : class, IContainer
         => GetAllChildrenAndSelf(predicate);

      public virtual IReadOnlyList<TEntity> GetAllChildrenAndSelf<TEntity>() where TEntity : class, IEntity => GetAllChildrenAndSelf<TEntity>(child => true);

      /// <summary>
      ///    Returns all children recursively of the container of type <typeparamref name="TEntity" /> including the container
      ///    itself if it satisfies the type and predicate
      /// </summary>
      public virtual IReadOnlyList<TEntity> GetAllChildrenAndSelf<TEntity>(Func<TEntity, bool> predicate) where TEntity : class, IEntity
      {
         var allChildren = GetAllChildren(predicate).ToList();
         if (this is TEntity entity && predicate(entity))
            allChildren.Add(entity);

         return allChildren;
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var container = source as IContainer;
         if (container == null) return;

         Mode = container.Mode;
         ContainerType = container.ContainerType;
         ParentPath = container.ParentPath?.Clone<ObjectPath>();
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