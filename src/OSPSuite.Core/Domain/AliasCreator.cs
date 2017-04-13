using System;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public interface IAliasCreator
   {
      string CreateAliasFrom(string name);
      string CreateAliasFrom(string name,IEnumerable<string> forbiddenAliases);
   }

   public class AliasCreator : IAliasCreator
   {
      private readonly IEnumerable<char> _illegalCharacters;
      private readonly char _replaceCharacter;

      public AliasCreator(IEnumerable<char> illegalCharacters, char replaceCharacter)
      {
         _illegalCharacters = illegalCharacters;
         _replaceCharacter = replaceCharacter;
      }

      public AliasCreator()
         : this(new[] { '+', '-', '*', '\\', '/', '^', '.', ',', '<', '>', '=', '(', ')', '[', ']', '{', '}', '\'', '\"', '|', '&', ';', '¬', ' ', '\t', '\n', '\r' },
         '_')
      {
      }

      public string CreateAliasFrom(string name)
      {
         var alias = name;
         alias =alias.Trim();
         _illegalCharacters.Each(illeagelCharacter => alias = replaceIn(alias, illeagelCharacter));
         alias = checkForNumeric(alias);
         return alias;
      }

      public string CreateAliasFrom(string name, IEnumerable<string> forbiddenAliases)
      {
         string baseAlias= CreateAliasFrom(name);
         string alias = baseAlias;
         int i = 1;
         while (forbiddenAliases.ContainsItem(alias))
         {
            alias = $"{baseAlias}{i++}";
         }
         return alias;
      }

      private string checkForNumeric(string name)
      {
         double d;
         if(double.TryParse(name, out d))
         {
            return $"_{name}";
         }
         return name;
      }
     
      private string replaceIn(string name, char illeagelCharacter)
      {
         return name.Replace(illeagelCharacter, _replaceCharacter);
      }

      
   }
}