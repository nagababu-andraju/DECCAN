# Quality Gates

To ensure DECCAN HEAT remains stable, maintainable, and highly performant on mobile devices, the following Quality Gates must be passed for all PRs and merges to main:

## 1. Automated Testing
- **Unit/EditMode Tests**: All logic not requiring runtime physics must have > 80% coverage.
- **PlayMode Tests**: Critical game loops (player movement, combat damage, vehicle entry/exit) must pass PlayMode tests.
- **Mission Validation**: All JSON/ScriptableObject mission files must pass structural and reference validation.

## 2. Performance Profiling
- **Framerate Budget**: 33.3ms (30 FPS) on target mid-range devices.
- **Memory**: Hard cap on GC allocations per frame (Zero GC allocation in update loops where possible).

## 3. Code Standards
- Adhere strictly to C# conventions.
- No direct hardcoding of textual assets; everything must go through the localization or data systems.
- Warnings treated as Errors in compilation.

## 4. Automation checks
- GitHub Actions must complete successfully before any merge.
