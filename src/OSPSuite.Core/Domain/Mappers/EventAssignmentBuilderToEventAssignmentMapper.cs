using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps one event assignment from building block into model
   /// </summary>
   internal interface IEventAssignmentBuilderToEventAssignmentMapper : IBuilderMapper<IEventAssignmentBuilder, IReadOnlyList<IEventAssignment>>
   {
   }

   internal class EventAssignmentBuilderToEventAssignmentMapper : IEventAssignmentBuilderToEventAssignmentMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;

      public EventAssignmentBuilderToEventAssignmentMapper(IObjectBaseFactory objectBaseFactory, IFormulaBuilderToFormulaMapper formulaMapper)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
      }

      public IReadOnlyList<IEventAssignment> MapFrom(IEventAssignmentBuilder assignmentBuilder, SimulationBuilder simulationBuilder)
      {
         if (!isForAllFloating(assignmentBuilder))
            return new[] {createAssignment(assignmentBuilder, simulationBuilder) };

         return simulationBuilder.AllFloatingMolecules()
            .Select(x => createMoleculeAssignment(x, assignmentBuilder, simulationBuilder))
            .ToList();
      }

      private IEventAssignment createMoleculeAssignment(IMoleculeBuilder moleculeBuilder, IEventAssignmentBuilder assignmentBuilder, SimulationBuilder simulationBuilder)
      {
         //We change the original name to ensure unicity in the container.
         //Assignment are named programatically and not by the user so there should not be any conflict.
         var name = $"{assignmentBuilder.Name}_{moleculeBuilder.Name}";
         var assignment = createAssignment(assignmentBuilder, simulationBuilder, name);
         assignment.ObjectPath.Replace(ObjectPathKeywords.ALL_FLOATING_MOLECULES, moleculeBuilder.Name);
         return assignment;
      }

      private IEventAssignment createAssignment(IEventAssignmentBuilder assignmentBuilder, SimulationBuilder simulationBuilder, string name = null)
      {
         var assignment = _objectBaseFactory.Create<IEventAssignment>()
            .WithName(name ?? assignmentBuilder.Name)
            .WithDimension(assignmentBuilder.Dimension)
            .WithFormula(_formulaMapper.MapFrom(assignmentBuilder.Formula, simulationBuilder));

         assignment.ObjectPath = assignmentBuilder.ObjectPath.Clone<ObjectPath>();
         assignment.UseAsValue = assignmentBuilder.UseAsValue;

         simulationBuilder.AddBuilderReference(assignment, assignmentBuilder);

         return assignment;
      }

      private bool isForAllFloating(IEventAssignmentBuilder assignmentBuilder) =>
         assignmentBuilder.ObjectPath.Contains(ObjectPathKeywords.ALL_FLOATING_MOLECULES);
   }
}