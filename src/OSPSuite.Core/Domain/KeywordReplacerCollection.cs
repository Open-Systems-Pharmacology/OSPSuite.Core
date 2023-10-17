using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class KeywordReplacerCollection : IEnumerable<IKeywordReplacer>
   {
      private readonly List<IKeywordReplacer> _allKeywordReplacer;

      public KeywordReplacerCollection()
      {
         _allKeywordReplacer = new List<IKeywordReplacer>();
      }

      public KeywordReplacerCollection(IEnumerable<IKeywordReplacer> keywordReplacements) : this()
      {
         keywordReplacements.Each(AddReplacement);
      }

      public void AddReplacement(IKeywordReplacer replacer) => _allKeywordReplacer.Add(replacer);

      public void AddReplacements(params IKeywordReplacer[] replacers) => replacers.Each(AddReplacement);

      /// <summary>
      ///    replace the keywords used in the given object path. This should only be call for object path that do not represent a
      ///    formula. For formula, used ReplaceReferences Returns true if the object path was changed otherwise false
      /// </summary>
      public void ReplaceIn(ObjectPath objectPath)
      {
         _allKeywordReplacer.OfType<IKeywordInObjectPathReplacer>().Each(r => r.ReplaceIn(objectPath));
      }

      public void ReplaceIn(IUsingFormula usingFormula)
      {
         replaceInFormula(usingFormula.Formula);

         if (!(usingFormula is IParameter parameter))
            return;

         replaceInFormula(parameter.RHSFormula);

         replaceTagsIn(usingFormula);
      }

      /// <summary>
      ///    Replace the tag value with all possible replacement induced by a IKeywordInTagReplacer
      /// </summary>
      public void ReplaceIn(Tags tags) => allTagReplacers.Each(r => r.ReplaceIn(tags));

      /// <summary>
      ///    Replace the criteria value with all possible replacement induced by a IKeywordInTagReplacer
      /// </summary>
      public void ReplaceIn(ITagCondition tagCondition)
         => _allKeywordReplacer.OfType<IKeywordInTagsReplacer>().Each(r => r.ReplaceIn(tagCondition));

      private IEnumerable<IKeywordInTagsReplacer> allTagReplacers => _allKeywordReplacer.OfType<IKeywordInTagsReplacer>();

      private void replaceTagsIn(IEntity entity) => ReplaceIn(entity.Tags);

      private void replaceInFormula(IFormula formula)
      {
         if (formula == null) return;
         formula.ObjectPaths.Each(ReplaceIn);

         replaceInDynamicFormula(formula as DynamicFormula);
      }

      private void replaceInDynamicFormula(DynamicFormula dynamicFormula) => dynamicFormula?.Criteria.Each(ReplaceIn);

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