using System.Collections.Generic;

namespace OSPSuite.Core.Domain
{
   public abstract class CategoryItem : ObjectBase
   {
      private string _displayName;
      public string Category { get; set; }

      public string DisplayName
      {
         set => _displayName = value;
         get => _displayName ?? Name;
      }

      public override bool Equals(object obj)
      {
         var categoryItem = obj as CategoryItem;
         return categoryItem != null && string.Equals(categoryItem.Name, Name)
                                     && Equals(categoryItem.Category, Category);
      }

      public override int GetHashCode()
      {
         return Category.GetHashCode() * 37 + Name.GetHashCode();
      }

      public override string ToString()
      {
         return DisplayName;
      }
   }

   public class CalculationMethod : CategoryItem
   {
      private readonly List<string> _allSpecies = new List<string>();
      private readonly List<string> _allModels = new List<string>();

      /// <summary>
      ///    Species for which calculation method is defined
      /// </summary>
      public IReadOnlyList<string> AllSpecies => _allSpecies;

      public void AddSpecies(string species) => _allSpecies.Add(species);

      /// <summary>
      ///    Model for which calculation method is defined
      /// </summary>
      public IReadOnlyList<string> AllModels => _allModels;

      public void AddModel(string model) => _allModels.Add(model);
   }
}