using System.Linq;
using OSPSuite.Core.Domain.Builder;
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
      private readonly IEntityTracker _entityTracker;

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

         addAdditionalParentReference(parameter, processBuilder);

         _entityTracker.Track(parameter, processBuilder, simulationBuilder);

         if (processBuilder.ProcessRateParameterPersistable)
            parameter.Persistable = true;

         parameter.AddTag(processBuilder.Name);
         parameter.AddTag(Constants.Parameters.PROCESS_RATE);

         return parameter;
      }

      private void addAdditionalParentReference(IParameter parameter, IProcessBuilder processBuilder) => parameter.Formula.ObjectPaths.Each(x => adjustRelativePath(x, processBuilder));

      private void adjustRelativePath(ObjectPath objectPath, IProcessBuilder processBuilder)
      {
         if (!objectPath.Any())
            return;

         // if the path starts with the process name then we need to find out whether the resolved
         // parameter build mode is local or global mode. Local parameters have to be adjusted,
         // not global parameters
         if (objectPathResolvesLocalParameter(objectPath, processBuilder))
            objectPath.RemoveAt(0);

         // if the path starts with ".."  or if the objectPath only has the name of the reference then it
         // should be adjusted to have an additional ".." at the front
         if (objectPath[0] == ObjectPath.PARENT_CONTAINER || objectPath.Count == 1)
         {
            objectPath.AddAtFront(ObjectPath.PARENT_CONTAINER);
         }
      }

      private static bool objectPathResolvesLocalParameter(ObjectPath objectPath, IProcessBuilder processBuilder)
      {
         var resolvedParameter = objectPath.TryResolve<IParameter>(processBuilder);

         return isTwoElementPathBeginningWithProcessName(objectPath, processBuilder) && isLocalBuildMode(resolvedParameter);
      }

      private static bool isLocalBuildMode(IParameter parameter) => parameter != null && parameter.BuildMode == ParameterBuildMode.Local;

      private static bool isTwoElementPathBeginningWithProcessName(ObjectPath objectPath, IProcessBuilder processBuilder) => objectPath.Count == 2 && string.Equals(objectPath[0], processBuilder.Name);
   }
}