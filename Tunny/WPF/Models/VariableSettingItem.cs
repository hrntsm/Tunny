namespace Tunny.WPF.Models
{
    internal class VariableSettingItem
    {
        public double Low { get; set; }
        public double High { get; set; }
        public double Step { get; set; }
        public bool IsLogScale { get; set; }
        public string Name { get; set; }
    }
}
