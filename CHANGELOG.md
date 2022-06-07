# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/) and this project adheres to [Semantic Versioning](http://semver.org/).

Please see [here](https://github.com/hrntsm/Tunny/releases) for the data released for each version.

## [UNRELEASED]

### Added

- Component CI build to get easily development build.

### Fix

- Stopped sampling when there was no geometry input

## [0.3.0] -2022-05-03

### Added

- Use json to set sampler detail settings
  - Since the settings are saved in Json, the previous values set in the UI remain saved when the window is closed and reopened.
- UI for above detail settings 
- If input "-10" in restore model number, Tunny output all result
- Fish component
  - Special GH_Param component for Tunny result to handle its result easier.
  - This is the result of your catching Tunny in a fishery called Optimization.
- FishMarket component
  - Feature to display results side by side in a viewport like a fish market.
- Version info to result rdb file
  - To avoid problems with different versions in the future
- Support Optuna-Dashboard
  - Real-time Web Dashboard for Optuna.
  - https://github.com/optuna/optuna-dashboard#features

### Change

- Combine Tunny component output into one called Fish.
- Component subcategory changed from Util to Tunny
- Model shape data recorded during optimization from a single mesh to multiple GeometryBases.
- Reduced size of distribution package
  - It used to include the dependent PythonPackage from the beginning, but it is now downloaded when it is started on Grasshopper.

## [0.2.0] -2022-05-02

### Added

- Restore progressbar
- Support Galapagos genepool input
- Result reflect button
  - This feature reflecting multi objective optimization result to slider & genepool to use input model number.
  - if input multi model number, the first one is reflect and popup notification about this.
- Support grid sampler

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
