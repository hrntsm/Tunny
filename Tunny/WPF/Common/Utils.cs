using System.Collections.ObjectModel;

using Optuna.Study;

using Tunny.WPF.Models;

namespace Tunny.WPF.Common
{
    internal static class Utils
    {
        internal static ObservableCollection<NameComboBoxItem> StudyNamesFromStudySummaries(StudySummary[] summaries)
        {
            var items = new ObservableCollection<NameComboBoxItem>();
            for (int i = 0; i < summaries.Length; i++)
            {
                StudySummary summary = summaries[i];
                items.Add(new NameComboBoxItem()
                {
                    Id = i,
                    Name = summary.StudyName
                });
            }
            return items;
        }
    }
}
