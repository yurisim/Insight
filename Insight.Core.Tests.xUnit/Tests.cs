using Insight.Core.Services;
using System.Threading.Tasks;
using Xunit;

namespace Insight.Core.Tests.XUnit
{
    // TODO WTS: Add appropriate unit tests.
    public class Tests
    {
        [Fact]
        public void Test1()
        {
        }

        // Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Fact]
        public async Task EnsureSampleDataServiceReturnsContentGridDataAsync()
        {
            var actual = await SampleDataService.GetContentGridDataAsync();

            Assert.NotEmpty(actual);
        }

        // Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Fact]
        public async Task EnsureSampleDataServiceReturnsGridDataAsync()
        {
            var actual = await SampleDataService.GetGridDataAsync();

            Assert.NotEmpty(actual);
        }

        // Remove or update this once your app is using real data and not the SampleDataService.
        // This test serves only as a demonstration of testing functionality in the Core library.
        [Fact]
        public async Task EnsureSampleDataServiceReturnsListDetailDataAsync()
        {
            var actual = await SampleDataService.GetListDetailDataAsync();

            Assert.NotEmpty(actual);
        }
    }
}