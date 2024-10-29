using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Media;
namespace game2048
{
    public partial class Game2048 : Form
    {
       
        SoundPlayer sound = new SoundPlayer(Application.StartupPath+"/andiem.wav");
        SoundPlayer sound2 = new SoundPlayer(Application.StartupPath+"/blip.wav");
        
        Random Rd = new Random();
        bool replay = true;
        static ArrayList array1 = new ArrayList();
        public Game2048()
        {           
            InitializeComponent();            
        }

        //Tạo màu cho số
        // Tạo từ điển cho màu sắc
        private readonly Dictionary<string, Color> tileColors = new Dictionary<string, Color>
        {
            { "", Color.DarkMagenta },
            { "2", Color.LightGray },
            { "4", Color.Gray },
            { "8", Color.Orange },
            { "16", Color.OrangeRed },
            { "32", Color.DarkOrange },
            { "64", Color.LightPink },
            { "128", Color.Red },
            { "256", Color.DarkRed },
            { "512", Color.LightBlue },
            { "1024", Color.Blue },
            { "2048", Color.Green }
        };

        // Phương thức gán màu ngắn gọn
        private void paint()
        {
            Label[,] Game = {
                        { lbl1, lbl2, lbl3, lbl4 },
                        { lbl5, lbl6, lbl7, lbl8 },
                        { lbl9, lbl10, lbl11, lbl12 },
                        { lbl13, lbl14, lbl15, lbl16 }
                    };

            foreach (Label tile in Game)
            {
                string text = tile.Text;
                tile.BackColor = tileColors.ContainsKey(text) ? tileColors[text] : Color.Black;
                tile.ForeColor = Color.White;
            }
        }


