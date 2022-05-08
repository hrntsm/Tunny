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

Tunny can be found in the same Params tab as Galapagos under Util if it has been installed.

![image](https://user-images.githubusercontent.com/23289252/163377645-6c397380-8896-4e33-8b74-8305e9a2ef04.png)

### Inputs

#### Variables

This component support Number slider & GenePool.
Optimization is performed when this value is changed by Tunny.

It is recommended that components be given nicknames, as this makes it easier to understand the resulting process. Here it is named x, y, z.

![image](https://user-images.githubusercontent.com/23289252/166185821-4b3da178-068b-444a-9d3f-9ee791c533b1.png)

#### Objectives

Optimization is performed to minimize the value input here. Multi-objective optimization is also supported.  

Each objective value have to be separated to a number component.
It is recommended to set nickname like input variables.

![image](https://user-images.githubusercontent.com/23289252/166185782-3d5ddb69-5912-4b65-8b59-c20f0f1cd6b2.png)

#### ModelMesh

This input is optional.

Mesh input is supported as a function to save the model during optimization.
If multiple meshes are entered as a list, only the first one will be saved.  
Input of large size meshes is deprecated because it makes the analysis heavier.

![image](https://user-images.githubusercontent.com/23289252/166185101-c82d1610-03a5-4906-920c-4ef33508716c.png)

### Optimization Window

Double-click on the component icon to open the form for performing optimization.

The component differs from other optimization components in that it does not graph the learning status during optimization.  
On the other hand, it is possible to save the learning status, and even after the optimization has been completed once, the study content can be used to perform ongoing optimization.

It is recommended that optimization be performed a small number of times, and after completion, the results should be reviewed to determine if continued optimization should be performed.

#### Optimize Tab

![image](https://user-images.githubusercontent.com/23289252/163382306-b44f5e7c-4c62-4887-8766-f399c23c33b4.png)

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

#### Result Tab

![image](https://user-images.githubusercontent.com/23289252/166185559-a5e64659-df48-4777-85dc-7ed01f3752b7.png)

Values that can be set and their meanings are as follows.

- Visualize type
  - The following types of graphing are supported. See the [Optuna.visualization](https://optuna.readthedocs.io/en/stable/reference/visualization/index.html) page below for more information.
    1. contour
    1. EDF
    1. intermediate values
    1. optimization history
    1. parallel coordinate
    1. param importance
    1. pareto front
    1. slice
- Open result file folder
  - Open the folder where the file containing the optimization results is located. The results are stored under the name "Tunny_Opt_Result.db".
- Clear result file
  - Deletes the optimization result file.
  - If the value of the input changes, delete it if necessary, since optimization using a file containing the same name study will cause content conflicts and optimization will not be performed.
- Set restore model number
  - The model with the number entered here is restored from the optimization results file and is the output of the component.
  - The model number matches the tree structure of the output.
  - -1 is input, the results of all models on the Pareto front will be the main focus.
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
