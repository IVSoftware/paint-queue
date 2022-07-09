using System;
using System.Drawing;
using System.Windows.Forms;

namespace paint_queue
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _paint.Refresh += (sender, e) =>
            {
                // Causes the control to repaint.
                Refresh();
                Text = _paint.CurrentTestColor.ToString();
            };
            // Draw diagonal line adding 25 to offset each time.
            buttonDraw.Click += (sender, e) =>
            {
                _paint.GetNextTestColor();
                var rect = ClientRectangle;
                var start = new PointF(
                        rect.X + (_testCount * 25),
                        rect.Y);
                var end = new PointF(
                        rect.X + rect.Width + (_testCount * 25),
                        rect.Y + rect.Height);
                _paint.Drawline(_paint.CurrentTestColor, start, end);
                _testCount++;
            };
            buttonClear.Click += (sender, e) =>
            {
                _paint.Clear();
                _testCount = 0;
            };
        }
        int _testCount = 0;
        PaintClass _paint = new PaintClass();
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if(_paint.Modified)
            {
                _paint.PaintAll(e.Graphics);
                _paint.Modified = false;
            }
        }
    }
}
