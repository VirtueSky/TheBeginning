using System.Runtime.CompilerServices;

// Test assemblies
#if UNITY_INCLUDE_TESTS
[assembly: InternalsVisibleTo("Unity.LevelPlay.Tests")]
#endif

[assembly: InternalsVisibleTo("Unity.LevelPlay.Editor")]
