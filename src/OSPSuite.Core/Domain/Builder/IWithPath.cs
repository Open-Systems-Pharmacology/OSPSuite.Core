namespace OSPSuite.Core.Domain.Builder
{
   public interface IWithPath
   {
      IObjectPath Path { get; set; }
      IObjectPath ContainerPath { get; set; }
   }
}