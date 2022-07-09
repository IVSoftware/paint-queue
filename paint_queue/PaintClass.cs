using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace paint_queue
{
    class PaintClass : Queue<Action>
    {
        public Graphics Graphics { get; set; }
        public new void Enqueue(Action action)
        {
            base.Enqueue(action);
            Refresh?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Refresh;

        public void Drawline()
        {
            CurrentTestColor = _knownColors[_randomColorForTest.Next(_knownColors.Length)];                

            if (Graphics != null)
            {
                var rect = Graphics.VisibleClipBounds;
                using (var pen = new Pen(CurrentTestColor, 2f))
                {
                    var start = new PointF(
                        rect.X,
                        rect.Y);
                    var end = new PointF(
                        rect.X + rect.Width,
                        rect.Y + rect.Height);
                    Graphics.DrawLine(pen, start, end);
                }
            }
        }
        // T E S T I N G
        Color[] _knownColors { get; } =
                Enum.GetValues(typeof(KnownColor))
                .Cast<KnownColor>()
                .Select(known=>Color.FromKnownColor(known))
                .ToArray();
        public Color CurrentTestColor { get; private set; }
        Random _randomColorForTest = new Random();
    }
}
