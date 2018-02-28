using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public abstract class ParameterIdentificationRunMode : IUpdatable
   {
      public string DisplayName { get; }

      public bool IsSingleRun { get; }
      protected ParameterIdentificationRunMode(string displayName, bool isSingleRun)
      {
         DisplayName = displayName;
         IsSingleRun = isSingleRun;
      }

      public ParameterIdentificationRunMode Clone()
      {
         var clone = CreateClone();
         clone.UpdatePropertiesFrom(this, null);
         return clone;
      }

      protected abstract ParameterIdentificationRunMode CreateClone();

      public virtual void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
      }
   }
}