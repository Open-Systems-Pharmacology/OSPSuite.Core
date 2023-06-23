using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
         get => _filePath;
         set
         {
            _filePath = value;
            _status = NotificationType.None;
            Messages = Enumerable.Empty<string>();
            OnPropertyChanged(() => FilePath);
            OnPropertyChanged(() => FileDefined);
         }
      }

      public bool FileDefined => !string.IsNullOrEmpty(FilePath);


      public virtual IEnumerable<string> Messages
      {
         get => _messages;
         set => SetProperty(ref _messages, value);
      }


      public virtual NotificationType Status
      {
         get => _status;
         set => SetProperty(ref _status, value);
      }


      public string Message => Messages.ToString("\n");

      public Image Image => Icon.ToImage();

      public ApplicationIcon Icon => imageFrom(Status);

      private ApplicationIcon imageFrom(NotificationType status)
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
               throw new ArgumentOutOfRangeException(nameof(status));
         }
      }

      private static class AllRules
      {
         private static IBusinessRule fileExists { get; } = GenericRules.FileExists<ImportFileSelectionDTO>(x => x.FilePath);

         private static IBusinessRule fileNotEmpty { get; } = GenericRules.NonEmptyRule<ImportFileSelectionDTO>(x => x.FilePath);

         private static IBusinessRule statusIsNotError { get; } = CreateRule.For<ImportFileSelectionDTO>()
            .Property(item => item.Status)
            .WithRule((item, status) => status != NotificationType.Error)
            .WithError((item, status) => item.Message);

         internal static IEnumerable<IBusinessRule> All()
         {
            yield return fileNotEmpty;
            yield return fileExists;
            yield return statusIsNotError;
         }
      }
   }
}