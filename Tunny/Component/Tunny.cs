using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Grasshopper.Kernel;

using Tunny.Util.RhinoComputeWrapper;

namespace Tunny.Component
{
    public class TestCompute : GH_Component
    {
        public TestCompute()
          : base("Tunny", "Nickname",
              "Description",
              "Tunny", "test")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "P", "Path", GH_ParamAccess.item);
            pManager.AddTextParameter("Values", "V", "Values", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "P", "Path", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = string.Empty;
            var values = new List<string>();
            DA.GetData(0, ref path);
            DA.GetDataList(1, values);

            var ghList = new GrasshopperDataTree("input");
            var ghObjs = new List<GrasshopperObject>();
            foreach (string value in values)
            {
                ghObjs.Add(new GrasshopperObject(value));
            }
            ghList.Append(ghObjs, "0");
            var trees = new List<GrasshopperDataTree> { ghList };

            List<GrasshopperDataTree> result = GrasshopperCompute.EvaluateDefinition(path, trees);

            GrasshopperDataTree resultTree = result.Where(ghDataTree => ghDataTree.ParamName == "result").FirstOrDefault();
            string resultData = resultTree.InnerTree.FirstOrDefault().Value.FirstOrDefault().Data;
            double reportValue = double.Parse(resultData.Replace("\"", ""), CultureInfo.InvariantCulture);

            DA.SetData(0, reportValue);
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("FC21DD53-F3CA-40AF-A21F-210F034BCCCA");
    }
}
