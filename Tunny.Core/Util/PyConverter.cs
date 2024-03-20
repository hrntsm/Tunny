using System.Collections.Generic;

using Python.Runtime;

namespace Tunny.Core.Util
{
    public static class PyConverter
    {
        public static PyList EnumeratorToPyList(IEnumerable<string> enumerator)
        {
            TLog.MethodStart();
            var pyList = new PyList();
            foreach (string item in enumerator)
            {
                pyList.Append(new PyString(item));
            }
            return pyList;
        }

        public static PyList EnumeratorToPyList(IEnumerable<int> enumerator)
        {
            TLog.MethodStart();
            var pyList = new PyList();
            foreach (int item in enumerator)
            {
                pyList.Append(new PyInt(item));
            }
            return pyList;
        }

        public static PyList EnumeratorToPyList(IEnumerable<double> enumerator)
        {
            TLog.MethodStart();
            var pyList = new PyList();
            foreach (double item in enumerator)
            {
                pyList.Append(new PyFloat(item));
            }
            return pyList;
        }
    }
}
