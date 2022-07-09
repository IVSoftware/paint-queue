using System;
using System.Drawing;
using System.Linq;
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
                Text = CurrentTestColor.ToString();
            };
            // Draw diagonal line adding 25 to offset each time.
            buttonDraw.Click += (sender, e) =>
            {
                GetNextTestColor();
                //var rect = ClientRectangle;
                //var start = new PointF(
                //        rect.X + (_testCount * 25),
                //        rect.Y);
                //var end = new PointF(
                //        rect.X + rect.Width + (_testCount * 25),
                //        rect.Y + rect.Height);
                //_paint.Drawline(_paint.CurrentTestColor, start, end);
                _paint.DrawDiagonal(GetNextTestColor(), offsetX: 25 * _testCount++);
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

        internal Color GetNextTestColor()
        {
            CurrentTestColor = _knownColors[_randomColorForTest.Next(_knownColors.Length)];
            return CurrentTestColor; // For convenience
        }

        // T E S T I N G
        Color[] _knownColors { get; } =
                Enum.GetValues(typeof(KnownColor))
                .Cast<KnownColor>()
                .Select(known => Color.FromKnownColor(known))
                .ToArray();
        public Color CurrentTestColor { get; private set; }
        Random _randomColorForTest = new Random();
    }
}
