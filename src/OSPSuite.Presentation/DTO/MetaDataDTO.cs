using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Validation;
using DevExpress.XtraEditors.DXErrorProvider;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Presentation.DTO
{
   public class MetaDataDTO : NonEmptyNameDTO, IDXDataErrorInfo
   {
      public string Value { set; get; }
      public bool NameEditable { get; set; }
      public bool ValueReadOnly { get; set; }
      public IEnumerable<DataRepository> DataRepositories { get; set; }
      public IEnumerable<string> ListOfValues { get; set; }

      public MetaDataDTO(IEnumerable<IBusinessRule> validationRules = null)
      {
         if (validationRules == null)
            validationRules = MetaDataDTORules.All();

         Rules.AddRange(validationRules);
         NameEditable = true;
         ValueReadOnly = false;
      }

      public virtual void GetPropertyError(string propertyName, ErrorInfo info)
      {
         this.UpdatePropertyError(propertyName, info);
      }

      public virtual void GetError(ErrorInfo info)
      {
         this.UpdateError(info);
      }

      public bool HasListOfValues => ListOfValues != null && ListOfValues.Any();

      private string nameAlreadyExistsError(string newName)
      {
         return Error.NameAlreadyExistsInContainerType(newName, ObjectTypes.ObservedData);
      }

      private bool isNameUnique(string newName)
      {
         if (DataRepositories == null)
            return true;

         if (string.Equals(newName, Name))
            return true;

         return DataRepositories.All(x => !x.ExtendedProperties.Contains(newName));
      }

      internal static class MetaDataDTORules
      {
         internal static IEnumerable<IBusinessRule> AllForMultipleEdits()
         {
            yield return uniqueName;
         }

         internal static IEnumerable<IBusinessRule> All()
         {
            foreach (var rule in AllForMultipleEdits())
            {
               yield return rule;
            }

            yield return valuePropertyNotEmpty;
         }

         private static IBusinessRule valuePropertyNotEmpty
         {
            get { return GenericRules.NonEmptyRule<MetaDataDTO>(x => x.Value); }
         }

         private static IBusinessRule uniqueName
         {
            get
            {
               return CreateRule.For<MetaDataDTO>()
                  .Property(item => item.Name)
                  .WithRule((dto, newName) => dto.isNameUnique(newName))
                  .WithError((dto, newName) => dto.nameAlreadyExistsError(newName));
            }
         }
      }
   }

}