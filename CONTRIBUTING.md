# How to contribute

👍🎉 First off, thanks for taking the time to contribute! 🎉👍

The following is a set of guidelines for contributing to this component.
These are mostly guidelines, not rules.
Use your best judgment, and feel free to propose changes to this document in a pull request.

## Note!

Writing code and suggesting fixes is not the only way to contribute!  
Your comments, problems, and opinions posted in [Discussion](https://github.com/hrntsm/Tunny/discussions) or [Issues](https://github.com/hrntsm/Tunny/issues) are one aspect of contribution.

Feel free to post it.

## Branch structure

Tunny's branch structure is as follows

- main
  - The latest version of the code that has been released can be found here.
  - Development content is not included.
- develop
  - Development for the next version will take place here.
  - There is always a buildable development version here.
  - **If you want to add some feature, Please fork and PullRequest this branch.**
- feature/xxx
  - Branches prefixed with feature represent branches where additional functionality is being developed.
- fix/xxx
  - A branch prefixed with fix indicates a branch that is fixing a bug.

## Setup Python environments

Tunny requires a Python library to run.
The release includes the Python libraries and is automatically set up by the Tunny UI, but this repository does not include them.
If you want to clone this repository, make some changes and see how it works, please follow the steps below to build your Python environment.

1. Open Tunny/Lib directory in PowerShell.
1. Run "setup-python-lib.bat".

If python.zip and whl.zip are created in the Lib directory, the Python environment has been created.

## Submitting changes

Please send a [GitHub Pull Request](https://github.com/hrntsm/Tunny/compare/develop...) with a clear list of what you've done (read more about [pull requests](http://help.github.com/pull-requests/)).
When you send a pull request, we will love you forever if you include examples.
Please follow our coding conventions (below) and make sure all of your commits are atomic (one feature per commit).

Always write a clear log message for your commits. One-line messages are fine for small changes, but bigger changes should look like this:

    $ git commit -m "A brief summary of the commit
    >
    > A paragraph describing what changed and its impact."

## Coding conventions

Start reading our code and you'll get the hang of it.
Please check your .editorconfig file and code accordingly and format it with `dotnet format` commands.

Thanks,
hrntsm
