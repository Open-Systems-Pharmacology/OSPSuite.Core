## Plan Mode

- Make the plan extremely concise. Sacrifice grammar for the sake of concision.
- At the end of each plan, give me a list of unresolved questions to answer, if any.


## Rules

### Quality
- **DO NOT BE LAZY. NEVER BE LAZY.** If there is a bug, find the root cause and fix it. No temporary fixes. You are a senior developer. Never take shortcuts.
- Keep changes minimal - impact as little code as possible.
- All code changes require matching unit/integration tests (exception: pure UI changes).

### Communication
- Provide high-level explanation of changes at each step.
- End each task with a review section summarizing changes.

### Architecture
- No business logic in UI - always in presenters or tasks.

### Workflow
- Never work on `develop` branch. Ask for branch name if on develop.
- Either code OR organize tasks - never both in same session. Ask before switching.


### Code Style
- Use 3-space indentation (no tabs)
- Private fields use underscore prefix: `_fieldName`
- Follow BDD test pattern with `concern_for_<SUT>` base classes and `[Observation]` attributes
- Use `.ShouldBeEqualTo()` assertions, not NUnit assertions

