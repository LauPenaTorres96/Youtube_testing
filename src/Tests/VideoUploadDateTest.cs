using NUnit.Framework;
using YoutubeTests.PageObjects;

namespace YoutubeTests.Tests
{
    [TestFixture]
    public class VideoUploadDateTest : BaseTest
    {
        [Test]
        public void VerifyVideoUploadDateApproximationMatches()
        {
            // Arrange
            YouTubeHomePage homePage = new YouTubeHomePage(Driver);
            YouTubeSearchResultsPage searchResultsPage = new YouTubeSearchResultsPage(Driver);
            YouTubeVideoPage videoPage = new YouTubeVideoPage(Driver);

            // Act - Navigate to YouTube and search for a video
            homePage.NavigateTo(AppSettings.YoutubeSettings.BaseUrl);
            homePage.SearchForVideo(AppSettings.YoutubeSettings.SearchQuery);

            // Wait for search results to load
            Assert.IsTrue(searchResultsPage.GetVideoCount() > 0, "Search results should contain at least one video");

            // Capture upload date from search results
            string uploadDateFromSearch = searchResultsPage.GetVideoUploadDate(1);
            Assert.IsFalse(string.IsNullOrEmpty(uploadDateFromSearch), "Upload date should be visible in search results");

            // Click on the first video
            searchResultsPage.ClickOnVideo(1);

            // Wait for video page to load
            Assert.IsTrue(videoPage.IsVideoPageLoaded(), "Video page should load successfully");

            // Capture upload date from video page
            string uploadDateFromVideoPage = videoPage.GetUploadDate();
            Assert.IsFalse(string.IsNullOrEmpty(uploadDateFromVideoPage), "Upload date should be visible on video page");

            // Assert - Verify the upload date approximations match
            Assert.AreEqual(uploadDateFromSearch, uploadDateFromVideoPage,
                $"Upload date from search results '{uploadDateFromSearch}' should match the date on video page '{uploadDateFromVideoPage}'");
        }
    }
}
