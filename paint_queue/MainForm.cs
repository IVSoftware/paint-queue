using System;
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
                _paint.Enqueue(() => _paint.Drawline());
            };
        }
        PaintClass _paint = new PaintClass();
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if(_paint.Count != 0)
            {
                _paint.Graphics = e.Graphics;
                while(_paint.TryDequeue(out Action action))
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
