using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps one event assignment from building block into model
   /// </summary>
   public interface IEventAssignmentBuilderToEventAssignmentMapper : IBuilderMapper<IEventAssignmentBuilder, IReadOnlyList<IEventAssignment>>
   {
   }

   public class EventAssignmentBuilderToEventAssignmentMapper : IEventAssignmentBuilderToEventAssignmentMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;

      public EventAssignmentBuilderToEventAssignmentMapper(IObjectBaseFactory objectBaseFactory, IFormulaBuilderToFormulaMapper formulaMapper)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
      }

      public IReadOnlyList<IEventAssignment> MapFrom(IEventAssignmentBuilder assignmentBuilder, IBuildConfiguration buildConfiguration)
      {
         if (!isForAllFloating(assignmentBuilder))
            return new[] {createAssignment(assignmentBuilder, buildConfiguration)};

         return buildConfiguration.Molecules.AllFloating()
            .Select(x => createMoleculeAssignment(x, assignmentBuilder, buildConfiguration))
            .ToList();
      }

      private IEventAssignment createMoleculeAssignment(IMoleculeBuilder moleculeBuilder, IEventAssignmentBuilder assignmentBuilder, IBuildConfiguration buildConfiguration)
      {
         var name = $"{assignmentBuilder.Name}_{moleculeBuilder.Name}";
         var assignment = createAssignment(assignmentBuilder, buildConfiguration, name);
         assignment.ObjectPath.Replace(ObjectPathKeywords.ALL_FLOATING, moleculeBuilder.Name);
         return assignment;
      }

      private IEventAssignment createAssignment(IEventAssignmentBuilder assignmentBuilder, IBuildConfiguration buildConfiguration, string name = null)
      {
         var assignment = _objectBaseFactory.Create<IEventAssignment>()
            .WithName(name ?? assignmentBuilder.Name)
            .WithDimension(assignmentBuilder.Dimension)
            .WithFormula(_formulaMapper.MapFrom(assignmentBuilder.Formula, buildConfiguration));

         assignment.ObjectPath = assignmentBuilder.ObjectPath.Clone<IObjectPath>();
         assignment.UseAsValue = assignmentBuilder.UseAsValue;

         buildConfiguration.AddBuilderReference(assignment, assignmentBuilder);

         return assignment;
      }

      private bool isForAllFloating(IEventAssignmentBuilder assignmentBuilder) =>
         assignmentBuilder.ObjectPath.Contains(ObjectPathKeywords.ALL_FLOATING);
   }
}