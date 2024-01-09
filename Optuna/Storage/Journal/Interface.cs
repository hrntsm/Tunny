using System;
using System.Collections.Generic;

using Optuna.Study;
using Optuna.Trial;

namespace Optuna.Storage.Journal
{
    public interface IJournalOperationBase
    {
        JournalOperation OpCode { get; set; }
        string WorkerId { get; set; }
    }

    public interface IJournalDateTime
    {
        DateTime? DateTimeStart { get; set; }
        DateTime? DateTimeComplete { get; set; }
    }

    public interface IJournalOperationCreateStudy : IJournalOperationBase
    {
        string StudyName { get; set; }
        StudyDirection[] Directions { get; set; }
    }

    public interface IJournalOperationDeleteStudy : IJournalOperationBase
    {
        int StudyId { get; set; }
    }

    public interface IJournalOperationCreateTrial : IJournalOperationBase, IJournalDateTime
    {
        int StudyId { get; set; }
        Dictionary<string, string> Distributions { get; set; }
        Dictionary<string, object> Params { get; set; }
        Dictionary<string, object> UserAttrs { get; set; }
        Dictionary<string, object> SystemAttrs { get; set; }
        TrialState State { get; set; }
        Dictionary<string, double> IntermediateValues { get; set; }
        double? Value { get; set; }
        double[] Values { get; set; }
    }


    public interface IJournalOperationSetTrialParam : IJournalOperationBase
    {
        int TrialId { get; set; }
        string ParamName { get; set; }
        double ParamValueInternal { get; set; }
    }

    public interface IJournalOperationSetTrialStateValues : IJournalOperationBase, IJournalDateTime
    {
        int TrialId { get; set; }
        TrialState State { get; set; }
        double[] Values { get; set; }
    }

    public interface IJournalOperationSetTrialIntermediateValue : IJournalOperationBase
    {
        int TrialId { get; set; }
        int Step { get; set; }
        double IntermediateValue { get; set; }
    }

    public interface IJournalOperationSetTrialUserAttr : IJournalOperationBase
    {
        int TrialId { get; set; }
        Dictionary<string, object> UserAttr { get; set; }
    }
}
