using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.Items;

namespace OSPSuite.Infrastructure.Reporting.TeXBuilder
{
   class ObserverBuilderTeXBuilder : OSPSuiteTeXBuilder<IObserverBuilder>
   {
      private readonly ITeXBuilderRepository _builderRepository;
      private const string PROPERTY_PROMPT_FORMAT = "{0}: {1}\n";
      private const string TYPE = "Type";
      private const string DIMENSION = "Dimension";
      private const string CALCULATED_FOR_ALL_MOLECULES = "Calculated for all molecules";
      private const string CALCULATED_FOR_ALL_MOLECULES_EXCEPT = "Calculated for all molecules except:";
      private const string CALCULATED_FOR_FOLLOWING_MOLECULES = "Calculated for following molecules:";
      private const string IN_CONTAINERS_WITH = "In containers with:";
      private const string MONITOR = "Monitor";

      public ObserverBuilderTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(IObserverBuilder observerBuilder, OSPSuiteTracker buildTracker)
      {
         var listToReport = new List<object>();
         listToReport.AddRange(this.ReportDescription(observerBuilder, buildTracker));

         var amountObserver = observerBuilder as AmountObserverBuilder;
         var containerObserver = observerBuilder as ContainerObserverBuilder;

         if (amountObserver != null)
            listToReport.Add(string.Format(PROPERTY_PROMPT_FORMAT, TYPE, Captions.MoleculeObserver));

         if (containerObserver != null)
            listToReport.Add(string.Format(PROPERTY_PROMPT_FORMAT, TYPE, Captions.ContainerObserver));

         listToReport.Add(string.Format(PROPERTY_PROMPT_FORMAT, DIMENSION, observerBuilder.Dimension));

         var hasExcludedMolecules = observerBuilder.MoleculeNamesToExclude().Any();
         if (observerBuilder.ForAll)
         {
            if (hasExcludedMolecules)
            {
               listToReport.Add(new Paragraph(CALCULATED_FOR_ALL_MOLECULES_EXCEPT));
               listToReport.Add(observerBuilder.MoleculeNamesToExclude().ToArray());
            }
            else
               listToReport.Add(string.Format(PROPERTY_PROMPT_FORMAT, CALCULATED_FOR_ALL_MOLECULES, observerBuilder.ForAll));
         }
         else
         {
            listToReport.Add(new Paragraph(CALCULATED_FOR_FOLLOWING_MOLECULES));
            listToReport.Add(observerBuilder.MoleculeNames().ToArray());
         }

         listToReport.Add(new Paragraph(IN_CONTAINERS_WITH));
         listToReport.Add(observerBuilder.ContainerCriteria);

         listToReport.Add(new Paragraph(MONITOR));
         listToReport.Add(observerBuilder.Formula);

         _builderRepository.Report(listToReport, buildTracker);
      }
   }
}