namespace OSPSuite.Presentation.Core
{
   public interface IBatchUpdatable
   {
      void BeginUpdate();
      void EndUpdate();
      bool Updating { get; }
   }
}