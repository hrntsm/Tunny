using System;
using System.Drawing;
using System.Linq;

using Grasshopper.GUI;
using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;

namespace Tunny.Component
{
    public partial class FishingComponent
    {
        private class FishingComponentAttributes : Tunny_ComponentAttributes
        {
            public FishingComponentAttributes(IGH_Component component) : base(component)
            {
            }

            public override GH_ObjectResponse RespondToMouseDoubleClick(GH_Canvas sender, GH_CanvasMouseEvent e)
            {
                ((FishingComponent)Owner).ShowOptimizationWindow();
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
        }
    }
}
