using System;
using System.Windows.Forms;

namespace AStarSimDisplay
{
    public partial class SFMLDrawingSurface : UserControl
    {
        public SFMLDrawingSurface()
        {
            InitializeComponent();
        }

        private void SFMLDrawingSurface_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Do not call base.OnPaint - all drawing will be handled by an SFML Window
            //base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //Do not call base.OnPaintBackground - all drawing will be handled by an SFML Window
            //base.OnPaintBackground(e);
        }
    }
}
