using System.Collections.Generic;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Mappers
{
   /// <summary>
   ///    Mapper for the creation of a Transport in the model from <see cref="TransportBuilder" />.
   /// </summary>
   internal interface ITransportBuilderToTransportMapper : IBuilderMapper<TransportBuilder, Transport>
   {
   }

   /// <summary>
   ///    Mapper for the creation of a Transport in the model from <see cref="TransportBuilder" />.
   /// </summary>
   internal class TransportBuilderToTransportMapper : ITransportBuilderToTransportMapper
   {
      private readonly IObjectBaseFactory _objectBaseFactory;
      private readonly IFormulaBuilderToFormulaMapper _formulaMapper;
      private readonly IParameterBuilderCollectionToParameterCollectionMapper _parameterMapper;
      private readonly IProcessRateParameterCreator _processRateParameterCreator;
      private readonly IObjectTracker _objectTracker;

      public TransportBuilderToTransportMapper(IObjectBaseFactory objectBaseFactory,
         IFormulaBuilderToFormulaMapper formulaMapper,
         IParameterBuilderCollectionToParameterCollectionMapper parameterMapper,
         IProcessRateParameterCreator processRateParameterCreator,
         IObjectTracker objectTracker)
      {
         _objectBaseFactory = objectBaseFactory;
         _formulaMapper = formulaMapper;
         _parameterMapper = parameterMapper;
         _processRateParameterCreator = processRateParameterCreator;
         _objectTracker = objectTracker;
      }

      public Transport MapFrom(TransportBuilder transportBuilder, SimulationBuilder simulationBuilder)
      {
         var transport = _objectBaseFactory.Create<Transport>()
            .WithName(transportBuilder.Name)
            .WithIcon(transportBuilder.Icon)
            .WithDimension(transportBuilder.Dimension)
            .WithFormula(_formulaMapper.MapFrom(transportBuilder.Formula, simulationBuilder));

         simulationBuilder.AddBuilderReference(transport, transportBuilder);
         _objectTracker.TrackObject(transport, transportBuilder, simulationBuilder);

         addLocalParameters(transport, transportBuilder, simulationBuilder);

         //lastly, add parameter rate transporter if required
         if (transportBuilder.CreateProcessRateParameter || simulationBuilder.CreateAllProcessRateParameters)
            transport.Add(processRateParameterFor(transportBuilder, simulationBuilder));

         return transport;
      }

      private void addLocalParameters(Transport transport, TransportBuilder transportBuilder, SimulationBuilder simulationBuilder)
      {
         transport.AddChildren(_parameterMapper.MapLocalFrom(transportBuilder, simulationBuilder));
      }

      private IParameter processRateParameterFor(TransportBuilder transportBuilder, SimulationBuilder simulationBuilder)
      {
         var parameter = _processRateParameterCreator.CreateProcessRateParameterFor(transportBuilder, simulationBuilder);

         parameter.AddTag(ObjectPathKeywords.MOLECULE);
         parameter.AddTag(ObjectPathKeywords.NEIGHBORHOOD);
         transportTypeTagsFor(transportBuilder).Each(parameter.AddTag);
         return parameter;
      }

      private IReadOnlyList<string> transportTypeTagsFor(TransportBuilder transportBuilder)
      {
         if (!transportBuilder.TransportType.Is(TransportType.Active))
            return new[] {Constants.PASSIVE};

         var activeType = transportBuilder.TransportType.Is(TransportType.Influx) ? Constants.INFLUX : Constants.NOT_INFLUX;
         return new[] {Constants.ACTIVE, activeType};
      }
   }
}