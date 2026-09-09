using NUnit.Framework;
using OpenQA.Selenium;
using YoutubeTests.PageObjects;
using System;

namespace YoutubeTests.Tests
{
    [TestFixture]
    public class VideoChannelConsistencyTest : BaseTest
    {
        [Test]
        public void VerifyChannelNameConsistencyAcrossPages()
        {
            // Arrange
            YouTubeHomePage homePage = new YouTubeHomePage(Driver);
            YouTubeSearchResultsPage searchResultsPage = new YouTubeSearchResultsPage(Driver);
            YouTubeVideoPage videoPage = new YouTubeVideoPage(Driver);
            YouTubeChannelPage channelPage = new YouTubeChannelPage(Driver);

            // Act - Navigate to YouTube and search for a video
            homePage.NavigateTo(AppSettings.YoutubeSettings.BaseUrl);
            homePage.SearchForVideo(AppSettings.YoutubeSettings.SearchQuery);

            // Wait for search results to load
            Assert.That(searchResultsPage.GetVideoCount() > 0, "Search results should contain at least one video");

            // Click on the first video
            searchResultsPage.ClickOnVideo(1);

            // Wait for video page to load
            Assert.That(videoPage.IsVideoPageLoaded(), "Video page should load successfully");

            // Capture channel name from video page
            string channelNameFromVideoPage = videoPage.GetChannelName();
            Assert.That(!string.IsNullOrEmpty(channelNameFromVideoPage), "Channel name should be visible on video page");

            // Click on channel name to navigate to channel page
            videoPage.ClickChannelLink();

            // Wait for channel page to load
            System.Threading.Thread.Sleep(2000);
            Assert.That(channelPage.IsChannelPageLoaded(), "Channel page should load successfully");

            // Capture channel name from channel page
            string channelNameFromChannelPage = channelPage.GetChannelName();
            Assert.That(!string.IsNullOrEmpty(channelNameFromChannelPage), "Channel name should be visible on channel page");

            // 💡 Add explicit verification logs to your console stream
            Console.WriteLine($"[INFO] Comparing watch page creator name against profile header title...");
            Console.WriteLine($"       -> Channel Name from Video Page:   '{channelNameFromVideoPage}'");
            Console.WriteLine($"       -> Channel Name from Channel Page: '{channelNameFromChannelPage}'");

            // Assert - Verify channel names match
            Assert.That(channelNameFromChannelPage, Is.EqualTo(channelNameFromVideoPage),
                $"Channel name from video page '{channelNameFromVideoPage}' should match channel page '{channelNameFromChannelPage}'");

            // 💡 Log a successful verification event if the assertion passes
            Console.WriteLine("[SUCCESS] ✅ Channel names match perfectly across pages!");
        }
    }
}
