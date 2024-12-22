using System;

using Optuna.Util;

using Python.Runtime;

namespace Optuna.Trial
{
    public class TrialWrapper
    {
        public dynamic PyObject { get; private set; }
        public int Id => PyObject._trial_id;
        public int Number => PyObject.number;

        public TrialWrapper(dynamic trial)
        {
            PyObject = trial;
        }

        public void Report(double value, int step)
        {
            PyObject.report(value, step);
        }

        public void SetUserAttribute(string key, PyObject value)
        {
            PyObject.set_user_attr(key, value);
        }

        public void SetUserAttribute(string key, string value)
        {
            PyObject.set_user_attr(key, value);
        }

        public void SetUserAttribute(string key, string[] values)
        {
            PyList valueList = PyConverter.EnumeratorToPyList(values);
            PyObject.set_user_attr(key, valueList);
        }

        public void SetUserAttribute(string key, double value)
        {
            PyObject.set_system_attr(key, value);
        }

        public void SetUserAttribute(string key, double[] values)
        {
            PyList valueList = PyConverter.EnumeratorToPyList(values);
            PyObject.set_system_attr(key, valueList);
        }

        //TODO: Fix Do not use try-catch block
        public bool ShouldPrune()
        {
            bool shouldPrune;
            try
            {
                shouldPrune = PyObject.should_prune();
            }
            catch (Exception)
            {
                PyObject pyShouldPrune = PyObject.should_prune().item();
                shouldPrune = pyShouldPrune.As<bool>();
            }

            return shouldPrune;
        }

        public string SuggestCategorical(string name, string[] choices)
        {
            return PyObject.suggest_categorical(name, choices);
        }

        public double SuggestFloat(string name, double low, double high, double step)
        {
            return PyObject.suggest_float(name, low, high, step: step);
        }

        public double SuggestFloat(string name, double low, double high, bool log)
        {
            return PyObject.suggest_float(name, low, high, log: log);
        }

        public int SuggestInt(string name, double low, double high, double step)
        {
            return PyObject.suggest_int(name, low, high, step: step);
        }

        public int SuggestInt(string name, double low, double high, bool log)
        {
            return PyObject.suggest_int(name, low, high, log: log);
        }
    }
}
