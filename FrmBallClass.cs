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
        BouncingBall BallMaster;
        BouncingBall[] Balls = new BouncingBall[300];
        Brush BallColor;
        public FrmBallClass()
        {
            InitializeComponent();
        }

        private void FrmBallClass_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            BallColor = Brushes.Black;
            this.Paint += FrmBallClass_Paint;

            BallMaster = new BouncingBall(this, this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2, 10, 10, 250, BallColor);

            Random rnd = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < Balls.Length; i++)
            {
                int w;
                Balls[i] = new BouncingBall(this,
                                            rnd.Next(0, this.ClientRectangle.Width - 150),
                                            rnd.Next(0, this.ClientRectangle.Height - 150),
                                            ((w = rnd.Next(0, 20)) - 10) == 0 ? -1 * rnd.Next(3, 20) : w,
                                            ((w = rnd.Next(0, 20)) - 10) == 0 ? -1 * rnd.Next(3, 20) : w,
                                            rnd.Next(35, 150),
                                            new System.Drawing.SolidBrush(Color.FromArgb(rnd.Next(50, 255),
                                                                    rnd.Next(50, 255), rnd.Next(50, 255))));
            }

            this.Text = $"Bouncing Class via Ball Class  ({Balls.Length} balls)";

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

            BallMaster.Display(e.Graphics);
            BallMaster.Update();

        }
    }
}


public class BouncingBall
{
    private int y, x, dy, dx, size;
    private Brush ballColor;
    private Form frm;

    public int X
    {
        get { return x; }
    }

    public int Y
    {
        get { return y; }
    }

    public int Size
    {
        get { return size; }
    }

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

        StringFormat sf = new StringFormat();
        sf.Alignment = StringAlignment.Center;
        sf.LineAlignment = StringAlignment.Center;

        g.DrawString($"{this.size}", new Font("Courier", 18), new SolidBrush(Color.White),
                          this.x + this.size / 2, this.y + this.size / 2, sf);
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
