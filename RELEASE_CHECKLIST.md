# Release Checklist

Before marking a build as a Release Candidate, verify the following:

- [ ] CI/CD pipeline is completely green (Lint, Test, Validate).
- [ ] Profiling shows steady 30 FPS on target mid-range devices in the most dense district.
- [ ] No Missing References or unassigned script variables in crucial prefabs.
- [ ] Mission Validator passes 100%.
- [ ] Save/Load state works accurately for player location, inventory, and mission progress.
- [ ] Licensing documentation is up to date and no unauthorized assets exist.
- [ ] Human Authorization explicitly provided for public distribution.
