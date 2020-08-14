﻿using System;
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
        BouncingBall[] Balls = new BouncingBall[1000];

        public FrmBallClass()
        {
            InitializeComponent();
        }

        private void FrmBallClass_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.Paint += FrmBallClass_Paint;
            this.BackColor = SystemColors.Control;

            this.MouseClick += FrmBallClass_MouseClick;

            //BallMaster = new BouncingBall(this, this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2, 10, 10, 250, Brushes.Black);

            BouncingBall.CreateBalls(Balls, this);

            this.Text = $"Bouncing Class via Ball Class  ({Balls.Length} balls)";

            Draw = new Timer();
            Draw.Interval = 10;
            Draw.Tick += Draw_Tick;
            Draw.Enabled = true;
        }

        private void FrmBallClass_MouseClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(e.X + "  -  " + e.Y);
        }

        private void Draw_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void FrmBallClass_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;


            foreach (BouncingBall ball in Balls)
            {
                ball.Display(e.Graphics);
                //ball.Display(e.Graphics, OverrideClassGraphic);
                ball.Update();
            }

            //BallMaster.Display(e.Graphics);
            //BallMaster.Update();
        }

        public void OverrideClassGraphic(Graphics g, BouncingBall ball)
        {
            g.FillEllipse(ball.BallColor, ball.X, ball.Y, ball.Size, ball.Size);
            g.FillEllipse(Brushes.Black, ball.X + (ball.Size / 8), ball.Y + (ball.Size / 8), ball.Size / 4, ball.Size / 4);
            g.FillEllipse(Brushes.Black, ball.X + (ball.Size / 2), ball.Y + (ball.Size / 8), ball.Size / 4, ball.Size / 4);
            g.FillEllipse(Brushes.Black, ball.X + (ball.Size / 4), ball.Y + (ball.Size / 2), ball.Size / 6, ball.Size / 6);

            //StringFormat sf = new StringFormat();
            //sf.Alignment = StringAlignment.Center;
            //sf.LineAlignment = StringAlignment.Center;
            //g.DrawString($"{ball.Size}", new Font("Courier", 18), new SolidBrush(Color.Red),
            //                  ball.X + ball.Size / 2, ball.Y + ball.Size / 2, sf);


            //g.DrawString($"{ball.Size}", new Font("Courier", 18), new SolidBrush(Color.Red),
            //                  ball.X + ball.Size / 2, ball.Y + ball.Size / 2);
        }
    }
}


public class BouncingBall
{
    int x, y, dx, dy, size;
    private Brush ballColor;
    private Form frm;

    public int X { get { return x; } }

    public int Y { get { return y; } }

    public int DX { get { return x; } }

    public int DY { get { return y; } }

    public int Size { get { return size; } }

    public Brush BallColor { get { return ballColor; } }

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
        g.FillEllipse(this.ballColor, this.x, this.y, this.size, this.size);

        g.DrawString($"{this.size}", new Font("Courier", 18), new SolidBrush(Color.White),
                          this.x + this.size / 2, this.y + this.size / 2);

        //StringFormat sf = new StringFormat();
        //sf.Alignment = StringAlignment.Center;
        //sf.LineAlignment = StringAlignment.Center;
        //g.DrawString($"{this.size}", new Font("Courier", 18), new SolidBrush(Color.White),
        //          this.x + this.size / 2, this.y + this.size / 2, sf);

    }

    public void Display(Graphics g, Action<Graphics, BouncingBall> customDisplay)
    {
        customDisplay(g, this);
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

    public static void CreateBalls(BouncingBall[] balls, Form frm)
    {
        Random rnd = new Random((int)DateTime.Now.Ticks);

        for (int i = 0; i < balls.Length; i++)
        {
            //    public BouncingBall(Form frm, int x, int y, int dx, int dy, int size, Brush ballColor)

            int dir;
            balls[i] = new BouncingBall(frm,
                                        rnd.Next(0, frm.ClientRectangle.Width - 150),
                                        rnd.Next(0, frm.ClientRectangle.Height - 150),
                                        ((dir = rnd.Next(0, 20)) - 10) == 0 ? -1 * rnd.Next(3, 20) : dir,
                                        ((dir = rnd.Next(0, 20)) - 10) == 0 ? -1 * rnd.Next(3, 20) : dir,
                                        rnd.Next(35, 150),
                                        new System.Drawing.SolidBrush(Color.FromArgb(rnd.Next(50, 255),
                                                                rnd.Next(50, 255), rnd.Next(50, 255))));


            //balls[i] = new BouncingBall(frm,
            //                rnd.Next(0, frm.ClientRectangle.Width - 150),
            //                rnd.Next(0, frm.ClientRectangle.Height - 150),
            //                rnd.Next(2, 20),
            //                rnd.Next(2, 20),
            //                rnd.Next(35, 150),
            //                new System.Drawing.SolidBrush(Color.FromArgb(rnd.Next(50, 255),
            //                                        rnd.Next(50, 255), rnd.Next(50, 255))));


        }
    }

}
