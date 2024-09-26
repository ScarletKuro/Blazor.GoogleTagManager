# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [4.0.0] - 2024-12-01
### Added
- `Url` property to `GoogleTagManagerOptions`.
- `url` parameter in `IGoogleTagManagerInterop.InitializeAsync`.

## [3.0.0] - 2024-08-26
### Removed
- `IGoogleTagManagerInterop` from `IGoogleTagManager`.

### Added
- `IGoogleTagManagerInterop`
- `ImportJsAutomatically` to `GoogleTagManagerOptions`.

## [2.0.0] - 2024-05-27
#### Removed
- Drop NET5 support.

## [1.1.1] - 2022-06-19
#### Added
- Nuget Source Link support.

## [1.1.0] - 2022-05-18
#### Added
- New IsInitialized property in IGoogleTagManager

## [1.0.2] - 2022-05-18
#### Added
- New DebugToConsole property in GoogleTagManagerOptions.

### Changed
- Code formatting.

## [1.0.1] - 2022-05-17
### Fixed
- Tracking would not be enabled by default.

## [1.0.0] - 2022-05-17
#### Added
- Trim support.

### Changed
- Remove Havit.Core.

[Unreleased]: https://github.com/ScarletKuro/Blazor.GoogleTagManager/compare/HEAD..4.0.0
[4.0.0]: https://github.com/ScarletKuro/Blazor.GoogleTagManager/compare/3.0.0..4.0.0
[3.0.0]: https://github.com/ScarletKuro/Blazor.GoogleTagManager/compare/2.0.0..3.0.0
[2.0.0]: https://github.com/ScarletKuro/Blazor.GoogleTagManager/compare/1.1.1..2.0.0
[1.1.1]: https://github.com/ScarletKuro/Blazor.GoogleTagManager/compare/1.1.0..1.1.1
[1.1.0]: https://github.com/ScarletKuro/Blazor.GoogleTagManager/compare/1.0.2..1.1.0
[1.0.2]: https://github.com/ScarletKuro/Blazor.GoogleTagManager/compare/1.0.1..1.0.2
[1.0.1]: https://github.com/ScarletKuro/Blazor.GoogleTagManager/compare/1.0.0..1.0.1
[1.0.0]: https://github.com/ScarletKuro/Blazor.GoogleTagManager/commits/1.0.0