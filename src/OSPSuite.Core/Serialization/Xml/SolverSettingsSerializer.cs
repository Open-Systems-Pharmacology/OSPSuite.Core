using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class SolverSettingsSerializer<T> : ContainerXmlSerializer<T> where T : SolverSettings
   {
   }

   public class SolverSettingsSerializer : SolverSettingsSerializer<SolverSettings>
   {
   }
}