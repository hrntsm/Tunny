using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using GalapagosComponents;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;

using Tunny.Type;
using Tunny.UI;

namespace Tunny.Util
{
    public class GrasshopperInOut
    {
        private readonly GH_Document _document;
        private readonly List<Guid> _inputGuids;
        private readonly GH_Component _component;
        private List<GalapagosGeneListObject> _genePool;
        private GH_FishAttribute _attributes;

        public List<IGH_Param> Objectives { get; set; }
        public List<GH_NumberSlider> Sliders { get; set; }
        public string ComponentFolder { get; }
        public List<Variable> Variables { get; set; }
        public Dictionary<string, FishEgg> EnqueueItems { get; set; }
        public bool HasConstraint { get; set; }
        public string DocumentPath { get; set; }
        public string DocumentName { get; set; }
        public bool IsLoadCorrectly { get; set; }

        public GrasshopperInOut(GH_Component component, bool getVariableOnly = false)
        {
            _component = component;
            ComponentFolder = Path.GetDirectoryName(Grasshopper.Instances.ComponentServer.FindAssemblyByObject(_component).Location);
            _document = _component.OnPingDocument();
            _inputGuids = new List<Guid>();

            if (getVariableOnly)
            {
                SetVariables();
            }
            else
            {
                SetVariables();
                IsLoadCorrectly = SetObjectives();
                SetAttributes();
            }
        }

