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

<img width="40%" src="https://user-images.githubusercontent.com/23289252/178102569-16b64446-1a67-4eb6-80fa-a4c846c4b294.png">

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

It is recommended that optimization be performed a small number of times, and after completion, the results should be reviewed to determine if continued optimization should be performed.

#### :sailboat: Optimize Tab

<img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/178103175-02ade5f2-fa3e-4f34-9757-1b944d699785.png">

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

<img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/178103288-6e7bc0fe-f730-4e23-8159-6d43df55f1ae.png">

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
- Show selected type of plots
  - Show the plot selected in Visualize type above.

#### :fishing_pole_and_fish: Output Tab

<img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/178103410-e2a589b2-ffd0-436e-b4b1-2deb4e7346ba.png">

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

<img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/178103758-3ca8fa88-4796-4b0c-8c17-f8a810a47caf.png">

Tunny stores the optimize settings in json.  
Detailed settings in optimization can now also be configured in Json.

- **IMPORTANT**: The default hyperparameter for each optimization contain Optuna defaults.
  - These are not necessarily the best values for your optimization.
  - Pay particular attention to the initial population or trial.
    - the recommended initial trial for TPE is "the number of Variable" x 11 - 1.
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

#### :turtle: File Tab

<img width="30%" alt="image" src="https://user-images.githubusercontent.com/23289252/178103993-d4448d03-5aef-45bd-a8a0-497a0cf417a4.png">

- Open result file folder
  - Open the folder where the file containing the optimization results is located.
  - Results are saved as "Tunny_Opt_Result.db" by default.
  - This can be made to be anything you want by rewriting the Setting.json file.
- Clear result file
  - Deletes the optimization result file.

## :surfer: Contact information

[![Twitter](https://img.shields.io/twitter/follow/hiron_rgkr?style=social)](https://twitter.com/hiron_rgkr)

- HP : [https://hiron.dev/](https://hiron.dev/)
- Mail : support(at)hrntsm.com
  - change (at) to @
- Postings to [the discussion page](https://github.com/hrntsm/Tunny/discussions) are also welcome.
