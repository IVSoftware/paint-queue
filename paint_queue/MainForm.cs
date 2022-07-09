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
            };
            buttonDraw.Click += (sender, e) =>
            {
                var rect = ClientRectangle;
                var start = new PointF(
                        rect.X,
                        rect.Y);
                var end = new PointF(
                        rect.X + rect.Width,
                        rect.Y + rect.Height);

                _paint.Add(() => _paint.Drawline(start, end));
            };
        }
        PaintClass _paint = new PaintClass();
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if(_paint.Count != 0)
            {
                _paint.Graphics = e.Graphics;
                foreach (var action in _paint)
                {
                    action();
                }
                BeginInvoke((MethodInvoker)delegate
                {
                    Text = _paint.CurrentTestColor.ToString();
                });
            }
        }
    }
}
