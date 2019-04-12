using System.Collections;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain
{
   public interface IKeywordReplacerCollection : IEnumerable<IKeywordReplacer>
   {
      /// <summary>
      ///    replace the keywords used in the given object path. This should only be call for object path that do not represent a
      ///    formula. For formula, used ReplaceReferences Returns true if the object path was changed otherwise false
      /// </summary>
      /// <param name="objectPath"> </param>
      void ReplaceIn(IObjectPath objectPath);

      /// <summary>
      ///    Adds the replacement.
      /// </summary>
      /// <param name="replacement"> The replacement. </param>
      void AddReplacement(IKeywordReplacer replacement);

      void ReplaceIn(IUsingFormula usingFormula);

      /// <summary>
      ///    Replace the tag value with all possible replacement induced by a IKeywordInTagReplacer
      /// </summary>
      /// <param name="tag"></param>
      void ReplaceIn(Tags tag);

      /// <summary>
      ///    Replace the criteria value with all possible replacement induced by a IKeywordInTagReplacer
      /// </summary>
      void ReplaceIn(IDescriptorCondition descriptorCondition);
   }

   public class KeywordReplacerCollection : IKeywordReplacerCollection
   {
      private readonly IList<IKeywordReplacer> _allKeywordReplacer;
      private readonly IList<IKeywordInObjectPathReplacer> _allObjectPathReplacer;
      private readonly IList<IKeywordInTagsReplacer> _allTagReplacer;

      public KeywordReplacerCollection()
      {
         _allKeywordReplacer = new List<IKeywordReplacer>();
         _allObjectPathReplacer = new List<IKeywordInObjectPathReplacer>();
         _allTagReplacer = new List<IKeywordInTagsReplacer>();
      }

      public KeywordReplacerCollection(IEnumerable<IKeywordReplacer> keywordReplacements) : this()
      {
         keywordReplacements.Each(AddReplacement);
      }

      public void AddReplacement(IKeywordReplacer replacer)
      {
         _allKeywordReplacer.Add(replacer);

         if (replacer.IsAnImplementationOf<IKeywordInObjectPathReplacer>())
            _allObjectPathReplacer.Add(replacer.DowncastTo<IKeywordInObjectPathReplacer>());

         if (replacer.IsAnImplementationOf<IKeywordInTagsReplacer>())
            _allTagReplacer.Add(replacer.DowncastTo<IKeywordInTagsReplacer>());
      }

      public void ReplaceIn(IUsingFormula usingFormula)
      {
         replaceInFormula(usingFormula.Formula);

         var parameter = usingFormula as IParameter;
         if (parameter == null) return;

         replaceInFormula(parameter.RHSFormula);

         replaceTagsIn(usingFormula);
      }

      public void ReplaceIn(Tags tags)
      {
         _allTagReplacer.Each(r => r.ReplaceIn(tags));
      }

      public void ReplaceIn(IDescriptorCondition descriptorCondition)
      {
         _allTagReplacer.Each(r => r.ReplaceIn(descriptorCondition));
      }

      private void replaceTagsIn(IEntity entity)
      {
         ReplaceIn(entity.Tags);
      }

      private void replaceInFormula(IFormula formula)
      {
         if (formula == null) return;
         formula.ObjectPaths.Each(ReplaceIn);

         replaceInDynamicFormula(formula as DynamicFormula);
      }

      private void replaceInDynamicFormula(DynamicFormula dynamicFormula)
      {
         dynamicFormula?.Criteria.Each(ReplaceIn);
      }

      public void ReplaceIn(IObjectPath objectPath)
      {
         _allObjectPathReplacer.Each(r => r.ReplaceIn(objectPath));
      }

      public IEnumerator<IKeywordReplacer> GetEnumerator()
      {
         return _allKeywordReplacer.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }
   }
}