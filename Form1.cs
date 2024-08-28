using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private Button[,] buttons; // Düðmeleri tutan 2D dizi
        private int gridSize = 5; // 5x5'lik bir ýzgara
        private Button emptyButton; // Boþ alaný temsil eden düðme

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Baþlangýçta pencere boyutunu belirleyin
            this.ClientSize = new Size(600, 400); // Geniþlik 600, yükseklik 400

            Load += Form1_Load; // Form yüklendiðinde çalýþacak olayý baðla
            this.Resize += Form1_Resize; // Pencere boyutlandýðýnda çalýþacak olayý baðla
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buttons = new Button[gridSize, gridSize];
            int[] numbers = Enumerable.Range(1, 24).ToArray();
            Shuffle(numbers); // Düðme numaralarýný karýþtýr

            // Initialize and arrange buttons
            InitializeButtons(numbers);
            ArrangeButtons();
        }

        private void InitializeButtons(int[] numbers)
        {
            int numberIndex = 0;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Button btn = new Button();
                    btn.Size = new Size(this.ClientSize.Width / gridSize, this.ClientSize.Height / gridSize);
                    btn.Location = new Point(j * btn.Width, i * btn.Height);

                    // Add button text except for the last button (empty button)
                    if (numberIndex < numbers.Length)
                    {
                        btn.Text = numbers[numberIndex].ToString();
                        numberIndex++;
                    }
                    else
                    {
                        btn.Text = ""; // The last button is the empty one
                        emptyButton = btn; // Save the empty button reference
                    }

                    btn.Click += Button_Click; // Add click event handler

                    // Add button to the form
                    this.Controls.Add(btn);
                    buttons[i, j] = btn;
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Pencere boyutlandýðýnda düðmeleri yeniden düzenleyin
            ArrangeButtons();
        }

        private void ArrangeButtons()
        {
            // Düðme boyutlarýný ve konumlarýný pencere boyutuna göre ayarlayýn
            int buttonWidth = this.ClientSize.Width / gridSize;
            int buttonHeight = this.ClientSize.Height / gridSize;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    buttons[i, j].Size = new Size(buttonWidth, buttonHeight);
                    buttons[i, j].Location = new Point(j * buttonWidth, i * buttonHeight);
                }
            }
        }

        private void Shuffle(int[] array)
        {
            Random rand = new Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            // Týklanan düðme boþ alanýn dört komþusundan biri mi veya çaprazýnda mý?
            if (IsAdjacentOrDiagonal(btn))
            {
                // Düðmenin text deðerini boþ düðmeye atayalým, boþ düðmenin text deðerini boþ yapalým
                emptyButton.Text = btn.Text;
                btn.Text = "";

                // Yeni boþ alaný belirleyelim
                emptyButton = btn;

                // Oyunun bitip bitmediðini kontrol et
                CheckIfSolved();
            }
        }

        private bool IsAdjacentOrDiagonal(Button btn)
        {
            // Týklanan düðmenin ve boþ düðmenin koordinatlarýný alalým
            int btnRow = btn.Location.Y / (this.ClientSize.Height / gridSize);
            int btnCol = btn.Location.X / (this.ClientSize.Width / gridSize);
            int emptyRow = emptyButton.Location.Y / (this.ClientSize.Height / gridSize);
            int emptyCol = emptyButton.Location.X / (this.ClientSize.Width / gridSize);

            // Týklanan düðmenin boþ düðmenin üstünde, altýnda, solunda, saðýnda veya çaprazýnda olup olmadýðýný kontrol et
            bool isAbove = (btnRow == emptyRow - 1) && (btnCol == emptyCol);
            bool isBelow = (btnRow == emptyRow + 1) && (btnCol == emptyCol);
            bool isLeft = (btnRow == emptyRow) && (btnCol == emptyCol - 1);
            bool isRight = (btnRow == emptyRow) && (btnCol == emptyCol + 1);

            // Çaprazlar için kontrol
            bool isTopLeftDiagonal = (btnRow == emptyRow - 1) && (btnCol == emptyCol - 1);
            bool isTopRightDiagonal = (btnRow == emptyRow - 1) && (btnCol == emptyCol + 1);
            bool isBottomLeftDiagonal = (btnRow == emptyRow + 1) && (btnCol == emptyCol - 1);
            bool isBottomRightDiagonal = (btnRow == emptyRow + 1) && (btnCol == emptyCol + 1);

            return isAbove || isBelow || isLeft || isRight || isTopLeftDiagonal || isTopRightDiagonal || isBottomLeftDiagonal || isBottomRightDiagonal;
        }

        private void CheckIfSolved()
        {
            int k = 1;
            bool solved = true;

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (buttons[i, j] != emptyButton && buttons[i, j].Text != k.ToString())
                    {
                        solved = false;
                        break;
                    }
                    k++;
                }
            }

            if (solved)
            {
                MessageBox.Show("Tebrikler! Oyunu kazandýnýz!");
            }
        }
    }
}
