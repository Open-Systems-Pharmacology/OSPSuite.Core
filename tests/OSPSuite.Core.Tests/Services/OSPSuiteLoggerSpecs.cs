using OSPSuite.BDDHelper;
using System;
using System.Collections.Generic;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using FakeItEasy.Configuration;
using OSPSuite.Core.Services.Logging;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_PKSimLoggerCreator : ContextSpecification<LoggerCreator>
   {
      protected override void Context()
      {
         sut = new LoggerCreator();
      }
   }

   public class When_the_pk_sim_logger_creator_is_configured_with_one_function : concern_for_PKSimLoggerCreator
   {
      private Func<ILoggingBuilder, ILoggingBuilder> _f1;

      protected override void Context()
      {
         base.Context();
         _f1 = A.Fake<Func<ILoggingBuilder, ILoggingBuilder>>();
      }

      protected override void Because()
      {
         sut.AddLoggingBuilderConfiguration(_f1);
      }

      [Observation]
      public void should_invoke_new_action()
      {
         var logger = new OSPSuiteLogger(sut);
         logger.AddToLog("test", new LogLevel(), "testcategory");
         A.CallTo(() => _f1.Invoke(A<ILoggingBuilder>.Ignored)).MustHaveHappened();
      }
   }


   public class When_the_pk_sim_logger_creator_is_configured_with_some_function : concern_for_PKSimLoggerCreator
   {

      [Observation]
      public void should_invoke_all_actions_in_order()
      {
         var iterations = 10;
         var functions = new List<Func<ILoggingBuilder, ILoggingBuilder>>();
         for (var i = 0; i < iterations; i++)
         {
            functions.Add(A.Fake<Func<ILoggingBuilder, ILoggingBuilder>>());
            sut.AddLoggingBuilderConfiguration(functions[i]);
         }

         var logger = new OSPSuiteLogger(sut);
         logger.AddToLog("test", new LogLevel(), "testcategory");

         for (var i = 0; i < iterations; i++)
         {
            IOrderableCallAssertion check = A.CallTo(() => functions[0].Invoke(A<ILoggingBuilder>.Ignored)).MustHaveHappened();
            for (var j = 1; j < iterations; j++)
            {
               check = check.Then(A.CallTo(() => functions[j].Invoke(A<ILoggingBuilder>.Ignored)).MustHaveHappened());
            }
         }
      }
   }
}
