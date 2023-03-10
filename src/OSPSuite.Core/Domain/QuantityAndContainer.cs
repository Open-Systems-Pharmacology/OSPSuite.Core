using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace OSPSuite.Core.Domain
{
   public interface IQuantityAndContainer : IQuantity, IContainer
   {
   }

   public abstract class QuantityAndContainer : Quantity, IQuantityAndContainer
   {
      private readonly IContainer _container;
      public ContainerType ContainerType { get; set; }
      public string ContainerTypeAsString => ContainerType.ToString();

      protected QuantityAndContainer()
      {
         //create embedded container via new, not via ObjectBaseFactory
         //(container should not be accessible anywhere else)
         _container = new Container();
         Mode = ContainerMode.Logical;
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         // ToList necessary to be able to remove child in visitor.
         Children.ToList().Each(child => child.AcceptVisitor(visitor));
      }

      public IReadOnlyList<IEntity> Children => _container.Children;

      public ObjectPath ParentPath
      {
         get => _container.ParentPath;
         set => _container.ParentPath = value;
      }

      public ContainerMode Mode
      {
         get => _container.Mode;
         set => _container.Mode = value;
      }

      public void Add(IEntity newChild)
      {
         _container.Add(newChild);
         newChild.ParentContainer = this;
      }

      public void RemoveChild(IEntity childToRemove) => _container.RemoveChild(childToRemove);

      public void RemoveChildren() => _container.RemoveChildren();

      public IReadOnlyList<T> GetAllChildren<T>() where T : class, IEntity => _container.GetAllChildren<T>();

      public IReadOnlyList<T> GetAllChildren<T>(Func<T, bool> predicate) where T : class, IEntity => _container.GetAllChildren(predicate);

      public IEnumerable<T> GetChildren<T>(Func<T, bool> predicate) where T : class, IEntity => _container.GetChildren(predicate);

      public IEnumerable<T> GetChildren<T>() where T : class, IEntity => _container.GetChildren<T>();

      public IReadOnlyList<IContainer> GetNeighborsFrom(IReadOnlyList<Neighborhood> neighborhoods) => _container.GetNeighborsFrom(neighborhoods);

      public IReadOnlyList<Neighborhood> GetNeighborhoods(IReadOnlyList<Neighborhood> neighborhoods) => _container.GetNeighborhoods(neighborhoods);

      public IReadOnlyList<TContainer> GetAllContainersAndSelf<TContainer>() where TContainer : class, IContainer => GetAllContainersAndSelf<TContainer>(x => true);

      public IReadOnlyList<TContainer> GetAllContainersAndSelf<TContainer>(Func<TContainer, bool> predicate) where TContainer : class, IContainer
      {
         //needs to re-implement the method since we do not want to return the internal _container
         var allChildren = GetAllChildren(predicate).ToList();
         var container = this as TContainer;
         if (container != null && predicate(container))
            allChildren.Add(container);

         return allChildren;
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
}