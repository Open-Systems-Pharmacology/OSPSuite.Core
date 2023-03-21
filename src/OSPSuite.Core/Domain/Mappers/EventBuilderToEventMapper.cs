using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps event builder object to an model event
   /// </summary>
   public interface IEventBuilderToEventMapper : IBuilderMapper<IEventBuilder, IEvent>
   {
   }

   public class EventBuilderToEventMapper : IEventBuilderToEventMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IParameterBuilderToParameterMapper _parameterMapper;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IEventAssignmentBuilderToEventAssignmentMapper _assignmentMapper;

      public EventBuilderToEventMapper(IObjectBaseFactory objectBaseFactory,
         IParameterBuilderToParameterMapper parameterMapper,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IEventAssignmentBuilderToEventAssignmentMapper assignmentMapper)
      {
         _objectBaseFactory = objectBaseFactory;
         _parameterMapper = parameterMapper;
         _formulaMapper = formulaMapper;
         _assignmentMapper = assignmentMapper;
      }

      public IEvent MapFrom(IEventBuilder eventBuilder, SimulationConfiguration simulationConfiguration)
      {
         var modelEvent = _objectBaseFactory.Create<IEvent>()
            .WithName(eventBuilder.Name)
            .WithDimension(eventBuilder.Dimension)
            .WithDescription(eventBuilder.Description)
            .WithFormula(_formulaMapper.MapFrom(eventBuilder.Formula, simulationConfiguration));

         simulationConfiguration.AddBuilderReference(modelEvent, eventBuilder);

         eventBuilder.Assignments
            .SelectMany(x => _assignmentMapper.MapFrom(x, simulationConfiguration))
            .Each(modelEvent.AddAssignment);

         foreach (var param in eventBuilder.Parameters)
         {
            modelEvent.Add(_parameterMapper.MapFrom(param, simulationConfiguration));
         }

         modelEvent.OneTime = eventBuilder.OneTime;

         return modelEvent;
      }
   }
}