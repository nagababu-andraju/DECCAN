using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace DeccanHeat.Tests.PlayMode
{
    public class PlayModeTestStub
    {
        [UnityTest]
        public IEnumerator CoreGameplayLoop_RunsWithoutExceptions()
        {
            // Placeholder: Initialize player, spawn vehicle, trigger entry animation.
            // Yield return null skips a frame.
            yield return null;

            Assert.Pass("PlayMode stub executed successfully.");
        }
    }
}
