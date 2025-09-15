using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IReactionMerger
   {
      IReadOnlyCollection<ReactionBuilder> Merge(
         IReadOnlyList<(ReactionBuildingBlock block, MergeBehavior behavior)> sources);
   }

   public class ReactionMerger : IReactionMerger
   {
      public IReadOnlyCollection<ReactionBuilder> Merge(
         IReadOnlyList<(ReactionBuildingBlock block, MergeBehavior behavior)> sources)
      {
         var result = new Dictionary<string, ReactionBuilder>(StringComparer.OrdinalIgnoreCase);
         if (sources == null || sources.Count == 0)
            return Array.Empty<ReactionBuilder>();

         foreach (var (block, behavior) in sources.Where(s => s.block != null))
         {
            foreach (var incoming in block)
            {
               if (!result.TryGetValue(incoming.Name, out var current))
               {
                  result[incoming.Name] = copyReaction(incoming);
                  continue;
               }

               if (behavior == MergeBehavior.Overwrite)
               {
                  result[incoming.Name] = copyReaction(incoming);
               }
               else
               {
                  ExtendReaction(current, incoming);
               }
            }
         }

         return result.Values.ToList();
      }

      private static ReactionBuilder copyReaction(ReactionBuilder r)
      {
         var copy = new ReactionBuilder
         {
            Name = r.Name,
            Description = r.Description,
            Icon = r.Icon,
            Dimension = r.Dimension,
            CreateProcessRateParameter = r.CreateProcessRateParameter,
            ProcessRateParameterPersistable = r.ProcessRateParameterPersistable,
            ContainerCriteria = r.ContainerCriteria
         };

         foreach (var e in r.Educts)
            copy.AddEduct(new ReactionPartnerBuilder(e.MoleculeName, e.StoichiometricCoefficient) { Dimension = e.Dimension });

         foreach (var p in r.Products)
            copy.AddProduct(new ReactionPartnerBuilder(p.MoleculeName, p.StoichiometricCoefficient) { Dimension = p.Dimension });

         foreach (var m in r.ModifierNames)
            copy.AddModifier(m);

         foreach (var param in r.Parameters)
            copy.AddParameter(param);

         copy.Formula = r.Formula;

         return copy;
      }

      private static void ExtendReaction(ReactionBuilder target, ReactionBuilder incoming)
      {
         var byNameParam = target.Parameters.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
         foreach (var p in incoming.Parameters)
         {
            if (byNameParam.TryGetValue(p.Name, out var existing))
               target.RemoveParameter(existing);
            target.AddParameter(p);
         }

         if (incoming.Formula != null)
            target.Formula = incoming.Formula;

         UpsertPartners(target, incoming, isEduct: true);

         UpsertPartners(target, incoming, isEduct: false);

         var mods = new HashSet<string>(target.ModifierNames, StringComparer.OrdinalIgnoreCase);
         foreach (var m in incoming.ModifierNames)
            if (mods.Add(m))
               target.AddModifier(m);

         target.Icon = incoming.Icon ?? target.Icon;
         target.Description = string.IsNullOrEmpty(incoming.Description) ? target.Description : incoming.Description;
         target.Dimension = incoming.Dimension ?? target.Dimension;

         if (incoming.ContainerCriteria != null)
            target.ContainerCriteria = incoming.ContainerCriteria;
      }

      private static void UpsertPartners(ReactionBuilder target, ReactionBuilder incoming, bool isEduct)
      {
         var (getList, add, remove) = isEduct
            ? ((Func<IEnumerable<ReactionPartnerBuilder>>)(() => target.Educts),
               (Action<ReactionPartnerBuilder>)(target.AddEduct),
               (Action<ReactionPartnerBuilder>)(target.RemoveEduct))
            : ((Func<IEnumerable<ReactionPartnerBuilder>>)(() => target.Products),
               (Action<ReactionPartnerBuilder>)(target.AddProduct),
               (Action<ReactionPartnerBuilder>)(target.RemoveProduct));

         var targetByMol = getList().ToDictionary(x => x.MoleculeName, StringComparer.OrdinalIgnoreCase);
         var incomingList = isEduct ? incoming.Educts : incoming.Products;

         foreach (var rp in incomingList)
         {
            if (targetByMol.TryGetValue(rp.MoleculeName, out var existing))
            {
               remove(existing);
               add(new ReactionPartnerBuilder(rp.MoleculeName, rp.StoichiometricCoefficient) { Dimension = rp.Dimension });
            }
            else
            {
               add(new ReactionPartnerBuilder(rp.MoleculeName, rp.StoichiometricCoefficient) { Dimension = rp.Dimension });
            }
         }
      }
   }
}
