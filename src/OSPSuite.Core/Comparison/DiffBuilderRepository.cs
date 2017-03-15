using System;
using OSPSuite.Utility;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core.Comparison
{
   public interface IDiffBuilderRepository : IBuilderRepository<IDiffBuilder>
   {
      IDiffBuilder DiffBuilderFor(Type objectType);
   }

   public class DiffBuilderRepository : BuilderRepository<IDiffBuilder>, IDiffBuilderRepository
   {
      public DiffBuilderRepository(IContainer container)
         : base(container, typeof(IDiffBuilder<>))
      {
      }

      public IDiffBuilder DiffBuilderFor(Type objectType)
      {
         return BuilderFor(objectType);
      }
   }
}