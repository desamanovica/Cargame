using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RacingGame
{
    public partial class Form1 : Form
    {
        private Point pos;  // Uzglabā peles klikšķa sākuma punktu
        private bool dragging, lose;  // Pārvalda loga pārbīdīšanu un zaudēšanas statusu
        private int countCoins = 0;  // Uzglabā monētu skaitu
        public Form1()
        {
            InitializeComponent();

            // Pievieno notikumu apstrādes metodes pelēm un loga pārbīdīšanai
            bg1.MouseDown += MouseClickDown;
            bg1.MouseUp += MouseClickUp;
            bg1.MouseMove += MouseClickMove;

            bg2.MouseDown += MouseClickDown;
            bg2.MouseUp += MouseClickUp;
            bg2.MouseMove += MouseClickMove;

            // Paslēpj nevajadzīgos spēles laikā uzrakstus
            labelLose.Visible = false;
            btnRestart.Visible = false;
            KeyPreview = true;  // Iespējo taustiņu notikumu apstrādi
        }


        // Notikumu apstrādes metodes peles kustībai un loga pārbīdīšanai

        private void MouseClickDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            pos.X = e.X;
            pos.Y = e.Y;
        }
        private void MouseClickUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        private void MouseClickMove(object sender, MouseEventArgs e)
        {
            if(dragging==true)
            {
                Point currPoint = PointToScreen(new Point(e.X, e.Y));
                this.Location = new Point(currPoint.X - pos.X, currPoint.Y - pos.Y + bg1.Top);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }


        // Taustiņu nospiešanas notikums
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Iziet no spēles, nospiežot 'Esc'
            if (e.KeyChar == (char)Keys.Escape) 
                this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }


        // Timer notikums, kas atkārtoti tiek izsaukts 
        private void timer_Tick(object sender, EventArgs e)
        {

            // Kustība foniem, pretiniekiem un monētai
            int speed = 3;
            bg1.Top += speed;
            bg2.Top += speed;

            int carSpeed = 5;
            enemy1.Top += carSpeed;
            enemy2.Top += carSpeed;

            coin.Top += speed;

            if (bg1.Top >= 650)
            {
                bg1.Top = 0;
                bg2.Top = -650;
            }

            if (coin.Top >= 650)
            {
                coin.Top = -50;
                Random rand = new Random();
                coin.Left = rand.Next(150, 560);
            }

            if (enemy1.Top >= 650)
            {
                enemy1.Top = -130;
                Random rand = new Random();
                enemy1.Left = rand.Next(150, 300);
            }
            if (enemy2.Top >= 650) 
            {
                Random rand = new Random();
                enemy2.Left = rand.Next(300, 560);
                enemy2.Top = -400;
            }

            // Sadursme ar pretiniekiem un apbalvojums par monētām

            // Pārbaude, vai spēlētājs zaudējis
            if (player.Bounds.IntersectsWith(enemy1.Bounds)
                || player.Bounds.IntersectsWith(enemy2.Bounds))
            {
                timer.Enabled = false;
                labelLose.Visible = true;
                btnRestart.Visible = true;
                lose = true;
            }

            // Pārbaude, vai spēlētājs savācis monētu
            if (player.Bounds.IntersectsWith(coin.Bounds)) 
            {
                countCoins++;
                labelCoins.Text = "Coins: " + countCoins.ToString();
                coin.Top = -50;
                Random rand = new Random();
                coin.Left = rand.Next(150, 560);
            }
        }


        // Taustiņu nospiešanas notikums
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (lose) return;   // Ja jau zaudēts, ignorē taustiņu nospiešanu

            int speed = 10;

            // Spēlētāja kustība pa labi un pa kreisi
            if ((e.KeyCode == Keys.Left || e.KeyCode == Keys.A) && player.Left > 150) {
                player.Left -= speed;
            }
            else if ((e.KeyCode == Keys.Right || e.KeyCode == Keys.D) && player.Right < 700)
            {
                player.Left += speed;
            }
        }

        private void labelLose_Click(object sender, EventArgs e)
        {

        }

        // Restartēšanas pogas nospiešanas notikums
        private void btnRestart_Click(object sender, EventArgs e)
        {
            // Atiestata spēles stāvokli uz sākuma vērtībām
            enemy1.Top = -130;
            enemy2.Top = -400;
            labelLose.Visible = false;
            btnRestart.Visible = false;
            timer.Enabled = true;
            lose = false;
            countCoins = 0;
            labelCoins.Text = "Coins: 0";
            coin.Top = -500;
        }
    }
}
