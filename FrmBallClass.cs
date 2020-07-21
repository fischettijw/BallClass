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
        BouncingBall[] Balls = new BouncingBall[100];
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

            Ball = new BouncingBall(this, 100, 100, 5, 2, 50, BallColor);

            Random rnd = new Random();
            for (int i = 0; i < Balls.Length; i++)
            {
                Balls[i] = new BouncingBall(this, rnd.Next(0, 1000), rnd.Next(0, 500), rnd.Next(2, 10), rnd.Next(2, 10), rnd.Next(20, 100), new System.Drawing.SolidBrush(Color.FromArgb(rnd.Next(50, 255), rnd.Next(50, 255), rnd.Next(50, 255))));
            }

            Draw = new Timer();
            Draw.Interval = 10;
            Draw.Tick += Draw_Tick;
            Draw.Enabled = true;
        }

        private void Draw_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void FrmBallClass_Paint(object sender, PaintEventArgs e)
        {
            this.BackColor = SystemColors.Control;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            foreach (BouncingBall ball in Balls)
            {
                ball.Display(e.Graphics);
                ball.Update();
            }
        }
    }
}


public class BouncingBall
{
    private int y, x, dy, dx, size;
    private Brush ballColor;
    private Form frm;

    public BouncingBall(Form frm, int x, int y, int dx, int dy, int size, Brush ballColor)
    {
        this.x = x;
        this.y = y;
        this.dy = dy;
        this.dx = dx;
        this.size = size;
        this.ballColor = ballColor;
        this.frm = frm;
    }

    public void Display(Graphics g)
    {
        frm.BackColor = SystemColors.Control;
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
