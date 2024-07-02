#if LEVELPLAY_DEPENDENCIES_INSTALLED
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NotNull = JetBrains.Annotations.NotNullAttribute;

namespace Unity.Services.LevelPlay.Editor.IntegrationManager
{
    static class SdkRangeFinder
    {
        const string k_None = "none";

        static readonly Dictionary<string, Func<Version, Version, Version, bool>> RangeNotationToFunctionMap = new Dictionary<string, Func<Version, Version, Version, bool>>()
        {
            ["[]"] = (start, end, value) => value >= start && value <= end,
            ["[["] = (start, end, value) => value >= start && value < end,
            ["]]"] = (start, end, value) => value > start && value <= end,
            ["]["] = (start, end, value) => value > start && value < end
        };

        public static PackageVersionNativeCompatibility GetLatestPackageSDK(List<PackageVersionNativeCompatibility> versionData)
        {
            Version maxVersion = null;
            var packageVersion = new PackageVersionNativeCompatibility();

            foreach (var package in versionData)
            {
                var currentPackageVersion = Version.Parse(package.PackageVersion);
                if (maxVersion == null || currentPackageVersion > maxVersion)
                {
                    maxVersion = currentPackageVersion;
                    packageVersion = package;
                }
            }

            return packageVersion;
        }

        [NotNull]
        public static string GetLatestNativeSDKInRange(PackageVersionNativeCompatibility nativeCompatibility, List<string> nativeVersions)
        {
            var range = ParseRange(nativeCompatibility.CompatibleNativeVersionRange);
            var brackets = ExtractBrackets(nativeCompatibility.CompatibleNativeVersionRange);
            var rangeFunction = RangeNotationToFunctionMap[brackets];
            return FindLargestVersionInRange(range.start, range.end, nativeVersions, rangeFunction);
        }

        internal static string ExtractBrackets(string input)
        {
            var regex = new Regex("[\\[\\]]+");
            var matches = regex.Matches(input);

            var extractedBrackets = "";
            foreach (Match match in matches)
            {
                extractedBrackets += match.Value;
            }

            return extractedBrackets;
        }

        internal static (string start, string end) ParseRange(string rangeString)
        {
            var rangeParts = rangeString.Trim('[', ']').Split(',');

            var start = rangeParts[0].Trim();
            var end = rangeParts[1].Trim();

            return (start, end);
        }

        [NotNull]
        internal static string FindLargestVersionInRange(string rangeStart, string rangeEnd, List<string> versions, Func<Version, Version, Version, bool> rangeFunction)
        {
            var allVersionsInRange = FindVersionsInRange(rangeStart, rangeEnd, versions, rangeFunction);
            return FindLargestVersion(allVersionsInRange);
        }

        internal static IEnumerable<string> FindVersionsInRange(string rangeStart, string rangeEnd, List<string> versions, Func<Version, Version, Version, bool> rangeFunction)
        {
            var output = new List<string>();
            foreach (var strVersion in versions)
            {
                if (IsWithinVersionRange(rangeStart, rangeEnd, strVersion, rangeFunction))
                {
                    output.Add(strVersion);
                }
            }

            return output;
        }

        internal static bool IsWithinVersionRange(string rangeStart, string rangeEnd, string value, Func<Version, Version, Version, bool> rangeFunction)
        {
            var startVersion = Version.Parse(rangeStart);
            var endVersion = Version.Parse(rangeEnd);
            var valueVersion = Version.Parse(value);

            return rangeFunction(startVersion, endVersion, valueVersion);
        }

        [NotNull]
        internal static string FindLargestVersion(IEnumerable<string> versions)
        {
            Version largestVersion = null;

            foreach (var versionString in versions)
            {
                var version = Version.Parse(versionString);

                if (largestVersion == null || version > largestVersion)
                {
                    largestVersion = version;
                }
            }

            return largestVersion?.ToString() ?? k_None;
        }
    }
}
#endif
