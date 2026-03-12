using OSPSuite.Core.Commands;

namespace OSPSuite.Core.Domain.Services
{
   public abstract class ParameterAnalysableAnalysisCreator : SimulationAnalysisCreator
   {
      private readonly ICloneManager _cloneManager;
      private readonly IObjectIdResetter _objectIdResetter;
      private readonly IIdGenerator _idGenerator;

      public override ISimulationAnalysis CreateAnalysisBasedOn(ISimulationAnalysis simulationAnalysis)
      {
         var clone = _cloneManager.Clone(simulationAnalysis as IUpdatable) as ISimulationAnalysis;
         _objectIdResetter.ResetIdFor(clone);
         return clone;
      }

      protected ParameterAnalysableAnalysisCreator(IContainerTask containerTask, IOSPSuiteExecutionContext context, ICloneManager cloneManager, IObjectIdResetter objectIdResetter, IIdGenerator idGenerator) : base(containerTask, context)
      {
         _cloneManager = cloneManager;
         _objectIdResetter = objectIdResetter;
         _idGenerator = idGenerator;
      }

      protected T AnalysisFor<T>(IAnalysable analysable) where T : ISimulationAnalysis, new()
      {
         var analysis = new T().WithId(_idGenerator.NewId());
         AddSimulationAnalysisTo(analysable, analysis);
         return analysis;
      }
   }
}