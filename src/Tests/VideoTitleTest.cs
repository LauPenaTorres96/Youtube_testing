using NUnit.Framework;
using YoutubeTests.PageObjects;

namespace YoutubeTests.Tests
{
    [TestFixture]
    public class VideoTitleTest : BaseTest
    {
        [Test]
        public void VerifyVideoTitleConsistency()
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

            // Capture title from search results
            string titleFromSearch = searchResultsPage.GetVideoTitle(1);
            Assert.That(!string.IsNullOrEmpty(titleFromSearch), "Video title should be visible in search results");

            // Click on the first video
            searchResultsPage.ClickOnVideo(1);

            // Wait for video page to load
            Assert.That(videoPage.IsVideoPageLoaded(), "Video page should load successfully");

            // Capture title from video page
            string titleFromVideoPage = videoPage.GetVideoTitle();
            Assert.That(!string.IsNullOrEmpty(titleFromVideoPage), "Video title should be visible on video page");

            // Assert - Verify the titles match
            Assert.That(titleFromVideoPage, Is.EqualTo(titleFromSearch),
                $"Title from search results '{titleFromSearch}' should match the title on video page '{titleFromVideoPage}'");
        }
    }
}
