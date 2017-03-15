using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Builder
{
   /// <summary>
   ///    Contains all Information needed to create a <see cref="IReaction" /> with the <see cref="IModelConstructor" />
   /// </summary>
   public interface IReactionBuilder : IProcessBuilder
   {
      IEnumerable<IReactionPartnerBuilder> Educts { get; }
      IEnumerable<IReactionPartnerBuilder> Products { get; }
      void AddEduct(IReactionPartnerBuilder educt);
      void AddProduct(IReactionPartnerBuilder product);
      void RemoveEduct(IReactionPartnerBuilder educt);
      void RemoveProduct(IReactionPartnerBuilder product);
      IEnumerable<string> ModifierNames { get; }
      void AddModifier(string modifierName);
      void RemoveModifier(string modifierToRemove);
      void ClearModifiers();

      /// <summary>
      ///    Criteria for containers where reaction should be created
      /// </summary>
      DescriptorCriteria ContainerCriteria { set; get; }

      /// <summary>
      ///    Returns an educt partner for molecule <paramref name="moleculeName" /> or null if not found
      /// </summary>
      IReactionPartnerBuilder EductBy(string moleculeName);

      /// <summary>
      ///    Returns an product partner for molecule <paramref name="moleculeName" /> or null if not found
      /// </summary>
      IReactionPartnerBuilder ProductBy(string moleculeName);
   }

   /// <summary>
   ///    Contains all Information needed to create a <see cref="IReaction" /> with the <see cref="IModelConstructor" />
   /// </summary>
   public class ReactionBuilder : ProcessBuilder, IReactionBuilder
   {
      private readonly IList<IReactionPartnerBuilder> _educts;
      private readonly IList<IReactionPartnerBuilder> _products;
      private readonly IList<string> _modifier;
      public DescriptorCriteria ContainerCriteria { get; set; }

      public ReactionBuilder()
      {
         _educts = new List<IReactionPartnerBuilder>();
         _products = new List<IReactionPartnerBuilder>();
         _modifier = new List<string>();
         ContainerCriteria = new DescriptorCriteria();
         Icon = IconNames.REACTION;
         ContainerType = ContainerType.Reaction;
      }

      public IEnumerable<IReactionPartnerBuilder> Educts => _educts;
      public IEnumerable<IReactionPartnerBuilder> Products => _products;

      public void AddEduct(IReactionPartnerBuilder educt)
      {
         if (_educts.Contains(educt))
         {
            throw new NotUniqueNameException(educt.MoleculeName, Name);
         }

         _educts.Add(educt);
         OnChanged();
      }

      public void AddProduct(IReactionPartnerBuilder product)
      {
         if (_products.Contains(product))
         {
            throw new NotUniqueNameException(product.MoleculeName, Name);
         }

         _products.Add(product);
         OnChanged();
      }

      public void RemoveEduct(IReactionPartnerBuilder educt)
      {
         _educts.Remove(educt);
      }

      public void RemoveProduct(IReactionPartnerBuilder product)
      {
         _products.Remove(product);
      }

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

      public void ClearModifiers()
      {
         _modifier.Clear();
      }

      public IReactionPartnerBuilder EductBy(string moleculeName)
      {
         return _educts.Find(x => string.Equals(x.MoleculeName, moleculeName));
      }

      public IReactionPartnerBuilder ProductBy(string moleculeName)
      {
         return _products.Find(x => string.Equals(x.MoleculeName, moleculeName));
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);

         var srcReactionBuilder = source as IReactionBuilder;
         if (srcReactionBuilder == null) return;

         srcReactionBuilder.Educts.Each(e => AddEduct(e.Clone()));
         srcReactionBuilder.Products.Each(p => AddProduct(p.Clone()));
         srcReactionBuilder.ModifierNames.Each(AddModifier);
         ContainerCriteria = srcReactionBuilder.ContainerCriteria.Clone();
      }
   }
}