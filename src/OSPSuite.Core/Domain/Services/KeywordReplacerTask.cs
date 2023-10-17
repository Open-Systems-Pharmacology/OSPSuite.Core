using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants.Parameters;
using static OSPSuite.Core.Domain.ObjectPathKeywords;

namespace OSPSuite.Core.Domain.Services
{
   public class ReplacementContext
   {
      public IContainer RootContainer { get; }
      public IReadOnlyList<string> TopContainerNames { get; set; }
      public string RootContainerName => RootContainer.Name;

      public ReplacementContext(IModel model) : this(model.Root)
      {
      }

      public ReplacementContext(IContainer rootContainer)
      {
         RootContainer = rootContainer;
         TopContainerNames = rootContainer?.GetChildren<IContainer>().AllNames();
      }
   }

   public interface IKeywordReplacerTask
   {
      /// <summary>
      ///    Replace the keywords used in the reaction (parameters and kinetic) with the appropriate names from the root
      ///    container
      /// </summary>
      void ReplaceInReactionContainer(IContainer reactionContainer, ReplacementContext replacementContext);

      /// <summary>
      ///    Replace the keywords used in the root container of a spatial structure (all formulas as well as neighborhoods)
      ///    container
      /// </summary>
      void ReplaceInSpatialStructure(IContainer rootContainer, ReplacementContext replacementContext);

      /// <summary>
      ///    Replace the keywords used in the observer formula with the appropriate names from the root container.
      ///    The Molecule keyword will also be replaced with the moleculeName
      /// </summary>
      void ReplaceIn(Observer observer, string moleculeName, ReplacementContext replacementContext);

      /// <summary>
      ///    Replace the keywords used in the neighborhoods entities with the appropriate names from the root container.
      ///    The Neighborhood keywords will also be replaced by the names from the neighborhood
      /// </summary>
      void ReplaceIn(Neighborhood neighborhood, ReplacementContext replacementContext);

      /// <summary>
      ///    Replace the keywords used in the event transport kinetic with the appropriate names from the root container.
      ///    The Molecule keyword will also be replaced with the moleculeName
      /// </summary>
      void ReplaceIn(Transport transport, string moleculeName, ReplacementContext replacementContext);

      /// <summary>
      ///    Replace the keywords used in the passive transport kinetic with the appropriate names from the root container.
      ///    The Molecule keyword will also be replaced with the moleculeName
      ///    The Neighborhood keywords will also be replaced by the names from the neighborhood
      /// </summary>
      void ReplaceIn(Transport passiveTransport, string moleculeName, Neighborhood neighborhood, ReplacementContext replacementContext);

      /// <summary>
      ///    Replace the keywords used in the active transport kinetic with the appropriate names from the root container.
      ///    The Molecule keyword will also be replaced with the moleculeName
      ///    The Neighborhood keywords will also be replaced by the names from the neighborhood
      ///    The keywords TRANSPORT, SOURCE and TARGET will also be replaced by the names from the transporter and transport
      /// </summary>
      void ReplaceIn(Transport realization, string moleculeName, Neighborhood neighborhood, string transportName,
         string transporterName, ReplacementContext replacementContext);

      /// <summary>
      ///    Replace the keywords used in the event group and all the defined event with the appropriate names from the root
      ///    container.
      /// </summary>
      void ReplaceIn(EventGroup eventGroup, EventGroupBuilder eventGroupBuilder, ReplacementContext replacementContext);

      /// <summary>
      ///    Create a new object path based on the given object path where the keyword have been replaced with the appropriate
      ///    names from the root container
      /// </summary>
      ObjectPath CreateModelPathFor(ObjectPath objectPath, ReplacementContext replacementContext);

      /// <summary>
      ///    Replaces recursively the keywords used in formula defined in neighborhoods and UsingEntityFormula with the
      ///    appropriate names from the root container of the model configuration
      /// </summary>
      void ReplaceIn(ModelConfiguration modelConfiguration);

      void ReplaceIn(IContainer moleculePropertiesContainer, string moleculeName, ReplacementContext replacementContext);

      void ReplaceIn(IParameter parameter, ReplacementContext replacementContext);
      void ReplaceIn(IParameter parameter, string moleculeName, ReplacementContext replacementContext);
      void ReplaceIn(MoleculeAmount moleculeAmount, ReplacementContext replacementContext);
   }

   internal class KeywordReplacerTask : IKeywordReplacerTask
   {
      private readonly IObjectPathFactory _objectPathFactory;

      public KeywordReplacerTask(IObjectPathFactory objectPathFactory)
      {
         _objectPathFactory = objectPathFactory;
      }

