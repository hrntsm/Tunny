<p align="center">
  <img width="30%"src="https://user-images.githubusercontent.com/23289252/179126574-3cbbd815-02d7-4105-80d0-98040862fd84.png" />
</p>
<h1 align="center">
  Tunny : A Grasshopper optimization component using Optuna
</h1>
<p align="center">
  <a href="https://github.com/hrntsm/Tunny/blob/master/LICENSE"><img src="https://img.shields.io/github/license/hrntsm/Tunny" alt="License"></a>
  <a href="https://github.com/hrntsm/Tunny/releases"><img src="https://img.shields.io/github/v/release/hrntsm/Tunny" alt="Release"></a>
  <a href="https://github.com/hrntsm/Tunny/releases"><img src="https://img.shields.io/github/downloads/hrntsm/Tunny/total" alt="download"></a>
  <a href="https://www.codacy.com/gh/hrntsm/Tunny/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=hrntsm/Tunny&amp;utm_campaign=Badge_Grade"><img src="https://app.codacy.com/project/badge/Grade/c7947be6770545e88153125060b41284" alt="Codacy Badge"></a>
  <a href="https://codeclimate.com/github/hrntsm/Tunny/maintainability"><img src="https://api.codeclimate.com/v1/badges/63a5b0a923062d25ad23/maintainability" alt="Maintainability"></a>
  <img src="https://img.shields.io/codeclimate/tech-debt/hrntsm/Tunny" alt="Code Climate technical debt">
</p>
<p align="center">
  <a href="https://github.com/hrntsm/Tunny"><img src="https://img.shields.io/github/stars/hrntsm/Tunny?style=social" alt="GitHub Repo stars"></a>
  <a href="https://github.com/hrntsm/Tunny/network"><img alt="GitHub forks" src="https://img.shields.io/github/forks/hrntsm/Tunny?style=social"></a>
  <a href="https://open.vscode.dev/hrntsm/Tunny"><img src="https://img.shields.io/static/v1?logo=visualstudiocode&amp;label=&amp;message=Open in Visual Studio Code&amp;labelColor=2c2c32&amp;color=007acc&amp;logoColor=007acc" alt="Open in Visual Studio Code"></a></p>
</p>

:fish:**Tunny**:fish: is Grasshopper's optimization component using Optuna, an open source hyperparameter auto-optimization framework.

The following is taken from the official website

