using System;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace OSPSuite.UI.Extensions
{
   public static class BaseEditExtensions
   {
      public static void FillComboBoxEditorWith<T>(this BaseEdit activeEditor, IEnumerable<T> listToAddToComboBoxRepository)
      {
         var comboBoxEdit = activeEditor as ComboBoxEdit;
         if (comboBoxEdit == null) return;

         comboBoxEdit.Properties.Items.Clear();
         listToAddToComboBoxRepository.Each(item => comboBoxEdit.Properties.Items.Add(item));
      }

      public static void FillImageComboBoxEditorWith<T>(this BaseEdit activeEditor, IEnumerable<T> listToAddToComboBoxRepository,  Func<T, int> imageIndexFor)
      {
         activeEditor.FillImageComboBoxEditorWith(listToAddToComboBoxRepository, imageIndexFor,x=>x.ToString());
      }

      public static void FillImageComboBoxEditorWith<T>(this BaseEdit activeEditor, IEnumerable<T> listToAddToComboBoxRepository, Func<T, int> imageIndexFor, Func<T,string> displayNames)
      {
         var imageComboBoxEdit = activeEditor as ImageComboBoxEdit;
         if (imageComboBoxEdit == null) return;
         imageComboBoxEdit.Properties.FillImageComboBoxRepositoryWith(listToAddToComboBoxRepository, imageIndexFor, displayNames);
      }

      public static void FillImageComboBoxRepositoryWith<T>(this RepositoryItemImageComboBox repositoryItemImageComboBox, IEnumerable<T> listToAddToComboBoxRepository, Func<T, int> imageIndexFor)
      {
         repositoryItemImageComboBox.FillImageComboBoxRepositoryWith(listToAddToComboBoxRepository, imageIndexFor, x => x.ToString());
      }

      public static void FillImageComboBoxRepositoryWith<T>(this RepositoryItemImageComboBox repositoryItemImageComboBox, IEnumerable<T> listToAddToComboBoxRepository, Func<T, int> imageIndexFor, Func<T, string> displayNames)
      {
         repositoryItemImageComboBox.Items.Clear();
         listToAddToComboBoxRepository.Each(item => repositoryItemImageComboBox.Items.Add(new ImageComboBoxItem(displayNames(item), item, imageIndexFor(item))));
      }

      public static void FillComboBoxRepositoryWith<T>(this RepositoryItemComboBox repositoryItemComboBox, IEnumerable<T> listToAddToComboBoxRepository)
      {
         repositoryItemComboBox.Items.Clear();
         listToAddToComboBoxRepository.Each(item => repositoryItemComboBox.Items.Add(item));
      }
   }
}