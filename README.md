![TunnyIcon](https://user-images.githubusercontent.com/23289252/162955418-1dbe2830-f0ed-4664-993b-b6f23aaa702a.png)

# Tunny : A Grasshopper optimization component using Optuna

[![License](https://img.shields.io/github/license/hrntsm/Tunny)](https://github.com/hrntsm/Tunny/blob/master/LICENSE)
[![Release](https://img.shields.io/github/v/release/hrntsm/Tunny)](https://github.com/hrntsm/Tunny/releases)
[![download](https://img.shields.io/github/downloads/hrntsm/Tunny/total)](https://github.com/hrntsm/Tunny/releases)

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/c7947be6770545e88153125060b41284)](https://www.codacy.com/gh/hrntsm/Tunny/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=hrntsm/Tunny&amp;utm_campaign=Badge_Grade)
[![Maintainability](https://api.codeclimate.com/v1/badges/63a5b0a923062d25ad23/maintainability)](https://codeclimate.com/github/hrntsm/Tunny/maintainability)
![Code Climate technical debt](https://img.shields.io/codeclimate/tech-debt/hrntsm/Tunny)

[![GitHub Repo stars](https://img.shields.io/github/stars/hrntsm/Tunny?style=social)](https://github.com/hrntsm/Tunny)

**Tunny** is Grasshopper's optimization component using Optuna, an open source hyperparameter auto-optimization framework.

The following is taken from the official website

> Optuna™, an open-source automatic hyperparameter optimization framework, automates the trial-and-error process of optimizing the hyperparameters. It automatically finds optimal hyperparameter values based on an optimization target. Optuna is framework agnostic and can be used with most Python frameworks, including Chainer, Scikit-learn, Pytorch, etc.
>
> Optuna is used in PFN projects with good results. One example is the second place award in the [Google AI Open Images 2018 – Object Detection Track](https://www.preferred.jp/en/news/pr20180907/) competition.

Optuna official site

- https://optuna.org/

## Install

First, Tunny runs on Windows only.

1. Download Tunny from [food4rhino](https://www.food4rhino.com/app/tunny) or [release page](https://github.com/hrntsm/tunny/releases)
1. Right-click the file > Properties > make sure there is no "blocked" text
1. In Grasshopper, choose File > Special Folders > Components folder. Move Tunny folder you downloaded there.
1. Restart Rhino and Grasshopper
1. Enjoy!

Tunny also support yak. So you can find Tunny in Rhinoceros package manager.

## Usage

### Quick usage

![tunny](https://user-images.githubusercontent.com/23289252/166186417-7541ccb9-efa0-4569-a068-373ebde1c0ed.gif)

### Component location

Tunny can be found in the Params tab if it has been installed.

![image](https://user-images.githubusercontent.com/23289252/172011237-583835a5-3ecc-4ca9-a77d-29c1f5a1687a.png)

### Inputs

#### Variables

This component support Number slider & GenePool.
Optimization is performed when this value is changed by Tunny.

It is recommended that components be given nicknames, as this makes it easier to understand the resulting process. Here it is named x1, x2, x3.

![image](https://user-images.githubusercontent.com/23289252/172011316-b8e812bd-0362-443f-8841-a2157032b7d1.png)

#### Objectives

Optimization is performed to minimize the value input here. Multi-objective optimization is also supported.  

Each objective value have to be separated to a number component.
It is recommended to set nickname like input variables.

![image](https://user-images.githubusercontent.com/23289252/172011407-e1c8a0ea-f5bc-466a-9670-d005f35af89c.png)

#### Geometries 

This input is optional.

GeometryBase input is supported as a function to save the model during optimization.
Input of large date size geometries are deprecated because it makes the analysis heavier.

![image](https://user-images.githubusercontent.com/23289252/172012910-d49e526b-db3f-43cd-af45-1f24a9f0e221.png)

### Other components

![image](https://user-images.githubusercontent.com/23289252/172013084-a74e08d7-9b8d-47a5-8eef-f6840a369ee1.png)

- The Fish component, a Param component dedicated to Tunny, was created to facilitate data handling.
- You can internalize and save the results to an gh file.
- The FishMarket comport allows the results obtained from optimization to be viewed side by side just like a fish market.

### Optimization Window

Double-click on the component icon to open the form for performing optimization.

The component differs from other optimization components in that it does not graph the learning status during optimization.  
On the other hand, it is possible to save the learning status, and even after the optimization has been completed once, the study content can be used to perform ongoing optimization.

It is recommended that optimization be performed a small number of times, and after completion, the results should be reviewed to determine if continued optimization should be performed.

#### Optimize Tab

![image](https://user-images.githubusercontent.com/23289252/172011516-a0d170f9-f118-4f62-a1aa-b76f4cbb9218.png)

Values that can be set and their meanings are as follows.

- Sampler
  - Sets the algorithm to perform the optimization. The following types are available.
  - All are optimization algorithms provided by Optuna.
    1. TPE (Bayesian optimization)
    1. NSGA-II (Genetic algorithm)
    1. CMA-ES (Evolution strategy)
    1. Random
    1. Grid
- Number of trial
  - This number of trials will be performed.
  - If the grid sampler is selected, the calculation is performed by dividing each entered Variable by this number.
    - **Note** that the number of calculations is (Number of trial) to the power of (Number of Variable).
- Load if study file exists
  - If the checkbox is checked and a file of optimization results is available, the results of the training will be used to perform ongoing optimization.
- Study Name
  - Name of the training result to be saved in the optimization result file
- RunOptimize
  - Push the button to perform the optimization.

#### Settings Tab

![image](https://user-images.githubusercontent.com/23289252/172011625-3b5476f8-c143-40a1-809b-e6e066379d1e.png)

Tunny stores the settings in this window in json.
Detailed settings in optimization can now also be configured in Json.

- Open API page
  - Open Optuna's API page with the meaning of each value in the advanced optimization settings.
- Save settings to json
  - Save the settings to Json.
  - Settings are also saved when the window is closed with the X button.
- Load settings from json
  - Loads a settings file.
- Open Settings.Json folder
  - Open the folder where the settings files are stored.
  - The file Settings.json is Tunny's settings file. Edit it with any text editor.

#### Result Tab

![image](https://user-images.githubusercontent.com/23289252/172011663-e60c9e10-f8fc-48bd-b909-2c15b0b23ca9.png)

Values that can be set and their meanings are as follows.

- Dashboard
  -  Run Real-time Web Dashboard for Optuna.
  - https://github.com/optuna/optuna-dashboard#features
  - You can now more easily check results or even see results in real time.
- Visualize type
  - The following types of graphing are supported. See the [Optuna.visualization](https://optuna.readthedocs.io/en/stable/reference/visualization/index.html) page below for more information.
    1. contour
    2. EDF
    3. intermediate values
    4. optimization history
    5. parallel coordinate
    6. param importance
    7. pareto front
    8. slice
- Open result file folder
  - Open the folder where the file containing the optimization results is located. The results are stored under the name "Tunny_Opt_Result.db".
- Clear result file
  - Deletes the optimization result file.
  - If the value of the input changes, delete it if necessary, since optimization using a file containing the same name study will cause content conflicts and optimization will not be performed.
- Set restore model number
  - The model with the number entered here is restored from the optimization results file and is the output of the component.
  - The model number matches the tree structure of the output.
  - -1 is input, the results of all models on the Pareto front will output.
  - -10 is input, the results of all models will output
  - Clicking the Reflect button will cause Grasshopper to reflect the results of the model number inputted.

## Contact information

[![Twitter](https://img.shields.io/twitter/follow/hiron_rgkr?style=social)](https://twitter.com/hiron_rgkr)

- HP : [https://hiron.dev/](https://hiron.dev/)
- Mail : support(at)hrntsm.com
  - change (at) to @

## Donation

This software is being updated with your support.
If you like this software, please donation.

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/G2G5C2MIU)

Or [pixivFANBOX](https://hiron.fanbox.cc/)

## License

Tunny is licensed under the [MIT](https://github.com/hrntsm/Tunny/blob/main/LICENSE) license.  
Copyright© 2022, hrntsm

Release package is embedded Python runtime & optuna libraries.
These depend on their own licenses.
Please see PYTHON_PACKAGE_LICENSE for more license informations.
