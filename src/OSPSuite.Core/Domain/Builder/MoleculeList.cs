using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   public class MoleculeList : IUpdatable
   {
      private readonly List<string> _moleculeNamesToInclude;
      private readonly List<string> _moleculeNamesToExclude;

      /// <summary>
      ///    If true, all molecules (with exception of those in <see cref="MoleculeNamesToExclude" />) will be used
      ///    If false, only the molecules in <see cref="MoleculeNames" /> will be used
      /// </summary>
      public bool ForAll { get; set; }

      public MoleculeList()
      {
         ForAll = true;
         _moleculeNamesToInclude = new List<string>();
         _moleculeNamesToExclude = new List<string>();
      }

      /// <summary>
      ///    If <see cref="ForAll" /> is set to false, only molecules from this list will be used.
      ///    <para />
      ///    Otherwise (<see cref="ForAll" />=true) not relevant
      /// </summary>
      public virtual IEnumerable<string> MoleculeNames
      {
         get { return _moleculeNamesToInclude; }
      }

      /// <summary>
      ///    If <see cref="ForAll" /> is set to true, molecules from this liost will be excluded from use.
      ///    <para />
      ///    Otherwise (<see cref="ForAll" />=false) not relevant
      /// </summary>
      public virtual IEnumerable<string> MoleculeNamesToExclude
      {
         get { return _moleculeNamesToExclude; }
      }

      /// <summary>
      ///    Add molecule name to use. If molecule name already exists in the list or in
      ///    <see cref="MoleculeNamesToExclude" />, an exception will be thrown
      /// </summary>
      public virtual void AddMoleculeName(string moleculeName)
      {
         if (string.IsNullOrEmpty(moleculeName))
            return;

         if (_moleculeNamesToInclude.Contains(moleculeName))
            return;

         if (_moleculeNamesToExclude.Contains(moleculeName))
            throw new OSPSuiteException(Error.MoleculeNameExistsInAnotherList(moleculeName));

         _moleculeNamesToInclude.Add(moleculeName);
      }

      /// <summary>
      ///    Removes molecule name from the list
      /// </summary>
      public virtual void RemoveMoleculeName(string moleculeName)
      {
         if (string.IsNullOrEmpty(moleculeName))
            return;

         if (!_moleculeNamesToInclude.Contains(moleculeName))
            return;

         _moleculeNamesToInclude.Remove(moleculeName);
      }

      /// <summary>
      ///    Add molecule name to be excluded from use.
      ///    <para />
      ///    If molecule name already exists in the list or in
      ///    <see cref="MoleculeNames" />, an exception will be thrown
      /// </summary>
      public virtual void AddMoleculeNameToExclude(string moleculeName)
      {
         if (string.IsNullOrEmpty(moleculeName))
            return;

         if (_moleculeNamesToExclude.Contains(moleculeName))
            return;

         if (_moleculeNamesToInclude.Contains(moleculeName))
            throw new OSPSuiteException(Error.MoleculeNameExistsInAnotherList(moleculeName));

         _moleculeNamesToExclude.Add(moleculeName);
      }

      /// <summary>
      ///    Removes molecule name from the list
      /// </summary>
      public virtual void RemoveMoleculeNameToExclude(string moleculeName)
      {
         if (string.IsNullOrEmpty(moleculeName))
            return;

         if (!_moleculeNamesToExclude.Contains(moleculeName))
            return;

         _moleculeNamesToExclude.Remove(moleculeName);
      }

      public virtual void UpdatePropertiesFrom(IUpdatable updatable, ICloneManager cloneManager)
      {
         var sourceMoleculeDependentBuilder = updatable as MoleculeList;
         if (sourceMoleculeDependentBuilder == null) return;
         ForAll = sourceMoleculeDependentBuilder.ForAll;
         _moleculeNamesToInclude.Clear();
         sourceMoleculeDependentBuilder.MoleculeNames.Each(AddMoleculeName);

         _moleculeNamesToExclude.Clear();
         sourceMoleculeDependentBuilder.MoleculeNamesToExclude.Each(AddMoleculeNameToExclude);
      }

      public virtual void Update(MoleculeList moleculeList)
      {
         UpdatePropertiesFrom(moleculeList, null);
      }

      public virtual MoleculeList Clone()
      {
         var clone = new MoleculeList();
         clone.UpdatePropertiesFrom(this, null);
         return clone;
      }

      /// <summary>
      ///    Molecule is used if:
      ///    <para />
      ///    - <see cref="ForAll" /> = true and <see cref="MoleculeNamesToExclude" /> does not contain the molecule name or
      ///    - <see cref="ForAll" /> = false and <see cref="MoleculeNames" /> contains the molecule name
      /// </summary>
      public virtual bool Uses(string moleculeName)
      {
         if (ForAll && !MoleculeNamesToExclude.Contains(moleculeName))
            return true;

         if (!ForAll && MoleculeNames.Contains(moleculeName))
            return true;

         return false;
      }

      /// <summary>
      ///    Replaces any occurence of <paramref name="templateName" /> with <paramref name="moleculeName" /> in both lists to
      ///    include and exclude
      /// </summary>
      public virtual void ReplaceMoleculeName(string templateName, string moleculeName)
      {
         ReplaceMoleculeName(templateName, new[] {moleculeName});
      }

      /// <summary>
      ///    Replaces each occurence of <paramref name="templateName" /> with one entry for each name in
      ///    <paramref name="moleculeNames" /> in both lists to include and exclude
      /// </summary>
      public virtual void ReplaceMoleculeName(string templateName, IReadOnlyList<string> moleculeNames)
      {
         replaceMoleculeNameIn(_moleculeNamesToInclude, templateName, moleculeNames);
         replaceMoleculeNameIn(_moleculeNamesToExclude, templateName, moleculeNames);
      }

      private void replaceMoleculeNameIn(List<string> moleculeList, string templateName, IReadOnlyList<string> moleculeNames)
      {
         if (!moleculeList.Contains(templateName))
            return;

         moleculeList.Remove(templateName);
         moleculeNames.Each(moleculeList.Add);
      }
   }
}