using OSPSuite.Core.Domain;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Services
{
   public interface IQualificationStepRunnerFactory
   {
      IQualificationStepRunner CreateFor(IQualificationStep qualificationStep);
   }
   
   public abstract class QualificationStepRunnerFactory : IQualificationStepRunnerFactory
   {
      protected readonly IContainer _container;

      protected QualificationStepRunnerFactory(IContainer container)
      {
         _container = container;
      }

      public abstract IQualificationStepRunner CreateFor(IQualificationStep qualificationStep);
   }
}