# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/) and this project adheres to [Semantic Versioning](http://semver.org/).

Please see [here](https://github.com/hrntsm/Tunny/releases) for the data released for each version.

## [UNRELEASED] -20xx-xx-xx


### Added

- Display of estimated remaining time during optimization run.

### Changed


### Deprecated


### Removed


### Fixed


### Security



## [v0.6.0] -2022-12-23

### Added

- Enable to save each visualize figure.
  - Interactive operations on the figure are kept because it is saved in html format, not as an image.
- Real-time display of trial number, best value, and Hypervolume in the UI.
- Created UI to set where to save optimization result files.
- SQLite handling in C#
  - Optimize result handling more smooth some case.
  - Previously, it used to read and handle python, which was sometimes slow, but now it calls SQLite directly from CS.
- Only non-dominated trial plot in pareto front.
- Boolean to skip the behavior of checking if the python library is installed in settings.
  - For some reason, the installer may be launched every time even if it is installed, so it can be forcibly skipped in the settings.
- FishEgg component
  - It is now possible to specify an individual of any variable and have it evaluated in the optimization first and foremost, rather than just setting up a variable completely by the sampler when conducting the optimization.
- Warn start CMA-ES support
  - The warm start CMA-ES setting has been added to the CMA-ES settings so that the information from the previous trials can be used.

### Changed

- When there are more than 10 params, the value of "Omit_values" is used instead of "params" to improve the visibility of the ParetoFront plot.
- Default name of optimization result file changed from "Tunny_Opt_Result.db" to "Fish.db".
- Easy-to-understand UI for creating study.
  - Study Create, Continue, and Copy are now clearly separated in the UI.
- Update visualize & output UI
  - Previously, the target study and objective function, variables could not be specified, but now they can be specified.
- Update optuna-dashboard to 0.8.0

### Deprecated

- Disable unused setting tab ui items

### Removed

### Fixed

- Even if there was an error in the input to the Tunny component, a window could be launched and the button to perform optimization could be pressed, so we made sure that this would not happen.
  - Subject to the following.
    - No input for variable and objective.
    - The name of the objective is not unique.
    - Multiple items entered in Attr.

### Security

- Bump joblib from 1.1.0 to 1.2.0 in /Tunny/Lib
- Bump mako from 1.2.0 to 1.2.2 in /Tunny/Lib

## [0.5.0] -2022-09-03

### Added

- Hypervolume visualization
  - It is useful for determining convergence in multi-objective optimization.
- Clustering visualization
  - Clustering of results during multi-objective optimization makes it easier to evaluate solutions.
- BoTorch Sampler
  - This sampler use Gaussian Process and support multi-objective optimization.
- Quasi-MonteCarlo Sampler
  - [Detail](https://optuna.readthedocs.io/en/latest/reference/samplers/generated/optuna.samplers.QMCSampler.html) about this sampler.
- Support Constraint.
  - Only TPE, GP, NSGAII can use constraint.
- Sampler detail settings UI
  - Previously it was necessary to change the JSON file of the settings, but now it can be changed in the UI
- Enable Text Bake in the FishMarket component.
- Allows selection of NSGA-II crossover methods.
  - `Uniform`, `BLXAlpha`, `SPX`, `SBX`, `VSBX`, `UNDX`
- Ability to set Popsize on CMA-ES restart
- Run GC after trial when has geometry attribute or setting always run.
  - **This change probably make optimize slower before**
  - If you want to cut this setting, set the value of "GcAfterTrial" to 2 in Settings.json.
- Show LICENSE button in Tunny UI.

### Changed

- When genepool is an input, it now creates variable names using nicknames.
- The output of the Pareto solution was made to consider the constraints.
- Multivariate in TPE sampler default option is false to true
- Updated Optuna used to v3.0.0
  - Use `suggest_int` and `suggest_float` instead of `suggest_uniform` for more accurate variable generation in optimization
  - Random and Grid samplers now support multi-objective optimization
  - The format of the db file in which the results are saved has changed. Please note that it is not compatible with the previous one.

### Fixed

- The PythonInstaller window now has no text on the progress bar.
- When more than one Study exists, another Study Name is set and RunOpt no longer causes a Solver Error.
- The error does not occur when the Brep of Geometry of Attribute is null.

## [0.4.0] -2022-07-09

### Added

- Component CI build to get easily development build.
  - If you want to dogfooding, go right ahead!
- Param_FishAttribute component
- Construct Fish Attribute component
  - Component that creates attribute information to be attached to each trial of optimization
- Deconstruct Fish component
  - Component that separates optimization results into Variables, Objectives, and Attributes
- Deconstruct Fish Attribute component
  - Components that output each attribute
- Python package licenses to clearly state the license of each package.
- requirements.txt file to avoid conflict python packages versions.
- Implemented Timeout to stop optimization over time.
- Input components are now highlighted in color, as are other optimization components.

### Changed

- Component location on the Tunny tab.
- The output of the Tunny component is made into Fish, a type that summarizes the results.
- The Geometry input of the Tunny component has been changed to Attribute to allow more attribute information to be handled.
- When restoring the results from Tunny component as Fishes, those that the trial did not work and Objective could not get are not output.
- Update UI
  - The UI of the Restore tab was confusing, so the UI was modified to make it easier to understand which button to press and how the results are output.
- The progress bar on the Output tab has been made to show progress in a more understandable way.
- Error messages are displayed when the input to Tunny component is not appropriate, and inappropriate input wires are automatically removed.
- Error massage in python runtime is more clear.
  - Added error message when the number of objective does not match the existing Study.
  - Added error message when a Study with the same name exists but is used without Loading.

### Fixed

- Stopped sampling when there was no geometry input
- Once optimize output error, the component won't run again
  - I've tried to do a proper Dispose to fix this problem, but it still doesn't work sometimes.
- Optuna-DashBoard and storage relate functions do not work properly when a different Storage path is specified in Settings than the default.
- Pressing the stop button in output and the stop button in Optimize does not stop the operation.
- Components were in their normal color instead of blue.
- Once an error occurs in PythonRuntime, the optimization will not work after that.

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
