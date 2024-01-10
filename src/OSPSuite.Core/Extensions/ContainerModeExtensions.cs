using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Extensions
{
   public static class ContainerModeExtensions
   {
      public static bool Is(this ContainerMode containerMode, ContainerMode otherContainerMode) => containerMode == otherContainerMode;
   }
}
