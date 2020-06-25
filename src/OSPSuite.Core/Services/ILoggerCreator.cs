using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Core.Services
{
   public interface ILoggerCreator
   {
      ILogger GetOrCreateLogger(string categoryName); 

      ILoggerCreator AddLoggingBuilderConfiguration(Func<ILoggingBuilder, ILoggingBuilder> configuration);
   }
}
