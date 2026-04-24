## Merge Behavior Documentation Verification

### 1. MergeBehavior Enum — `Domain/Module.cs`

**File:** `/src/OSPSuite.Core/Domain/Module.cs`, lines 18–34 & 56

✅ **Confirmed.** The enum is declared at the module level with exactly two values:
- `Overwrite` (value 0, **default** — `MergeBehavior MergeBehavior { get; set; } = MergeBehavior.Overwrite;`, line 56)
- `Extend` (value 1)

`ModuleConfiguration.MergeBehavior` (in `ModuleConfiguration.cs` line 27) delegates to `Module.MergeBehavior`, so the behavior is truly module-level.

---

### 2. SimulationBuilder.cs — Dispatch and Merge Methods

**File:** `/src/OSPSuite.Core/Domain/Builder/SimulationBuilder.cs`

**`performMerge()` (lines 96–106):** All five entity types are routed through the generic `mergeBuilders<T>` helper:

```
_reactions     → mergeBuilders(x => x.Reactions,        mergeReactions)     // line 98
_eventGroups   → mergeBuilders(x => x.EventGroups,      mergeEvents)        // line 99
_molecules     → mergeBuilders(x => x.Molecules,        mergeMolecules)     // line 100
_passiveTransports → mergeBuilders(x => x.PassiveTransports, mergeTransports) // line 101
_observers     → mergeBuilders(x => x.Observers,        mergeObservers)     // line 102
```

**`analyzeBuilderMerges<T>()` (lines 432–484):**
- Groups all builders (across all modules) by **name**
- If only one builder exists with that name → used as-is (no clone needed)
- If the **last** builder in the chain is `Overwrite` → that entire builder is used as-is (preceding ones are discarded)
- Otherwise, the last `Overwrite` in the chain (or the first builder if none) becomes the **base**; all subsequent builders are the "extend" list

**`mergeBuilders<T>()` (lines 108–135):**
- For each `BuilderMergeInfo`, clones the base if extensions are present
- Calls **`tryExtendContainers(baseBuilder, finalSourceBuilderThatExtend)`** (line 125) — handles container-level merging
- Then calls the **type-specific** `extendStrategyAction` (line 126) — handles type-specific fields

**`tryExtendContainers<T>()` (lines 143–151):**  
Casts both target and source to `IContainer`; if both are containers, calls `mergeContainers`, which caches entity sources and delegates to `ContainerMergeTask.MergeContainers`.

✅ **Confirmed.** All five merge methods exist and the dispatch mechanism is exactly as described. One notable nuance: `mergeReactions` calls `tryExtendContainers` a **second time** (line 175), making it effectively a double container-merge for reactions (idempotent due to replace semantics, but worth noting).

---

### 3. Spatial Structure Merge — `SpatialStructureMerger.cs` & `ContainerMergeTask.cs`

**Files:**  
- `/src/OSPSuite.Core/Domain/Services/SpatialStructureMerger.cs`  
- `/src/OSPSuite.Core/Domain/Services/ContainerMergeTask.cs`

**MoleculeProperties always extended:**  
`replaceOrMergeContainerIntoParent` (SpatialStructureMerger.cs line 141–148):
```csharp
if (mergeBehavior == MergeBehavior.Extend || containerToMerge.IsNamed(Constants.MOLECULE_PROPERTIES))
    _containerMergeTask.AddOrMergeContainer(parentContainer, containerToMerge);
else
    _containerMergeTask.AddOrReplaceInContainer(parentContainer, containerToMerge);
```
Additionally, `GlobalMoleculeContainers` from **all** spatial structures are unconditionally merged (lines 81–93) regardless of `MergeBehavior`. ✅ **Confirmed — MoleculeProperties is always extended, even in Overwrite mode.**

**Parameters behavior (ContainerMergeTask.cs lines 42–63):**  
`MergeContainers` splits children into sub-containers vs leaf entities. Leaf entities (parameters, formulas declared as child nodes) are all run through `AddOrReplaceInContainer` (line 62), which does a remove-then-add. ✅ **Confirmed — existing parameters are replaced by name in Extend mode.**

