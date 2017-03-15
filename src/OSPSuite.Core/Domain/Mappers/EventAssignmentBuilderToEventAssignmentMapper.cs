using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   /// Maps one event assignment from building block into model
   /// </summary>
   public interface IEventAssignmentBuilderToEventAssignmentMapper : IBuilderMapper<IEventAssignmentBuilder, IEventAssignment>
   {
   }

   class EventAssignmentBuilderToEventAssignmentMapper : IEventAssignmentBuilderToEventAssignmentMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;

      public EventAssignmentBuilderToEventAssignmentMapper(IObjectBaseFactory objectBaseFactory,IFormulaBuilderToFormulaMapper formulaMapper)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
      }

      public IEventAssignment MapFrom(IEventAssignmentBuilder assignmentBuilder, IBuildConfiguration buildConfiguration)
      {
         var assignment = _objectBaseFactory.Create<IEventAssignment>()
            .WithName(assignmentBuilder.Name)
            .WithDimension(assignmentBuilder.Dimension)
            .WithFormula(_formulaMapper.MapFrom(assignmentBuilder.Formula,buildConfiguration));

         assignment.ObjectPath = assignmentBuilder.ObjectPath.Clone<IObjectPath>();
         assignment.UseAsValue = assignmentBuilder.UseAsValue;

         buildConfiguration.AddBuilderReference(assignment, assignmentBuilder);
         return assignment;
      }
   }
}