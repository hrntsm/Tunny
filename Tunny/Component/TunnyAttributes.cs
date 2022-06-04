using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

namespace Tunny.Component
{
    public partial class TunnyComponent
    {
        private class TunnyAttributes : GH_ComponentAttributes
        {
            public TunnyAttributes(IGH_Component component) : base(component)
            {
            }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                ((TunnyComponent)Owner).ShowOptimizationWindow();
                return GH_ObjectResponse.Handled;
            }

            protected override void Render(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
            {
                if (channel == GH_CanvasChannel.Objects)
                {
                    GH_PaletteStyle normalStyle = GH_Skin.palette_normal_standard;
                    GH_PaletteStyle warningStyle = GH_Skin.palette_warning_standard;
                    GH_Skin.palette_normal_standard = new GH_PaletteStyle(Color.CornflowerBlue, Color.Blue, Color.Black);
                    GH_Skin.palette_warning_standard = new GH_PaletteStyle(Color.CornflowerBlue, Color.Blue, Color.Black);
                    base.Render(canvas, graphics, channel);
                    GH_Skin.palette_normal_standard = normalStyle;
                    GH_Skin.palette_warning_standard = warningStyle;
                }
                else if (channel == GH_CanvasChannel.Wires)
                {
                    DrawVariableWire(canvas, graphics);
                    DrawObjectiveWire(canvas, graphics);
                    DrawModelMeshWire(canvas, graphics);
                }
                else
                {
                    base.Render(canvas, graphics, channel);
                }
            }

            private void DrawVariableWire(GH_Canvas canvas, Graphics graphics)
            {
                IGH_Param param = Owner.Params.Input[0];
                PointF p1 = param.Attributes.InputGrip;

                int wireWidth = 1;
                var wireColor = Color.FromArgb(Convert.ToInt32("3300008B", 16));
                if (Owner.Attributes.Selected)
                {
                    wireWidth = 3;
                    wireColor = Color.DarkBlue;

                }

                foreach (IGH_Param source in param.Sources)
                {
                    PointF p0 = source.Attributes.OutputGrip;
                    if (!canvas.Painter.ConnectionVisible(p0, p1))
                    {
                        continue;
                    }

                    GraphicsPath wirePath = GH_Painter.ConnectionPath(p0, p1, GH_WireDirection.right, GH_WireDirection.left);
                    if (wirePath == null)
                    {
                        continue;
                    }

                    var wirePen = new Pen(wireColor, wireWidth);
                    graphics.DrawPath(wirePen, wirePath);
                    wirePen.Dispose();
                    wirePath.Dispose();
                }
            }

            private void DrawObjectiveWire(GH_Canvas canvas, Graphics graphics)
            {
                IGH_Param param = Owner.Params.Input[1];
                PointF p1 = param.Attributes.InputGrip;

                int wireWidth = 2;
                var wireColor = Color.FromArgb(Convert.ToInt32("33008000", 16));
                if (Owner.Attributes.Selected)
                {
                    wireWidth = 3;
                    wireColor = Color.Green;
                }

                foreach (IGH_Param source in param.Sources)
                {
                    PointF p0 = source.Attributes.OutputGrip;
                    if (!canvas.Painter.ConnectionVisible(p0, p1))
                    {
                        continue;
                    }

                    GraphicsPath wirePath = GH_Painter.ConnectionPath(p0, p1, GH_WireDirection.right, GH_WireDirection.left);
                    if (wirePath == null)
                    {
                        continue;
                    }

                    var wirePen = new Pen(wireColor, wireWidth);
                    graphics.DrawPath(wirePen, wirePath);
                    wirePen.Dispose();
                    wirePath.Dispose();
                }
            }

            private void DrawModelMeshWire(GH_Canvas canvas, Graphics graphics)
            {
                IGH_Param param = Owner.Params.Input[2];
                PointF p1 = param.Attributes.InputGrip;

                int wireWidth = 2;
                var wireColor = Color.FromArgb(Convert.ToInt32("338B008B", 16));
                if (Owner.Attributes.Selected)
                {
                    wireWidth = 3;
                    wireColor = Color.DarkMagenta;
                }

                foreach (IGH_Param source in param.Sources)
                {
                    PointF p0 = source.Attributes.OutputGrip;
                    if (!canvas.Painter.ConnectionVisible(p0, p1))
                    {
                        continue;
                    }

                    GraphicsPath wirePath = GH_Painter.ConnectionPath(p0, p1, GH_WireDirection.right, GH_WireDirection.left);
                    if (wirePath == null)
                    {
                        continue;
                    }

                    var wirePen = new Pen(wireColor, wireWidth);
                    graphics.DrawPath(wirePen, wirePath);
                    wirePen.Dispose();
                    wirePath.Dispose();
                }
            }
        }
    }
}