**Container mode (ContainerMergeTask.cs lines 65–68):**  
```csharp
targetContainer.Mode = containerToMerge.Mode;
```
✅ **Confirmed — the incoming container's Mode overwrites the existing one.**

**Tags (ContainerMergeTask.cs line 68):**  
```csharp
containerToMerge.Tags.Each(targetContainer.AddTag);
```
⚠️ **Nuanced.** Tags from the source are **added** to the target (additive/accumulative), not replaced. Existing tags are not removed.

**Formulas:**  
Formulas are not direct named children of containers in the model tree; they live in the building-block formula cache. `ContainerMergeTask` only handles `IEntity` children, so formula-cache merging is handled upstream (when building blocks are combined for simulation, not via ContainerMergeTask). ⚠️ **Nuanced — formula merging at the building-block level is outside ContainerMergeTask's scope.**

**Neighborhoods:**  
Handled separately in `NeighborhoodCollectionToContainerMapper` (see item 11).

---

### 4. Molecules Merge — `mergeMolecules` (SimulationBuilder.cs lines 266–279)

```csharp
target.DefaultStartFormula = incoming.DefaultStartFormula;  // line 269
target.Dimension            = incoming.Dimension;           // line 270
target.DisplayUnit          = incoming.DisplayUnit;         // line 271
target.IsFloating           = incoming.IsFloating;          // line 272
target.IsXenobiotic         = incoming.IsXenobiotic;        // line 273
target.QuantityType         = incoming.QuantityType;        // line 274
target.ClearUsedCalculationMethods();                        // line 277
incoming.UsedCalculationMethods.Each(x => target.AddUsedCalculationMethod(x.Clone())); // line 278
```

**`QuantityType` in Extend mode:** ❌ **Discrepancy.** The code at line 274 **does** set `target.QuantityType = incoming.QuantityType` in Extend mode. If documentation claims QuantityType is NOT changed in Extend, this is contradicted by the code.

**`IsFloating` (Stationary flag):** ⚠️ **Nuanced.** `IsFloating` IS overwritten (`target.IsFloating = incoming.IsFloating` line 272) in Extend mode — same observation as QuantityType.

**Calculation methods:** All previous methods are cleared (`ClearUsedCalculationMethods`, line 277) and replaced with clones of the incoming ones. ✅ **Confirmed — completely replaced.**

**Parameters:** Handled **before** `mergeMolecules` via `tryExtendContainers` (line 125 of `mergeBuilders`). Since `MoleculeBuilder extends Container`, parameter children are merged via `ContainerMergeTask.MergeContainers` — existing parameters (by name) are removed and replaced. ✅

**Distributed parameters** (parameter type): `ContainerMergeTask.MergeContainers` line 46 explicitly excludes `IDistributedParameter` from the container list — distributed parameters are treated as leaf entities and replaced wholesale (including their sub-parameters). ✅

**Active transports / `TransporterMoleculeContainerCollection`:** These are `TransporterMoleculeContainer` objects — which extend `Container`, so they are included in `allChildrenContainerToMerge` and recursively merged by `ContainerMergeTask`. ✅ **Confirmed — active transports follow the recursive container-merge logic.**

---

### 5. Reactions Merge — `mergeReactions` (SimulationBuilder.cs lines 172–196)

```csharp
tryExtendContainers(targetReaction, sourceReaction);         // line 175 (2nd call!)
targetReaction.Formula = incoming.Formula;                   // line 176
targetReaction.CreateProcessRateParameter = ...;             // line 177
targetReaction.ProcessRateParameterPersistable = ...;        // line 178
upsertPartners(targetReaction, incoming, isEduct: true);     // line 180 - educts
upsertPartners(targetReaction, incoming, isEduct: false);    // line 181 - products
// modifiers: additive (HashSet, lines 183–188)
targetReaction.ContainerCriteria = incoming.ContainerCriteria; // line 195 (if non-null)
```

