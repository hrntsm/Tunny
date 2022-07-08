using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

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
                switch (channel)
                {
                    case GH_CanvasChannel.First:
                        if (Selected)
                        {
                            RenderInputComponentBoxes(graphics);
                        }
                        break;
                    case GH_CanvasChannel.Objects:
                        DrawObjects(canvas, graphics, channel);
                        break;
                    case GH_CanvasChannel.Wires:
                        DrawWires(canvas, graphics);
                        break;
                    default:
                        base.Render(canvas, graphics, channel);
                        break;
                }
            }

            private void DrawObjects(GH_Canvas canvas, Graphics graphics, GH_CanvasChannel channel)
            {
                GH_PaletteStyle normalStyle = GH_Skin.palette_normal_standard;
                GH_PaletteStyle warningStyle = GH_Skin.palette_warning_standard;
                GH_PaletteStyle hiddenStyle = GH_Skin.palette_hidden_standard;
                GH_Skin.palette_normal_standard = new GH_PaletteStyle(Color.CornflowerBlue, Color.Blue, Color.Black);
                GH_Skin.palette_warning_standard = new GH_PaletteStyle(Color.CornflowerBlue, Color.Blue, Color.Black);
                GH_Skin.palette_hidden_standard = new GH_PaletteStyle(Color.CornflowerBlue, Color.Blue, Color.Black);
                base.Render(canvas, graphics, channel);
                GH_Skin.palette_normal_standard = normalStyle;
                GH_Skin.palette_warning_standard = warningStyle;
                GH_Skin.palette_hidden_standard = hiddenStyle;
            }

            private void DrawWires(GH_Canvas canvas, Graphics graphics)
            {
                Wire[] wires = Owner.Attributes.Selected
                    ? (new[]
                    {
                        new Wire(3, Color.DarkBlue),
                        new Wire(3, Color.Green),
                        new Wire(3, Color.DarkMagenta),
                    })
                    : (new[]
                    {
                        new Wire(2, Color.FromArgb(Convert.ToInt32("3300008B", 16))),
                        new Wire(2, Color.FromArgb(Convert.ToInt32("33008000", 16))),
                        new Wire(2, Color.FromArgb(Convert.ToInt32("338B008B", 16))),
                    });
                for (int i = 0; i < 3; i++)
                {
                    DrawPath(canvas, graphics, Owner.Params.Input[i], wires[i]);
                }
            }

            private void RenderInputComponentBoxes(Graphics graphics)
            {
                Brush[] fill = new[]
                {
                    new SolidBrush(Color.FromArgb(Convert.ToInt32("9900008B", 16))),
                    new SolidBrush(Color.FromArgb(Convert.ToInt32("99008000", 16))),
                    new SolidBrush(Color.FromArgb(Convert.ToInt32("998B008B", 16))),
                };
                Pen[] edge = new[] { Pens.DarkBlue, Pens.Green, Pens.DarkMagenta };
                for (int i = 0; i < 3; i++)
                {
                    foreach (Guid guid in Owner.Params.Input[i].Sources.Select(s => s.InstanceGuid))
                    {
                        RenderBox(graphics, fill[i], edge[i], guid);
                    }
                }
            }

            private void RenderBox(Graphics graphics, Brush fill, Pen edge, Guid guid)
            {
                GH_Document doc = Owner.OnPingDocument();
                if (doc == null)
                {
                    return;
                }
                IGH_DocumentObject obj = doc.FindObject(guid, false);
                if (obj == null)
                {
                    return;
                }
                if (!obj.Attributes.IsTopLevel)
                {
                    Guid topLevelGuid = obj.Attributes.GetTopLevel.InstanceGuid;
                    obj = doc.FindObject(topLevelGuid, true);
                }
                var rectangle = GH_Convert.ToRectangle(obj.Attributes.Bounds);
                rectangle.Inflate(5, 5);
                graphics.FillRectangle(fill, rectangle);
                graphics.DrawRectangle(edge, rectangle);
            }

            private static void DrawPath(GH_Canvas canvas, Graphics graphics, IGH_Param param, Wire wire)
            {
                PointF p1 = param.Attributes.InputGrip;
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

                    var wirePen = new Pen(wire.Color, wire.Width);
                    graphics.DrawPath(wirePen, wirePath);
                    wirePen.Dispose();
                    wirePath.Dispose();
                }
            }

            private class Wire
            {
                public int Width;
                public Color Color;

                public Wire(int wireWidth, Color wireColor)
                {
                    Width = wireWidth;
                    Color = wireColor;
                }
            }
        }
    }
}
