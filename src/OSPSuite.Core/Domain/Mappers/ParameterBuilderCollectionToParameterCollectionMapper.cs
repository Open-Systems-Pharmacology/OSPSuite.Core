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
      ///    Only parameters having one of <paramref name="parameterBuildModesToMap" /> will be mapped or all if
      ///    <paramref name="parameterBuildModesToMap" /> was not specified
      /// </summary>
      IEnumerable<IParameter> MapFrom(IEnumerable<IParameter> parameterBuilders, SimulationBuilder simulationBuilder, params ParameterBuildMode[] parameterBuildModesToMap);

      /// <summary>
      ///    Only direct children parameters  of <paramref name="container" /> having one of
      ///    <paramref name="parameterBuildModesToMap" /> will be mapped or all if <paramref name="parameterBuildModesToMap" />
      ///    was not specified
      /// </summary>
      IEnumerable<IParameter> MapFrom(IContainer container, SimulationBuilder simulationBuilder, params ParameterBuildMode[] parameterBuildModesToMap);

      /// <summary>
      ///    Only direct children parameters  of <paramref name="container" /> of type <see cref="ParameterBuildMode.Global" /> will be mapped
      /// </summary>
      IEnumerable<IParameter> MapGlobalOrPropertyFrom(IContainer container, SimulationBuilder simulationBuilder);

      /// <summary>
      ///    Only direct children parameters  of <paramref name="container" /> of type <see cref="ParameterBuildMode.Local" />
      ///    will be mapped
      /// </summary>
      IEnumerable<IParameter> MapLocalFrom(IContainer container, SimulationBuilder simulationBuilder);

      /// <summary>
      ///    All direct children parameters  of <paramref name="container" />  will be mapped
      /// </summary>
      IEnumerable<IParameter> MapAllFrom(IContainer container, SimulationBuilder simulationBuilder);
   }

   internal class ParameterBuilderCollectionToParameterCollectionMapper : IParameterBuilderCollectionToParameterCollectionMapper
   {
      private readonly IParameterBuilderToParameterMapper _parameterMapper;

      public ParameterBuilderCollectionToParameterCollectionMapper(IParameterBuilderToParameterMapper parameterMapper)
      {
         _parameterMapper = parameterMapper;
      }

      public IEnumerable<IParameter> MapFrom(IEnumerable<IParameter> parameterBuilders, SimulationBuilder simulationBuilder, params ParameterBuildMode[] parameterBuildModesToMap)
      {
         return parameterBuilders.Where(p => canBeMapped(p, parameterBuildModesToMap))
            .Select(parameterBuilder => _parameterMapper.MapFrom(parameterBuilder, simulationBuilder)).ToList();
      }

      public IEnumerable<IParameter> MapFrom(IContainer container, SimulationBuilder simulationBuilder, params ParameterBuildMode[] parameterBuildModesToMap)
      {
         return MapFrom(container.GetChildren<IParameter>(), simulationBuilder, parameterBuildModesToMap);
      }

      public IEnumerable<IParameter> MapGlobalOrPropertyFrom(IContainer container, SimulationBuilder simulationBuilder)
      {
         return MapFrom(container, simulationBuilder, ParameterBuildMode.Global);
      }

      public IEnumerable<IParameter> MapLocalFrom(IContainer container, SimulationBuilder simulationBuilder)
      {
         return MapFrom(container, simulationBuilder, ParameterBuildMode.Local);
      }

      public IEnumerable<IParameter> MapAllFrom(IContainer container, SimulationBuilder simulationBuilder)
      {
         return MapFrom(container, simulationBuilder);
      }

      private bool canBeMapped(IParameter parameter, params ParameterBuildMode[] parameterBuildModesToMap)
      {
         return !parameterBuildModesToMap.Any() || parameterBuildModesToMap.Any(buildModeToMap => parameter.BuildMode == buildModeToMap);
      }
   }
}