        //Tạo số ngẫu nhiên
        public void randomNumber() {
            array1.Clear();

            Label[,] Game = {
                                {lbl1,lbl2,lbl3,lbl4},
                                {lbl5,lbl6,lbl7,lbl8},
                                {lbl9,lbl10,lbl11,lbl12},
                                {lbl13,lbl14,lbl15,lbl16}
                              };
            for (int i = 0; i < 4;i++ )
            {
                for (int j = 0; j < 4;j++)
                {
                    if(Game[i,j].Text==""){
                        array1.Add(i*4+j+1);
                    }
                }
            }
            
            if(array1.Count>0){
               
                int rdNumber = int.Parse(array1[Rd.Next(0,array1.Count-1)].ToString());
                int i0 = (rdNumber - 1) / 4;
                int j0 = (rdNumber - 1) - i0 *4;
                int array2 = Rd.Next(1,10);
                if (array2 == 1 || array2 == 2 || array2 == 3 || array2 == 4 || array2 == 5||array2==6)
                {
                    Game[i0, j0].Text = "2";
                }
                else { 
                    Game[i0,j0].Text="4";
                }

            }
            paint();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            randomNumber();
            randomNumber();
            randomNumber();
        }
        // Phương thức di chuyển chung cho mọi hướng
        public void MoveTiles(int directionX, int directionY)
        {
            bool hasMoved = false;
            bool checkWin = false;
            bool newNumber = false;
            Label[,] Game = {
                        { lbl1, lbl2, lbl3, lbl4 },
                        { lbl5, lbl6, lbl7, lbl8 },
                        { lbl9, lbl10, lbl11, lbl12 },
                        { lbl13, lbl14, lbl15, lbl16 }
                    };

            // Đặt thứ tự duyệt ô dựa trên hướng
            int startX = (directionX == 1) ? 3 : 0;
            int startY = (directionY == 1) ? 3 : 0;
            int endX = (directionX == 1) ? -1 : 4;
            int endY = (directionY == 1) ? -1 : 4;

            for (int i = startX; i != endX; i -= directionX == 0 ? 1 : directionX)
            {
                for (int j = startY; j != endY; j -= directionY == 0 ? 1 : directionY)
                {
                    int x = i, y = j;
                    if (Game[x, y].Text == "") continue;

                    // Di chuyển và gộp ô theo hướng chỉ định
                    while (true)
                    {
                        int nextX = x + directionX;
                        int nextY = y + directionY;

                        if (nextX < 0 || nextX >= 4 || nextY < 0 || nextY >= 4) break;
                        if (Game[nextX, nextY].Text == "")
                        {
                            Game[nextX, nextY].Text = Game[x, y].Text;
                            Game[x, y].Text = "";
                            x = nextX;
                            y = nextY;
                            hasMoved = true;
                        }
                        else if (Game[nextX, nextY].Text == Game[x, y].Text)
                        {
                            Game[nextX, nextY].Text = (int.Parse(Game[nextX, nextY].Text) * 2).ToString();
                            lblScore.Text = (int.Parse(lblScore.Text) + int.Parse(Game[nextX, nextY].Text)).ToString();
                            Game[x, y].Text = "";
                            hasMoved = true;
                            newNumber = true;
                            checkWin = int.Parse(Game[nextX, nextY].Text) == 2048;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            // Phát âm thanh
            if (hasMoved)
            {
                if (checkWin) sound.Play();
                else if (newNumber) sound2.Play();
                if (newNumber) randomNumber();
            }
        }

        // Gọi các hàm di chuyển
        public void moveUp() => MoveTiles(0, -1);
        public void moveDown() => MoveTiles(0, 1);
        public void moveLeft() => MoveTiles(-1, 0);
        public void moveRight() => MoveTiles(1, 0);

        //cập nhật giao diện
        public bool Number() {
            Label[,] Game = {
                                {lbl1,lbl2,lbl3,lbl4},
                                {lbl5,lbl6,lbl7,lbl8},
                                {lbl9,lbl10,lbl11,lbl12},
                                {lbl13,lbl14,lbl15,lbl16}
                              };
            for (int i = 0; i < 4;i++ )
            {
                for (int j = 0; j < 4;j++ )
                {
                    if(Game[i,j].Text==""){
                        return false;
                    }
                    for (int k = j+1; k < 4;k++ )
                    {
                        if(Game[i,j].Text!=""){
                            if(Game[i,j].Text==Game[i,k].Text){
                                return false;
                            }
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Game[j, i].Text == "")
                    {
                        return false;
                    }
                    for (int k = j + 1; k < 4; k++)
                    {
                        if (Game[k,i].Text != "")
                        {
                            if (Game[j,i].Text == Game[k,i].Text)
                            {
                                return false;
                            }
                            break;
                        }
                    }
                }
            }
            return true;
        }
        //Kết nối với bàn phím
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Number() == false)
            {
                if (e.KeyCode == Keys.Up)
                {
                    moveUp();

                }
                if (e.KeyCode == Keys.Down)
                {
                    moveDown();
                }
                if (e.KeyCode == Keys.Left)
                {
                    moveLeft();
                }
                if (e.KeyCode == Keys.Right)
                {
                    moveRight();
                }
               

            }
            else {
                replay = false;
                lblGameOver.Visible = true;
                btnNewGame.Visible = true;
                btnExit.Visible = true;
                btnExit.Enabled = true;
                btnNewGame.Enabled = true;
                this.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            }
           
        }
        // Hàm khởi động lại trò chơi sau khi kết thúc game over. 
        // Hàm "new game" sẽ được sử dụng để khởi động lại trò chơi trong khi trò chơi vẫn đang diễn ra.
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            lblScore.Text = "0";
            Label[,] Game = {
                                {lbl1,lbl2,lbl3,lbl4},
                                {lbl5,lbl6,lbl7,lbl8},
                                {lbl9,lbl10,lbl11,lbl12},
                                {lbl13,lbl14,lbl15,lbl16}
                              };
            lblGameOver.Visible = false;
            btnExit.Visible = false;
            replay = true;
            btnNewGame.Visible = false;
            btnNewGame.Enabled = false;
            btnExit.Enabled = false;
            for (int i = 0; i < 4;i++ )
            {
                for (int j = 0; j < 4;j++ )
                {
                    Game[i, j].Text = "";
                }
            }
            randomNumber();
            randomNumber();
            randomNumber();            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lblAbout.Visible = false;
            label2.Visible = true;
            lblScore.Visible = true;

            if(replay==false){
                this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            }
            replay = true;
            lblScore.Text = "0";
            Label[,] Game = {
                                {lbl1,lbl2,lbl3,lbl4},
                                {lbl5,lbl6,lbl7,lbl8},
                                {lbl9,lbl10,lbl11,lbl12},
                                {lbl13,lbl14,lbl15,lbl16}
                              };
            lblGameOver.Visible = false;
            btnExit.Visible = false;
            btnNewGame.Visible = false;
            btnNewGame.Enabled = false;
            btnExit.Enabled = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Game[i, j].Visible = true;
                    Game[i, j].Text = "";
                }
            }
            randomNumber();
            randomNumber();
            randomNumber();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnNewGame_MouseHover(object sender, EventArgs e)
        {
            btnNewGame.BackColor = System.Drawing.Color.Green;
        }

        private void btnNewGame_MouseLeave(object sender, EventArgs e)
        {
            btnNewGame.BackColor = System.Drawing.Color.Orange;
        }

        private void btnExit_MouseHover(object sender, EventArgs e)
        {
            btnExit.BackColor = System.Drawing.Color.Green;
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            btnExit.BackColor = System.Drawing.Color.Orange;
        }

        private void ptbImage_Click(object sender, EventArgs e)
        {

        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lbl3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lblScore_Click(object sender, EventArgs e)
        {

        }

        private void lblGameOver_Click(object sender, EventArgs e)
        {

        }

        private void gamePlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
          MessageBox.Show("This game a single player sliding block puzzle game. Use arrow keys to move the tiles.When two tiles with the same number touch, they merge into one. The game's objective is to slide numbered tiles on a grid to combine them to create a tile with the number 2048. 2048 was originally written in JavaScript and CSS by  Italian web developer Gabriele Cirulli.","How To Play",MessageBoxButtons.OK,MessageBoxIcon.Information);
            
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void lbl2_Click(object sender, EventArgs e)
        {

        }

        private void lbl1_Click(object sender, EventArgs e)
        {

        }
    }
}
