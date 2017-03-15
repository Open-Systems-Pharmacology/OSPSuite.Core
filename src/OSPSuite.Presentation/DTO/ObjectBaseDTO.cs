using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Presentation.DTO
{
   public class ObjectBaseDTO : NonEmptyNameDTO
   {
      protected readonly List<string> _usedNames;
      public bool DescriptionRequired { get; set; }
      public virtual string ContainerType { get; set; }
      public virtual string Description { get; set; }

      public ObjectBaseDTO()
      {
         _usedNames = new List<string>();
         DescriptionRequired = false;
         Rules.Add(ObjectBaseDTORules.UniqueName);
         Rules.Add(ObjectBaseDTORules.DescriptionNotEmpty);
      }

      public void AddUsedNames(IEnumerable<string> usedNames)
      {
         _usedNames.AddRange(usedNames.Select(x => x.ToLower()));
      }

      protected virtual string NameAlreadyExistsError(string newName)
      {
         return Error.NameAlreadyExistsInContainerType(newName, ContainerType);
      }

      protected virtual bool IsNameUnique(string newName)
      {
         if (string.IsNullOrEmpty(newName))
            return true;

         return !_usedNames.Contains(newName.ToLower().Trim());
      }

      protected bool IsDescriptionDefined(string description)
      {
         if (!DescriptionRequired)
            return true;

         return description.StringIsNotEmpty();
      }

      private static class ObjectBaseDTORules
      {
         public static IBusinessRule UniqueName
         {
            get
            {
               return CreateRule.For<ObjectBaseDTO>()
                  .Property(item => item.Name)
                  .WithRule((dto, newName) => dto.IsNameUnique(newName))
                  .WithError((dto, newName) => dto.NameAlreadyExistsError(newName));
            }
         }

         public static IBusinessRule DescriptionNotEmpty
         {
            get
            {
               return CreateRule.For<ObjectBaseDTO>()
                  .Property(item => item.Description)
                  .WithRule((dto, desc) => dto.IsDescriptionDefined(desc))
                  .WithError((dto, newName) => Error.DescriptionIsRequired);
            }
         }
      }
   }

   public class ObjectBaseDTO<T> : ObjectBaseDTO where T : IValidatable, INotifier
   {
      public ObjectBaseDTO(T underlyingObject)
      {
         this.AddRulesFrom(underlyingObject);
         underlyingObject.PropertyChanged += (o, e) => OnPropertyChanged(e.PropertyName);
      }
   }
}