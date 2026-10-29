using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Comparison
{
   public class ApplicationBuilderDiffBuilder : DiffBuilder<ApplicationBuilder>
   {
      private readonly EntityDiffBuilder _entityDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;

      public ApplicationBuilderDiffBuilder(EntityDiffBuilder entityDiffBuilder, EnumerableComparer enumerableComparer)
      {
         _entityDiffBuilder = entityDiffBuilder;
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<ApplicationBuilder> comparison)
      {
         _entityDiffBuilder.Compare(comparison);
         //container properties are compared here as the children are compared below with a special treatment for the application molecules
         CompareValues(x => x.ContainerType, x => x.ContainerType, comparison);
         CompareValues(x => x.Mode, x => x.Mode, comparison);
         CompareValues(x => x.ParentPath, x => x.ParentPath, comparison);
         CompareStringValues(x => x.EventGroupType, x => x.EventGroupType, comparison);
         CompareStringValues(x => x.MoleculeName, x => x.MoleculeName, comparison);
         CompareValues(x => x.SourceCriteria, x => x.SourceCriteria, comparison);
         //Special treatment for application molecule builder, better compare relative container path then name

         //Here we have a special treatment for application molecule builder, find missing by Container Path
         _enumerableComparer.CompareEnumerables(comparison, x => x.Molecules, item => item.RelativeContainerPath);

         // Treat all other children, application molecule builder are ignored
         _enumerableComparer.CompareEnumerables(comparison, getChildrenButMolecules, item => item.Name);
      }

      private IEnumerable<IEntity> getChildrenButMolecules(IContainer container)
      {
         return container.GetChildren<IEntity>(child => !child.IsAnImplementationOf<ApplicationMoleculeBuilder>());
      }
   }
}