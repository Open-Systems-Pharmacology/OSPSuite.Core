using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Mapper for the creation of a Transport in the model from <see cref="ITransportBuilder" />.
   /// </summary>
   internal interface ITransportBuilderToTransportMapper : IBuilderMapper<ITransportBuilder, ITransport>
   {
   }

   /// <summary>
   ///    Mapper for the creation of a Transport in the model from <see cref="ITransportBuilder" />.
   /// </summary>
   internal class TransportBuilderToTransportMapper : ITransportBuilderToTransportMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IParameterBuilderCollectionToParameterCollectionMapper _parameterMapper;
      private readonly IProcessRateParameterCreator _processRateParameterCreator;

      public TransportBuilderToTransportMapper(IObjectBaseFactory objectBaseFactory,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IParameterBuilderCollectionToParameterCollectionMapper parameterMapper,
         IProcessRateParameterCreator processRateParameterCreator)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
         _parameterMapper = parameterMapper;
         _processRateParameterCreator = processRateParameterCreator;
      }

      public ITransport MapFrom(ITransportBuilder transportBuilder, SimulationBuilder simulationBuilder)
      {
         var transport = _objectBaseFactory.Create<ITransport>()
            .WithName(transportBuilder.Name)
            .WithIcon(transportBuilder.Icon)
            .WithDimension(transportBuilder.Dimension)
            .WithFormula(_formulaMapper.MapFrom(transportBuilder.Formula, simulationBuilder));

         simulationBuilder.AddBuilderReference(transport, transportBuilder);

         addLocalParameters(transport, transportBuilder, simulationBuilder);

         //lastly, add parameter rate transporter if required
         if (transportBuilder.CreateProcessRateParameter)
            transport.Add(processRateParameterFor(transportBuilder, simulationBuilder));

         return transport;
      }

      private void addLocalParameters(ITransport transport, ITransportBuilder transportBuilder, SimulationBuilder simulationBuilder)
      {
         transport.AddChildren(_parameterMapper.MapLocalFrom(transportBuilder, simulationBuilder));
      }

      private IParameter processRateParameterFor(ITransportBuilder transportBuilder, SimulationBuilder simulationBuilder)
      {
         var parameter = _processRateParameterCreator.CreateProcessRateParameterFor(transportBuilder, simulationBuilder);

         parameter.AddTag(ObjectPathKeywords.MOLECULE);
         parameter.AddTag(ObjectPathKeywords.NEIGHBORHOOD);
         transportTypeTagsFor(transportBuilder).Each(parameter.AddTag);
         return parameter;
      }

      private IReadOnlyList<string> transportTypeTagsFor(ITransportBuilder transportBuilder)
      {
         if (!transportBuilder.TransportType.Is(TransportType.Active))
            return new[] {Constants.PASSIVE};

         var activeType = transportBuilder.TransportType.Is(TransportType.Influx) ? Constants.INFLUX : Constants.NOT_INFLUX;
         return new[] {Constants.ACTIVE, activeType};
      }
   }
}