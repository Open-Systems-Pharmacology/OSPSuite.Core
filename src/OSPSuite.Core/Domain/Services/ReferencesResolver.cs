using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.Services
{
   public interface IReferencesResolver
   {
      void ResolveReferencesIn(IModel model);
      void ResolveReferencesIn(IContainer container);
   }

   public class ReferencesResolver : IReferencesResolver
   {
      public void ResolveReferencesIn(IModel model)
      {
         ResolveReferencesIn(model.Root);
      }

      public void ResolveReferencesIn(IContainer container)
      {
         resolveReferences(container.GetAllChildren<IUsingFormula>().ToList());
      }

      private void resolveReferences(IEnumerable<IUsingFormula> entityUsingFormulas)
      {
         foreach (var usingFormula in entityUsingFormulas)
         {
            usingFormula.Formula.ResolveObjectPathsFor(usingFormula);
            resolveReferenceFor(usingFormula as IParameter);
            resolveReferenceFor(usingFormula as IEventAssignment);
         }
      }

      private void resolveReferenceFor(IParameter parameter)
      {
         parameter?.RHSFormula?.ResolveObjectPathsFor(parameter);
      }

      private void resolveReferenceFor(IEventAssignment eventAssignment)
      {
         eventAssignment?.ResolveChangedEntity();
      }
   }
}