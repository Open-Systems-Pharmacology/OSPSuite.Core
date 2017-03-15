using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Services
{
   public interface IMRUProvider : IRepository<string>
   {
      void Add(string fileFullPathToAdd);
   }

   public class MRUProvider : IMRUProvider
   {
      private readonly IPresentationUserSettings _userSettings;
      private readonly LinkedList<string> _allProjectPath;

      public MRUProvider(IPresentationUserSettings userSettings)
      {
         _userSettings = userSettings;
         _allProjectPath = new LinkedList<string>();
      }

      public IEnumerable<string> All()
      {
         readFromSettings();
         return _allProjectPath;
      }

      public void Add(string fileFullPathToAdd)
      {
         readFromSettings();
         var file = _allProjectPath.Find(fileFullPathToAdd);
         if (file != null)
         {
            _allProjectPath.Remove(file);
            _allProjectPath.AddFirst(file);
         }
         else
         {
            _allProjectPath.AddFirst(fileFullPathToAdd);
         }

         saveToSettings();
      }

      private void readFromSettings()
      {
         _allProjectPath.Clear();
         foreach (var fileFullPath in _userSettings.ProjectFiles)
         {
            if (!FileHelper.FileExists(fileFullPath))
               continue;

            if (_allProjectPath.Count >= _userSettings.MRUListItemCount) return;
            _allProjectPath.AddLast(fileFullPath);

         }
      }

      private void saveToSettings()
      {
         _userSettings.ProjectFiles = _allProjectPath.ToList();
      }
   }

}