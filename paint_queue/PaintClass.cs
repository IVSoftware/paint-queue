using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace paint_queue
{
    class PaintClass : List<PaintClassContext>
    {
        public new void Add(PaintClassContext context)
        {
            base.Add(context);
            Modified = true;
            Refresh?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler Refresh;
        public bool Modified { get; set; }
        public void Drawline(Color color, PointF start, PointF end) =>
            Add(new PaintClassContext
            {
                PaintOp = PaintOp.DrawLine,
                Color = color,
                Start = start,
                End = end,
            });

        public new void Clear()
        {
            base.Clear();
            Add(new PaintClassContext
            {
                PaintOp = PaintOp.Clear,
            });
        }

        private void paint(Graphics graphics,  PaintClassContext context)
        {
            switch (context.PaintOp)
            {
                case PaintOp.DrawLine:
                    using (var pen = new Pen(context.Color, 2f))
                    {
                        graphics.DrawLine(pen, context.Start, context.End);
                    }
                    break;
                case PaintOp.Clear:
                    using (var brush = new SolidBrush(Color.Transparent))
                    {
                        graphics.FillRectangle(brush, graphics.VisibleClipBounds);
                    }
                    base.Clear(); // Remove the clear action
                    break;
                default:
                    throw new NotImplementedException("ToDo");
            }
        }

        internal void PaintAll(Graphics graphics)
        {
            foreach (var context in this.ToArray())
            {
                paint(graphics, context);
            }
        }

        internal void GetNextTestColor() =>
            CurrentTestColor = _knownColors[_randomColorForTest.Next(_knownColors.Length)];

        // T E S T I N G
        Color[] _knownColors { get; } =
                Enum.GetValues(typeof(KnownColor))
                .Cast<KnownColor>()
                .Select(known=>Color.FromKnownColor(known))
                .ToArray();
        public Color CurrentTestColor { get; private set; }
        Random _randomColorForTest = new Random();
    }
    enum PaintOp{ DrawLine, Clear, DrawDiagonal}
    class PaintClassContext
    {
        public PaintOp PaintOp { get; set; }
        public Color Color { get; set; }
        public PointF Start { get; set; }
        public PointF End { get; set; }
    }
}
