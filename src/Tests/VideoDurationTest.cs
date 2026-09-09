using NUnit.Framework;
using YoutubeTests.PageObjects;
using System;

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

            string durationFromSearchPage = searchResultsPage.GetVideoDurationByIndex(1);
            Assert.That(!string.IsNullOrEmpty(durationFromSearchPage), "Video duration badge should be visible in search results");

            // Click on the first video
            searchResultsPage.ClickOnVideo(1);

            // Wait for video page to load
            Assert.That(videoPage.IsVideoPageLoaded(), "Video page should load successfully");

            // Capture duration from video page
            string durationFromVideoPage = videoPage.GetVideoDuration();
            Assert.That(!string.IsNullOrEmpty(durationFromVideoPage), "Video duration should be visible on video page");

            Console.WriteLine($"[INFO] Comparing search list runtime against live player asset timeline...");
            Console.WriteLine($"       -> Duration from Search Badge: '{durationFromSearchPage}'");
            Console.WriteLine($"       -> Duration from Player API:    '{durationFromVideoPage}'");

            // Assert - Verify the structural time layout contains a colon separator
            Assert.That(durationFromVideoPage.Contains(":"), "Video duration should be in time format (HH:MM:SS or MM:SS)");

            // 💡 NEW ASSERTION: Verify the parsed runtimes are completely identical
            Assert.That(durationFromVideoPage, Is.EqualTo(durationFromSearchPage),
                $"Duration from search results '{durationFromSearchPage}' should match the watch player duration '{durationFromVideoPage}'");

            Console.WriteLine("[SUCCESS] ✅ Video durations match perfectly across search and playback views!");
        }
    }
}
