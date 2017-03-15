using OSPSuite.Core.Commands;

namespace OSPSuite.Core.Domain.Services
{
   public abstract class ParameterAnalysableAnalysisCreator : SimulationAnalysisCreator
   {
      private readonly IOSPSuiteExecutionContext _context;
      private readonly IObjectIdResetter _objectIdResetter;
      private readonly IIdGenerator _idGenerator;

      public override ISimulationAnalysis CreateAnalysisBasedOn(ISimulationAnalysis simulationAnalysis)
      {
         var stream = _context.Serialize(simulationAnalysis);
         var clone = _context.Deserialize<ISimulationAnalysis>(stream);
         _objectIdResetter.ResetIdFor(clone);
         return clone;
      }

      protected ParameterAnalysableAnalysisCreator(IContainerTask containerTask, IOSPSuiteExecutionContext context, IObjectIdResetter objectIdResetter, IIdGenerator idGenerator) : base(containerTask, context)
      {
         _context = context;
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