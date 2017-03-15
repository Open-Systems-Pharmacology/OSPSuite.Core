using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   /// Maps event builder object to an model event
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

      public IEvent MapFrom(IEventBuilder eventBuilder,IBuildConfiguration buildConfiguration)
      {
         var modelEvent = _objectBaseFactory.Create<IEvent>()
            .WithName(eventBuilder.Name)
            .WithDimension(eventBuilder.Dimension)
            .WithDescription(eventBuilder.Description)
            .WithFormula(_formulaMapper.MapFrom(eventBuilder.Formula,buildConfiguration));

         buildConfiguration.AddBuilderReference(modelEvent, eventBuilder);

         foreach(var assignment in eventBuilder.Assignments)
         {
            modelEvent.AddAssignment(_assignmentMapper.MapFrom(assignment,buildConfiguration));
         }

         foreach (var param in eventBuilder.Parameters)
         {
            modelEvent.Add(_parameterMapper.MapFrom(param,buildConfiguration));
         }

         modelEvent.OneTime = eventBuilder.OneTime;

         return modelEvent;
      }
   }
}