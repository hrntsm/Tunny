using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;

namespace Tunny.Component
{
    public partial class ConstructFishEgg
    {
        private class ConstructFishEggAttributes : GH_ComponentAttributes
        {
            public ConstructFishEggAttributes(IGH_Component component) : base(component)
            {
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
                    case GH_CanvasChannel.Wires:
                        DrawWires(canvas, graphics);
                        break;
                    default:
                        base.Render(canvas, graphics, channel);
                        break;
                }
            }

            private void RenderInputComponentBoxes(Graphics graphics)
            {
                Brush fill = new SolidBrush(Color.FromArgb(Convert.ToInt32("99FFA500", 16)));
                Pen edge = Pens.Orange;
                foreach (Guid guid in Owner.Params.Input[0].Sources.Select(s => s.InstanceGuid))
                {
                    RenderBox(graphics, fill, edge, guid);
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

            private void DrawWires(GH_Canvas canvas, Graphics graphics)
            {
                Wire[] wires = Owner.Attributes.Selected
                    ? (new[]
                    {
                        new Wire(3, Color.Orange),
                        new Wire(3, GH_Skin.wire_selected_a, GH_Skin.wire_selected_b),
                        new Wire(3, GH_Skin.wire_selected_a, GH_Skin.wire_selected_b),
                    })
                    : (new[]
                    {
                        new Wire(2, Color.FromArgb(Convert.ToInt32("33FFA500", 16))),
                        new Wire(3, GH_Skin.wire_default),
                        new Wire(3, GH_Skin.wire_default)
                    });


                for (int i = 0; i < 3; i++)
                {
                    DrawPath(canvas, graphics, Owner.Params.Input[i], wires[i]);
                }
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

                    var brush = new LinearGradientBrush(p0, p1, wire.ColorEnd, wire.ColorStart);

                    var wirePen = new Pen(brush, wire.Width);
                    graphics.DrawPath(wirePen, wirePath);
                    wirePen.Dispose();
                    wirePath.Dispose();
                }
            }

            private class Wire
            {
                public int Width;
                public Color ColorStart;
                public Color ColorEnd;

                public Wire(int wireWidth, Color wireColor)
                {
                    Width = wireWidth;
                    ColorStart = wireColor;
                    ColorEnd = wireColor;
                }
                public Wire(int wireWidth, Color wireColorStart, Color wireColorEnd)
                {
                    Width = wireWidth;
                    ColorStart = wireColorStart;
                    ColorEnd = wireColorEnd;
                }
            }
        }
    }
}
