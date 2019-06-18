using OSPSuite.Assets;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO
{
   public class RenameObjectDTO : ObjectBaseDTO
   {
      public string OriginalName { get; set; }

      /// <summary>
      /// Set to <c>true</c> (default), renaming a parameter using another case would be accepted. This is ok for rename scenario but not for clone scenario
      /// </summary>
      public bool AllowSameNameAsOriginalInDifferentCase { get; set; } = true;

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

         //This allow for rename scenario such as PAram=>Param
         return sameAsOriginal(newName, shouldCompareUsingOriginalCase: false);
      }

      private bool sameAsOriginal(string newName, bool shouldCompareUsingOriginalCase)
      {
         var newNameToCheck = newName.Trim();
         var originalNameToCheck = OriginalName;
         if (!shouldCompareUsingOriginalCase)
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
            .WithRule((dto, name) => (!dto.sameAsOriginal(name, dto.AllowSameNameAsOriginalInDifferentCase))) 
            .WithError(Error.RenameSameNameError);
      }
   }
}