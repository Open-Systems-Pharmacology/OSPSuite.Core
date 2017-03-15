using System;
using System.Collections.Generic;
using System.Drawing;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Diagram.Elements;
using OSPSuite.Presentation.Extensions;

namespace OSPSuite.Presentation.Diagram
{
   public abstract class BaseDiagramManager<TContainerNode, TNeighborhoodNode, TModel> : IDiagramManager<TModel>
      where TModel : class, IWithDiagramFor<TModel>
      where TContainerNode : class, IContainerNode, new()
      where TNeighborhoodNode : class, INeighborhoodNode, new()
   {
      private PointF _currentInsertLocation;
      protected PointF _previousInsertLocation;
      public TModel PkModel { get; protected set; }
      public PointF PreviousInsertLocation => _previousInsertLocation;
      public IDiagramOptions DiagramOptions { get; set; }
      public bool IsInitialized { get; private set; }
      protected IDiagramModel DiagramModel => PkModel.DiagramModel;

      protected BaseDiagramManager()
      {
         CurrentInsertLocation = new PointF(10, 10);
         UpdateMethods = new Dictionary<Type, Action<IObjectBase, IBaseNode>>();
         RegisterUpdateMethod(typeof (IContainer), UpdateContainer);
         RegisterUpdateMethod(typeof (INeighborhoodBase), UpdateNeighborhoodBase);
      }

      public PointF CurrentInsertLocation
      {
         get { return _currentInsertLocation; }
         set
         {
            _previousInsertLocation = _currentInsertLocation;
            _currentInsertLocation = value;
         }
      }

      /// <summary>
      ///    Returns the next node insertion point for the diagram
      /// </summary>
      public PointF GetNextInsertLocation()
      {
         if (InsertLocationHasChanged())
            CurrentInsertLocation = CurrentInsertLocation.Plus(Assets.Diagram.Base.InsertLocationOffset);

         _previousInsertLocation = CurrentInsertLocation;
         return CurrentInsertLocation;
      }

      public bool InsertLocationHasChanged()
      {
         return CurrentInsertLocation == PreviousInsertLocation;
      }

      public bool MustHandleExisting(string id) 
      {
         if (string.IsNullOrEmpty(id) || PkModel == null)
            return false;

         return DiagramModel.GetNode(id) != null;
      }

      public void InitializeWith(TModel pkModel, IDiagramOptions diagramOptions)
      {
         PkModel = pkModel;
         DiagramOptions = diagramOptions ?? new DiagramOptions();
         DiagramModel.DiagramOptions = DiagramOptions;
         if (IsInitialized) return;

         try
         {
            DiagramModel.BeginUpdate();
            UpdateDiagramModel(PkModel, DiagramModel, coupleAll: true);
            RefreshFromDiagramOptions();
         }
         finally
         {
            DiagramModel.EndUpdate();
         }
         IsInitialized = true;
      }

      protected abstract void UpdateDiagramModel(TModel pkModel, IDiagramModel diagramModel, bool coupleAll);

      public virtual void RefreshFromDiagramOptions()
      {
         foreach (var containerNode in DiagramModel.GetAllChildren<IContainerNode>())
         {
            if (containerNode.Name == Constants.MOLECULE_PROPERTIES)
               containerNode.IsVisible = DiagramOptions.MoleculePropertiesVisible;
         }

         foreach (var containerNode in DiagramModel.GetAllChildren<IContainerNode>())
         {
            containerNode.CalculateBounds();
            // to adjust neighborhoodlinks to possibly smaller container node
            containerNode.PostLayoutStep();         
         }

         foreach (var node in DiagramModel.GetAllChildren<IBaseNode>())
         {
            node.SetColorFrom(DiagramOptions.DiagramColors);
         }
      }

      public void RefreshDiagramFromModel() 
      {
         try
         {
            DiagramModel.BeginUpdate();
            // do not couple already existing nodes again
            UpdateDiagramModel(PkModel, DiagramModel, coupleAll: false); 
         }
         finally
         {
            DiagramModel.EndUpdate();
         }
      }

      protected internal IDictionary<Type, Action<IObjectBase, IBaseNode>> UpdateMethods { get; }

      protected abstract void DecoupleModel();

      public void Cleanup()
      {
         if (PkModel == null) return;
         DecoupleModel();
         PkModel = null;
      }

      public void UpdateInsertLocation()
      {
         CurrentInsertLocation = CurrentInsertLocation;
      }

      protected abstract bool MustHandleNew(IObjectBase obj);

      public void AddObjectBase(IObjectBase objectBase)
      {
         if (!MustHandleNew(objectBase)) return;

         // Determine parentContainerBase = containerNode of ParentContainer or DiagramModel
         IContainerBase parentContainerBase = DiagramModel;

         // if AddedObject is an entity take its parentContainer to find a parentContainerNode
         var entity = objectBase as IEntity;
         if (entity?.ParentContainer != null)
         {
            parentContainerBase = (IContainerBase) DiagramModel.GetNode(entity.ParentContainer.Id);
            if (parentContainerBase == null) 
               return; //do not add container or parameter in containers not shown, e.g. in globalmoleculedependentproperties

            // therefore the node for neighborhoodsContainer has to be created (see InitializeWith)
         }

         var node = AddObjectBase(parentContainerBase, objectBase, true, true);
         //because cannot undo this action, reset undo stack
         if (node != null)
            DiagramModel.ClearUndoStack();
      }

      protected virtual bool DecoupleObjectBase(IObjectBase objectBase, bool recursive)
      {
         if (objectBase.GetType() == typeof (Container))
         {
            var container = objectBase.DowncastTo<IContainer>();
            if (recursive)
            {
               foreach (var child in container.Children)
               {
                  DecoupleObjectBase(child,recursive: true);
               }
            }
            return Decouple<IContainer, IContainerNode>(container);
         }

         if (objectBase.IsAnImplementationOf<INeighborhoodBase>())
            return Decouple<INeighborhoodBase, TNeighborhoodNode>(objectBase.DowncastTo<INeighborhoodBase>());

         return false; // not removed by this implementation
      }

      protected virtual void UpdateContainer(IObjectBase containerAsEntity, IBaseNode containerNodeAsBaseNode)
      {
         var container = containerAsEntity as IContainer;
         var containerNode = containerNodeAsBaseNode as IContainerNode;
         containerNode.IsLogical = (container.Mode == ContainerMode.Logical);

         if (!DiagramOptions.MoleculePropertiesVisible && containerNode.Name == Constants.MOLECULE_PROPERTIES)
            containerNode.IsVisible = false;
      }

      protected virtual void UpdateNeighborhoodBase(IObjectBase neighborhoodBaseAsEntity, IBaseNode neighborhoodNodeAsBaseNode)
      {
         var neighborhoodBase = neighborhoodBaseAsEntity as INeighborhoodBase;
         var neighborhoodBaseNode = neighborhoodNodeAsBaseNode as INeighborhoodNode;
         neighborhoodBaseNode.Description = neighborhoodBase.Name;
         
         if (!string.IsNullOrEmpty(neighborhoodBase.Description))
            neighborhoodBaseNode.Description += Environment.NewLine + neighborhoodBase.Description;
         else
            neighborhoodBaseNode.Description += Environment.NewLine + "Neighborhood between " + neighborhoodBase.FirstNeighbor.EntityPath() + " and " + neighborhoodBase.SecondNeighbor.EntityPath();
      }

      protected void RegisterUpdateMethod(Type entityType, Action<IObjectBase, IBaseNode> updateMethod)
      {
         UpdateMethods.Add(entityType, updateMethod);
      }

      protected virtual IBaseNode AddObjectBase(IContainerBase parent, IObjectBase objectBase, bool recursive, bool coupleAll)
      {
         IBaseNode node = AddNeighborhood(objectBase as INeighborhoodBase);
         if (node != null) return node;
         node = AddContainer(parent, objectBase as IContainer, recursive, coupleAll);
         return node;
      }

      protected TNode Update<TObject, TNode>(TObject objectBase)
         where TObject : class, IObjectBase
         where TNode : class, IBaseNode
      {
         if (objectBase == null) return null;
         var node = NodeFor<TNode>(objectBase);
         if (node == null)
            throw new NotFoundException("Node for " + objectBase.GetType().Name + " " + objectBase.Name);

         // update base objectBase properties
         node.Name = objectBase.Name;
         node.Description = objectBase.Name;
         if (!string.IsNullOrEmpty(objectBase.Description))
            node.Description += "\n" + objectBase.Description;

         // update further properties, if available);
         if (UpdateMethods.ContainsKey(typeof (TObject)))
            UpdateMethods[typeof (TObject)](objectBase, node);

         node.SetColorFrom(DiagramOptions.DiagramColors);
         return node;
      }

      protected TNode NodeFor<TNode>(IWithId withId) where TNode : class, IBaseNode
      {
         return DiagramModel.GetNode<TNode>(withId.Id);
      }

      protected TNode AddAndCoupleNode<TObject, TNode>(IContainerBase parentContainerBase, TObject objectBase, bool coupleAll)
         where TObject : class, IObjectBase
         where TNode : class, IBaseNode, new()
      {
         if (objectBase == null) return null;
         var node = NodeFor<TNode>(objectBase);
         if (node == null)
         {
            node = DiagramModel.CreateNode<TNode>(objectBase.Id, CurrentInsertLocation, parentContainerBase);
            Update<TObject, TNode>(objectBase);
            // couple only once at creation
            Couple(objectBase, node); 
         }
         else
         {
            Update<TObject, TNode>(objectBase);
            if (coupleAll)
               Couple(objectBase, node); // if called by InitializeWith
         }
         return node;
      }

      protected virtual TContainerNode AddContainer(IContainerBase parent, IContainer container, bool recursive, bool coupleAll)
      {
         if (container == null || container.GetType() != typeof (Container))
            return null;

         var containerNode = AddAndCoupleNode<IContainer, TContainerNode>(parent, container, coupleAll);

         if (recursive)
            foreach (var child in container.Children)
            {
               AddObjectBase(containerNode, child, true, coupleAll);
            }

         return containerNode;
      }

      protected virtual INeighborhoodNode AddNeighborhood(INeighborhoodBase neighborhood)
      {
         if (neighborhood == null)
            return null;

         var firstNeighborContainerNode = NodeFor<IContainerNode>(neighborhood.FirstNeighbor);
         var secondNeighborContainerNode = NodeFor <IContainerNode>(neighborhood.SecondNeighbor);

         if (firstNeighborContainerNode == null || secondNeighborContainerNode == null)
            return null; 

         var neighborhoodNode = NodeFor<TNeighborhoodNode>(neighborhood);

         if (neighborhoodNode == null)
         {
            neighborhoodNode = DiagramModel.CreateNode<TNeighborhoodNode>(neighborhood.Id, CurrentInsertLocation, DiagramModel);
            neighborhoodNode.Initialize(firstNeighborContainerNode, secondNeighborContainerNode);
            neighborhoodNode.AdjustPosition();
            Update<INeighborhoodBase, TNeighborhoodNode>(neighborhood);
            Couple(neighborhood, neighborhoodNode);
         }
         else
         {
            neighborhoodNode.Initialize(firstNeighborContainerNode, secondNeighborContainerNode);
            neighborhoodNode.AdjustPosition();
            Update<INeighborhoodBase, TNeighborhoodNode>(neighborhood);
         }

         return neighborhoodNode;
      }

      protected void Couple<TObject, TNode>(TObject objectBase, TNode node)
         where TObject : class, IObjectBase
         where TNode : class, IBaseNode
      {
         objectBase.Changed += OnChanged<TObject, TNode>;
      }

      protected void OnChanged<TObject, TNode>(object obj)
         where TObject : class, IObjectBase
         where TNode : class, IBaseNode
      {
         Update<TObject, TNode>(obj as TObject);
      }

      public bool RemoveAndDecoupleNode<TObject, TNode>(TObject objectBase)
         where TObject : class, IObjectBase
         where TNode : class, IBaseNode
      {
         if (objectBase == null) return false;
         Decouple<TObject, TNode>(objectBase);
         DiagramModel.RemoveNode(objectBase.Id);
         return true;
      }

      protected bool Decouple<TObject, TNode>(TObject objectBase)
         where TObject : class, IObjectBase
         where TNode : class, IBaseNode
      {
         if (objectBase == null) return false;
         objectBase.Changed -= OnChanged<TObject, TNode>;
         return true;
      }

      public void RemoveObjectBase(IObjectBase objectBase)
      {
         if (objectBase == null) return;
         if (!MustHandleExisting(objectBase.Id)) return;
         RemoveObjectBase(objectBase, true);
         //because cannot undo this action, reset undo stack
         DiagramModel.ClearUndoStack(); 
      }

      protected virtual bool RemoveObjectBase(IObjectBase objectBase, bool recursive)
      {
         if (RemoveAndDecoupleNode<INeighborhoodBase, TNeighborhoodNode>(objectBase as INeighborhoodBase)) return true;
         if (RemoveContainer(objectBase as IContainer, recursive)) return true;
         return false;
      }

      protected virtual bool RemoveContainer(IContainer container, bool recursive)
      {
         if (container == null) return false;

         if (recursive)
            foreach (var child in container.Children)
            {
               RemoveObjectBase(child, true);
            }

         RemoveAndDecoupleNode<IContainer, TContainerNode>(container);
         return true;
      }

      public abstract IDiagramManager<TModel> Create();
   }
}