**Educts/Products (`upsertPartners`, lines 198–222):** Upsert semantics — for each incoming partner, if one with the same molecule name exists it is removed first, then the incoming one (cloned) is added. ✅ **Confirmed — stoichiometry (from `ReactionPartnerBuilder.Clone()`) is preserved with the clone.**

**Modifiers (lines 183–188):** Additive — new modifier names are added only if not already present (via `HashSet.Add`). Existing modifiers are never removed in Extend mode. ✅

**Kinetic equation (formula):** Replaced unconditionally (`targetReaction.Formula = incoming.Formula`, line 176). ✅

**Parameters:** Handled via `ContainerMergeTask` (first `tryExtendContainers` from `mergeBuilders` at line 125, then a **second** call at line 175 inside `mergeReactions` — redundant but idempotent). ✅

**`ContainerCriteria`:** Replaced only if `incoming.ContainerCriteria != null` (lines 194–195). ✅

**`CreateProcessRateParameter` / `ProcessRateParameterPersistable`:** Both set from incoming. ✅

---

### 6. Passive Transports Merge — `mergeTransports` (SimulationBuilder.cs lines 224–234)

```csharp
mergeMoleculeLists(target, source);                              // line 227
mergeDescriptorCriteria(target.SourceCriteria, source.SourceCriteria); // line 228
mergeDescriptorCriteria(target.TargetCriteria, source.TargetCriteria); // line 229
target.CreateProcessRateParameter = source.CreateProcessRateParameter; // line 230
target.ProcessRateParameterPersistable = source.ProcessRateParameterPersistable; // line 231
target.Formula = source.Formula;                                 // line 233
```

**Kinetic equation:** Formula unconditionally replaced. ✅

**Parameters:** Handled via `tryExtendContainers` in `mergeBuilders` (since `TransportBuilder extends ProcessBuilder extends Container`). Parameters replaced by name. ✅

**Source/Target criteria (`mergeDescriptorCriteria`, lines 236–240):**
```csharp
target.Operator = source.Operator;       // operator replaced
source.Each(t => target.Add(t.CloneCondition())); // conditions APPENDED
```
⚠️ **Nuanced.** The `Operator` is replaced by the source's, but conditions from the source are **appended** (not replacing existing ones). This is an additive accumulation of criteria conditions.

**Molecule lists (`mergeMoleculeLists`, lines 242–259):**
- `ForAll` (the "All checkbox") is taken from source
- `MoleculeNames` from source are upserted (removed from exclude list, added to include list)
- `MoleculeNamesToExclude` from source are upserted (removed from include list, added to exclude list)

✅ **Confirmed — ForAll/Include/Exclude all handled.**

---

### 7. Observers Merge — `mergeObservers` (SimulationBuilder.cs lines 261–264)

```csharp
private void mergeObservers(ObserverBuilder target, BuilderSource<ObserverBuilder> source)
{
    mergeMoleculeLists(target, source.Builder);
}
```

**Critical finding — `ObserverBuilder extends Entity` (NOT Container):**  
`ObserverBuilder.cs` line 12: `public class ObserverBuilder : Entity, IUsingFormula, IMoleculeDependentBuilder`

Since `ObserverBuilder` is NOT a `Container`, `tryExtendContainers` (called at line 125 of `mergeBuilders`) **returns early** — the cast `builderSource.Builder as IContainer` returns null.

❌ **Discrepancy.** In Extend mode for observers:
- **Only** `MoleculeList` (include/exclude/ForAll) is merged from the extending builder
- `Formula` is **NOT** updated from the extending observer — it remains from the base builder
- `ContainerCriteria` is **NOT** updated from the extending observer — it remains from the base builder

If documentation claims formula and container criteria are updated in Extend mode for observers, this contradicts the code.

---

### 8. Events Merge — `mergeEvents` (SimulationBuilder.cs lines 164–170)

