# YouTube Automated Testing Suite

A comprehensive C# Selenium WebDriver testing suite for YouTube UI automation using the Page Object Model (POM) pattern.

## Project Overview

This project demonstrates 5 automated test cases that verify data consistency across multiple YouTube pages. Each test scenario uses modular JSON configuration for maintainability.

## Test Scenarios

1. **Video Upload Date Approximation** - Verify upload date shown in search results matches video page
2. **Video View Count Consistency** - Verify view counts are consistent across search results and video page
3. **Video Title Consistency** - Verify video titles match between search results and video page
4. **Channel Name Consistency** - Verify channel names match across video page and channel page
5. **Video Duration Display** - Verify video duration is correctly displayed on video page

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

- **C# .NET 8.0** - Programming language and runtime
- **Selenium WebDriver 4.15** - Browser automation
- **NUnit 4.0** - Testing framework
- **WebDriverManager 2.16** - Automatic driver management
- **Newtonsoft.Json 13.0** - JSON configuration parsing

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
