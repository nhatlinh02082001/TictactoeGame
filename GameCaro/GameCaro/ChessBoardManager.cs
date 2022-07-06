using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public class ChessBoardManager
    {

        #region Properties
        private Panel chessBoard;
        public Panel ChessBoard
        {
            get { return chessBoard; }
            set { chessBoard = value; }
        }

        public List<Player> Player
        {
            get
            {
                return player;
            }

            set
            {
                player = value;
            }
        }

        public int CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }

            set
            {
                currentPlayer = value;
            }
        }

        public TextBox PlayerName
        {
            get
            {
                return playerName;
            }

            set
            {
                playerName = value;
            }
        }

        public PictureBox PlayerMark
        {
            get
            {
                return playerMark;
            }

            set
            {
                playerMark = value;
            }
        }

        public List<List<Button>> Matrix
        {
            get
            {
                return matrix;
            }

            set
            {
                matrix = value;
            }
        }

        private List<Player> player;
        private int currentPlayer;
        private TextBox playerName;
        private PictureBox playerMark;
        private List<List<Button>> matrix;
        private int vertical;
        #endregion

        #region Initialize
        public ChessBoardManager(Panel chessBoard, TextBox playerName, PictureBox mark)
        {
            this.ChessBoard = chessBoard;
            this.PlayerName = playerName;
            this.PlayerMark = mark; 
            this.Player = new List<Player>()
            {
                new Player("Player I", Image.FromFile(Application.StartupPath + "\\Resources\\markX.png")),
                new Player("Player II", Image.FromFile(Application.StartupPath + "\\Resources\\markO.png"))
            };
            
        }
        #endregion

        #region Methods
        public void DrawChessBoard()//Vẽ bàn cờ
        {
            ChessBoard.Controls.Clear();//xóa bàn cờ, chơi lại

            CurrentPlayer = 0;
            ChangePlayer();

            Matrix = new List<List<Button>>();
            Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };
            for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                    Button bt = new Button()
                    {
                        Width = Cons.CHESS_WIDTH,
                        Height = Cons.CHESS_HEIGHT,
                        Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),
                        BackgroundImageLayout=ImageLayout.Stretch,
                        Tag=i.ToString()
                    };
                    bt.Click += Bt_Click;
                    ChessBoard.Controls.Add(bt);

                    Matrix[i].Add(bt);

                    oldButton = bt;
                }
                oldButton.Location = new Point(0, oldButton.Location.Y + Cons.CHESS_HEIGHT);
                oldButton.Width = 0;
                oldButton.Height = 0;
            }
        }
        
        private void Bt_Click(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            if (bt.BackgroundImage != null)
                return;
            Mark(bt);

            ChangePlayer();
            
            if(isEndGame(bt))
            {
                EndGame();
            }           
        }

        private void EndGame()
        {
            MessageBox.Show("Thua rồi, gà vc :))");
        }

        private bool isEndGame(Button bt)
        {
            return isEndHorizontal(bt) || isEndVertical(bt) || isEndVertical(bt) || isEndSub(bt);
        }

        private Point GetChessPoint(Button bt)
        {
            
            int vertical = Convert.ToInt32(bt.Tag);
            int horizontal = Matrix[vertical].IndexOf(bt);
            Point point = new Point(horizontal, vertical);
            return point;
           
        }

        private bool isEndHorizontal(Button bt)//kết thúc ở hàng ngang
        {
            Point point = GetChessPoint(bt);
            int countLeft = 0;
            for (int i = point.X; i >= 0; i--)
            {
                if (Matrix[point.Y][i].BackgroundImage == bt.BackgroundImage)
                {
                     countLeft++;
                }
                else
                    break;
            }
            int countRight = 0;
            
            for (int i = point.X + 1; i < Cons.CHESS_BOARD_WIDTH; i++ )
            {
                if (Matrix[point.Y][i].BackgroundImage == bt.BackgroundImage)
                {
                    countRight++;
                }
                else
                    break;
            }
            return countLeft + countRight == 5;

        }

        private bool isEndVertical(Button bt)//kết thúc ở hàng dọc
        {
            Point point = GetChessPoint(bt);
            int countTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].BackgroundImage == bt.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }
            int countBottom = 0;

            for (int i = point.Y + 1; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                if (Matrix[i][point.X].BackgroundImage == bt.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }
            return countTop + countBottom == 5;
        }
        private bool isEndPrimary(Button bt)//kết thúc ở đường chéo chính
        {
            Point point = GetChessPoint(bt);
            int countTop = 0;
            for (int i = 0 ; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;

                if (Matrix[point.Y - i][point.X - i].BackgroundImage == bt.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }
            int countBottom = 0;

            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.Y + i >= Cons.CHESS_BOARD_WIDTH || point.X + i >= Cons.CHESS_BOARD_HEIGHT)
                    break;
                if (Matrix[point.Y + i][point.X + i].BackgroundImage == bt.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }
            return countTop + countBottom == 5;
        }
        private bool isEndSub(Button bt)//kết thúc ở đường chéo phụ
        {
            Point point = GetChessPoint(bt);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Cons.CHESS_BOARD_WIDTH || point.Y - i < 0)
                    break;

                if (Matrix[point.Y - i][point.X + i].BackgroundImage == bt.BackgroundImage)
                {
                    countTop++;
                }
                else
                    break;
            }
            int countBottom = 0;

            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.Y + i >= Cons.CHESS_BOARD_HEIGHT || point.X - i < 0)
                    break;

                if (Matrix[point.Y + i][point.X - i].BackgroundImage == bt.BackgroundImage)
                {
                    countBottom++;
                }
                else
                    break;
            }
            return countTop + countBottom == 5;
        }
        

        private void Mark(Button bt)
        {
            bt.BackgroundImage = Player[CurrentPlayer].Mark;
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
        }
        private void ChangePlayer()
        {
            PlayerName.Text = Player[CurrentPlayer].Name;
            PlayerMark.Image = Player[CurrentPlayer].Mark;
        }
        #endregion

    }
}
