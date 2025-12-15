using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   internal interface IProcessRateParameterCreator
   {
      IParameter CreateProcessRateParameterFor(IProcessBuilder processBuilder, SimulationBuilder simulationBuilder);
   }

   internal class ProcessRateParameterCreator : IProcessRateParameterCreator
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IEntityTracker _entityTracker
         ;

      public ProcessRateParameterCreator(IObjectBaseFactory objectBaseFactory, IFormulaBuilderToFormulaMapper formulaMapper, IEntityTracker entityTracker)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
         _entityTracker = entityTracker;
      }

      public IParameter CreateProcessRateParameterFor(IProcessBuilder processBuilder, SimulationBuilder simulationBuilder)
      {
         var parameter = _objectBaseFactory
            .Create<IParameter>()
            .WithName(Constants.Parameters.PROCESS_RATE)
            .WithDimension(processBuilder.Dimension)
            .WithFormula(_formulaMapper.MapFrom(processBuilder.Formula, simulationBuilder));

         parameter.Visible = false;
         parameter.Editable = false;
         parameter.IsDefault = true;

         addAdditionalParentReference(parameter.Formula, processBuilder.Name);

         _entityTracker.Track(parameter, processBuilder, simulationBuilder);

         if (processBuilder.ProcessRateParameterPersistable)
            parameter.Persistable = true;

         parameter.AddTag(processBuilder.Name);
         parameter.AddTag(Constants.Parameters.PROCESS_RATE);

         return parameter;
      }

      private void addAdditionalParentReference(IFormula formula, string processName) => formula.ObjectPaths.Each(x => adjustRelativePath(x, processName));

      private void adjustRelativePath(ObjectPath objectPath, string processName)
      {
         if (!objectPath.Any())
            return;

         // if the path starts with ".."  or if the objectPath only has the name of the reference then it
         // should be adjusted to have an additional ".." at the front
         if (objectPath[0] == ObjectPath.PARENT_CONTAINER || objectPath.Count == 1)
         {
            objectPath.AddAtFront(ObjectPath.PARENT_CONTAINER);
            return;
         }

         // if the path contains two elements and the first one is the process name then we need to move up two containers at the front
         // to compensate. That's because a container name can be ignored when a path is resolved from within that container
         if (objectPath.Count == 2 && string.Equals(objectPath[0], processName))
         {
            objectPath.AddAtFront(new ObjectPath(ObjectPath.PARENT_CONTAINER, ObjectPath.PARENT_CONTAINER));
         }
      }
   }
}