        private void SetVariables()
        {
            Sliders = new List<GH_NumberSlider>();
            _genePool = new List<GalapagosGeneListObject>();

            _inputGuids.AddRange(_component.Params.Input[0].Sources.Select(source => source.InstanceGuid));
            if (_inputGuids.Count == 0)
            {
                TunnyMessageBox.Show("No input variables found. Please connect a number slider to the input of the component.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var variables = new List<Variable>();
            FilterInputVariables();
            SetInputSliderValues(variables);
            SetInputGenePoolValues(variables);
            Variables = variables;
        }

        private void FilterInputVariables()
        {
            var errorGuids = new List<Guid>();
            foreach ((IGH_DocumentObject docObject, int i) in _inputGuids.Select((guid, i) => (_document.FindObject(guid, false), i)))
            {
                switch (docObject)
                {
                    case GH_NumberSlider slider:
                        Sliders.Add(slider);
                        break;
                    case GalapagosGeneListObject genePool:
                        _genePool.Add(genePool);
                        break;
                    case Param_FishEgg fishEgg:
                        var ghFishEgg = (GH_FishEgg)fishEgg.VolatileData.AllData(true).First();
                        EnqueueItems = ghFishEgg.Value;
                        break;
                    default:
                        errorGuids.Add(docObject.InstanceGuid);
                        break;
                }
            }
            CheckHasIncorrectVariableInput(errorGuids);
        }

        private void CheckHasIncorrectVariableInput(List<Guid> errorGuids)
        {
            if (errorGuids.Count > 0)
            {
                ShowIncorrectVariableInputMessage(errorGuids);
            }
        }

        private void ShowIncorrectVariableInputMessage(List<Guid> errorGuids)
        {
            TunnyMessageBox.Show("Input variables must be either a number slider or a gene pool.\nError input will automatically remove.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
            foreach (Guid guid in errorGuids)
            {
                _component.Params.Input[0].RemoveSource(guid);
            }
            _component.ExpireSolution(true);
        }

        private void SetInputSliderValues(ICollection<Variable> variables)
        {
            int i = 0;

            foreach (GH_NumberSlider slider in Sliders)
            {
                decimal min = slider.Slider.Minimum;
                decimal max = slider.Slider.Maximum;
                decimal value = slider.Slider.Value;
                Guid id = slider.InstanceGuid;

                decimal lowerBond;
                decimal upperBond;
                bool isInteger;
                string nickName = slider.NickName;
                if (nickName == "")
                {
                    nickName = "param" + i++;
                }
                double eps = Convert.ToDouble(slider.Slider.Epsilon);
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

                variables.Add(new Variable(Convert.ToDouble(lowerBond), Convert.ToDouble(upperBond), isInteger, nickName, eps, Convert.ToDouble(value), id));
            }
        }

        private void SetInputGenePoolValues(ICollection<Variable> variables)
        {
            var nickNames = new List<string>();
            for (int i = 0; i < _genePool.Count; i++)
            {
                GalapagosGeneListObject genePool = _genePool[i];
                string nickName = nickNames.Contains(genePool.NickName) ? genePool.NickName + i + "-" : genePool.NickName;
                nickNames.Add(nickName);
                bool isInteger = genePool.Decimals == 0;
                decimal lowerBond = genePool.Minimum;
                decimal upperBond = genePool.Maximum;
                double eps = Math.Pow(10, -genePool.Decimals);
                Guid id = genePool.InstanceGuid;
                for (int j = 0; j < genePool.Count; j++)
                {
                    IGH_Goo[] goo = genePool.VolatileData.AllData(false).ToArray();
                    var ghNumber = (GH_Number)goo[j];
                    string name = nickNames[i] + j;
                    variables.Add(new Variable(Convert.ToDouble(lowerBond), Convert.ToDouble(upperBond), isInteger, name, eps, ghNumber.Value, id));
                }
            }
        }

        private bool SetObjectives()
        {
            if (_component.Params.Input[1].SourceCount == 0)
            {
                TunnyMessageBox.Show("No objective found. Please connect a number to the objective of the component.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            var noNumberObjectives = new List<IGH_Param>();
            foreach (IGH_Param ghParam in _component.Params.Input[1].Sources)
            {
                if (ghParam.ToString() != "Grasshopper.Kernel.Parameters.Param_Number")
                {
                    noNumberObjectives.Add(ghParam);
                }
            }

            if (noNumberObjectives.Count > 0)
            {
                ShowIncorrectObjectiveInputMessage(noNumberObjectives);
                return false;
            }

            if (!CheckObjectiveNicknameDuplication(_component.Params.Input[1].Sources.ToArray())) { return false; }
            Objectives = _component.Params.Input[1].Sources.ToList();
            return true;
        }

        private static bool CheckObjectiveNicknameDuplication(IGH_Param[] objectives)
        {
            var nickname = objectives.Select(x => x.NickName).GroupBy(name => name).Where(name => name.Count() > 1).Select(group => group.Key).ToList();
            if (nickname.Count > 0)
            {
                TunnyMessageBox.Show("Objective nicknames must be unique.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void ShowIncorrectObjectiveInputMessage(List<IGH_Param> noNumberObjectives)
        {
            TunnyMessageBox.Show("Objective supports only the Number input.\nError input will automatically remove.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
            foreach (IGH_Param noNumberSource in noNumberObjectives)
            {
                _component.Params.Input[1].RemoveSource(noNumberSource);
            }
            _component.ExpireSolution(true);
        }

        private void SetAttributes()
        {
            _attributes = new GH_FishAttribute();
            if (_component.Params.Input[2].SourceCount == 0)
            {
                return;
            }
            else if (_component.Params.Input[2].SourceCount >= 2)
            {
                ShowIncorrectAttributeInputMessage();
                return;
            }

            IGH_StructureEnumerator enumerator = _component.Params.Input[2].Sources[0].VolatileData.AllData(true);
            foreach (IGH_Goo goo in enumerator)
            {
                if (goo is GH_FishAttribute fishAttr)
                {
                    _attributes = fishAttr;
                    HasConstraint = fishAttr.Value.ContainsKey("Constraint");
                    break;
                }
            }
        }

        private void ShowIncorrectAttributeInputMessage()
        {
            TunnyMessageBox.Show("Inputs to Attribute should be grouped together into a single input.\nError input will automatically remove.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
            while (_component.Params.Input[2].SourceCount > 1)
            {
                _component.Params.Input[2].RemoveSource(_component.Params.Input[2].Sources[1]);
            }
            _component.ExpireSolution(true);
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

            foreach (GalapagosGeneListObject genePool in _genePool)
            {
                for (int j = 0; j < genePool.Count; j++)
                {
                    genePool.set_NormalisedValue(j, GetNormalisedGenePoolValue(parameters[i++], genePool));
                    genePool.ExpireSolution(false);
                }
            }

            return true;
        }

        private static decimal GetNormalisedGenePoolValue(decimal unnormalized, GalapagosGeneListObject genePool)
        {
            return (unnormalized - genePool.Minimum) / (genePool.Maximum - genePool.Minimum);
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
            SetObjectives();
            SetAttributes();
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

        public List<string> GetGeometryJson()
        {
            var json = new List<string>();

            if (_attributes.Value == null || !_attributes.Value.ContainsKey("Geometry"))
            {
                return json;
            }

            var geometries = _attributes.Value["Geometry"] as List<object>;
            foreach (object param in geometries)
            {
                if (param is IGH_Goo goo)
                {
                    json.Add(Converter.GooToString(goo, true));
                }
            }

            return json;
        }


        public Dictionary<string, List<string>> GetAttributes()
        {
            var attrs = new Dictionary<string, List<string>>();
            if (_attributes.Value == null)
            {
                return attrs;
            }

            foreach (string key in _attributes.Value.Keys)
            {
                if (key == "Geometry")
                {
                    continue;
                }

                var value = new List<string>();
                if (key == "Constraint")
                {
                    object obj = _attributes.Value[key];
                    if (obj is double val)
                    {
                        value.Add(val.ToString());
                    }
                }
                else
                {
                    var objList = _attributes.Value[key] as List<object>;
                    foreach (object param in objList)
                    {
                        if (param is IGH_Goo goo)
                        {
                            value.Add(Converter.GooToString(goo, true));
                        }
                    }
                }

                attrs.Add(key, value);
            }

            return attrs;
        }
    }
}
