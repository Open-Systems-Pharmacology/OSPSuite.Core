using OSPSuite.Assets;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO
{
   public class RenameObjectDTO : ObjectBaseDTO
   {
      public string OriginalName { get; set; }

      public RenameObjectDTO(string name)
      {
         Rules.Add(RenameObjectDTORules.NameShouldNotBeTheSame);
         OriginalName = name;
         Name = name;
      }

      protected override bool IsNameUnique(string newName)
      {
         if (base.IsNameUnique(newName))
            return true;

         return sameAsOriginalInLowCase(newName);
      }

      private bool sameAsOriginalInLowCase(string newName) => sameAsOriginal(newName, compareUsingCase: false);

      private bool sameAsOriginal(string newName, bool compareUsingCase)
      {
         var newNameToCheck = newName.Trim();
         var originalNameToCheck = OriginalName;
         if (!compareUsingCase)
         {
            newNameToCheck = newNameToCheck.ToLower();
            originalNameToCheck = OriginalName.ToLower();

         }

         return !string.IsNullOrEmpty(OriginalName) && string.Equals(originalNameToCheck, newNameToCheck);
      }

      private static class RenameObjectDTORules
      {
         public static IBusinessRule NameShouldNotBeTheSame { get; } = CreateRule.For<RenameObjectDTO>()
            .Property(x => x.Name)
            .WithRule((dto, name) => (!dto.sameAsOriginal(name, compareUsingCase:true))) //Compare using case to ensure that we can indeed rename a typo => PAram => Param
            .WithError(Error.RenameSameNameError);
      }
   }
}