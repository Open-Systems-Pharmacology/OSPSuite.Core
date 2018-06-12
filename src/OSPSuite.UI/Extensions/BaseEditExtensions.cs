using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using OSPSuite.Assets;
using OSPSuite.UI.RepositoryItems;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.UI.Extensions
{
   public static class BaseEditExtensions
   {
      public static void FillComboBoxEditorWith<T>(this BaseEdit activeEditor, IEnumerable<T> listToAddToComboBoxRepository)
      {
         var comboBoxEdit = activeEditor as ComboBoxEdit;
         if (comboBoxEdit == null) return;

         FillComboBoxRepositoryWith(comboBoxEdit.Properties, listToAddToComboBoxRepository);
      }

      public static void FillImageComboBoxEditorWith<T>(this BaseEdit activeEditor, IEnumerable<T> listToAddToComboBoxRepository, Func<T, int> imageIndexFor)
      {
         activeEditor.FillImageComboBoxEditorWith(listToAddToComboBoxRepository, imageIndexFor, x => x.ToString());
      }

      public static void FillImageComboBoxEditorWith<T>(this BaseEdit activeEditor, IEnumerable<T> listToAddToComboBoxRepository, Func<T, int> imageIndexFor, Func<T, string> displayNames)
      {
         var imageComboBoxEdit = activeEditor as ImageComboBoxEdit;
         if (imageComboBoxEdit == null) return;
         FillImageComboBoxRepositoryWith(imageComboBoxEdit.Properties, listToAddToComboBoxRepository, imageIndexFor, displayNames);
      }

      public static void FillImageComboBoxRepositoryWith<T>(this RepositoryItemImageComboBox repositoryItemImageComboBox, IEnumerable<T> listToAddToComboBoxRepository, Func<T, int> imageIndexFor)
      {
         FillImageComboBoxRepositoryWith(repositoryItemImageComboBox, listToAddToComboBoxRepository, imageIndexFor, x => x.ToString());
      }

      public static void FillImageComboBoxRepositoryWith<T>(this RepositoryItemImageComboBox repositoryItemImageComboBox, IEnumerable<T> listToAddToComboBoxRepository, Func<T, int> imageIndexFor, Func<T, string> displayNames)
      {
         repositoryItemImageComboBox.Items.Clear();
         listToAddToComboBoxRepository.Each(item => repositoryItemImageComboBox.Items.Add(new ImageComboBoxItem(displayNames(item), item, imageIndexFor(item))));
      }

      public static UxRepositoryItemImageComboBox FillImageComboBoxRepositoryWith<T>(this UxRepositoryItemImageComboBox repositoryItemImageComboBox, IEnumerable<T> listToAddToComboBoxRepository, Func<T, ApplicationIcon> iconFor)
      {
         return FillImageComboBoxRepositoryWith(repositoryItemImageComboBox, listToAddToComboBoxRepository, iconFor, x => x.ToString());
      }

      public static UxRepositoryItemImageComboBox FillImageComboBoxRepositoryWith<T>(this UxRepositoryItemImageComboBox repositoryItemImageComboBox, IEnumerable<T> listToAddToComboBoxRepository, Func<T, ApplicationIcon> iconFor, Func<T, string> displayFunc)
      {
         listToAddToComboBoxRepository.Each(item => { repositoryItemImageComboBox.AddItem(item, iconFor(item)); });
         return repositoryItemImageComboBox;
      }

      public static void FillComboBoxRepositoryWith<T>(this RepositoryItemComboBox repositoryItemComboBox, IEnumerable<T> listToAddToComboBoxRepository)
      {
         repositoryItemComboBox.Items.Clear();
         listToAddToComboBoxRepository.Each(item => repositoryItemComboBox.Items.Add(item));
      }
   }
}