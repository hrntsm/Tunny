using System.Collections.Generic;

using Python.Runtime;

namespace Optuna.Util
{
    public static class PyConverter
    {
        public static PyList EnumeratorToPyList(IEnumerable<string> enumerator)
        {
            var pyList = new PyList();
            foreach (string item in enumerator)
            {
                pyList.Append(new PyString(item));
            }
            return pyList;
        }

        public static PyList EnumeratorToPyList(IEnumerable<int> enumerator)
        {
            var pyList = new PyList();
            foreach (int item in enumerator)
            {
                pyList.Append(new PyInt(item));
            }
            return pyList;
        }

        public static PyList EnumeratorToPyList(IEnumerable<double> enumerator)
        {
            var pyList = new PyList();
            foreach (double item in enumerator)
            {
                pyList.Append(new PyFloat(item));
            }
            return pyList;
        }
    }
}
