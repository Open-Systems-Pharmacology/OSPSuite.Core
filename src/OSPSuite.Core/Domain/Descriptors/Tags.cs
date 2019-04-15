using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Descriptors
{
   /// <summary>
   ///    A Collection of Tags describing an entity. Common use for identifying source and target of transports
   /// </summary>
   public class Tags : IEnumerable<Tag>
   {
      private readonly List<Tag> _allTags;
      private readonly List<string> _allTagValues;

      public Tags() : this(Enumerable.Empty<Tag>())
      {
      }

      public Tags(IEnumerable<Tag> tags)
      {
         _allTags = new List<Tag>(tags);
         _allTagValues = new List<string>(_allTags.Select(x => x.Value));
      }

      public IEnumerator<Tag> GetEnumerator()
      {
         return _allTags.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public void Add(Tags tags) => tags?.Each(Add);

      public void Add(Tag tag)
      {
         if (_allTags.Contains(tag)) return;
         _allTags.Add(tag);
         _allTagValues.Add(tag.Value);
      }

      public bool Contains(string valueOfTag)
      {
         return _allTagValues.Contains(valueOfTag);
      }

      public void Remove(Tag tag)
      {
         Remove(tag.Value);
      }

      public void Remove(string valueOfTag)
      {
         var index = _allTagValues.IndexOf(valueOfTag);
         if (index < 0) return;
         _allTags.RemoveAt(index);
         _allTagValues.RemoveAt(index);
      }

      public void Replace(string keyword, string replacement)
      {
         for (int i = 0; i < _allTagValues.Count; i++)
         {
            if (!string.Equals(_allTagValues[i], keyword))
               continue;

            _allTags[i] = new Tag(replacement);
            _allTagValues[i] = replacement;
         }
      }
   }
}