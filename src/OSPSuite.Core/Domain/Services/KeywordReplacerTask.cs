using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;
using static OSPSuite.Core.Domain.ObjectPathKeywords;

namespace OSPSuite.Core.Domain.Services
{
   public interface IKeywordReplacerTask
   {
      /// <summary>
      ///    Replace the keywords used in the reaction (parameters and kinetic) with the appropriate names from the root
      ///    container
      /// </summary>
      void ReplaceInReactionContainer(IContainer reactionContainer, IContainer rootContainer);

      /// <summary>
      ///    Replace the keywords used in the observer formula with the appropriate names from the root container.
      ///    The Molecule keyword will also be replaced with the moleculeName
      /// </summary>
      void ReplaceIn(IObserver observer, IContainer rootContainer, string moleculeName);

      /// <summary>
      ///    Replace the keywords used in the observer formula with the appropriate names from the root container.
      ///    The Molecule keyword will also be replaced with the moleculeName
      ///    The Neighborhood keywords will also be replaced by the names from the neighborhood
      /// </summary>
      void ReplaceIn(IObserver observer, IContainer rootContainer, string moleculeName, Neighborhood neighborhood);

      /// <summary>
      ///    Replace the keywords used in the neighborhoods entities with the appropriate names from the root container.
      ///    The Neighborhood keywords will also be replaced by the names from the neighborhood
      /// </summary>
      void ReplaceIn(Neighborhood neighborhood, IContainer rootContainer);

      /// <summary>
      ///    Replace the keywords used in the event transport kinetic with the appropriate names from the root container.
      ///    The Molecule keyword will also be replaced with the moleculeName
      /// </summary>
      void ReplaceIn(ITransport eventTransport, IContainer rootContainer, string moleculeName);

      /// <summary>
      ///    Replace the keywords used in the passive transport kinetic with the appropriate names from the root container.
      ///    The Molecule keyword will also be replaced with the moleculeName
      ///    The Neighborhood keywords will also be replaced by the names from the neighborhood
      /// </summary>
      void ReplaceIn(ITransport passiveTransport, IContainer rootContainer, string moleculeName, Neighborhood neighborhood);

      /// <summary>
      ///    Replace the keywords used in the active transport kinetic with the appropriate names from the root container.
      ///    The Molecule keyword will also be replaced with the moleculeName
      ///    The Neighborhood keywords will also be replaced by the names from the neighborhood
      ///    The keywords TRANSPORT, SOURCE and TARGET will also be replaced by the names from the transporter and transport
      /// </summary>
      void ReplaceIn(ITransport realization, IContainer rootContainer, string moleculeName, Neighborhood neighborhood, string transportName,
         string transporterName);

      /// <summary>
      ///    Replace the keywords used in the event group and all the defined event with the appropriate names from the root
      ///    container.
      /// </summary>
      void ReplaceIn(IEventGroup eventGroup, IContainer rootContainer, IEventGroupBuilder eventGroupBuilder, IMoleculeBuildingBlock molecules);

      /// <summary>
      ///    Create a new object path based on the given object path where the keyword have been replaced with the appropriate
      ///    names from the root container
      /// </summary>
      ObjectPath CreateModelPathFor(ObjectPath objectPath, IContainer rootContainer);

      /// <summary>
      ///    Replaces recursively the keywords used in formula defined in neighborhoods and UsingEntityFormula with the
      ///    appropriate names from the root container
      /// </summary>
      /// <param name="rootContainer">root container of model</param>
      void ReplaceIn(IContainer rootContainer);

      void ReplaceIn(IContainer moleculePropertiesContainer, IContainer rootContainer, string moleculeName);

      void ReplaceIn(IParameter parameter, IContainer rootContainer);
      void ReplaceIn(IParameter parameter, IContainer rootContainer, string moleculeName);
      void ReplaceIn(IMoleculeAmount moleculeAmount, IContainer rootContainer);

      void ReplaceIn(IMoleculeAmount moleculeAmount);
   }

   internal class KeywordReplacerTask : IKeywordReplacerTask,
      IVisitor<Neighborhood>,
      IVisitor<IUsingFormula>,
      IVisitor<IMoleculeAmount>
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private IContainer _rootContainer;

      public KeywordReplacerTask(IObjectPathFactory objectPathFactory)
      {
         _objectPathFactory = objectPathFactory;
      }

