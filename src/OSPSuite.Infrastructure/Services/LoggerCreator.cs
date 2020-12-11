using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Infrastructure.Services
{
   public interface ILoggerCreator
   {
      ILogger GetOrCreateLogger(string categoryName);

      ILoggerCreator AddLoggingBuilderConfiguration(Func<ILoggingBuilder, ILoggingBuilder> configuration);
   }

   public class LoggerCreator : ILoggerCreator
   {
      private readonly ConcurrentDictionary<string, ILogger> _loggerDict = new ConcurrentDictionary<string, ILogger>();

      private readonly List<Func<ILoggingBuilder, ILoggingBuilder>> _loggingBuilderConfigurations = new List<Func<ILoggingBuilder, ILoggingBuilder>>
         {builder => builder};

      public ILoggerCreator AddLoggingBuilderConfiguration(Func<ILoggingBuilder, ILoggingBuilder> configuration)
      {
         _loggingBuilderConfigurations.Add(configuration);
         return this;
      }

      public ILogger GetOrCreateLogger(string categoryName)
      {
         return _loggerDict.GetOrAdd(categoryName, (_) => setupLogger(categoryName));
      }

      private ILogger setupLogger(string categoryName)
      {
         var factoryConfiguration = _loggingBuilderConfigurations.Aggregate((f1, f2) => config => f1.Compose(f2, config));
         using (var loggerFactory = LoggerFactory.Create(x => factoryConfiguration(x)))
         {
            return loggerFactory.CreateLogger(categoryName);
         }
      }
   }
}