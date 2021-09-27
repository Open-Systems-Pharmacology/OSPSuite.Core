using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Utils.Svg;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO
{
   public class ImportFileSelectionDTO : DxValidatableDTO
   {
      private string _filePath;
      private NotificationType _status;

      private IEnumerable<string> _messages;

      public ImportFileSelectionDTO()
      {
         Rules.AddRange(AllRules.All());
         FilePath = string.Empty;
      }

      /// <summary>
      ///    Full path of file to import
      /// </summary>
      public virtual string FilePath
      {
         get { return _filePath; }
         set
         {
            _filePath = value;
            _status = NotificationType.None;
            Messages = Enumerable.Empty<string>();
            OnPropertyChanged(() => FilePath);
            OnPropertyChanged(() => FileDefined);
         }
      }

      public bool FileDefined
      {
         get { return !string.IsNullOrEmpty(FilePath); }
      }

      public IEnumerable<string> Messages
      {
         get { return _messages; }
         set
         {
            _messages = value;
            OnPropertyChanged(() => Messages);
         }
      }

      public NotificationType Status
      {
         get { return _status; }
         set
         {
            _status = value;
            OnPropertyChanged(() => Status);
         }
      }

      public string Message
      {
         get { return Messages.ToString("\n"); }
      }

      public SvgImage Image
      {
         get { return imageFrom(Status); }
      }

      private SvgImage imageFrom(NotificationType status)
      {
         switch (status)
         {
            case NotificationType.Info:
               return ApplicationIcons.OK;
            case NotificationType.Warning:
               return ApplicationIcons.Warning;
            case NotificationType.Error:
               return ApplicationIcons.Error;
            case NotificationType.None:
               return ApplicationIcons.Help;
            default:
               throw new ArgumentOutOfRangeException("status");
         }
      }

      private static class AllRules
      {
         private static IBusinessRule fileExists
         {
            get { return GenericRules.FileExists<ImportFileSelectionDTO>(x => x.FilePath); }
         }

         private static IBusinessRule fileNotEmpty
         {
            get { return GenericRules.NonEmptyRule<ImportFileSelectionDTO>(x => x.FilePath); }
         }

         private static IBusinessRule statusIsNotError
         {
            get
            {
               return CreateRule.For<ImportFileSelectionDTO>()
                  .Property(item => item.Status)
                  .WithRule((item, status) => status != NotificationType.Error)
                  .WithError((item, status) => item.Message);
            }
         }

         internal static IEnumerable<IBusinessRule> All()
         {
            yield return fileNotEmpty;
            yield return fileExists;
            yield return statusIsNotError;
         }
      }
   }
}