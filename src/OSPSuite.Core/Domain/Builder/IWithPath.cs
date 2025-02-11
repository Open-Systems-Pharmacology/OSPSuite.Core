namespace OSPSuite.Core.Domain.Builder
{
   public interface IWithPath
   {
      ObjectPath Path { get; set; }
      ObjectPath ContainerPath { get; set; }
   }
}