using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OSPSuite.Core.Diagram;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram;
using OSPSuite.Presentation.Extensions;
using OSPSuite.UI.Diagram.Elements;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Diagram.Managers
{
   public class ReactionDiagramManager<T> : BaseDiagramManager<MultiPortContainerNode, SimpleNeighborhoodNode, T>, IReactionDiagramManager<T>
      where T : class, IWithDiagramFor<T>, IEnumerable<IReactionBuilder>
   {
      public ReactionDiagramManager()
      {
         RegisterUpdateMethod(typeof (IReactionBuilder), UpdateReactionBuilder);
      }

      protected override void UpdateDiagramModel(T reactionBuildingBlock, IDiagramModel diagramModel, bool coupleAll)
      {
         var unusedReactionIds = new HashSet<string>();

         foreach (var reactionNode in diagramModel.GetAllChildren<ReactionNode>())
         {
            unusedReactionIds.Add(reactionNode.Id);
         }

         if (reactionBuildingBlock != null)
         {
            foreach (var reactionBuilder in reactionBuildingBlock)
            {
               AddObjectBase(diagramModel, reactionBuilder, recursive: true, coupleAll: coupleAll);
               unusedReactionIds.Remove(reactionBuilder.Id);
            }
         }

         // remove all unused reactionNodes 
         foreach (var reactionBuilderId in unusedReactionIds)
         {
            diagramModel.RemoveNode(reactionBuilderId);
         }

         DiagramModel.ClearUndoStack();
      }

      // removes all eventHandler (which are references to this presenter)
      protected override void DecoupleModel()
      {
         foreach (var reactionBuilder in PkModel)
         {
            DecoupleObjectBase(reactionBuilder, recursive: true);
         }
      }

      protected override bool DecoupleObjectBase(IObjectBase objectBase, bool recursive)
      {
         return Decouple<IReactionBuilder, ReactionNode>(objectBase as IReactionBuilder);
      }

      protected override bool MustHandleNew(IObjectBase obj)
      {
         if (obj == null || PkModel == null)
            return false;

         return obj.IsAnImplementationOf<IReactionBuilder>() && PkModel.Contains(obj.DowncastTo<IReactionBuilder>());
      }

      public void AddMolecule(IReactionBuilder reactionBuilder, string moleculeName)
      {
         var reactionNode = ReactionNodeFor(reactionBuilder);
         // insert new Modifier node above reaction
         CurrentInsertLocation = reactionNode.Location.Plus(new PointF(10F, -30F));
         // create new MoleculeNode at CurrentInsertLocation, if not already available
         var newMoleculeNode = getMoleculeNode(moleculeName, reactionNode.Location);
         newMoleculeNode.ToFront();

         UpdateReactionBuilder(reactionBuilder);
      }

      public void RemoveMolecule(IReactionBuilder reactionBuilder, string moleculeName)
      {
         UpdateReactionBuilder(reactionBuilder);
      }

      public void RenameMolecule(IReactionBuilder reactionBuilder, string oldMoleculeName, string newMoleculeName)
      {
         // try to keep same location of renamed molecule
         var reactionNode = ReactionNodeFor(reactionBuilder);

         //molecule does not exist with the given name: nothing to rename here (might have been renamed already)
         var oldMoleculeNode = getMoleculeNode(oldMoleculeName, reactionNode.Location, create: false);
         if (oldMoleculeNode == null)
            return;

         var oldLocation = oldMoleculeNode.Location;
         oldMoleculeNode.Location = oldMoleculeNode.Location.Plus(Assets.Diagram.Reaction.OldMoleculeNodeOffsetInRename);
         var newMoleculeNode = getMoleculeNode(newMoleculeName, reactionNode.Location);
         newMoleculeNode.Location = oldLocation;
         newMoleculeNode.ToFront();
         UpdateReactionBuilder(reactionBuilder);

         if (!oldMoleculeNode.IsConnectedToReactions)
            DiagramModel.RemoveNode(oldMoleculeNode.Id);
      }

      protected override IBaseNode AddObjectBase(IContainerBase parent, IObjectBase objectBase, bool recursive, bool coupleAll)
      {
         var node = AddAndCoupleNode<IReactionBuilder, ReactionNode>(parent, objectBase as IReactionBuilder, coupleAll);
         if (node != null)
            return node;

         return base.AddObjectBase(parent, objectBase, recursive, coupleAll);
      }

      protected override bool RemoveObjectBase(IObjectBase objectBase, bool recursive)
      {
         if (RemoveAndDecoupleNode<IReactionBuilder, ReactionNode>(objectBase as IReactionBuilder))
            return true;

         return base.RemoveObjectBase(objectBase, recursive);
      }

      // signature is necessary for use as argument in RegisterUpdateMethod
      public void UpdateReactionBuilder(IObjectBase reactionAsObjectBase, IBaseNode reactionNodeAsBaseNode)
      {
         var reactionBuilder = reactionAsObjectBase.DowncastTo<IReactionBuilder>();
         var reactionNode = reactionNodeAsBaseNode.DowncastTo<ReactionNode>();

         reactionNode.ClearLinks();

         foreach (var rpb in reactionBuilder.Educts)
         {
            createReactionLink(ReactionLinkType.Educt, reactionNode, getMoleculeNode(rpb.MoleculeName, reactionNode.Location));
         }

         foreach (var rpb in reactionBuilder.Products)
         {
            createReactionLink(ReactionLinkType.Product, reactionNode, getMoleculeNode(rpb.MoleculeName, reactionNode.Location));
         }

         foreach (var modifierName in reactionBuilder.ModifierNames)
         {
            createReactionLink(ReactionLinkType.Modifier, reactionNode, getMoleculeNode(modifierName, reactionNode.Location));
         }

         reactionNode.SetColorFrom(DiagramOptions.DiagramColors);
      }

      public void UpdateReactionBuilder(IReactionBuilder reactionBuilder)
      {
         var reactionNode = ReactionNodeFor(reactionBuilder);
         if (reactionNode == null) return;
         UpdateReactionBuilder(reactionBuilder, reactionNode);
      }

      public IReactionNode ReactionNodeFor(IReactionBuilder reactionBuilder)
      {
         return NodeFor<ReactionNode>(reactionBuilder);
      }

      private void createReactionLink(ReactionLinkType type, ReactionNode reactionNode, IMoleculeNode moleculeNode)
      {
         var reactionLink = new ReactionLink();
         reactionLink.Initialize(type, reactionNode, moleculeNode);
         reactionLink.SetColorFrom(DiagramOptions.DiagramColors);
      }

      public IEnumerable<IMoleculeNode> GetMoleculeNodes()
      {
         return DiagramModel.GetDirectChildren<MoleculeNode>();
      }

      public IEnumerable<IMoleculeNode> GetMoleculeNodes(string moleculeName)
      {
         return GetMoleculeNodes().Where(n => n.Name == moleculeName);
      }

      public IMoleculeNode AddMoleculeNode(string moleculeName)
      {
         var newMoleculeNode = createMoleculeNode(moleculeName);

         foreach (var moleculeNode in GetMoleculeNodes(moleculeName))
         {
            foreach (var reactionNode in moleculeNode.GetLinkedNodes<ReactionNode>())
            {
               UpdateReactionBuilder(PkModel.First(node => node.Id == reactionNode.Id), reactionNode);
            }
         }
         return newMoleculeNode;
      }

      public void RemoveMoleculeNode(IMoleculeNode moleculeNode)
      {
         DiagramModel.RemoveNode(moleculeNode.Id);
         foreach (var reactionNode in moleculeNode.GetLinkedNodes<ReactionNode>())
         {
            UpdateReactionBuilder(PkModel.First(node => node.Id == reactionNode.Id), reactionNode);
         }
      }

      private IMoleculeNode createMoleculeNode(string name)
      {
         var moleculeNode = DiagramModel.CreateNode<MoleculeNode>(Guid.NewGuid().ToString(), GetNextInsertLocation(), DiagramModel);
         moleculeNode.Name = name;
         moleculeNode.Description = name;
         moleculeNode.SetColorFrom(DiagramOptions.DiagramColors);
         return moleculeNode;
      }

      private IMoleculeNode getMoleculeNode(string moleculeName, PointF location, bool create = true)
      {
         var moleculeNodes = GetMoleculeNodes(moleculeName).ToList();

         if (moleculeNodes.Count == 0)
            return create ? createMoleculeNode(moleculeName) : null;

         var moleculeNode = moleculeNodes.First();
         if (moleculeNodes.Count == 1)
            return moleculeNode;

         foreach (var node in moleculeNodes)
         {
            if (node.Location.DistanceTo(location) < moleculeNode.Location.DistanceTo(location))
               moleculeNode = node;
         }

         return moleculeNode;
      }

      public override IDiagramManager<T> Create()
      {
         return new ReactionDiagramManager<T>();
      }
   }
}