using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO
{
   public class NonEmptyNameDTO : ValidatableDTO
   {
      private string _name;

      public virtual string Name
      {
         get => _name;
         set
         {
            _name = value.TrimmedValue();
            OnPropertyChanged(() => Name);
         }
      }

      public NonEmptyNameDTO()
      {
         Rules.Add(AllRules.NameNotEmpty);
         Rules.Add(AllRules.NameDoesNotContainIllegalCharacters);
      }

      private static bool nameDoesNotContainerIllegalCharacters(string name)
      {
         if (string.IsNullOrEmpty(name))
            return true;

         return !Constants.ILLEGAL_CHARACTERS.Any(name.Contains);
      }

      protected static class AllRules
      {
         public static IBusinessRule NameNotEmpty { get; } = GenericRules.NonEmptyRule<NonEmptyNameDTO>(x => x.Name, Error.NameIsRequired);

         public static IBusinessRule NameDoesNotContainIllegalCharacters { get; } = CreateRule.For<NonEmptyNameDTO>()
            .Property(item => item.Name)
            .WithRule((dto, name) => nameDoesNotContainerIllegalCharacters(name))
            .WithError(Error.NameCannotContainIllegalCharacters(Constants.ILLEGAL_CHARACTERS));
      }
   }
}