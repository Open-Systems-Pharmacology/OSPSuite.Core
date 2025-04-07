using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps event builder object to an model event
   /// </summary>
   internal interface IEventBuilderToEventMapper : IBuilderMapper<EventBuilder, Event>
   {
   }

   internal class EventBuilderToEventMapper : IEventBuilderToEventMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IEventAssignmentBuilderToEventAssignmentMapper _assignmentMapper;
      private readonly IEntityTracker _entityTracker;

      public EventBuilderToEventMapper(IObjectBaseFactory objectBaseFactory,
         IParameterBuilderToParameterMapper parameterMapper,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IEventAssignmentBuilderToEventAssignmentMapper assignmentMapper,
         IEntityTracker entityTracker
         )
      {
         _objectBaseFactory = objectBaseFactory;
         _parameterMapper = parameterMapper;
         _formulaMapper = formulaMapper;
         _assignmentMapper = assignmentMapper;
         _entityTracker = entityTracker;
      }

      public Event MapFrom(EventBuilder eventBuilder, SimulationBuilder simulationBuilder)
      {
         var modelEvent = _objectBaseFactory.Create<Event>()
            .WithName(eventBuilder.Name)
            .WithDimension(eventBuilder.Dimension)
            .WithDescription(eventBuilder.Description)
            .WithFormula(_formulaMapper.MapFrom(eventBuilder.Formula, simulationBuilder));

         _entityTracker.Track(modelEvent, eventBuilder, simulationBuilder);

         eventBuilder.Assignments
            .SelectMany(x => _assignmentMapper.MapFrom(x, simulationBuilder))
            .Each(modelEvent.AddAssignment);

         foreach (var param in eventBuilder.Parameters)
         {
            modelEvent.Add(_parameterMapper.MapFrom(param, simulationBuilder));
         }

         modelEvent.OneTime = eventBuilder.OneTime;

         return modelEvent;
      }
   }
}