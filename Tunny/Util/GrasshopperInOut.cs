using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

using GalapagosComponents;

using Grasshopper.GUI.Base;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Types;

using Rhino.Geometry;

using Tunny.Component.Params;
using Tunny.Component.Util;
using Tunny.Core.Input;
using Tunny.Core.Util;
using Tunny.Input;
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
        private List<GH_NumberSlider> _sliders;
        private List<TunnyValueList> _valueLists;

        public Objective Objectives { get; private set; }
        public List<VariableBase> Variables { get; private set; }
        public Artifact Artifacts { get; private set; }
        public Dictionary<string, FishEgg> EnqueueItems { get; private set; }
        public bool HasConstraint { get; private set; }
        public bool IsLoadCorrectly { get; }

        public GrasshopperInOut(GH_Component component, bool getVariableOnly = false)
        {
            TLog.MethodStart();
            _component = component;
            _document = _component.OnPingDocument();
            _inputGuids = new List<Guid>();

            IsLoadCorrectly = getVariableOnly
                ? SetVariables()
                : SetVariables() && SetObjectives() && SetAttributes() && SetArtifacts();
        }

        private bool SetVariables()
        {
            TLog.MethodStart();
            _sliders = new List<GH_NumberSlider>();
            _valueLists = new List<TunnyValueList>();
            _genePool = new List<GalapagosGeneListObject>();

            _inputGuids.AddRange(_component.Params.Input[0].Sources.Select(source => source.InstanceGuid));
            if (_inputGuids.Count == 0)
            {
                NoVariableInputError();
                return false;
            }

            var variables = new List<VariableBase>();
            if (!FilterInputVariables()) { return false; }
            SetInputSliderValues(variables);
            SetInputGenePoolValues(variables);
            SetInputValueItem(variables);
            Variables = variables;
            return true;
        }

        private static void NoVariableInputError()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "No input variables found. \nPlease connect a number slider to the input of the component.",
                "Tunny",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        private bool FilterInputVariables()
        {
            TLog.MethodStart();
            var errorInputGuids = new List<Guid>();
            foreach ((IGH_DocumentObject docObject, int _) in _inputGuids.Select((guid, i) => (_document.FindObject(guid, false), i)))
            {
                switch (docObject)
                {
                    case GH_NumberSlider slider:
                        _sliders.Add(slider);
                        break;
                    case GalapagosGeneListObject genePool:
                        _genePool.Add(genePool);
                        break;
                    case TunnyValueList valueList:
                        _valueLists.Add(valueList);
                        break;
                    case Param_FishEgg fishEgg:
                        if (fishEgg.VolatileDataCount != 0)
                        {
                            EnqueueItems = ((GH_FishEgg)fishEgg.VolatileData.AllData(true).First()).Value;
                        }
                        break;
                    default:
                        errorInputGuids.Add(docObject.InstanceGuid);
                        break;
                }
            }
            return CheckHasIncorrectVariableInput(errorInputGuids);
        }

        private bool CheckHasIncorrectVariableInput(IReadOnlyCollection<Guid> errorInputGuids)
        {
            TLog.MethodStart();
            return errorInputGuids.Count <= 0 || ShowIncorrectVariableInputMessage(errorInputGuids);
        }

        private bool ShowIncorrectVariableInputMessage(IEnumerable<Guid> errorGuids)
        {
            TLog.MethodStart();
            TunnyMessageBox.Show(
                "Input variables must be either a number slider or a gene pool.\nError input will automatically remove.",
                "Tunny",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            foreach (Guid guid in errorGuids)
            {
                _component.Params.Input[0].RemoveSource(guid);
            }
            _component.ExpireSolution(true);
            return false;
        }

        private void SetInputSliderValues(List<VariableBase> variables)
        {
            TLog.MethodStart();
            int i = 0;

            foreach (GH_NumberSlider slider in _sliders)
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
                    case GH_SliderAccuracy.Even:
                        lowerBond = min / 2;
                        upperBond = max / 2;
                        isInteger = true;
                        break;
                    case GH_SliderAccuracy.Odd:
                        lowerBond = (min - 1) / 2;
                        upperBond = (max - 1) / 2;
                        isInteger = true;
                        break;
                    case GH_SliderAccuracy.Integer:
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

                variables.Add(new NumberVariable(Convert.ToDouble(lowerBond), Convert.ToDouble(upperBond), isInteger, nickName, eps, Convert.ToDouble(value), id));
            }
        }

        private void SetInputGenePoolValues(List<VariableBase> variables)
        {
            TLog.MethodStart();
            var nickNames = new List<string>();
            for (int i = 0; i < _genePool.Count; i++)
            {
                GalapagosGeneListObject genePool = _genePool[i];
                string nickName = nickNames.Contains(genePool.NickName)
                    ? genePool.NickName + i + "-" : genePool.NickName;
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
                    variables.Add(new NumberVariable(Convert.ToDouble(lowerBond), Convert.ToDouble(upperBond), isInteger, name, eps, ghNumber.Value, id));
                }
            }
        }

        private void SetInputValueItem(List<VariableBase> variables)
        {
            TLog.MethodStart();
            foreach (TunnyValueList valueList in _valueLists)
            {
                string nickName = valueList.NickName;
                Guid id = valueList.InstanceGuid;
                string[] categories = valueList.ListItems.Select(value => value.Name).ToArray();
                string selectedItem = valueList.FirstSelectedItem.Name;
                variables.Add(new CategoricalVariable(categories, selectedItem, nickName, id));
            }
        }

        private bool SetObjectives()
        {
            TLog.MethodStart();
            if (_component.Params.Input[1].SourceCount == 0)
            {
                return ShowNoObjectiveFoundMessage();
            }
            var unsupportedObjectives = new List<IGH_Param>();
            foreach (IGH_Param param in _component.Params.Input[1].Sources)
            {
                switch (param)
                {
                    case Param_Number _:
                    case Param_FishPrint _:
                        break;
                    default:
                        unsupportedObjectives.Add(param);
                        break;
                }
            }
            if (unsupportedObjectives.Count > 0)
            {
                return ShowIncorrectObjectiveInputMessage(unsupportedObjectives);
            }
            if (!CheckObjectiveNicknameDuplication(_component.Params.Input[1].Sources.ToArray())) { return false; }
            Objectives = new Objective(_component.Params.Input[1].Sources.ToList());
            return true;
        }

        private static bool ShowNoObjectiveFoundMessage()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show("No objective found.\nPlease connect number or FishPrint to the objective.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private static bool CheckObjectiveNicknameDuplication(IEnumerable<IGH_Param> objectives)
        {
            TLog.MethodStart();
            var nickname = objectives.Select(x => x.NickName)
                                     .GroupBy(name => name).Where(name => name.Count() > 1).Select(group => group.Key).ToList();
            if (nickname.Count > 0)
            {
                TunnyMessageBox.Show("Objective nicknames must be unique.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool ShowIncorrectObjectiveInputMessage(List<IGH_Param> unsupportedSources)
        {
            TLog.MethodStart();
            TunnyMessageBox.Show("Objective supports only the Number or FishPrint input.\nError input will automatically remove.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
            foreach (IGH_Param unsupportedSource in unsupportedSources)
            {
                _component.Params.Input[1].RemoveSource(unsupportedSource);
            }
            _component.ExpireSolution(true);

            return false;
        }

        private bool SetAttributes()
        {
            TLog.MethodStart();
            _attributes = new GH_FishAttribute();
            if (_component.Params.Input[2].SourceCount == 0)
            {
                return true;
            }
            else if (_component.Params.Input[2].SourceCount >= 2 || _component.Params.Input[2].VolatileDataCount > 1)
            {
                return ShowIncorrectAttributeInputMessage();
            }

            IGH_StructureEnumerator enumerator = _component.Params.Input[2].Sources[0].VolatileData.AllData(true);
            foreach (IGH_Goo goo in enumerator)
            {
                if (goo is GH_FishAttribute fishAttr)
                {
                    _attributes = fishAttr;
                    HasConstraint = fishAttr.Value.ContainsKey("Constraint");
                    SetDirection(fishAttr);
                    break;
                }
            }
            return true;
        }

        private void SetDirection(GH_FishAttribute fishAttr)
        {
            int[] directions = new[] { -1 };
            if (fishAttr.Value.TryGetValue("Direction", out object fishDirection))
            {
                if (fishDirection is List<int> dirList)
                {
                    directions = dirList.ToArray();
                }
                else
                {
                    string message = "Direction must be either 1(maximize) or -1(minimize).";
                    TunnyMessageBox.Show(message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw new ArgumentException(message);
                }
            }
            if (!Objectives.SetDirections(directions))
            {
                string message = "The number of the direction in FishAttr must be the same as the number of the objective.";
                TunnyMessageBox.Show(message, "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new ArgumentException(message);
            }
        }

        private static bool ShowIncorrectAttributeInputMessage()
        {
            TLog.MethodStart();
            TunnyMessageBox.Show("Inputs to Attribute should be grouped together into one FishAttribute.", "Tunny", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void SetCategoryValues(string[] categoryParameters)
        {
            TLog.MethodStart();
            int i = 0;
            foreach (TunnyValueList valueList in _valueLists)
            {
                if (valueList == null)
                {
                    return;
                }
                string[] categories = valueList.ListItems.Select(item => item.Name).ToArray();
                int index = Array.IndexOf(categories, categoryParameters[i++]);
                if (index == -1)
                {
                    return;
                }
                valueList.SelectItemUnsafe(index);
            }
        }

        private void SetSliderValues(decimal[] parameters)
        {
            TLog.MethodStart();
            int i = 0;

            foreach (GH_NumberSlider slider in _sliders)
            {
                decimal val;

                switch (slider.Slider.Type)
                {
                    case GH_SliderAccuracy.Even:
                        val = (int)parameters[i++] * 2;
                        break;
                    case GH_SliderAccuracy.Odd:
                        val = (int)(parameters[i++] * 2) + 1;
                        break;
                    case GH_SliderAccuracy.Integer:
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
        }

        private static decimal GetNormalisedGenePoolValue(decimal unnormalized, GalapagosGeneListObject genePool)
        {
            TLog.MethodStart();
            return (unnormalized - genePool.Minimum) / (genePool.Maximum - genePool.Minimum);
        }

        private void Recalculate()
        {
            TLog.MethodStart();
            while (_document.SolutionState != GH_ProcessStep.PreProcess || _document.SolutionDepth != 0) { }
            _document.NewSolution(false);
            while (_document.SolutionState != GH_ProcessStep.PostProcess || _document.SolutionDepth != 0) { }
        }

        public void NewSolution(IList<Parameter> parameters)
        {
            TLog.MethodStart();
            decimal[] decimalParameters = parameters.Where(p => p.HasNumber).Select(p => (decimal)p.Number).ToArray();
            string[] categoryParameters = parameters.Where(p => p.HasCategory).Select(p => p.Category).ToArray();
            SetSliderValues(decimalParameters);
            SetCategoryValues(categoryParameters);
            ExpireInput(_component.Params.Input[1]); // objectives
            ExpireInput(_component.Params.Input[3]); // artifacts
            Recalculate();
            SetObjectives();
            SetAttributes();
            SetArtifacts();
        }

        private void ExpireInput(IGH_Param input)
        {
            TLog.MethodStart();
            // TopLevel is acquired
            // because it is necessary to Expire the component itself, not the value of the output.
            foreach (Guid guid in input.Sources.Select(p => p.InstanceGuid))
            {
                IGH_DocumentObject obj = _document.FindObject(guid, false);
                if (!obj.Attributes.IsTopLevel)
                {
                    Guid topLevelGuid = obj.Attributes.GetTopLevel.InstanceGuid;
                    obj = _document.FindObject(topLevelGuid, true);
                }
                obj.ExpireSolution(false);
            }
        }

        public string[] GetGeometryJson()
        {
            TLog.MethodStart();
            var json = new List<string>();

            if (_attributes.Value == null
                || !_attributes.Value.TryGetValue("Geometry", out object value) || !(value is List<object> geometries))
            {
                return json.ToArray();
            }

            foreach (object param in geometries)
            {
                if (param is IGH_Goo goo)
                {
                    json.Add(GooConverter.GooToString(goo, true));
                }
            }

            return json.ToArray();
        }

        public Dictionary<string, List<string>> GetAttributes()
        {
            TLog.MethodStart();
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
                    if (obj is List<double> val)
                    {
                        value.AddRange(val.Select(v => v.ToString(CultureInfo.InvariantCulture)));
                    }
                }
                else
                {
                    AddGooValues(key, value);
                }
                attrs.Add(key, value);
            }

            return attrs;
        }

        private void AddGooValues(string key, List<string> value)
        {
            TLog.MethodStart();
            if (_attributes.Value[key] is List<object> objList)
            {
                foreach (object param in objList)
                {
                    if (param is IGH_Goo goo)
                    {
                        value.Add(GooConverter.GooToString(goo, true));
                    }
                }
            }
        }

        private bool SetArtifacts()
        {
            TLog.MethodStart();
            Artifacts = new Artifact();
            if (_component.Params.Input[3].SourceCount == 0)
            {
                return true;
            }

            foreach (IGH_Param param in _component.Params.Input[3].Sources)
            {
                IGH_StructureEnumerator enumerator = param.VolatileData.AllData(true);
                foreach (IGH_Goo goo in enumerator)
                {
                    bool result = goo.CastTo(out GeometryBase geometry);
                    if (result)
                    {
                        Artifacts.Geometries.Add(geometry);
                        continue;
                    }

                    if (goo is GH_FishPrint fishPrint)
                    {
                        Artifacts.Images.Add(fishPrint.Value);
                        continue;
                    }

                    result = goo.CastTo(out string path);
                    if (result)
                    {
                        Artifacts.AddFilePathToArtifact(path);
                    }
                }
            }
            return Artifacts.Count() != 0;
        }
    }
}
