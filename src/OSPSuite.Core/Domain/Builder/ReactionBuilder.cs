using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Contains all Information needed to create a <see cref="Reaction" /> with the <see cref="IModelConstructor" />
   /// </summary>
   public class ReactionBuilder : ProcessBuilder
   {
      private readonly List<ReactionPartnerBuilder> _educts;
      private readonly List<ReactionPartnerBuilder> _products;
      private readonly List<string> _modifier;

      /// <summary>
      ///    Criteria for containers where reaction should be created
      /// </summary>
      public DescriptorCriteria ContainerCriteria { get; set; }

      public ReactionBuilder()
      {
         _educts = new List<ReactionPartnerBuilder>();
         _products = new List<ReactionPartnerBuilder>();
         _modifier = new List<string>();
         ContainerCriteria = new DescriptorCriteria();
         Icon = IconNames.REACTION;
         ContainerType = ContainerType.Reaction;
      }

      public IEnumerable<ReactionPartnerBuilder> Educts => _educts;
      public IEnumerable<ReactionPartnerBuilder> Products => _products;

      public void AddEduct(ReactionPartnerBuilder educt)
      {
         if (_educts.Contains(educt))
         {
            throw new NotUniqueNameException(educt.MoleculeName, Name);
         }

         _educts.Add(educt);
         OnChanged();
      }

      public void AddProduct(ReactionPartnerBuilder product)
      {
         if (_products.Contains(product))
         {
            throw new NotUniqueNameException(product.MoleculeName, Name);
         }

         _products.Add(product);
         OnChanged();
      }

      public void RemoveEduct(ReactionPartnerBuilder educt) => _educts.Remove(educt);

      public void RemoveProduct(ReactionPartnerBuilder product) => _products.Remove(product);

      public IEnumerable<string> ModifierNames => _modifier;

      public void AddModifier(string modifierName)
      {
         if (_modifier.Contains(modifierName))
         {
            throw new NotUniqueNameException(modifierName, Name);
         }

         _modifier.Add(modifierName);
         OnChanged();
      }

      public void RemoveModifier(string modifierToRemove)
      {
         _modifier.Remove(modifierToRemove);
      }

      public void ClearModifiers() => _modifier.Clear();

      /// <summary>
      ///    Returns an educt partner for molecule <paramref name="moleculeName" /> or null if not found
      /// </summary>
      public ReactionPartnerBuilder EductBy(string moleculeName)
      {
         return _educts.Find(x => string.Equals(x.MoleculeName, moleculeName));
      }

      /// <summary>
      ///    Returns an product partner for molecule <paramref name="moleculeName" /> or null if not found
      /// </summary>
      public ReactionPartnerBuilder ProductBy(string moleculeName)
      {
         return _products.Find(x => string.Equals(x.MoleculeName, moleculeName));
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcReactionBuilder = source as ReactionBuilder;
         if (srcReactionBuilder == null) return;

         srcReactionBuilder.Educts.Each(e => AddEduct(e.Clone()));
         srcReactionBuilder.Products.Each(p => AddProduct(p.Clone()));
         srcReactionBuilder.ModifierNames.Each(AddModifier);
         ContainerCriteria = srcReactionBuilder.ContainerCriteria.Clone();
      }
   }
}