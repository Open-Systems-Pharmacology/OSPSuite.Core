using OSPSuite.Assets;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public class CannotSelectTheObservedDataMoreThanOnceException : OSPSuiteException
   {
      public CannotSelectTheObservedDataMoreThanOnceException(DataRepository obsservedData) : base(Error.CannotSelectTheObservedDataMoreThanOnce(obsservedData.Name))
      {
      }
   }
}