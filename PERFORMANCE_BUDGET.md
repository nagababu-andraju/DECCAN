# Performance Budget

## Target Profile
- **Device**: Mid-range Android (e.g., Snapdragon 6/7 series equivalent).
- **Framerate**: Stable 30 FPS.
- **Optional**: 60 FPS mode for high-end devices.

## Budgets per Frame
- **CPU Time**: < 33.3ms for 30fps.
- **Draw Calls**: < 300 per frame.
- **Triangles**: < 500k visible per frame.
- **GC Allocations**: Zero steady-state allocations during gameplay loops.

## Optimization Techniques Required
1. Object Pooling (Mandatory for all spawned entities).
2. LODs for all environment and character meshes.
3. Occlusion & Frustum culling.
4. Streamed world sectors (Addressables).
5. Bounded NPC/Traffic simulation distance.
