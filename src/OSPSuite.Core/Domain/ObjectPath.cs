using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class ObjectPath : IReadOnlyCollection<string>
   {
      /// <summary>
      ///    String indicating that the referenced objectBase is located in a
      ///    Container above the referencing <see cref="IObjectBase" />
      /// </summary>
      public const string PARENT_CONTAINER = "..";

      /// <summary>
      ///    String separating elements of the <see cref="FormulaUsablePath" /> in String Representation
      /// </summary>
      public const string PATH_DELIMITER = "|";

      protected readonly List<string> _pathEntries;

      public static ObjectPath Empty { get; } = new ObjectPath();

      public ObjectPath() : this(new List<string>())
      {
      }

      public ObjectPath(params ObjectPath[] from) : this(from.SelectMany(x => x._pathEntries))
      {
      }

      public ObjectPath(params string[] pathEntries) : this(pathEntries.ToList())
      {
      }

      public ObjectPath(IEnumerable<string> pathEntries) => _pathEntries = pathEntries.ToList();

      /// <summary>
      ///    Single string describing the path
      /// </summary>
      public virtual string PathAsString
      {
         get
         {
            if (_pathEntries.Count == 0)
               return string.Empty;

            var returnString = new StringBuilder();
            foreach (var str in this)
            {
               returnString.Append(PATH_DELIMITER);
               returnString.Append(str);
            }

            return returnString.ToString().Substring(PATH_DELIMITER.Length);
         }
      }

      /// <summary>
      ///    Add entries at the end of the path
      /// </summary>
      /// <param name="pathEntriesToAdd">path entries to add</param>
      public virtual void AddRange(IEnumerable<string> pathEntriesToAdd) => pathEntriesToAdd.Each(Add);

      /// <summary>
      ///    Replace the first matching path entry with the given replacement if the entry is used in the current path
      /// </summary>
      /// <param name="entry">path entry to be replaced</param>
      /// <param name="replacement">replacements for the path entry</param>
      public virtual void Replace(string entry, string replacement)
      {
         int index = _pathEntries.IndexOf(entry);
         if (index == Constants.NOT_FOUND_INDEX)
            return;

         _pathEntries[index] = replacement;
      }

      /// <summary>
      ///    Replace the first matching path entry with the given replacements if the entry is used in the current path
      ///    returns true if a replacement was performed otherwise false
      /// </summary>
      /// <param name="entry">path entry to be replaced</param>
      /// <param name="replacements">list of replacements for the path entry </param>
      public virtual void Replace(string entry, IEnumerable<string> replacements)
      {
         int index = _pathEntries.IndexOf(entry);
         if (index == Constants.NOT_FOUND_INDEX)
            return;

         _pathEntries.RemoveAt(index);
         foreach (var replaceEntry in replacements.Reverse())
         {
            _pathEntries.Insert(index, replaceEntry);
         }
      }

      /// <summary>
      ///    Removes the first occurrence of specified <paramref name="entry" />.
      /// </summary>
      /// <param name="entry">The entry to be removed.</param>
      public virtual void Remove(string entry) => _pathEntries.Remove(entry);

      /// <summary>
      ///    Returns the entity of type <typeparamref name="T" /> with the given path relative to the
      ///    <paramref name="refEntity" />
      /// </summary>
      /// <exception cref="Exception">
      ///    is thrown if object could not be retrieve or if the specified type <typeparamref name="T" /> does not match the
      ///    retrieved type
      /// </exception>
      public virtual T Resolve<T>(IEntity refEntity) where T : class
      {
         if (_pathEntries.Count == 0)
            return null;

         var usePath = new List<string>(_pathEntries);

         var dependentObject = refEntity;
         //Do we have a valid relative path from the current object
         var resolvedEntity = resolvePath<T>(dependentObject, usePath);
         if (resolvedEntity != null)
            return resolvedEntity;

         var root = refEntity.RootContainer;

         removeMatchingParentPath(root, usePath);

         var firstEntry = usePath[0];

         //We have an absolute Path from the ref entity
         if (string.Equals(firstEntry, refEntity.Name))
            usePath.RemoveAt(0);

         //We have an absolute Path from the root container
         else if (root != null && string.Equals(firstEntry, root.Name))
         {
            if (_pathEntries.Count == 1)
               return root as T;

            usePath.RemoveAt(0);
            dependentObject = root;
         }

         return resolvePath<T>(dependentObject, usePath);
      }

      private void removeMatchingParentPath(IContainer root, List<string> usePath)
      {
         var matchingElements = root?.ParentPath != null &&
                                new ObjectPath(_pathEntries).StartsWith(root?.ParentPath);

         if (matchingElements)
            usePath.RemoveRange(0, root.ParentPath.Count);
      }

      public virtual T Clone<T>() where T : ObjectPath => new ObjectPath(_pathEntries).DowncastTo<T>();

      /// <summary>
      ///    Gets or set the item at the given index (Should be used for replace only)
      ///    Throws an exception if the index is bigger than the actual size of the path
      /// </summary>
      public virtual string this[int index]
      {
         get => _pathEntries[index];
         set => _pathEntries[index] = value;
      }

      /// <summary>
      ///    Remove the path element defined at the given index.
      ///    Throws an exception if the index is bigger than the actual size of the path
      /// </summary>
      /// <param name="index">The zero-based index of the item to remove</param>
      public virtual void RemoveAt(int index) => _pathEntries.RemoveAt(index);

      /// <summary>
      ///    Removes the first element. This is equivalent to RemoveAt(0).
      ///    Throws an exception if the path does not contain any element
      /// </summary>
      public virtual void RemoveFirst() => RemoveAt(0);

      /// <summary>
      ///    Replaces the path with the path entries in <paramref name="pathEntries" />
      /// </summary>
      /// <param name="pathEntries">Path entries used to replace the path</param>
      public virtual void ReplaceWith(IEnumerable<string> pathEntries)
      {
         _pathEntries.Clear();
         _pathEntries.AddRange(pathEntries);
      }

      public virtual void ConcatWith(ObjectPath objectPath) => AddRange(objectPath._pathEntries);

      public IEnumerator<string> GetEnumerator() => _pathEntries.GetEnumerator();

      IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

      /// <summary>
      ///    Add path entries to the end of the path
      /// </summary>
      public virtual void Add(string pathToAdd) => pathToAdd.ToPathArray().Each(x => _pathEntries.Add(x));

      /// <summary>
      ///    Add path entries to the front of the path
      /// </summary>
      public virtual void AddAtFront(string pathToAdd) => addAtFront(pathToAdd.ToPathArray());

      private void addAtFront(IReadOnlyList<string> pathElements) => pathElements.Reverse().Each(x => _pathEntries.Insert(0, x));

      /// <summary>
      ///    Add a multipart <paramref name="pathToAdd" /> at the front
      /// </summary>
      public virtual void AddAtFront(ObjectPath pathToAdd) => addAtFront(pathToAdd._pathEntries);

      private T resolvePath<T>(IEntity currentEntity, IEnumerable<string> path) where T : class
      {
         if (currentEntity == null)
            return null;

         var usedPath = new List<string>(path);
         //no entry in the path. return the current entity
         if (usedPath.Count == 0)
            return currentEntity as T;

         var pathElement = usedPath[0];
         usedPath.RemoveAt(0);

         //Relative path: Resolve entity in parent container
         if (pathElement.Equals(PARENT_CONTAINER))
            return resolvePath<T>(currentEntity.ParentContainer, usedPath);

         //Absolute path: current entity has to be a container and we resolve in the child container 
         var currentContainer = currentEntity as IContainer;
         if (currentContainer == null)
            return null;

         return resolvePath<T>(currentContainer.GetSingleChildByName(pathElement), usedPath);
      }

      public bool Equals(ObjectPath other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;

         if (other.Count() != this.Count()) return false;
         int i = 0;
         foreach (var entry in other)
         {
            if (!_pathEntries[i].Equals(entry)) return false;
            i++;
         }

         return true;
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         return Equals(obj as ObjectPath);
      }

      public override int GetHashCode()
      {
         var hashCode = 0;
         if (_pathEntries != null && _pathEntries.Any())
         {
            hashCode = _pathEntries[0].GetHashCode();
            for (var i = 1; i < _pathEntries.Count(); i++)
            {
               hashCode = hashCode ^ _pathEntries[i].GetHashCode();
            }
         }

         return hashCode;
      }

      public override string ToString() => PathAsString;

      public static implicit operator string(ObjectPath objectPath)
      {
         return objectPath.ToString();
      }

      public int Count => _pathEntries.Count;

      public bool Contains(string entry)
      {
         //todo Optimize this
         return _pathEntries.Contains(entry);
      }

      public bool StartsWith(ObjectPath otherPath)
      {
         if (Count < otherPath.Count)
            return false;

         return !otherPath.Where((x, i) => _pathEntries[i] != x).Any();
      }
   }
}