using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Services
{
   public class DirectoryMap
   {
      public string Key { get; set; }
      public string Path { get; set; }
   }

   public class DirectoryMapSettings
   {
      public ICache<string, DirectoryMap> UsedDirectories { get; }

      public DirectoryMapSettings()
      {
         UsedDirectories = new Cache<string, DirectoryMap>(dm => dm.Key, s => new DirectoryMap {Key = s, Path = string.Empty});
      }

      public void AddUsedDirectory(string directoryKey, string path)
      {
         AddUsedDirectory(new DirectoryMap {Key = directoryKey, Path = path});
      }

      public void AddUsedDirectory(DirectoryMap directoryMap)
      {
         UsedDirectories[directoryMap.Key] = directoryMap;
      }
   }
}