```csharp
private void mergeEvents(EventGroupBuilder targetBuilder, BuilderSource<EventGroupBuilder> source)
{
    var sourceBuilder = source.Builder;
    targetBuilder.EventGroupType = sourceBuilder.EventGroupType;               // line 167
    mergeDescriptorCriteria(targetBuilder.SourceCriteria, sourceBuilder.SourceCriteria); // line 169
}
```

`EventGroupBuilder extends Container`, so `tryExtendContainers` IS active for events — the event tree (child `EventBuilder` objects, parameters, assignments) is merged via `ContainerMergeTask` as container children.

**EventGroupType:** Unconditionally replaced. ✅

**SourceCriteria:** `mergeDescriptorCriteria` — Operator replaced, conditions appended (see item 6). ⚠️ **Nuanced.**

**Event tree (parameters, assignments, start condition formula):** These are child entities inside `EventGroupBuilder` (a Container). They are handled by `tryExtendContainers` → `ContainerMergeTask.MergeContainers`. Sub-containers are recursively merged; leaf entities (parameters, assignments) are replaced by name. ✅

**Start condition formula:** `EventBuilder` children and their start condition formulas are leaf entities in the container hierarchy and are replaced by name. ✅

---

### 9. Parameter Values Application Order — `QuantityValuesUpdater.cs` lines 57–71

```csharp
public ValidationResult UpdateQuantitiesValues(ModelConfiguration modelConfiguration)
{
    updateMoleculeAmountFromInitialConditions(modelConfiguration);       // ICs (not PVs)
    updateParameterFromExpressionProfiles(valueUpdater);                 // Expression Profiles
    updateParameterFromIndividualValues(valueUpdater);                   // Individual
    updateParameterValueFromParameterValues(valueUpdater);               // PV BBs last
}
```

The code comment at line 62: *"Add expressions profile before individual as some settings might be overwritten in the individual for aging"*

✅ **Confirmed order:** Expression Profiles → Individual → PV BBs (last wins for parameters)

⚠️ **Nuanced.** Documentation may describe the order as "Individual → Expression Profiles → PV BBs" but the actual priority (last-wins) is the **reverse**: Expression Profiles can be overwritten by Individual values. The comment explicitly acknowledges this is intentional for aging scenarios.

---

### 10. Initial Conditions Application Order — `SimulationBuilder.cs` lines 310–322

```csharp
private void mergeInitialConditions()
{
    mergeParameterValueBuilders(x => x.SelectedInitialConditions, initialConditionsFromConfigurationsCache);
    var expressionProfileInitialConditions = allInitialConditionsFromExpressionProfileSources();
    expressionProfileInitialConditionsCache.AddRange(...);

    // comment: "Concat order is important so that the values from expression profiles are overwritten if duplicated"
    _initialConditions.AddRange(expressionProfileInitialConditionsCache.Concat(initialConditionsFromConfigurationsCache));
}
```

`PathAndValueEntityCache` uses `MergeCache`, whose `Add` is `this[GetKey(value)] = value` (last writer wins). So in the concat:

1. Expression Profile ICs are added first → **lower priority**
2. IC BBs from module configurations are added second → **overwrite** EP ICs for duplicates

**Pre-step (model construction):** `MoleculeBuilderToMoleculeAmountMapper.createMoleculeAmountDefaultFormula` (line 91) sets the initial formula from `moleculeBuilder.DefaultStartFormula` — the Molecules BB default — before any IC override.

✅ **Confirmed.** The full priority chain is: **Molecules BB default → Expression Profile ICs → IC BBs** (each stage can overwrite the previous for duplicates).

---

### 11. NeighborhoodCollectionToContainerMapper — `NeighborhoodCollectionToContainerMapper.cs`

**`mergeNeighborhoodsInStructure` (lines 80–101):**

