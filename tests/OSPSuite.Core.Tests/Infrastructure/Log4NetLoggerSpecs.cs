using System;
using FakeItEasy;
using log4net;
using OSPSuite.BDDHelper;
using OSPSuite.Utility.Logging;
using OSPSuite.Infrastructure.Logging.Log4NetLogging;

namespace OSPSuite.Infrastructure
{
   public class When_told_to_log_an_informational_message : ContextSpecification<ILogger>
   {
      private ILog log;
      private string textToLog;

      protected override void Context()
      {
         log = A.Fake<ILog>();
         sut = new Log4NetLogger(log);
         textToLog = "blah";
      }

      protected override void Because()
      {
         sut.Informational(textToLog);
      }

      [Observation]
      public void should_leverage_the_info_method_of_the_log4net_logger()
      {
         A.CallTo(() => log.Info(textToLog)).MustHaveHappened();
      }
   }

   public class when_told_to_log_an_error : ContextSpecification<ILogger>
   {
      private ILog log;
      private Exception e;

      protected override void Context()
      {
         log = A.Fake<ILog>();
         sut = new Log4NetLogger(log);
         e = new ArgumentException("toto");
      }

      protected override void Because()
      {
         sut.Error(e);
      }

      [Observation]
      public void should_leverage_the_error_method_of_the_log4net_logger()
      {
         A.CallTo(() => log.Error(e.ToString())).MustHaveHappened();
      }
   }

   public class when_told_to_log_a_debug_info_when_the_debug_mode_is_activated : ContextSpecification<ILogger>
   {
      private ILog log;
      private string debugInfo;

      protected override void Context()
      {
         log = A.Fake<ILog>();
         sut = new Log4NetLogger(log);
         debugInfo = "toto";
         A.CallTo(() => log.IsDebugEnabled).Returns(true);
      }

      protected override void Because()
      {
         sut.Debug(debugInfo);
      }

      [Observation]
      public void should_leverage_the_log_method_of_the_log4net_logger()
      {
         A.CallTo(() => log.Debug(debugInfo)).MustHaveHappened();
      }
   }

   public class when_told_to_log_a_debug_info_when_the_debug_mode_is_not_activated : ContextSpecification<ILogger>
   {
      private ILog log;
      private string debugInfo;

      protected override void Context()
      {
         log = A.Fake<ILog>();
         sut = new Log4NetLogger(log);
         debugInfo = "toto";
         A.CallTo(() => log.IsDebugEnabled).Returns(false);
      }

      protected override void Because()
      {
         sut.Debug(debugInfo);
      }

      [Observation]
      public void should_not_call_the_log_method_of_the_log4net_logger()
      {
         A.CallTo(() => log.Debug(debugInfo)).MustNotHaveHappened();
      }
   }
}