      public void ReplaceInReactionContainer(IContainer reactionContainer, IContainer rootContainer)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, rootContainer);
         var reaction = reactionContainer as IReaction;
         if (reaction != null)
            keywordReplacer.ReplaceIn(reaction);

         replaceInContainer(reactionContainer, rootContainer);
      }

      public void ReplaceIn(IObserver observer, IContainer rootContainer, string moleculeName)
      {
         ReplaceIn(observer, rootContainer, moleculeName, null);
      }

      public void ReplaceIn(Neighborhood neighborhood, IContainer rootContainer)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, rootContainer);
         addCommonNeighborhoodReplacersTo(keywordReplacer, neighborhood);
         neighborhood.GetChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
         neighborhood.GetChildren<IContainer>().Each(x => replaceWithMoleculeKeywords(keywordReplacer, x, x.Name));
      }

      public void ReplaceIn(ITransport eventTransport, IContainer rootContainer, string moleculeName)
      {
         ReplaceIn(eventTransport, rootContainer, moleculeName, null);
      }

      public void ReplaceIn(ITransport passiveTransport, IContainer rootContainer, string moleculeName, Neighborhood neighborhood)
      {
         ReplaceIn(passiveTransport, rootContainer, moleculeName, neighborhood, null, null);
      }

      public void ReplaceIn(ITransport realization, IContainer rootContainer, string moleculeName, Neighborhood neighborhood, string transportName,
         string transporterName)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, rootContainer);
         addMoleculeReplacersTo(keywordReplacer, moleculeName);
         addCommonNeighborhoodReplacersTo(keywordReplacer, neighborhood);

         keywordReplacer.AddReplacement(new KeywordWithPathReplacer(SOURCE,
            _objectPathFactory.CreateAbsoluteObjectPath(realization.SourceAmount.ParentContainer)));
         keywordReplacer.AddReplacement(new KeywordWithPathReplacer(TARGET,
            _objectPathFactory.CreateAbsoluteObjectPath(realization.TargetAmount.ParentContainer)));
         keywordReplacer.AddReplacement(new KeywordWithPathReplacer(REALIZATION, new ObjectPath(transportName, realization.Name)));
         keywordReplacer.AddReplacement(new SimpleKeywordReplacer(TRANSPORT, transportName));
         keywordReplacer.AddReplacement(new SimpleKeywordReplacer(TRANSPORTER, transporterName));
         keywordReplacer.ReplaceIn(realization);
         replaceInContainer(realization, rootContainer);

         //replaceInContainer only replaces standard keywords. Transport specific keywords need to be replaced in all children explicitly
         var transportContainer = realization.ParentContainer ?? realization;
         transportContainer.GetAllChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
      }

      public void ReplaceIn(IEventGroup eventGroup, IContainer rootContainer, IEventGroupBuilder eventGroupBuilder, IMoleculeBuildingBlock molecules)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, rootContainer);

         var applicationBuilder = eventGroupBuilder as IApplicationBuilder;
         if (applicationBuilder != null)
         {
            addMoleculeReplacersTo(keywordReplacer, applicationBuilder.MoleculeName);
         }

         replaceInEventGroup(eventGroup, keywordReplacer);
      }

      private void replaceInEventGroup(IEventGroup eventGroup, KeywordReplacerCollection keywordReplacer)
      {
         eventGroup.GetAllChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
         eventGroup.GetAllChildren<IEventAssignment>().Select(x => x.ObjectPath).Each(keywordReplacer.ReplaceIn);
      }

      public ObjectPath CreateModelPathFor(ObjectPath objectPath, IContainer rootContainer)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, rootContainer);
         var modelPath = objectPath.Clone<ObjectPath>();
         keywordReplacer.ReplaceIn(modelPath);
         return modelPath;
      }

      public void ReplaceIn(IContainer rootContainer) => replaceInContainer(rootContainer, rootContainer);

      private void replaceInContainer(IContainer container, IContainer rootContainer)
      {
         try
         {
            _rootContainer = rootContainer;
            container.AcceptVisitor(this);
         }
         finally
         {
            _rootContainer = null;
         }
      }

      public void ReplaceIn(IContainer moleculePropertiesContainer, IContainer rootContainer, string moleculeName)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, rootContainer);
         addMoleculeReplacersTo(keywordReplacer, moleculeName);
         moleculePropertiesContainer.GetAllChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
      }

      private void replaceWithMoleculeKeywords(IEnumerable<IKeywordReplacer> generalKeywordReplacer, IContainer container, string moleculeName)
      {
         var keywordReplacer = new KeywordReplacerCollection(generalKeywordReplacer);
         addMoleculeReplacersTo(keywordReplacer, moleculeName);
         container.GetChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
      }

      public void ReplaceIn(IObserver observer, IContainer rootContainer, string moleculeName, Neighborhood neighborhood)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, rootContainer);
         addCommonNeighborhoodReplacersTo(keywordReplacer, neighborhood);
         addMoleculeReplacersTo(keywordReplacer, moleculeName);
         keywordReplacer.ReplaceIn(observer);
      }

      private void addCommonModelReplacersTo(IKeywordReplacerCollection keywordReplacer, IContainer rootContainer)
      {
         //Replace the predefined keywords 
         keywordReplacer.AddReplacement(new TopContainerPathReplacer(rootContainer.Name, rootContainer.GetChildren<IContainer>().AllNames()));
         keywordReplacer.AddReplacement(new TopContainerPathReplacer(rootContainer.Name, new[] {MOLECULE, Constants.NEIGHBORHOODS}));
      }

      private void addCommonNeighborhoodReplacersTo(IKeywordReplacerCollection keywordReplacer, Neighborhood neighborhood)
      {
         if (neighborhood == null) return;
         keywordReplacer.AddReplacement(new KeywordWithPathReplacer(FIRST_NEIGHBOR, _objectPathFactory.CreateAbsoluteObjectPath(neighborhood.FirstNeighbor)));
         keywordReplacer.AddReplacement(new KeywordWithPathReplacer(SECOND_NEIGHBOR, _objectPathFactory.CreateAbsoluteObjectPath(neighborhood.SecondNeighbor)));
         keywordReplacer.AddReplacement(new KeywordWithPathReplacer(NEIGHBORHOOD, _objectPathFactory.CreateAbsoluteObjectPath(neighborhood)));
         //should be placed after the KeywordWithPathReplacer so that NEIGHBORHOOD is only replaced if not found yet
         keywordReplacer.AddReplacement(new SimpleKeywordReplacer(NEIGHBORHOOD, neighborhood.Name));
      }

      private void addMoleculeReplacersTo(IKeywordReplacerCollection keywordReplacer, string moleculeName)
      {
         if (string.IsNullOrEmpty(moleculeName)) return;
         keywordReplacer.AddReplacement(new SimpleKeywordReplacer(MOLECULE, moleculeName));
      }

      private void replaceInUsingFormula(IUsingFormula usingFormula, IContainer rootContainer)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, rootContainer);
         keywordReplacer.ReplaceIn(usingFormula);
      }

      public void ReplaceIn(IParameter parameter, IContainer rootContainer)
      {
         var parameterContainer = parameter.ParentContainer;

         //Global molecule container or local molecule amount container
         var isInMolecule = parameterContainer.IsAnImplementationOf<IMoleculeAmount>() ||
                            parameterContainer.ContainerType == ContainerType.Molecule;

         ReplaceIn(parameter, rootContainer, isInMolecule ? parameterContainer.Name : string.Empty);
      }

      public void ReplaceIn(IParameter parameter, IContainer rootContainer, string moleculeName)
      {
         replaceIn(parameter, rootContainer, moleculeName);
      }

      public void ReplaceIn(IMoleculeAmount moleculeAmount, IContainer rootContainer)
      {
         replaceIn(moleculeAmount, rootContainer, moleculeAmount.Name);
         replaceIn(moleculeAmount.GetSingleChildByName<IParameter>(Constants.Parameters.START_VALUE), rootContainer, moleculeAmount.Name);
      }

      private void replaceIn(IUsingFormula usingFormula, IContainer rootContainer, string moleculeName)
      {
         if (usingFormula == null) return;
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, rootContainer);
         addMoleculeReplacersTo(keywordReplacer, moleculeName);
         keywordReplacer.ReplaceIn(usingFormula);
      }

      public void ReplaceIn(IMoleculeAmount moleculeAmount)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addMoleculeReplacersTo(keywordReplacer, moleculeAmount.Name);
         moleculeAmount.GetAllChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
      }

      public void Visit(Neighborhood neighborhood)
      {
         ReplaceIn(neighborhood, _rootContainer);
      }

      public void Visit(IUsingFormula usingFormula)
      {
         replaceInUsingFormula(usingFormula, _rootContainer);
      }

      public void Visit(IMoleculeAmount moleculeAmount)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, _rootContainer);
         addMoleculeReplacersTo(keywordReplacer, moleculeAmount.Name);
         keywordReplacer.ReplaceIn(moleculeAmount);
      }
   }
}