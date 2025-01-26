using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class GamePage : Form
    {
        private List<Question> questions;
        private int currentQuestionIndex = 0;
        private string correctAnswer;
        private int correct = 0;  //
        private int wrong = 0;
        private System.Windows.Forms.Timer gameTimer;
        private int remainingTime = 60;

        public GamePage()
        {
            InitializeComponent();
            LoadQuestions();
            ShowQuestion();
            gameTimer = new System.Windows.Forms.Timer(); // Timer nesnesini başlatma
            gameTimer.Interval = 1000; // 1 saniye aralıklarla
            gameTimer.Tick += GameTimer_Tick; // Timer her tick olduğunda bu metodu çalıştıracak
            gameTimer.Start(); // Timer'ı başlatıyoruz

        }
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            remainingTime--; // Zamanı bir saniye azaltıyoruz
            label2.Text = "Kalan Süre: " + remainingTime + " saniye"; // Kalan zamanı etiketin üzerine yazıyoruz

            if (remainingTime <= 0)
            {
                gameTimer.Stop(); // Zaman bittiğinde timer'ı durduruyoruz
                Final page= new Final(correct,wrong);
                page.Show();
                this.Hide();
            }
        }
        private void GamePage_Load(object sender, EventArgs e)
        {

        }
        public class Question
        {
            public string Soru { get; set; }
            public string Şık1 { get; set; }
            public string Şık2 { get; set; }
            public string Şık3 { get; set; }
            public string Şık4 { get; set; }
            public string DoğruŞık { get; set; }
        }


        private void LoadQuestions() // burda veriyi çekiyoruz sadece işleme sokmadık
        {
            questions = new List<Question>();
            string connectionString = "Data Source=DESKTOP-95JJ5FK\\SQLEXPRESS;Initial Catalog=game;Integrated Security=True;TrustServerCertificate=True";

            string query = "SELECT Soru, Şık1, Şık2, Şık3, Şık4, Doğru_Şık FROM Game";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        questions.Add(new Question
                        {
                            Soru = reader["Soru"].ToString(),
                            Şık1 = reader["Şık1"].ToString(),
                            Şık2 = reader["Şık2"].ToString(),
                            Şık3 = reader["Şık3"].ToString(),
                            Şık4 = reader["Şık4"].ToString(),
                            DoğruŞık = reader["Doğru_Şık"].ToString()
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }
        private void ShowQuestion() // burda vriyi artık ekranda işlene sokuyorum
        {
            if (currentQuestionIndex >= questions.Count)
            {
                MessageBox.Show("Tüm soruları tamamladınız!");
                return;
            }

            // Şu anki soruyu al
            var currentQuestion = questions[currentQuestionIndex];

            // Soruyu ve şıkları ata
            label1.Text = currentQuestion.Soru;
            button1.Text = currentQuestion.Şık1;
            button2.Text = currentQuestion.Şık2;
            button3.Text = currentQuestion.Şık3;
            button4.Text = currentQuestion.Şık4;
            // Doğru şık bilgisini sakla
            correctAnswer = currentQuestion.DoğruŞık;

            // Renkleri sıfırla
            ResetButtonColors();
        }
        private void CheckAnswer(Button selectedButton)
        {
            if (selectedButton.Text == correctAnswer)
            {
                selectedButton.BackColor = Color.Green;
                correct++;

            }
            else
            {
                selectedButton.BackColor = Color.Red;
                wrong++;

            }
            DisableAnswerButtons();

            // Sonraki soruya geçiş için "Next" butonunu aktif yap
            button5.Enabled = true;
        }
        private void DisableAnswerButtons()
        {
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
        }

        private void ResetButtonColors()
        {
            button1.BackColor = SystemColors.Control;
            button2.BackColor = SystemColors.Control;
            button3.BackColor = SystemColors.Control;
            button4.BackColor = SystemColors.Control;

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;

            button5.Enabled = false; // "Next" butonunu devre dışı bırak
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckAnswer(button1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CheckAnswer(button2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CheckAnswer(button3);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            CheckAnswer(button4);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            currentQuestionIndex++;
            ShowQuestion();
        }
    }
}
