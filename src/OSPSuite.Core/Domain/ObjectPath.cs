using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public interface IObjectPath : IReadOnlyCollection<string>
   {
      /// <summary>
      ///    Single string describing the path
      /// </summary>
      string PathAsString { get; }

      /// <summary>
      ///    Add one entry at the end of the path
      /// </summary>
      /// <param name="pathEntry">path entry to add</param>
      void Add(string pathEntry);

      /// <summary>
      ///    Add one entry at the front of the path
      /// </summary>
      /// <param name="pathEntry">path entry to add</param>
      void AddAtFront(string pathEntry);

      /// <summary>
      ///    Replace the first matching path entry with the given replacement if the entry is used in the current path
      /// </summary>
      /// <param name="entry">path entry to be replaced</param>
      /// <param name="replacement">replacements for the path entry</param>
      void Replace(string entry, string replacement);

      /// <summary>
      ///    Replace the first matching path entry with the given replacements if the entry is used in the current path
      ///    returns true if a replacement was performed otherwise false
      /// </summary>
      /// <param name="entry">path entry to be replaced</param>
      /// <param name="replacements">list of replacements for the path entry </param>
      void Replace(string entry, IEnumerable<string> replacements);

      /// <summary>
      ///    Removes the first occurence of specified <paramref name="entry" />.
      /// </summary>
      /// <param name="entry">The entry to be removed.</param>
      void Remove(string entry);

      /// <summary>
      ///    Returns the entity of type <typeparamref name="T" /> with the given path relatve to the
      ///    <paramref name="refEntity" />
      /// </summary>
      /// <exception cref="Exception">
      ///    is thrown if object could not be retrieve or if the specified type <typeparamref name="T" /> does not match the
      ///    retrieved type
      /// </exception>
      T Resolve<T>(IEntity refEntity) where T : class;

      T Clone<T>() where T : IObjectPath;

      /// <summary>
      ///    Gets or set the item at the given index (Should be used for replace only)
      ///    Throws an exception if the index is bigger than the actual size of the path
      /// </summary>
      string this[int index] { get; set; }

      /// <summary>
      ///    Remove the path element defined at the given index.
      ///    Throws an exception if the index is bigger than the actual size of the path
      /// </summary>
      /// <param name="index">The zero-based index of the item to remove</param>
      void RemoveAt(int index);

      /// <summary>
      ///    Removes the first element. This is equivalent to RemoveAt(0).
      ///    Throws an exception if the path does not contain any element
      /// </summary>
      void RemoveFirst();
   }

   public class ObjectPath : IObjectPath
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

      protected readonly IList<string> _pathEntries;

      public static IObjectPath Empty { get; } =  new ObjectPath();

      public ObjectPath() : this(new List<string>())
      {
      }

      public ObjectPath(params string[] pathEntries) : this(pathEntries.ToList())
      {
      }

      public ObjectPath(IEnumerable<string> pathEntries)
      {
         _pathEntries = pathEntries.ToList();
      }

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

      public void Add(string pathEntry)
      {
         _pathEntries.Add(pathEntry);
      }

      public void Replace(string entry, string replacement)
      {
         int index = _pathEntries.IndexOf(entry);
         if (index == Constants.NOT_FOUND_INDEX)
            return;

         _pathEntries[index] = replacement;
      }

      public void Replace(string entry, IEnumerable<string> replacements)
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

      public void Remove(string entry)
      {
         _pathEntries.Remove(entry);
      }

      public virtual T Resolve<T>(IEntity refEntity) where T : class
      {
         if (_pathEntries.Count == 0)
            return null;

         var firstEntry = _pathEntries[0];
         var root = refEntity.RootContainer;
         var usePath = new List<string>(_pathEntries);
         IEntity dependentObject;

         //We have an absolute Path from the root container
         if (root != null && string.Equals(firstEntry, root.Name))
         {
            if (_pathEntries.Count == 1)
               return root as T;

            usePath.RemoveAt(0);
            dependentObject = root;
         }

         //We have an absolute Path from the ref entity
         else if (string.Equals(firstEntry, refEntity.Name))
         {
            usePath.RemoveAt(0);
            dependentObject = refEntity;
         }
         //We have a relative path
         else
            dependentObject = refEntity;

         return resolvePath<T>(dependentObject, usePath);
      }

      public virtual T Clone<T>() where T : IObjectPath
      {
         return new ObjectPath(_pathEntries).DowncastTo<T>();
      }

      public string this[int index]
      {
         get { return _pathEntries[index]; }
         set { _pathEntries[index] = value; }
      }

      public void RemoveAt(int index)
      {
         _pathEntries.RemoveAt(index);
      }

      public void RemoveFirst()
      {
         RemoveAt(0);
      }

      public IEnumerator<string> GetEnumerator()
      {
         return _pathEntries.GetEnumerator();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
         return GetEnumerator();
      }

      public void AddAtFront(string pathEntry)
      {
         _pathEntries.Insert(0, pathEntry);
      }

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

      public override string ToString()
      {
         return PathAsString;
      }

      public static implicit operator string(ObjectPath objectPath)
      {
         return objectPath.ToString();
      }

      public int Count
      {
         get { return _pathEntries.Count; }
      }
   }
}