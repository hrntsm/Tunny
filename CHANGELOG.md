# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/) and this project adheres to [Semantic Versioning](http://semver.org/).

Please see [here](https://github.com/hrntsm/Tunny/releases) for the data released for each version.

## [UNRELEASED]

### Added

- Restore progressbar
- Support Galapagos genepool input
- Reflect button
  - This feature reflecting multi objective optimization result to slider & genepool to use input model number.
  - if input multi model number, the first one is reflect and popup notification about this.

### Changed

- Restore feature was made asynchronous.
- Visualize graph axis name now use input objective's nickname.
- Update supported Rhino version to 7.13.
- Disable optimize window resize.

### Fixed

- Optimization does not stop when the value of Objective is null.
  - When the objective values is null, optimizer try to get another variable and resolve solution.
  - If it is 10 trial to get objectives in 1 optimize loop, optimizer throw error.
- Enable visualize param importances function
  - this function need sklearn, but tunny's python package doesn't include it.
- Optimize window UI is broken when using Hi-DPI environment.
  - Support multi DPI

## [0.1.1] -2022-04-17

### Fixed

- If there is no modelmesh input and use restore function, gh is crashed bug.

## [0.1.0] -2022-04-14

### Added

- Base Tunny component files.
- This CHANGELOG file to hopefully serve as an evolving example of a standardized open source project CHANGELOG.

---

## Template

## [X.Y.Z] -20xx-xx-xx

### Added

for new features.

### Changed

for changes in existing functionality.

### Deprecated

for soon-to-be removed features.

### Removed

for now removed features.

### Fixed

for any bug fixes.

### Security

in case of vulnerabilities.
