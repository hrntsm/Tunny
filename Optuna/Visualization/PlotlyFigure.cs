namespace Optuna.Visualization
{
    public class PlotlyFigure
    {
        public dynamic PyFigure { get; private set; }

        public PlotlyFigure(dynamic plotlyFigure)
        {
            PyFigure = plotlyFigure;
        }

        public void UpdateLayout(FigureLayout layout)
        {
            PyFigure.update_layout(paper_bgcolor: layout.PaperBgColor);
        }

        public void Show()
        {
            PyFigure.show();
        }

        public void WriteHtml(string path)
        {
            PyFigure.write_html(path);
        }
    }

    public class FigureLayout
    {
        public string PaperBgColor { get; set; } = "rgba(0,0,0,0)";
    }
}
