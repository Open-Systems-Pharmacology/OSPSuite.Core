using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.Exceptions;

namespace OSPSuite.Infrastructure.Import
{
   public abstract class concern_for_ParseErrors : ContextSpecification<ParseErrors>
   {
      protected override void Context()
      {
         sut = new ParseErrors();
      }
   }

   public class When_retrieving_the_distinct_messages_of_errors_reported_for_several_data_sets : concern_for_ParseErrors
   {
      protected override void Context()
      {
         base.Context();
         sut.Add(A.Fake<IDataSet>(), new List<ParseErrorDescription> { new InvalidDimensionParseErrorDescription("", "Time"), new NaNParseErrorDescription() });
         sut.Add(A.Fake<IDataSet>(), new InvalidDimensionParseErrorDescription("", "Time"));
      }

      [Observation]
      public void should_return_each_message_only_once()
      {
         sut.DistinctMessages().ShouldOnlyContain(Error.InvalidDimensionException("", "Time"), Error.NaNOnData);
      }
   }
}