> Optuna™, an open-source automatic hyperparameter optimization framework, automates the trial-and-error process of optimizing the hyperparameters. It automatically finds optimal hyperparameter values based on an optimization target. Optuna is framework agnostic and can be used with most Python frameworks, including Chainer, Scikit-learn, Pytorch, etc.
>
> Optuna is used in PFN projects with good results. One example is the second place award in the [Google AI Open Images 2018 – Object Detection Track](https://www.preferred.jp/en/news/pr20180907/) competition.

Optuna official site

- https://optuna.org/

## :tropical_fish: Install

First, Tunny runs on Windows only.

1. Download Tunny from [food4rhino](https://www.food4rhino.com/app/tunny) or [release page](https://github.com/hrntsm/tunny/releases)
1. Right-click the file > Properties > make sure there is no "blocked" text
1. In Grasshopper, choose File > Special Folders > Components folder. Move Tunny folder you downloaded there.
1. Restart Rhino and Grasshopper
1. In Grasshopper, Place the Tunny component and double-click the icon to start downloading the necessary libraries.
1. Enjoy!

## :sushi: Support

This software is being updated with your support.
If you like this software, please donation.

[![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/G2G5C2MIU)

Or [pixivFANBOX](https://hiron.fanbox.cc/)

## :blowfish: License

Tunny is licensed under the [MIT](https://github.com/hrntsm/Tunny/blob/main/LICENSE) license.  
Copyright© 2022, hrntsm

Tunny use Python runtime & some python packages.
These depend on their own licenses.
Please see PYTHON_PACKAGE_LICENSE for more license information.

## :dolphin: Usage

### :speedboat: Quick usage

https://user-images.githubusercontent.com/23289252/178105107-5e9dd9f7-5680-40d4-97b0-840a4f1f329c.mp4

### :fish_cake: Select sampler flow chart

<img width="75%" alt="image" src="https://user-images.githubusercontent.com/23289252/188254450-1e718d97-f81e-49a1-949b-e158837bc44f.png">

### :anchor: Component location

Tunny can be found in the Tunny tab if it has been installed.

<img width="20%" alt="image" src="https://user-images.githubusercontent.com/23289252/178104435-25ea999a-5f98-443d-b645-f157c4252d0b.png">

### :whale: Inputs

#### :ocean: Variables

Tunny support Number slider & GenePool.
Optimization is performed when this value is changed by Tunny.

It is recommended that components be given nicknames, as this makes it easier to understand the resulting process. Here it is named x1, x2, x3.
The genepool values are nicknamed from the top as genepool1, genepool2, and so on.

<img width="40%" src="https://user-images.githubusercontent.com/23289252/178102419-903887d3-6a30-4485-adf8-369ac218a28b.png">

#### :whale2: Objectives

Optimization is performed to minimize the value input here. Multi-objective optimization is also supported.

Each objective value have to be separated to a number component.
It is recommended to set nickname like input variables.

<img width="40%" src="https://user-images.githubusercontent.com/23289252/178102527-4d8a90f1-c2d6-4611-8b20-ea655c9f752b.png">

#### :fried_shrimp: Attributes

This input is optional.

The ConstructFishAttribute component allows you to set an Attribute for each trial of optimization.
ZUI is supported, so you can increase the number of inputs to any number and set Attribute.

The nickname of the ConstructFishAttribute component input is stored paired with the value entered as the name of that Attribute.

The Geometry input has a special meaning; what is entered here will be displayed as a Geometry when the results are sorted in the FishMarket component described below.

Constraint inputs are also special inputs.**(new in v0.5.0)** The values entered here are the constraints in the optimization.
When this value is less than 0, the constraint is considered satisfied. Constraint conditions are supported by TPE, GP and NSGAII.

<img width="60%" alt="image" src="https://user-images.githubusercontent.com/23289252/188254609-3c8432ba-3f1c-45f4-bd2a-9e3f08271c2b.png">

### :octopus: Other components

<img width="934" alt="image" src="https://user-images.githubusercontent.com/23289252/178102825-18bd8884-1d3d-4a3a-a3da-a9a2fb8e9324.png">

- The **Fish** component, a Param component dedicated to Tunny, was created to facilitate data handling.
  - You can internalize and save the results to a .gh file.
- The **FishMarket** component allows the results obtained from optimization to be viewed side by side just like a fish market.
- The **Deconstruct Fish** and **Deconstruct Fish Attribute** components are used to retrieve each value.

  - The DeconstructFishAttribute component, like the ExplodeTree component, creates Outputs for the number of Attributes by selecting Match outputs from the context menu.

    <img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/178102966-f3960954-254d-465c-8a19-03fceecda83e.png">

### :shell: Optimization Window

Double-click on the component icon to open the form for performing optimization.

The component differs from other optimization components in that it does not graph the learning status during optimization in GrasshopperUI.  
On the other hand, it is possible to save the learning status, and even after the optimization has been completed once, the study content can be used to perform ongoing optimization.

On the other hand, Running the Dashboard function, which charts the results, starts a server that handles the results and allows you to see the optimization results in real time in your browser.  
This feature is useful for analyzing post-optimization results, as it allows the user to not only see the results in real time, but also to view several figures at once.

#### :sailboat: Optimize Tab

<img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/188254704-533139c7-169a-4aee-bcd5-0e98ff45f35e.png">

Values that can be set and their meanings are as follows.

- Sampler
  - Sets the algorithm to perform the optimization. The following types are available.
  - All are optimization algorithms provided by Optuna.
    1. Bayesian optimization(TPE)
    2. Bayesian optimization(GP) **(new in v0.5.0)**
    3. Genetic algorithm(NSGA-II)
    4. Evolution strategy(CMA-ES)
    5. Quasi-MonteCarlo **(new in v0.5.0)**
    6. Random
    7. Grid
- Number of trial
  - This number of trials will be performed.
    - Bayesian optimization first performs random sampling to create a surrogate model. This is the setting for how many trials random sampling is performed.
    - The original paper recommends “number of variables” \* 11-1.
  - If the grid sampler is selected, the calculation is performed by dividing each entered Variable by this number.
    - **Note** that the number of calculations is (Number of trial) to the power of (Number of Variable).
- Timeout(sec)
  - After the time set here elapses, optimization stops.
  - If 0 is input, no stop by time is performed.
- Load if study file exists
  - If the checkbox is checked and a file of optimization results is available, the results of the training will be used to perform ongoing optimization.
- Study Name
  - Name of the training result to be saved in the optimization result file
- RunOptimize
  - Push the button to perform the optimization.
- Stop
  - Forces optimization to stop.
  - Even when stopped, the system automatically saves the results up to the most recent evaluation.

#### :boat: Visualize Tab

<img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/188255757-8532948f-b784-45b7-b116-17ba79db9536.png">

Values that can be set and their meanings are as follows.

- Open Optuna-Dashboard
  - Run Real-time Web Dashboard for handling optimization results.
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
    9. hypervolume **(new in v0.5.0)**
  - Show selected type of plots
    - Show the plot selected in Visualize type above.
- k-means clustering
  - Cluster the results of the Pareto front when performing multi-objective optimization.Any number of clusters can be specified.

#### :fishing_pole_and_fish: Output Tab

<img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/188255933-62df2324-29b0-40e2-9568-6b825ed147dd.png">

Values that can be set and their meanings are as follows.

- First, the model number matches the tree structure of the output.
  - If the model number is 42, the output is {0; 42}.
- Parato solutions
  - Output Parato solutions.
- All trials
  - Output all trials.
- Use model number
  - Multiple values can be entered separated by commas, such as "1,3,42".
- Output
  - The model with the number entered here is restored from the optimization results file and is the output of the component.
  - The model number can be found on each plot.
  - In the example below, model number 424 with a value of -11.49 for the first objective function is selected.

    <img width="50%" alt="image" src="https://user-images.githubusercontent.com/23289252/178103519-70286138-3dc4-4d65-ae66-7c918e5b4805.png">

- Reflect the result on the sliders
  - The result of the input model number is reflected in Slider and Genepool.

#### :droplet: Settings Tab

<img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/188255969-9355c626-36d3-4d9b-8f50-0a8bda79bf8e.png">

**(new in v0.5.0)**
Allows detailed optimization settings to be performed in the UI.
See below for the meaning of each setting.

[Guideline for selecting optimization algorithms](https://graceful-stag-dae.notion.site/Guidelines-for-Selecting-a-Optimization-Algorithm-8505b2e020ee4af2a77272f25acc6094)

#### :turtle: File Tab

<img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/188256059-82679c52-4909-4e29-a9ad-36b3bab5c577.png">

- Result
  - Open result file folder
    - Open the folder where the file containing the optimization results is located.
    - Results are saved as "Tunny_Opt_Result.db" by default.
    - This can be made to be anything you want by rewriting the Setting.json file.
  - Clear result file
    - Deletes the optimization result file.
- License **(new in v0.5.0)**
  - You can check license to push each button.


## :surfer: Contact information

[![Twitter](https://img.shields.io/twitter/follow/hiron_rgkr?style=social)](https://twitter.com/hiron_rgkr)

- HP : [https://hiron.dev/](https://hiron.dev/)
- Mail : support(at)hrntsm.com
  - change (at) to @
- Postings to [the discussion page](https://github.com/hrntsm/Tunny/discussions) are also welcome.
