using NUnit.Framework;
using YoutubeTests.PageObjects;
using System;

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
            Assert.That(searchResultsPage.GetVideoCount() > 0, "Search results should contain at least one video");

            // Capture upload date from search results
            string uploadDateFromSearch = searchResultsPage.GetVideoUploadDate(1);
            Assert.That(!string.IsNullOrEmpty(uploadDateFromSearch), "Upload date should be visible in search results");

            // Click on the first video
            searchResultsPage.ClickOnVideo(1);

            // Wait for video page to load
            Assert.That(videoPage.IsVideoPageLoaded(), "Video page should load successfully");

            // Capture upload date from video page
            string uploadDateFromVideoPage = videoPage.GetUploadDate();
            Assert.That(!string.IsNullOrEmpty(uploadDateFromVideoPage), "Upload date should be visible on video page");

            // Extract the numbers (e.g., "7" from "7mo ago" and "7 months ago")
            string searchNumber = System.Text.RegularExpressions.Regex.Match(uploadDateFromSearch, @"\d+").Value;
            string videoNumber = System.Text.RegularExpressions.Regex.Match(uploadDateFromVideoPage, @"\d+").Value;

            // 💡 Add explicit verification logs to your console stream
            Console.WriteLine($"[INFO] Comparing search list timeline relative approximation...");
            Console.WriteLine($"       -> Raw String from Search:  '{uploadDateFromSearch}' (Extracted digit: {searchNumber})");
            Console.WriteLine($"       -> Raw String from Video:   '{uploadDateFromVideoPage}' (Extracted digit: {videoNumber})");

            // Assert - Verify the upload date approximations match
            Assert.That(videoNumber, Is.EqualTo(searchNumber),
                $"The relative time frame number ({videoNumber}) does not align with search data.");

            // 💡 Log a successful verification event if the assertion passes
            Console.WriteLine("[SUCCESS] ✅ Upload date approximation numbers match perfectly!");

        }
    }
}
