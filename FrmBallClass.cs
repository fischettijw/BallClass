using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BallClass
{
    public partial class FrmBallClass : Form
    {
        Timer Draw;
        BouncingBall Ball;
        Brush BallColor;
        public FrmBallClass()
        {
            InitializeComponent();
        }

        private void FrmBallClass_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            BallColor = Brushes.Red;
            this.Paint += FrmBallClass_Paint;

            Ball = new BouncingBall(this.CreateGraphics(), this, 100, 100, 2, 2, 50, BallColor);

            Draw = new Timer();
            Draw.Interval = 10;
            Draw.Tick += Draw_Tick;
            Draw.Enabled = true;

        }

        private void FrmBallClass_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            Ball.Display();
            Ball.Update();
        }

        private void Draw_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

    }
}


public class BouncingBall
{
    private int y, x, dy, dx, size;
    private Brush ballColor;
    private Graphics g;
    private Form frm;
    public BouncingBall(Graphics g, Form frm, int x, int y, int dx, int dy, int size, Brush ballColor)
    {
        this.x = x;
        this.y = y;
        this.dy = dy;
        this.dx = dx;
        this.size = size;
        this.ballColor = ballColor;
        this.g = g;
        this.frm = frm;
    }

    public void Display()
    {
        frm.BackColor = Color.AntiqueWhite;
        g.FillEllipse(ballColor, x, y, size, size);

    }

    public void Update()
    {
        this.Move();
        this.checkCollisionsWithWalls();
    }

    public void Move()
    {
        y += dy;
        x += dx;
    }

    public void checkCollisionsWithWalls()
    {
        if (isCollingWithHorizontalWalls()) dy *= -1;
        if (isCollingWithVerticalWalls()) dx *= -1;
    }

    public Boolean isCollingWithVerticalWalls()
    {
        if ((x + size > frm.ClientRectangle.Width) || (x < 0))
        {
            return true;
        }
        return false;
    }

    public Boolean isCollingWithHorizontalWalls()
    {
        if ((y + size > frm.ClientRectangle.Height) || (y < 0))
        {
            return true;
        }
        return false;
    }
}
