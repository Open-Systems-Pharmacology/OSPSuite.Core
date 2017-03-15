using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core.Domain.Services
{
   public interface ITransportCreator
   {
      void CreatePassiveTransport(IModel model, ITransportBuilder passiveTransportBuilder, IBuildConfiguration buildConfiguration);
      void CreateActiveTransport(IModel model, IBuildConfiguration buildConfiguration);
   }

   public class TransportCreator : ITransportCreator
   {
      private readonly ITransportBuilderToTransportMapper _transportMapper;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly IMoleculePropertiesContainerTask _moleculePropertiesContainerTask;

      public TransportCreator(ITransportBuilderToTransportMapper transportMapper, IKeywordReplacerTask keywordReplacerTask,
         IMoleculePropertiesContainerTask moleculePropertiesContainerTask)
      {
         _transportMapper = transportMapper;
         _keywordReplacerTask = keywordReplacerTask;
         _moleculePropertiesContainerTask = moleculePropertiesContainerTask;
      }

      public void CreatePassiveTransport(IModel model, ITransportBuilder passiveTransportBuilder, IBuildConfiguration buildConfiguration)
      {
         var allNeighborhoods = model.Neighborhoods.GetChildren<INeighborhood>().ToList();

         foreach (var molecule in buildConfiguration.Molecules.AllFloating())
         {
            addPassiveTransportToModel(model, passiveTransportBuilder, allNeighborhoods, molecule, buildConfiguration);
         }
      }

      public void CreateActiveTransport(IModel model, IBuildConfiguration buildConfiguration)
      {
         var allNeighborhoods = model.Neighborhoods.GetChildren<INeighborhood>().ToList();
         var molecules = buildConfiguration.Molecules;
         foreach (var molecule in molecules.AllFloating())
         {
            foreach (var transporterMolecule in molecule.TransporterMoleculeContainerCollection)
            {
               var transporter = molecules[transporterMolecule.Name];
               // transporter not available in molecules (can happen when swapping building block)
               if (transporter == null)
                  continue;

               foreach (var activeTransport in transporterMolecule.ActiveTransportRealizations)
               {
                  addActiveTransportToModel(model, activeTransport, allNeighborhoods, molecule, transporterMolecule, buildConfiguration);
               }
            }
         }
      }

      private void addPassiveTransportToModel(IModel model, ITransportBuilder passiveTransportBuilder, IEnumerable<INeighborhood> allNeighborhoods, IMoleculeBuilder molecule, IBuildConfiguration buildConfiguration)
      {
         // first check if the molecule should be transported
         if (!passiveTransportBuilder.TransportsMolecule(molecule.Name))
            return;

         var neighborhoods = getNeigborhoodsForPassiveTransport(passiveTransportBuilder, allNeighborhoods, molecule.Name);

         foreach (var neighborhood in neighborhoods)
         {
            var passiveTransport = mapFrom(passiveTransportBuilder, neighborhood, molecule.Name, buildConfiguration);
            addPassiveTransportToNeighborhood(neighborhood, molecule.Name, passiveTransport);
            _keywordReplacerTask.ReplaceIn(passiveTransport, model.Root, molecule.Name, neighborhood);
         }
      }

      private void addActiveTransportToModel(IModel model, ITransportBuilder activeTransportBuilder, IEnumerable<INeighborhood> allNeighborhoods, IMoleculeBuilder molecule, TransporterMoleculeContainer transporterMolecule, IBuildConfiguration buildConfiguration)
      {
         var neighborhoods = getNeigborhoodsForActiveTransport(activeTransportBuilder, allNeighborhoods, molecule.Name, transporterMolecule.Name);

         foreach (var neighborhood in neighborhoods)
         {
            var activeTransport = mapFrom(activeTransportBuilder, neighborhood, molecule.Name, buildConfiguration);
            var activeTransportInMolecule = addActiveTransportToNeighborhood(neighborhood, activeTransport, transporterMolecule, molecule.Name, buildConfiguration);
            buildConfiguration.AddBuilderReference(activeTransportInMolecule, activeTransportBuilder);
            _keywordReplacerTask.ReplaceIn(activeTransport, model.Root, molecule.Name, neighborhood, transporterMolecule.TransportName, transporterMolecule.Name);
         }
      }

      private IEnumerable<INeighborhood> getNeigborhoodsForActiveTransport(ITransportBuilder transport, IEnumerable<INeighborhood> allNeighborhoods, string moleculeName, string transporterMoleculeName)
      {
         try
         {
            return (from useNeighborhoods in getNeighborhoodsByNeighborCriteria(allNeighborhoods, transport.SourceCriteria, transport.TargetCriteria, moleculeName)
               where useNeighborhoods.GetNeighborSatisfying(transport.SourceCriteria).GetSingleChildByName<IMoleculeAmount>(transporterMoleculeName) != null
               select useNeighborhoods).ToList();
         }
         catch (BothNeighborsSatisfyingCriteriaException exception)
         {
            throw new OSPSuiteException(Error.BothNeighborsSatisfyingForTransport(exception.Message, transport.Name));
         }
      }

      private IEnumerable<INeighborhood> getNeigborhoodsForPassiveTransport(ITransportBuilder passiveTransportBuilder, IEnumerable<INeighborhood> allNeighborhoods, string moleculeName)
      {
         return getNeighborhoodsByNeighborCriteria(allNeighborhoods, passiveTransportBuilder.SourceCriteria, passiveTransportBuilder.TargetCriteria, moleculeName);
      }

      private IEnumerable<INeighborhood> getNeighborhoodsByNeighborCriteria(IEnumerable<INeighborhood> neighborhoods, DescriptorCriteria conditionsForOneNeighbor,
         DescriptorCriteria conditionsForTheOtherNeighbor, string name)
      {
         return getNeighborhoodsByNeighborCriteria(neighborhoods, conditionsForOneNeighbor, conditionsForTheOtherNeighbor, new[] {name});
      }

      private IEnumerable<INeighborhood> getNeighborhoodsByNeighborCriteria(IEnumerable<INeighborhood> neighborhoods, DescriptorCriteria conditionsForOneNeighbor,
         DescriptorCriteria conditionsForTheOtherNeighbor, IEnumerable<string> moleculeNames)
      {
         return from neighborhood in neighborhoods
            where neighborhood.Satisfies(conditionsForOneNeighbor, conditionsForTheOtherNeighbor)
            where neighborhood.FirstNeighbor.ContainsNames(moleculeNames)
            where neighborhood.SecondNeighbor.ContainsNames(moleculeNames)
            select neighborhood;
      }

      private IContainer addPassiveTransportToNeighborhood(INeighborhood neighborhood, string moleculeName, ITransport transport)
      {
         return _moleculePropertiesContainerTask.NeighborhoodMoleculeContainerFor(neighborhood, moleculeName)
            .WithChild(transport);
      }

      private IContainer addActiveTransportToNeighborhood(INeighborhood neighborhood, ITransport transport, TransporterMoleculeContainer transporterMolecule, string transportedMoleculeName, IBuildConfiguration buildConfiguration)
      {
         return _moleculePropertiesContainerTask.NeighborhoodMoleculeTransportContainerFor(neighborhood, transportedMoleculeName, transporterMolecule, transport.Name, buildConfiguration)
            .WithChild(transport);
      }

      private ITransport mapFrom(ITransportBuilder transportBuilder, INeighborhood neighborhood, string moleculeName, IBuildConfiguration buildConfiguration)
      {
         var transport = _transportMapper.MapFrom(transportBuilder, buildConfiguration);
         transport.SourceAmount = neighborhood.GetNeighborSatisfying(transportBuilder.SourceCriteria).GetSingleChildByName<IMoleculeAmount>(moleculeName);
         transport.TargetAmount = neighborhood.GetNeighborSatisfying(transportBuilder.TargetCriteria).GetSingleChildByName<IMoleculeAmount>(moleculeName);
         return transport;
      }
   }
}