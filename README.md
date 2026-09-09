# YouTube Automated Testing Suite

A comprehensive C# Selenium WebDriver testing suite for YouTube UI automation using the Page Object Model (POM) pattern.

## Project Overview

This project demonstrates 5 automated test cases that verify data consistency across multiple YouTube pages. Each test scenario uses modular JSON configuration and explicit console test stream tracking wrappers for long-term maintainability.

## Test Scenarios

1. **Video Upload Date Approximation** - Verify relative timeline approximation metrics (`7mo ago` vs `7 months ago`) align between search results and the watch page player.
2. **Video View Count Consistency** - Verify view counts are consistent across search displays and the main watch metadata fields.
3. **Video Title Consistency** - Verify video titles match perfectly between the search results list and the video page frame layout.
4. **Channel Name Consistency** - Verify creator channel names match across the video playback view and the modern channel profile page.
5. **Video Duration Display** - Verify video duration is correctly displayed on the video page and matches the thumbnail run time status badge.

## Project Structure

```
Youtube_testing/
├── src/
│   ├── Configuration/
│   │   ├── AppSettings.cs          # Settings model classes
│   │   └── ConfigurationLoader.cs  # JSON configuration loader
│   ├── Drivers/
│   │   └── WebDriverFactory.cs     # WebDriver initialization
│   ├── PageObjects/
│   │   ├── BasePage.cs             # Base class with common methods
│   │   ├── YouTubeHomePage.cs      # Home page interactions
│   │   ├── YouTubeSearchResultsPage.cs # Search results interactions
│   │   ├── YouTubeVideoPage.cs     # Video page interactions
│   │   └── YouTubeChannelPage.cs   # Channel page interactions
│   └── Tests/
│       ├── BaseTest.cs             # Base test class with setup/teardown
│       ├── VideoUploadDateTest.cs  # Upload date verification tests
│       ├── VideoViewCountTest.cs   # View count verification tests
│       ├── VideoTitleTest.cs       # Title verification tests
│       ├── VideoChannelConsistencyTest.cs # Channel name tests
│       └── VideoDurationTest.cs    # Duration verification tests
├── config/
│   └── testdata.json               # Test scenarios and data
├── appsettings.json                # Browser and YouTube settings
└── YoutubeTests.csproj             # Project file
```

## Configuration Files

### appsettings.json
Contains browser settings and YouTube base URL configuration.

### config/testdata.json
Contains all 5 test scenarios with descriptions and expected behaviors.

## Technologies Used

- **C# .NET 8.0** - Core programming language framework and compiler target
- **Selenium WebDriver 4.15+** - Core browser automation driver bindings
- **NUnit 4.5.0** - Central test assertion configuration engine
- **NUnit3TestAdapter 4.5.0** - Visual Studio IDE Test Explorer integration listener
- **WebDriverManager 2.17.0** - Automated ChromeDriver resolution management
- **Newtonsoft.Json 13.0** - Configuration file deserializer library

## Notes

- **Resilient Modern Selectors:** To protect scripts against YouTube's active A/B layout deployments and custom Web Components (`yt-dynamic-text-view-model`), long index XPaths have been removed entirely. Elements are tracked via lightweight, flat class name collections (`ytd-video-renderer`), filtering out list locations via clean C# zero-indexed array tracking wrappers.
- **Shadow DOM and Hidden Layout Extraction:** For elements subject to intermittent visibility states (like the fading video player tracking bar or asynchronous layout frames), selectors are targeted directly at inner content spans using fallback methods like `.GetAttribute("textContent")` to prevent framework sync timeouts.
- **Pre-Roll Ad Isolation:** The duration verification workflow avoids layout interference entirely by executing native JavaScript runtime hooks directly against YouTube's player layer (`movie_player.getDuration()`), allowing accurate tracking even if ad clips are processing over the active browser layout canvas.
- **Console Traceability:** The test scripts incorporate informational logging banners (`[INFO]`, `[SUCCESS]`, `[WARN]`) written to the standard terminal stream, allowing clear trace visibility during debugging runs (`--verbosity normal`).


## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Git

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/LauPenaTorres96/Youtube_testing.git
   cd Youtube_testing
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

### Running Tests

Run all tests:
```bash
dotnet test
```

Run specific test:
```bash
dotnet test --filter "VideoUploadDateTest"
```

Run with verbose output:
```bash
dotnet test --verbosity normal
```

## Key Design Patterns

### Page Object Model (POM)
- Each YouTube page (Home, Search Results, Video, Channel) has its own class
- All page interactions are encapsulated within page classes
- Tests use page objects to interact with the UI
- Reduces code duplication and improves maintainability

### JSON Configuration
- Test data is externalized to JSON files
- Allows non-technical stakeholders to update test scenarios
- Supports environment-specific configurations
- Easy to extend with new test scenarios

### Base Classes
- `BasePage` - Common methods for element interaction (click, sendKeys, getText, etc.)
- `BaseTest` - Common setup/teardown logic for all tests

## Notes

- YouTube's UI is dynamically rendered; XPath locators may need adjustment based on current HTML structure
- Tests are configured to run in non-headless mode for debugging; change in `appsettings.json` for CI/CD
- Implicit and page load timeouts are configurable in `appsettings.json`
