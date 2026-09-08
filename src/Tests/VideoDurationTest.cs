using NUnit.Framework;
using YoutubeTests.PageObjects;

namespace YoutubeTests.Tests
{
    [TestFixture]
    public class VideoDurationTest : BaseTest
    {
        [Test]
        public void VerifyVideoDurationConsistency()
        {
            // Arrange
            YouTubeHomePage homePage = new YouTubeHomePage(Driver);
            YouTubeSearchResultsPage searchResultsPage = new YouTubeSearchResultsPage(Driver);
            YouTubeVideoPage videoPage = new YouTubeVideoPage(Driver);

            // Act - Navigate to YouTube and search for a video
            homePage.NavigateTo(AppSettings.YoutubeSettings.BaseUrl);
            homePage.SearchForVideo(AppSettings.YoutubeSettings.SearchQuery);

            // Wait for search results to load
            Assert.That(searchResultsPage.GetVideoCount() > 0, "Search results should contain at least one video");

            // Click on the first video
            searchResultsPage.ClickOnVideo(1);

            // Wait for video page to load
            Assert.That(videoPage.IsVideoPageLoaded(), "Video page should load successfully");

            // Capture duration from video page
            string durationFromVideoPage = videoPage.GetVideoDuration();
            Assert.That(!string.IsNullOrEmpty(durationFromVideoPage), "Video duration should be visible on video page");

            // Assert - Verify duration is displayed
            Assert.That(durationFromVideoPage, Is.Not.Empty, "Video duration should not be empty");
            Assert.That(durationFromVideoPage.Contains(":"), "Video duration should be in time format (HH:MM:SS or MM:SS)");
        }
    }
}
