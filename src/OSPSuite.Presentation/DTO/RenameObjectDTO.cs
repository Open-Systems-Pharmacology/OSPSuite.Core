namespace OSPSuite.Presentation.DTO
{
   public class RenameObjectDTO : ObjectBaseDTO
   {
      public RenameObjectDTO(string name)
      {
         Rules.Add(AllRules.NameShouldNotBeTheSame);
         OriginalName = name;
         Name = name;
      }

      protected override bool IsNameUnique(string newName)
      {
         if (base.IsNameUnique(newName))
            return true;

         return sameAsOriginalInLowCase(newName);
      }

      private bool sameAsOriginalInLowCase(string newName)
      {
         var newNameToLower = newName.ToLower().Trim();

         return !string.IsNullOrEmpty(OriginalName) && string.Equals(OriginalName.ToLower(), newNameToLower);
      }
   }
}