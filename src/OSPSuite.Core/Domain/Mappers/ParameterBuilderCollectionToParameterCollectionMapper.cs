using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Maps collection of ParameterBuilder-Objects into Collection of (Model-)parameters
   /// </summary>
   public interface IParameterBuilderCollectionToParameterCollectionMapper
   {
      /// <summary>
      ///    Only parameters having one of <paramref name="parameterBuildModesToMap" /> will be mapped or all if <paramref name="parameterBuildModesToMap" /> was not specified
      /// </summary>
      IEnumerable<IParameter> MapFrom(IEnumerable<IParameter> parameterBuilders, SimulationConfiguration simulationConfiguration, params ParameterBuildMode[] parameterBuildModesToMap);

      /// <summary>
      ///    Only direct children parameters  of <paramref name="container"/> having one of <paramref name="parameterBuildModesToMap" /> will be mapped or all if <paramref name="parameterBuildModesToMap" /> was not specified
      /// </summary>
      IEnumerable<IParameter> MapFrom(IContainer container, SimulationConfiguration simulationConfiguration, params ParameterBuildMode[] parameterBuildModesToMap);

      /// <summary>
      ///    Only direct children parameters  of <paramref name="container"/> of type <see cref="ParameterBuildMode.Global"/> or <seealso cref="ParameterBuildMode.Property"/> will be mapped
      /// </summary>
      IEnumerable<IParameter> MapGlobalOrPropertyFrom(IContainer container, SimulationConfiguration simulationConfiguration);

      /// <summary>
      ///    Only direct children parameters  of <paramref name="container"/> of type <see cref="ParameterBuildMode.Local"/> will be mapped
      /// </summary>
      IEnumerable<IParameter> MapLocalFrom(IContainer container, SimulationConfiguration simulationConfiguration);

      /// <summary>
      ///    All direct children parameters  of <paramref name="container"/>  will be mapped
      /// </summary>
      IEnumerable<IParameter> MapAllFrom(IContainer container, SimulationConfiguration simulationConfiguration);
   }

   public class ParameterBuilderCollectionToParameterCollectionMapper : IParameterBuilderCollectionToParameterCollectionMapper
   {
      private readonly IParameterBuilderToParameterMapper _parameterMapper;

      public ParameterBuilderCollectionToParameterCollectionMapper(IParameterBuilderToParameterMapper parameterMapper)
      {
         _parameterMapper = parameterMapper;
      }

      public IEnumerable<IParameter> MapFrom(IEnumerable<IParameter> parameterBuilders, SimulationConfiguration simulationConfiguration, params ParameterBuildMode[] parameterBuildModesToMap)
      {
         return parameterBuilders.Where(p => canBeMapped(p, parameterBuildModesToMap))
            .Select(parameterBuilder => _parameterMapper.MapFrom(parameterBuilder, simulationConfiguration)).ToList();
      }

      public IEnumerable<IParameter> MapFrom(IContainer container, SimulationConfiguration simulationConfiguration, params ParameterBuildMode[] parameterBuildModesToMap)
      {
         return MapFrom(container.GetChildren<IParameter>(), simulationConfiguration, parameterBuildModesToMap);
      }

      public IEnumerable<IParameter> MapGlobalOrPropertyFrom(IContainer container, SimulationConfiguration simulationConfiguration)
      {
         return MapFrom(container, simulationConfiguration, ParameterBuildMode.Global, ParameterBuildMode.Property);
      }

      public IEnumerable<IParameter> MapLocalFrom(IContainer container, SimulationConfiguration simulationConfiguration)
      {
         return MapFrom(container, simulationConfiguration, ParameterBuildMode.Local);
      }

      public IEnumerable<IParameter> MapAllFrom(IContainer container, SimulationConfiguration simulationConfiguration)
      {
         return MapFrom(container, simulationConfiguration);
      }

      private bool canBeMapped(IParameter parameter, params ParameterBuildMode[] parameterBuildModesToMap)
      {
         return !parameterBuildModesToMap.Any() || parameterBuildModesToMap.Any(buildModeToMap => parameter.BuildMode == buildModeToMap);
      }
   }
}