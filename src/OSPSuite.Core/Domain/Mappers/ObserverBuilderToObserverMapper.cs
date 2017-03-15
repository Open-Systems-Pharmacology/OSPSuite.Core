using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps Observer Builder object to its counterpart in the simulation
   /// </summary>
   public interface IObserverBuilderToObserverMapper : IBuilderMapper<IObserverBuilder, IObserver>
   {
   }

   public class ObserverBuilderToObserverMapper : IObserverBuilderToObserverMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;

      public ObserverBuilderToObserverMapper(IObjectBaseFactory objectBaseFactory, IFormulaBuilderToFormulaMapper formulaMapper)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
      }

      public IObserver MapFrom(IObserverBuilder observerBuilder, IBuildConfiguration buildConfiguration)
      {
         var observer = _objectBaseFactory.Create<IObserver>()
            .WithName(observerBuilder.Name)
            .WithIcon(observerBuilder.Icon)
            .WithDescription(observerBuilder.Description)
            .WithDimension(observerBuilder.Dimension)
            .WithFormula(_formulaMapper.MapFrom(observerBuilder.Formula,buildConfiguration));

         buildConfiguration.AddBuilderReference(observer, observerBuilder);
         return observer;
      }
   }
}