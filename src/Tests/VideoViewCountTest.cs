using NUnit.Framework;
using YoutubeTests.PageObjects;
using System;

namespace YoutubeTests.Tests
{
    [TestFixture]
    public class VideoViewCountTest : BaseTest
    {
        [Test]
        public void VerifyVideoViewCountConsistency()
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

            // Capture view count from search results
            string viewCountFromSearch = searchResultsPage.GetVideoViewCount(1);
            Assert.That(!string.IsNullOrEmpty(viewCountFromSearch), "View count should be visible in search results");

            // Click on the first video
            searchResultsPage.ClickOnVideo(1);

            // Wait for video page to load
            Assert.That(videoPage.IsVideoPageLoaded(), "Video page should load successfully");

            // Capture view count from video page
            string viewCountFromVideoPage = videoPage.GetViewCount();
            Assert.That(!string.IsNullOrEmpty(viewCountFromVideoPage), "View count should be visible on video page");

            // 💡 Add explicit verification logs to your console stream
            Console.WriteLine($"[INFO] Comparing search metrics against watch page metrics...");
            Console.WriteLine($"       -> View Count from Search Results: '{viewCountFromSearch}'");
            Console.WriteLine($"       -> View Count from Video Page:     '{viewCountFromVideoPage}'");

            // Assert - Verify the view counts match
            Assert.That(viewCountFromVideoPage, Is.EqualTo(viewCountFromSearch),
                $"View count from search results '{viewCountFromSearch}' should match the count on video page '{viewCountFromVideoPage}'");

            // 💡 Log a successful verification event if the assertion passes
            Console.WriteLine("[SUCCESS] ✅ Video view counts match perfectly!");

        }
    }
}
