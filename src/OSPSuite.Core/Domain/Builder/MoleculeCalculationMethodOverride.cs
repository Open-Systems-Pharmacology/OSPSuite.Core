using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using System;
using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Builder;

/// <summary>
///    Represents a container for calculation method overrides associated with a specific molecule.
/// </summary>
public class MoleculeCalculationMethodOverride
{
   private readonly Cache<string, UsedCalculationMethod> _usedCalculationMethods = new(getKey: x => x.Category);

   [Obsolete("For serialization")]
   public MoleculeCalculationMethodOverride()
   {
      
   }

   public MoleculeCalculationMethodOverride(string moleculeName)
   {
      MoleculeName = moleculeName;
   }

   public string MoleculeName { get; set; }

   public IReadOnlyCollection<UsedCalculationMethod> UsedCalculationMethods => _usedCalculationMethods;

   /// <summary>
   /// Adds a <paramref name="usedCalculationMethod"/> to override. If a method for the same category already exists, it will be replaced with the new one.
   /// </summary>
   public void AddUsedCalculationMethod(UsedCalculationMethod usedCalculationMethod)
   {
      _usedCalculationMethods[usedCalculationMethod.Category] = usedCalculationMethod;
   }

   public MoleculeCalculationMethodOverride Clone()
   {
      var clone = new MoleculeCalculationMethodOverride(MoleculeName);
      _usedCalculationMethods.Each(x => clone.AddUsedCalculationMethod(x.Clone()));
      return clone;
   }
}