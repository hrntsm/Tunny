using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;

using Rhino.FileIO;

using Tunny.Component;
using Tunny.UI;

namespace Tunny.Util
{
    public class GrasshopperInOut
    {
        private readonly GH_Document _document;
        private readonly List<Guid> _inputGuids;
        private readonly TunnyComponent _component;
        private IGH_Param _modelMesh;
        public List<IGH_Param> Objectives;
        public List<GH_NumberSlider> Sliders;

        public readonly string ComponentFolder;
        public List<Variable> Variables;
        public string DocumentPath;
        public string DocumentName;

        public GrasshopperInOut(TunnyComponent component)
        {
            _component = component;
            ComponentFolder = Path.GetDirectoryName(Grasshopper.Instances.ComponentServer.FindAssemblyByObject(_component).Location);
            _document = _component.OnPingDocument();
            _inputGuids = new List<Guid>();
            SetInputs();
        }

        private bool SetInputs()
        {
            SetVariables();
            SetObjectives();
            SetModelMesh();

            return true;
        }

        private bool SetVariables()
        {
            Sliders = new List<GH_NumberSlider>();

            foreach (IGH_Param source in _component.Params.Input[0].Sources)
            {
                _inputGuids.Add(source.InstanceGuid);
            }

            if (_inputGuids.Count == 0)
            {
                TunnyMessageBox.Show("No input variables found. Please connect a number slider to the input of the component.", "Tunny");
                return false;
            }

            foreach (Guid guid in _inputGuids)
            {
                IGH_DocumentObject input = _document.FindObject(guid, true);

                if (input is GH_NumberSlider slider)
                {
                    Sliders.Add(slider);
                }
            }

            SetSliderValues();
            return true;
        }

        private void SetSliderValues()
        {
            int i = 0;
            var variables = new List<Variable>();

            foreach (GH_NumberSlider slider in Sliders)
            {
                decimal min = slider.Slider.Minimum;
                decimal max = slider.Slider.Maximum;

                decimal lowerBond;
                decimal upperBond;
                bool isInteger;
                string nickName = slider.NickName;
                if (nickName == "")
                {
                    nickName = "param" + i++;
                }

                switch (slider.Slider.Type)
                {
                    case Grasshopper.GUI.Base.GH_SliderAccuracy.Even:
                        lowerBond = min / 2;
                        upperBond = max / 2;
                        isInteger = true;
                        break;
                    case Grasshopper.GUI.Base.GH_SliderAccuracy.Odd:
                        lowerBond = (min - 1) / 2;
                        upperBond = (max - 1) / 2;
                        isInteger = true;
                        break;
                    case Grasshopper.GUI.Base.GH_SliderAccuracy.Integer:
                        lowerBond = min;
                        upperBond = max;
                        isInteger = true;
                        break;
                    default:
                        lowerBond = min;
                        upperBond = max;
                        isInteger = false;
                        break;
                }

                variables.Add(new Variable(lowerBond, upperBond, isInteger, nickName));
            }

            Variables = variables;
        }

        private bool SetObjectives()
        {
            if (_component.Params.Input[1].SourceCount == 0)
            {
                TunnyMessageBox.Show("No objective found. Please connect a number to the objective of the component.", "Tunny");
                return false;
            }

            Objectives = _component.Params.Input[1].Sources.ToList();
            return true;
        }

        private bool SetModelMesh()
        {
            if (_component.Params.Input[2].SourceCount == 0)
            {
                return false;
            }

            _modelMesh = _component.Params.Input[2].Sources[0];
            return true;
        }

        private bool SetSliderValues(IList<decimal> parameters)
        {
            int i = 0;

            foreach (GH_NumberSlider slider in Sliders)
            {
                if (slider == null)
                {
                    return false;
                }
                decimal val;

                switch (slider.Slider.Type)
                {
                    case Grasshopper.GUI.Base.GH_SliderAccuracy.Even:
                        val = (int)parameters[i++] * 2;
                        break;
                    case Grasshopper.GUI.Base.GH_SliderAccuracy.Odd:
                        val = (int)(parameters[i++] * 2) + 1;
                        break;
                    case Grasshopper.GUI.Base.GH_SliderAccuracy.Integer:
                        val = (int)parameters[i++];
                        break;
                    default:
                        val = parameters[i++];
                        break;
                }

                slider.Slider.RaiseEvents = false;
                slider.SetSliderValue(val);
                slider.ExpireSolution(false);
                slider.Slider.RaiseEvents = true;
            }

            return true;
        }

        private void Recalculate()
        {
            while (_document.SolutionState != GH_ProcessStep.PreProcess || _document.SolutionDepth != 0) { }

            _document.NewSolution(true);

            while (_document.SolutionState != GH_ProcessStep.PostProcess || _document.SolutionDepth != 0) { }
        }

        public void NewSolution(IList<decimal> parameters)
        {
            SetSliderValues(parameters);
            Recalculate();
        }

        public List<double> GetObjectiveValues()
        {
            var values = new List<double>();

            foreach (IGH_Param objective in Objectives)
            {
                IGH_StructureEnumerator ghEnumerator = objective.VolatileData.AllData(false);
                if (ghEnumerator.Count() > 1)
                {
                    TunnyMessageBox.Show(
                        "Tunny doesn't handle list output.\n Separate each objective if you want multiple objectives",
                        "Tunny",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return new List<double>();
                }
                foreach (IGH_Goo goo in ghEnumerator)
                {
                    if (goo is GH_Number num)
                    {
                        values.Add(num.Value);
                    }
                    else if (goo == null)
                    {
                        values.Add(double.NaN);
                    }
                }
            }

            return values;
        }

        public string GetModelDraco()
        {
            if (_modelMesh == null)
            {
                return string.Empty;
            }

            IGH_StructureEnumerator ghEnumerator = _modelMesh.VolatileData.AllData(true);
            foreach (IGH_Goo goo in ghEnumerator)
            {
                if (goo is GH_Mesh mesh)
                {
                    var option = new DracoCompressionOptions
                    {
                        CompressionLevel = 10
                    };
                    return DracoCompression.Compress(mesh.Value, option).ToBase64String();
                }
            }

            return string.Empty;
        }
    }
}