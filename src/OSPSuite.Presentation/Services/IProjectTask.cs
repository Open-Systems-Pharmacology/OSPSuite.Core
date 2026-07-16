using System.Threading.Tasks;

namespace OSPSuite.Presentation.Services
{
   public interface IProjectTask
   {
      void OpenProjectFrom(string projectFile);
      Task ExportCurrentProjectToSnapshot();
      void LoadProjectFromSnapshot();
   }
}