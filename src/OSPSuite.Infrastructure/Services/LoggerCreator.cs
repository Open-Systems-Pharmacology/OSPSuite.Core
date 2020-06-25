using Microsoft.Extensions.Logging;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Services
{
   public class LoggerCreator : ILoggerCreator
   {
      private readonly ConcurrentDictionary<string, ILogger> _loggerDict = new ConcurrentDictionary<string, ILogger>();
      private List<Func<ILoggingBuilder, ILoggingBuilder>> _loggingBuilderConfigurations = new List<Func<ILoggingBuilder, ILoggingBuilder>>() { builder => builder };

      public ILoggerCreator AddLoggingBuilderConfiguration(Func<ILoggingBuilder, ILoggingBuilder> configuration)
      {
         _loggingBuilderConfigurations.Add(configuration);
         return this;
      }

      public ILogger GetOrCreateLogger(string categoryName)
      {
         var logger = _loggerDict.GetOrAdd(categoryName, (_) => SetupLogger(categoryName));
         return logger;
      }

      private ILogger SetupLogger(string categoryName)
      {
         ILogger logger;

         using (var loggerFactory = LoggerFactory.Create(
             builder =>
             _loggingBuilderConfigurations.Aggregate(
                 (f1, f2) => config => f1.Compose(f2, config)
             ).Invoke(builder)
         ))
         {
            logger = loggerFactory.CreateLogger(categoryName);
         };
         return logger;
      }
   }
}