```csharp
if (mergeBehavior == MergeBehavior.Extend)
{
    var mergeNeighbor = _containerMergeTask.AddOrMergeContainer(neighborhoods, neighborhoodToMerge) as Neighborhood;
    updateNeighbors(mergeNeighbor, neighborhoodToMerge);  // updates FirstNeighbor/SecondNeighbor refs
}
else
    _containerMergeTask.AddOrReplaceInContainer(neighborhoods, neighborhoodToMerge);
```

**Extend mode:** Neighborhood containers are merged recursively (including their molecule property sub-containers). After merge, `FirstNeighbor` and `SecondNeighbor` reference-properties are updated from the source. ✅

**Overwrite mode:** Entire neighborhood replaced. ✅

The first spatial structure's neighborhoods are always added directly (no merge or replace check) — only subsequent structures respect the merge behavior.

---

## Summary Table

| # | Claim / Topic | File & Lines | Status | Notes |
|---|---|---|---|---|
| 1 | `MergeBehavior` enum — two values, module-level, Overwrite default | `Module.cs` 18–34, 56 | ✅ Confirmed | Exactly two values; default = `Overwrite` |
| 2 | `analyzeBuilderMerges` dispatch, `tryExtendContainers`, 5 type-specific methods | `SimulationBuilder.cs` 96–135, 143–151, 164/172/224/261/266 | ✅ Confirmed | All present; reactions call `tryExtendContainers` twice (idempotent) |
| 3a | Spatial Structure — MoleculeProperties always extended (even in Overwrite) | `SpatialStructureMerger.cs` 81–93, 144 | ✅ Confirmed | Hard-coded special case for `MOLECULE_PROPERTIES` name |
| 3b | Spatial Structure — parameters replaced by name in Extend | `ContainerMergeTask.cs` 62 | ✅ Confirmed | `AddOrReplaceInContainer` for all leaf entities |
| 3c | Spatial Structure — container Mode overwritten | `ContainerMergeTask.cs` 67 | ✅ Confirmed | `targetContainer.Mode = containerToMerge.Mode` |
| 3d | Spatial Structure — tags behavior | `ContainerMergeTask.cs` 68 | ⚠️ Nuanced | Tags are **additive** (source tags appended, existing not removed) |
| 4a | Molecules — `QuantityType` NOT changed in Extend (doc claim) | `SimulationBuilder.cs` 274 | ❌ Discrepancy | Code **does** set `target.QuantityType = incoming.QuantityType` |
| 4b | Molecules — `IsFloating` behavior | `SimulationBuilder.cs` 272 | ❌ Discrepancy (if doc claims unchanged) | Code **does** set `target.IsFloating = incoming.IsFloating` |
| 4c | Molecules — calculation methods replaced | `SimulationBuilder.cs` 277–278 | ✅ Confirmed | Cleared then replaced wholesale |
| 4d | Molecules — parameters merged by name | `SimulationBuilder.cs` 125 → `ContainerMergeTask.cs` 62 | ✅ Confirmed | Via `tryExtendContainers` + `AddOrReplaceInContainer` |
| 4e | Molecules — distributed parameter type handling | `ContainerMergeTask.cs` 46 | ✅ Confirmed | Treated as leaf entity; replaced wholesale |
| 4f | Molecules — active transports | `ContainerMergeTask.cs` 50–59 | ✅ Confirmed | `TransporterMoleculeContainer` (a Container) recursively merged |
| 5a | Reactions — formula (kinetic equation) replaced | `SimulationBuilder.cs` 176 | ✅ Confirmed | Unconditional assignment |
| 5b | Reactions — educts/products upserted by molecule name | `SimulationBuilder.cs` 198–222 | ✅ Confirmed | Remove-existing-then-add; stoichiometry cloned |
| 5c | Reactions — modifiers additive | `SimulationBuilder.cs` 183–188 | ✅ Confirmed | HashSet union; no removals |
| 5d | Reactions — ContainerCriteria replaced | `SimulationBuilder.cs` 194–195 | ✅ Confirmed | Replaced only if incoming non-null |
| 5e | Reactions — CreateProcessRateParameter/Persistable | `SimulationBuilder.cs` 177–178 | ✅ Confirmed | Both set from incoming |
| 6a | Passive Transports — formula replaced | `SimulationBuilder.cs` 233 | ✅ Confirmed | |
| 6b | Passive Transports — source/target criteria | `SimulationBuilder.cs` 228–229, 236–240 | ⚠️ Nuanced | Operator replaced; conditions **appended** (additive) |
| 6c | Passive Transports — molecule list (ForAll/include/exclude) | `SimulationBuilder.cs` 242–259 | ✅ Confirmed | ForAll taken from source; both lists upserted |
| 7a | Observers — `ObserverBuilder extends Entity` (NOT Container) | `ObserverBuilder.cs` 12 | ✅ Confirmed | Critical — changes Extend semantics |
| 7b | Observers — formula updated in Extend | `SimulationBuilder.cs` 261–264 | ❌ Discrepancy | Formula is **NOT** updated; `mergeObservers` only calls `mergeMoleculeLists` and ObserverBuilder is not a Container |
| 7c | Observers — ContainerCriteria updated in Extend | `SimulationBuilder.cs` 261–264 | ❌ Discrepancy | ContainerCriteria is **NOT** updated; same reason as 7b |
| 7d | Observers — molecule list behavior | `SimulationBuilder.cs` 262–263 | ✅ Confirmed | ForAll/include/exclude all handled via `mergeMoleculeLists` |
| 8a | Events — event tree (parameters, assignments) merged | `SimulationBuilder.cs` 125 → `ContainerMergeTask.cs` | ✅ Confirmed | `EventGroupBuilder` is a Container; tree merged recursively |
| 8b | Events — EventGroupType replaced | `SimulationBuilder.cs` 167 | ✅ Confirmed | |
| 8c | Events — SourceCriteria behavior | `SimulationBuilder.cs` 169, 236–240 | ⚠️ Nuanced | Operator replaced; conditions appended (additive) |
| 9 | Parameter Values — order Individual vs Expression Profiles vs PV BBs | `QuantityValuesUpdater.cs` 62–68 | ⚠️ Nuanced | Actual order: **ExpressionProfiles → Individual → PV BBs**; code comment confirms Individual can overwrite EP (for aging) |
| 10 | Initial Conditions — Molecules BB → Expression Profiles → IC BBs | `SimulationBuilder.cs` 310–322; `MoleculeBuilderToMoleculeAmountMapper.cs` 91–111 | ✅ Confirmed | Three-stage override chain; last-writer-wins cache |
| 11 | Neighborhood merge — Extend vs Overwrite | `NeighborhoodCollectionToContainerMapper.cs` 80–101 | ✅ Confirmed | Extend: container-merge + update neighbor refs; Overwrite: full replace |

---

### Key Discrepancies Found

1. **`QuantityType` (item 4a, `SimulationBuilder.cs` line 274):** Code unconditionally sets `target.QuantityType = incoming.QuantityType` in Extend mode. If documentation states this is NOT modified in Extend, the code contradicts it.

2. **`IsFloating` (item 4b, line 272):** Same — unconditionally overwritten in Extend mode.

3. **Observer Formula & ContainerCriteria (items 7b/7c):** Because `ObserverBuilder` inherits from `Entity` (not `Container`), `tryExtendContainers` is a no-op for observers, and `mergeObservers` only updates the molecule list. Formula and ContainerCriteria are left unchanged from the base builder in Extend mode.

4. **Parameter Value application order (item 9):** The actual execution order is `ExpressionProfiles → Individual → PV BBs` — meaning Individual values override Expression Profile values for the same parameter (not the other way around). Depending on the documentation's phrasing, this may represent a subtle ordering discrepancy.

5. **DescriptorCriteria accumulation (items 6b, 8c):** In `mergeDescriptorCriteria`, conditions are **appended** (never removed) from the incoming builder. The operator is replaced, but conditions accumulate across merges. This could lead to unintended criteria expansion if not documented as additive.
