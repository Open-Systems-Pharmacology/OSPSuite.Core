using OSPSuite.Utility.Visitor;
using OSPSuite.Core.Domain;

namespace OSPSuite
{
   public class RegisterTaskForSpecs : IVisitor<IObjectBase>
   {
      private readonly IWithIdRepository _withIdRepository;

      public RegisterTaskForSpecs(IWithIdRepository withIdRepository)
      {
         _withIdRepository = withIdRepository;
      }

      public void RegisterAllIn(IContainer container)
      {
         container.AcceptVisitor(this);
      }

      public void Visit(IObjectBase objToVisit)
      {
         _withIdRepository.Register(objToVisit);
      }
   }
}