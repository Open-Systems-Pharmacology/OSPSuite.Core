using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class CannotSelectTheObservedDataMoreThanOnceException : OSPSuiteException
   {
      public CannotSelectTheObservedDataMoreThanOnceException(DataRepository obsservedData) : base(Error.CannotSelectTheObservedDataMoreThanOnce(obsservedData.Name))
      {
      }
   }
}