using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Diagram
{
   public interface IReactionDiagramManager<T> : IDiagramManager<T> where T : IWithDiagramFor<T>
   {
      void AddMolecule(IReactionBuilder reactionBuilder, string moleculeName);
      void RemoveMolecule(IReactionBuilder reactionBuilder, string moleculeName);
      void RenameMolecule(IReactionBuilder reactionBuilder, string oldMoleculeName, string newMoleculeName);
      IMoleculeNode AddMoleculeNode(string moleculeName);
      void RemoveMoleculeNode(IMoleculeNode moleculeNode);
      IEnumerable<IMoleculeNode> GetMoleculeNodes(string moleculeName);
      IEnumerable<IMoleculeNode> GetMoleculeNodes();
      void UpdateReactionBuilder(IObjectBase reactionAsObjectBase, IBaseNode reactionNodeAsBaseNode);
      void UpdateReactionBuilder(IReactionBuilder reactionBuilder);
      IReactionNode ReactionNodeFor(IReactionBuilder reactionBuilder);
   }
}