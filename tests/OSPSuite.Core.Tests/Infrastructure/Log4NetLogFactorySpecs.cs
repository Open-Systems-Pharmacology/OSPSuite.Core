using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Logging;
using OSPSuite.Infrastructure.Logging.Log4NetLogging;

namespace OSPSuite.Infrastructure
{
   public class When_creating_a_logger_for_a_specific_type : ContextSpecification<Log4NetLogFactory>
   {
      private ILogger _result;

      protected override void Context()
      {
         sut = new Log4NetLogFactory();
      }

      protected override void Because()
      {
         _result = sut.CreateFor(GetType());
      }

      [Observation]
      public void should_return_a_log4Net_Logger_instance()
      {
         _result.ShouldBeAnInstanceOf<Log4NetLogger>();
      }
   }
}