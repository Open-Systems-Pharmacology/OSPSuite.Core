using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain.Services
{
   internal interface ITransportCreator
   {
      void CreatePassiveTransport(TransportBuilder passiveTransportBuilder, ModelConfiguration modelConfiguration);
      void CreateActiveTransport(ModelConfiguration modelConfiguration);
   }

   internal class TransportCreator : ITransportCreator
   {
      private readonly ITransportBuilderToTransportMapper _transportMapper;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly IMoleculePropertiesContainerTask _moleculePropertiesContainerTask;
      private readonly IEntityTracker _entityTracker;

      public TransportCreator(ITransportBuilderToTransportMapper transportMapper, IKeywordReplacerTask keywordReplacerTask,
         IMoleculePropertiesContainerTask moleculePropertiesContainerTask, IEntityTracker entityTracker)
      {
         _transportMapper = transportMapper;
         _keywordReplacerTask = keywordReplacerTask;
         _moleculePropertiesContainerTask = moleculePropertiesContainerTask;
         _entityTracker = entityTracker;
      }

      public void CreatePassiveTransport(TransportBuilder passiveTransportBuilder, ModelConfiguration modelConfiguration)
      {
         var (model, simulationConfiguration) = modelConfiguration;
         var allNeighborhoods = model.Neighborhoods.GetChildren<Neighborhood>().ToList();

         foreach (var molecule in simulationConfiguration.AllFloatingMolecules())
         {
            addPassiveTransportToModel(passiveTransportBuilder, allNeighborhoods, molecule, modelConfiguration);
         }
      }

      public void CreateActiveTransport(ModelConfiguration modelConfiguration)
      {
         var (model, simulationBuilder) = modelConfiguration;
         var allNeighborhoods = model.Neighborhoods.GetChildren<Neighborhood>().ToList();
         foreach (var molecule in simulationBuilder.AllFloatingMolecules())
         {
            foreach (var transporterMolecule in molecule.TransporterMoleculeContainerCollection)
            {
               var transporter = simulationBuilder.MoleculeByName(transporterMolecule.Name);
               // transporter not available in molecules (can happen when swapping building block)
               if (transporter == null)
                  continue;

               foreach (var activeTransport in transporterMolecule.ActiveTransportRealizations)
               {
                  addActiveTransportToModel(activeTransport, allNeighborhoods, molecule, transporterMolecule, modelConfiguration);
               }
            }
         }
      }

      private void addPassiveTransportToModel(TransportBuilder passiveTransportBuilder, IEnumerable<Neighborhood> allNeighborhoods,
         MoleculeBuilder molecule, ModelConfiguration modelConfiguration)
      {
         var (_, simulationBuilder, replacementContext) = modelConfiguration;
         // first check if the molecule should be transported
         if (!simulationBuilder.MoleculeListFor(passiveTransportBuilder).Uses(molecule.Name))
            return;

         var neighborhoods = getNeighborhoodsForPassiveTransport(passiveTransportBuilder, allNeighborhoods, molecule.Name);

         foreach (var neighborhood in neighborhoods)
         {
            var passiveTransport = mapFrom(passiveTransportBuilder, neighborhood, molecule.Name, simulationBuilder);
            addPassiveTransportToNeighborhood(neighborhood, molecule.Name, passiveTransport);
            _keywordReplacerTask.ReplaceIn(passiveTransport, molecule.Name, neighborhood, replacementContext);
         }
      }

      private void addActiveTransportToModel(TransportBuilder activeTransportBuilder, IEnumerable<Neighborhood> allNeighborhoods,
         MoleculeBuilder molecule, TransporterMoleculeContainer transporterMolecule, ModelConfiguration modelConfiguration)
      {
         var (model, simulationBuilder, replacementContext) = modelConfiguration;
         var neighborhoods = getNeighborhoodsForActiveTransport(activeTransportBuilder, allNeighborhoods, molecule.Name, transporterMolecule.Name);

         foreach (var neighborhood in neighborhoods)
         {
            var activeTransport = mapFrom(activeTransportBuilder, neighborhood, molecule.Name, simulationBuilder);
            var activeTransportInMolecule =
               addActiveTransportToNeighborhood(neighborhood, activeTransport, transporterMolecule, molecule.Name, simulationBuilder);

            _entityTracker.Track(activeTransportInMolecule, activeTransportBuilder, simulationBuilder);
            _keywordReplacerTask.ReplaceIn(activeTransport, molecule.Name, neighborhood, transporterMolecule.TransportName, transporterMolecule.Name, replacementContext);
         }
      }

      private IEnumerable<Neighborhood> getNeighborhoodsForActiveTransport(TransportBuilder transport, IEnumerable<Neighborhood> allNeighborhoods,
         string moleculeName, string transporterName)
      {
         try
         {
            //Transporter can either be defined in source or target container. Therefore we need to check that at least ONE neighbor has a transporter instance
            return getNeighborhoodsByNeighborCriteria(allNeighborhoods, transport.SourceCriteria, transport.TargetCriteria, moleculeName)
               .Where(x =>
                  x.GetNeighborSatisfying(transport.SourceCriteria).GetSingleChildByName<MoleculeAmount>(transporterName) != null ||
                  x.GetNeighborSatisfying(transport.TargetCriteria).GetSingleChildByName<MoleculeAmount>(transporterName) != null)
               .ToList();
         }
         catch (BothNeighborsSatisfyingCriteriaException exception)
         {
            throw new OSPSuiteException(Error.BothNeighborsSatisfyingForTransport(exception.Message, transport.Name));
         }
      }

      private IEnumerable<Neighborhood> getNeighborhoodsForPassiveTransport(TransportBuilder passiveTransportBuilder, IEnumerable<Neighborhood> allNeighborhoods, string moleculeName)
      {
         return getNeighborhoodsByNeighborCriteria(allNeighborhoods, passiveTransportBuilder.SourceCriteria, passiveTransportBuilder.TargetCriteria, moleculeName);
      }

      private IEnumerable<Neighborhood> getNeighborhoodsByNeighborCriteria(IEnumerable<Neighborhood> neighborhoods,
         DescriptorCriteria conditionsForOneNeighbor,
         DescriptorCriteria conditionsForTheOtherNeighbor, string name)
      {
         return getNeighborhoodsByNeighborCriteria(neighborhoods, conditionsForOneNeighbor, conditionsForTheOtherNeighbor, new[] {name});
      }

      private IEnumerable<Neighborhood> getNeighborhoodsByNeighborCriteria(IEnumerable<Neighborhood> neighborhoods,
         DescriptorCriteria conditionsForOneNeighbor,
         DescriptorCriteria conditionsForTheOtherNeighbor, IEnumerable<string> moleculeNames)
      {
         return from neighborhood in neighborhoods
            where neighborhood.Satisfies(conditionsForOneNeighbor, conditionsForTheOtherNeighbor)
            where neighborhood.FirstNeighbor.ContainsNames(moleculeNames)
            where neighborhood.SecondNeighbor.ContainsNames(moleculeNames)
            select neighborhood;
      }

      private IContainer addPassiveTransportToNeighborhood(Neighborhood neighborhood, string moleculeName, Transport transport)
      {
         return _moleculePropertiesContainerTask.NeighborhoodMoleculeContainerFor(neighborhood, moleculeName)
            .WithChild(transport);
      }

      private IContainer addActiveTransportToNeighborhood(Neighborhood neighborhood, Transport transport,
         TransporterMoleculeContainer transporterMolecule, string transportedMoleculeName, SimulationBuilder simulationBuilder)
      {
         return _moleculePropertiesContainerTask
            .NeighborhoodMoleculeTransportContainerFor(neighborhood, transportedMoleculeName, transporterMolecule, transport.Name, simulationBuilder)
            .WithChild(transport);
      }

      private Transport mapFrom(TransportBuilder transportBuilder, Neighborhood neighborhood, string moleculeName,
         SimulationBuilder simulationBuilder)
      {
         var transport = _transportMapper.MapFrom(transportBuilder, simulationBuilder);
         transport.SourceAmount = neighborhood.GetNeighborSatisfying(transportBuilder.SourceCriteria)
            .GetSingleChildByName<MoleculeAmount>(moleculeName);

         transport.TargetAmount = neighborhood.GetNeighborSatisfying(transportBuilder.TargetCriteria)
            .GetSingleChildByName<MoleculeAmount>(moleculeName);
         return transport;
      }
   }
}