using NUnit.Framework;
using OpenQA.Selenium;
using YoutubeTests.PageObjects;

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
            videoPage.Click(By.XPath("//ytd-channel-name//a[@class='yt-simple-endpoint style-scope yt-formatted-string']"));

            // Wait for channel page to load
            Assert.That(channelPage.IsChannelPageLoaded(), "Channel page should load successfully");

            // Capture channel name from channel page
            string channelNameFromChannelPage = channelPage.GetChannelName();
            Assert.That(!string.IsNullOrEmpty(channelNameFromChannelPage), "Channel name should be visible on channel page");

            // Assert - Verify channel names match
            Assert.That(channelNameFromChannelPage, Is.EqualTo(channelNameFromVideoPage),
                $"Channel name from video page '{channelNameFromVideoPage}' should match channel page '{channelNameFromChannelPage}'");
        }
    }
}