      public void ReplaceInReactionContainer(IContainer reactionContainer, ReplacementContext replacementContext)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, replacementContext);
         var reaction = reactionContainer as Reaction;
         if (reaction != null)
            keywordReplacer.ReplaceIn(reaction);

         replaceInAllUsingFormulasDefinedIn(reactionContainer, replacementContext);
      }

      public void ReplaceIn(ModelConfiguration modelConfiguration)
      {
         //When we validate the whole configuration, we update the replacement context to make sure that the replacement context is up to date
         modelConfiguration.UpdateReplacementContext();

         var (model, _, replacementContext) = modelConfiguration;
         ReplaceInSpatialStructure(model.Root, replacementContext);
      }

      public void ReplaceInSpatialStructure(IContainer rootContainer, ReplacementContext replacementContext)
      {
         //neighborhood replacements
         rootContainer.GetAllChildren<Neighborhood>().Each(x => ReplaceIn(x, replacementContext));

         replaceInAllUsingFormulasDefinedIn(rootContainer, replacementContext);
      }

      public void ReplaceIn(Neighborhood neighborhood, ReplacementContext replacementContext)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, replacementContext);
         addCommonNeighborhoodReplacersTo(keywordReplacer, neighborhood);
         neighborhood.GetChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
         neighborhood.GetChildren<IContainer>().Each(x => replaceWithMoleculeKeywords(keywordReplacer, x, x.Name));
      }

      public void ReplaceIn(Transport transport, string moleculeName, ReplacementContext replacementContext)
      {
         ReplaceIn(transport, moleculeName, null, replacementContext);
      }

      public void ReplaceIn(Transport passiveTransport, string moleculeName, Neighborhood neighborhood, ReplacementContext replacementContext)
      {
         ReplaceIn(passiveTransport, moleculeName, neighborhood, null, null, replacementContext);
      }

      public void ReplaceIn(Transport realization, string moleculeName, Neighborhood neighborhood, string transportName,
         string transporterName, ReplacementContext replacementContext)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, replacementContext);
         addMoleculeReplacersTo(keywordReplacer, moleculeName);
         addCommonNeighborhoodReplacersTo(keywordReplacer, neighborhood);

         keywordReplacer.AddReplacements(
            new KeywordWithPathReplacer(SOURCE, _objectPathFactory.CreateAbsoluteObjectPath(realization.SourceAmount.ParentContainer)),
            new KeywordWithPathReplacer(TARGET, _objectPathFactory.CreateAbsoluteObjectPath(realization.TargetAmount.ParentContainer)),
            new KeywordWithPathReplacer(REALIZATION, new ObjectPath(transportName, realization.Name)),
            new SimpleKeywordReplacer(TRANSPORT, transportName),
            new SimpleKeywordReplacer(TRANSPORTER, transporterName));
         keywordReplacer.ReplaceIn(realization);

         replaceInAllUsingFormulasDefinedIn(realization, replacementContext);

         //replaceInContainer only replaces standard keywords. Transport specific keywords need to be replaced in all children explicitly
         var transportContainer = realization.ParentContainer ?? realization;
         transportContainer.GetAllChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
      }

      public void ReplaceIn(EventGroup eventGroup, EventGroupBuilder eventGroupBuilder, ReplacementContext replacementContext)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, replacementContext);

         var applicationBuilder = eventGroupBuilder as ApplicationBuilder;
         if (applicationBuilder != null)
         {
            addMoleculeReplacersTo(keywordReplacer, applicationBuilder.MoleculeName);
         }

         replaceInEventGroup(eventGroup, keywordReplacer);
      }

      private void replaceInEventGroup(EventGroup eventGroup, KeywordReplacerCollection keywordReplacer)
      {
         eventGroup.GetAllChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
         eventGroup.GetAllChildren<EventAssignment>().Select(x => x.ObjectPath).Each(keywordReplacer.ReplaceIn);
      }

      public ObjectPath CreateModelPathFor(ObjectPath objectPath, ReplacementContext replacementContext)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, replacementContext);
         var modelPath = objectPath.Clone<ObjectPath>();
         keywordReplacer.ReplaceIn(modelPath);
         return modelPath;
      }

      private void replaceInAllUsingFormulasDefinedIn(IContainer container, ReplacementContext replacementContext)
      {
         void replace(IUsingFormula usingFormula)
         {
            switch (usingFormula)
            {
               case MoleculeAmount moleculeAmount:
                  replaceIn(moleculeAmount, replacementContext);
                  break;
               default:
                  replaceInUsingFormula(usingFormula, replacementContext);
                  break;
            }
         }

         container.GetAllChildren<IUsingFormula>().Each(replace);
      }

      public void ReplaceIn(IContainer moleculePropertiesContainer, string moleculeName, ReplacementContext replacementContext)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, replacementContext);
         addMoleculeReplacersTo(keywordReplacer, moleculeName);
         moleculePropertiesContainer.GetAllChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
      }

      private void replaceWithMoleculeKeywords(IEnumerable<IKeywordReplacer> generalKeywordReplacer, IContainer container, string moleculeName)
      {
         var keywordReplacer = new KeywordReplacerCollection(generalKeywordReplacer);
         addMoleculeReplacersTo(keywordReplacer, moleculeName);
         container.GetChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
      }

      public void ReplaceIn(Observer observer, string moleculeName, ReplacementContext replacementContext)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, replacementContext);
         addMoleculeReplacersTo(keywordReplacer, moleculeName);
         keywordReplacer.ReplaceIn(observer);
      }

      private void addCommonModelReplacersTo(KeywordReplacerCollection keywordReplacer, ReplacementContext replacementContext)
      {
         //Replace the predefined keywords 
         keywordReplacer.AddReplacements(
            new TopContainerPathReplacer(replacementContext.RootContainerName, replacementContext.TopContainerNames),
            new TopContainerPathReplacer(replacementContext.RootContainerName, new[] {MOLECULE, Constants.NEIGHBORHOODS}));
      }

      private void addCommonNeighborhoodReplacersTo(KeywordReplacerCollection keywordReplacer, Neighborhood neighborhood)
      {
         if (neighborhood == null)
            return;

         keywordReplacer.AddReplacements(
            new KeywordWithPathReplacer(FIRST_NEIGHBOR, _objectPathFactory.CreateAbsoluteObjectPath(neighborhood.FirstNeighbor)),
            new KeywordWithPathReplacer(SECOND_NEIGHBOR, _objectPathFactory.CreateAbsoluteObjectPath(neighborhood.SecondNeighbor)),
            new KeywordWithPathReplacer(NEIGHBORHOOD, _objectPathFactory.CreateAbsoluteObjectPath(neighborhood)),
            //should be placed after the KeywordWithPathReplacer so that NEIGHBORHOOD is only replaced if not found yet
            new SimpleKeywordReplacer(NEIGHBORHOOD, neighborhood.Name));
      }

      private void addMoleculeReplacersTo(KeywordReplacerCollection keywordReplacer, string moleculeName)
      {
         if (string.IsNullOrEmpty(moleculeName))
            return;

         keywordReplacer.AddReplacement(new SimpleKeywordReplacer(MOLECULE, moleculeName));
      }

      private void replaceInUsingFormula(IUsingFormula usingFormula, ReplacementContext replacementContext)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, replacementContext);
         keywordReplacer.ReplaceIn(usingFormula);
      }

      public void ReplaceIn(IParameter parameter, ReplacementContext replacementContext)
      {
         var parameterContainer = parameter.ParentContainer;

         //Global molecule container or local molecule amount container
         var isInMolecule = parameterContainer.IsAnImplementationOf<MoleculeAmount>() ||
                            parameterContainer.ContainerType == ContainerType.Molecule;

         ReplaceIn(parameter, isInMolecule ? parameterContainer.Name : string.Empty, replacementContext);
      }

      public void ReplaceIn(IParameter parameter, string moleculeName, ReplacementContext replacementContext)
      {
         replaceIn(parameter, replacementContext, moleculeName);
      }

      public void ReplaceIn(MoleculeAmount moleculeAmount, ReplacementContext replacementContext)
      {
         replaceIn(moleculeAmount, replacementContext, moleculeAmount.Name);
         replaceIn(moleculeAmount.Parameter(START_VALUE), replacementContext, moleculeAmount.Name);
      }

      private void replaceIn(MoleculeAmount moleculeAmount, ReplacementContext replacementContext)
      {
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, replacementContext);
         addMoleculeReplacersTo(keywordReplacer, moleculeAmount.Name);
         keywordReplacer.ReplaceIn(moleculeAmount);
         moleculeAmount.GetAllChildren<IUsingFormula>().Each(keywordReplacer.ReplaceIn);
      }

      private void replaceIn(IUsingFormula usingFormula, ReplacementContext replacementContext, string moleculeName)
      {
         if (usingFormula == null) return;
         var keywordReplacer = new KeywordReplacerCollection();
         addCommonModelReplacersTo(keywordReplacer, replacementContext);
         addMoleculeReplacersTo(keywordReplacer, moleculeName);
         keywordReplacer.ReplaceIn(usingFormula);
      }
   }
}