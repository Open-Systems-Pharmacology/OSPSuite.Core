using System.Collections.Generic;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.TeXReporting.Builder;
using OSPSuite.TeXReporting.TeX.Converter;

namespace OSPSuite.Infrastructure.Reporting.TeXBuilder
{
   public class DescriptorCriteriaTeXBuilder : TeXChunkBuilder<DescriptorCriteria>
   {
      private readonly ITeXBuilderRepository _builderRepository;
      private const string TAGGED_WITH = "tagged with";
      private const string NOT_TAGGED_WITH = "not tagged with";
      private const string IN_CONTAINER = "in container";

      public DescriptorCriteriaTeXBuilder(ITeXBuilderRepository builderRepository)
      {
         _builderRepository = builderRepository;
      }

      public override void Build(DescriptorCriteria descriptorCriteria, BuildTracker buildTracker)
      {
         buildTracker.TeX.Append(TeXChunk(descriptorCriteria));
         buildTracker.Track(descriptorCriteria);
      }

      public override string TeXChunk(DescriptorCriteria descriptorCriteria)
      {
         return _builderRepository.ChunkFor(listFor(descriptorCriteria));
      }

      private string[] listFor(DescriptorCriteria descriptorCriteria)
      {
         var strings = new List<string>();
         foreach (var criteria in descriptorCriteria)
         {
            switch (criteria)
            {
               case MatchTagCondition match:
                  strings.Add($"{TAGGED_WITH} {DefaultConverter.Instance.StringToTeX(match.Tag)}");
                  break;
               case NotMatchTagCondition notmatch:
                  strings.Add($"{NOT_TAGGED_WITH} {DefaultConverter.Instance.StringToTeX(notmatch.Tag)}");
                  break;
               case MatchAllCondition allmatch:
                  strings.Add(allmatch.Tag);
                  break;
               case InContainerCondition inContainerCondition:
                  strings.Add($"{IN_CONTAINER} {DefaultConverter.Instance.StringToTeX(inContainerCondition.Tag)}");
                  break;
            }
         }

         return strings.ToArray();
      }
   }
}