using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps Observer Builder object to its counterpart in the simulation
   /// </summary>
   public interface IObserverBuilderToObserverMapper : IBuilderMapper<ObserverBuilder, Observer>
   {
   }

   internal class ObserverBuilderToObserverMapper : IObserverBuilderToObserverMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IEntityTracker _entityTracker;

      public ObserverBuilderToObserverMapper(IObjectBaseFactory objectBaseFactory, IFormulaBuilderToFormulaMapper formulaMapper, IEntityTracker entityTracker)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
         _entityTracker = entityTracker;
      }

      public Observer MapFrom(ObserverBuilder observerBuilder, SimulationBuilder simulationBuilder)
      {
         var observer = _objectBaseFactory.Create<Observer>()
            .WithName(observerBuilder.Name)
            .WithIcon(observerBuilder.Icon)
            .WithDescription(observerBuilder.Description)
            .WithDimension(observerBuilder.Dimension)
            .WithFormula(_formulaMapper.MapFrom(observerBuilder.Formula, simulationBuilder));

         _entityTracker.Track(observer, observerBuilder, simulationBuilder);

         return observer;
      }
   }
}