﻿using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using CloudNimble.Breakdance.Assemblies;
using FluentAssertions;
using Microsoft.Restier.Breakdance;
using Microsoft.Restier.Tests.Shared;
using Microsoft.Restier.Tests.Shared.Scenarios.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Restier.Tests.AspNet.FeatureTests
{

    [TestClass]
    public class MetadataTests : RestierTestBase
    {

        #region Private Members

        private const string relativePath = "..//..//..//..//Microsoft.Restier.Tests.AspNet//";
        private const string baselineFolder = "Baselines//";

        #endregion

        #region LibraryApi

        [TestMethod]
        public async Task LibraryApi_CompareCurrentApiMetadataToPriorRun()
        {
            var fileName = $"{Path.Combine(relativePath, baselineFolder)}{typeof(LibraryApi).Name}-ApiMetadata.txt";
            File.Exists(fileName).Should().BeTrue();

            var oldReport = File.ReadAllText(fileName);
            var newReport = await RestierTestHelpers.GetApiMetadataAsync<LibraryApi, LibraryContext>();
            oldReport.Should().BeEquivalentTo(newReport.ToString());
        }

        [TestMethod]
        public async Task LibraryApi_CompareCurrentVisibilityMatrixToPriorRun()
        {
            var api = await RestierTestHelpers.GetTestableApiInstance<LibraryApi, LibraryContext>();
            var fileName = $"{Path.Combine(relativePath, baselineFolder)}{api.GetType().Name}-ApiSurface.txt";

            File.Exists(fileName).Should().BeTrue();
            var oldReport = File.ReadAllText(fileName);
            var newReport = api.GenerateVisibilityMatrix();
            oldReport.Should().BeEquivalentTo(newReport);
        }

        [BreakdanceManifestGenerator]
        public async Task LibraryApi_SaveMetadataDocument(string projectPath)
        {
            await RestierTestHelpers.WriteCurrentApiMetadata<LibraryApi, LibraryContext>(Path.Combine(projectPath, baselineFolder));
            File.Exists($"{Path.Combine(projectPath, baselineFolder)}{typeof(LibraryApi).Name}-ApiMetadata.txt").Should().BeTrue();
        }

        [BreakdanceManifestGenerator]
        public async Task LibraryApi_SaveVisibilityMatrix(string projectPath)
        {
            var api = await RestierTestHelpers.GetTestableApiInstance<LibraryApi, LibraryContext>();
            api.WriteCurrentVisibilityMatrix(Path.Combine(projectPath, baselineFolder));

            File.Exists($"{Path.Combine(projectPath, baselineFolder)}{api.GetType().Name}-ApiSurface.txt").Should().BeTrue();
        }

        #endregion

        #region StoreApi

        [TestMethod]
        public async Task StoreApi_CompareCurrentApiMetadataToPriorRun()
        {
            var fileName = $"{Path.Combine(relativePath, baselineFolder)}{typeof(StoreApi).Name}-ApiMetadata.txt";
            File.Exists(fileName).Should().BeTrue();

            var oldReport = File.ReadAllText(fileName);
            var newReport = await RestierTestHelpers.GetApiMetadataAsync<StoreApi, DbContext>(serviceCollection: (services) => { services.AddTestStoreApiServices(); });
            oldReport.Should().BeEquivalentTo(newReport.ToString());
        }

        [TestMethod]
        public async Task StoreApi_CompareCurrentVisibilityMatrixToPriorRun()
        {
            var api = await RestierTestHelpers.GetTestableApiInstance<StoreApi, DbContext>(serviceCollection: (services) => { services.AddTestStoreApiServices(); });
            var fileName = $"{Path.Combine(relativePath, baselineFolder)}{api.GetType().Name}-ApiSurface.txt";

            File.Exists(fileName).Should().BeTrue();
            var oldReport = File.ReadAllText(fileName);
            var newReport = api.GenerateVisibilityMatrix();
            oldReport.Should().BeEquivalentTo(newReport);
        }

        [BreakdanceManifestGenerator]
        public async Task StoreApi_SaveMetadataDocument(string projectPath)
        {
            await RestierTestHelpers.WriteCurrentApiMetadata<StoreApi, DbContext>(Path.Combine(projectPath, baselineFolder));
            File.Exists($"{Path.Combine(projectPath, baselineFolder)}{typeof(StoreApi).Name}-ApiMetadata.txt").Should().BeTrue();
        }

        [BreakdanceManifestGenerator]
        public async Task StoreApi_SaveVisibilityMatrix(string projectPath)
        {
            var api = await RestierTestHelpers.GetTestableApiInstance<StoreApi, DbContext>(serviceCollection: (services) => { services.AddTestStoreApiServices(); });
            api.WriteCurrentVisibilityMatrix(Path.Combine(projectPath, baselineFolder));

            File.Exists($"{Path.Combine(projectPath, baselineFolder)}{api.GetType().Name}-ApiSurface.txt").Should().BeTrue();
        }

        #endregion


    }

}