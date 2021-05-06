using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Checker
{
    
    public partial class Form1 : Form
    {
        public string path;
        byte number_of_questions;
        Random rand = new Random();
        int position;
        int rightAnswers;

        private struct Data
        {
            public string question;
            public string answer;

            public int rand_weight;
        }

        List<Data> list;
        

        public Form1()
        {
            InitializeComponent();
            list = new List<Data>();
            rightAnswers = 0;
            number_of_questions = 0;
            position = 0;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                PathtextBox.Text = path;

            }

            StartCheckButton.Enabled = false;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            number_of_questions = 0;

            if ((!String.IsNullOrEmpty(QuesNumComboBox.Text))&&(Byte.TryParse(QuesNumComboBox.Text, out number_of_questions)))
            {
                if (!(number_of_questions ==0))
                {
                    if (!String.IsNullOrEmpty(path))
                    {
                        
                        StartCheck();
                    }
                    else
                    {
                        MessageBox.Show("Некорректно указан путь к файлу.");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Некорректно указано количество вопросов.");
                }
                

            }

            else
            {
                MessageBox.Show("Некорректно указано количество вопросов.");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void StartCheck()
        {

            MessageBox.Show("Пошла проверка. Количество вопросов " + number_of_questions);

            string all = "";
            position = 0;

            using (var sr = new StreamReader(path))
            {
                all = sr.ReadToEnd();



            }


            string[] stringArr = all.Split('\n');

            Data[] data = new Data[(int)stringArr.Length / 2];

            for (int i = 0; i < data.Length; i++)
            {
                data[i].question = stringArr[2 * i];
                data[i].answer = stringArr[2 * i + 1];
                data[i].rand_weight = rand.Next(1, 100);
            }

            WorkButton.Visible = true;
            WorkButton.Enabled = true;
            RightAnswerButton.Visible = true;
            RightAnswerButton.Enabled = false;
            WrongAnswerButton.Visible = true;
            WrongAnswerButton.Enabled = false;


            Data[] datasort = new Data[data.Length];
            int t = 0;
            for (int i = 1; i < 100; i++)
            {
                for (int j = 0; j < data.Length; j++)
                {
                    if (data[j].rand_weight == i)
                    {
                        if (t==datasort.Length)
                        {
                            MessageBox.Show("Ошибка перемешивания.");
                            break;
                        }
                        datasort[t] = data[j];


                        int questionSize = (data[j].question.Length - (data[j].question.Length % 60)) / 60;
                        int answernSize = (data[j].answer.Length - (data[j].answer.Length % 60)) / 60;

                        if (questionSize>0)
                        {
                            for (int que = 1; que <= questionSize; que++)
                            {
                                data[j].question = data[j].question.Insert(60*que-1, "\n");
                            }
                        }


                        if (answernSize > 0)
                        {
                            for (int que = 1; que <= answernSize; que++)
                            {
                                data[j].answer = data[j].answer.Insert(60 * que - 1, "\n");
                            }
                        }


                        list.Add(data[j]);
                        t++;
                    }
                }
            }



            QuestionLabel.Text = list[position].question;

            




        }

        private void FormatText(string all)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void WorkButton_Click(object sender, EventArgs e)
        {

            SetState(false);

            
        }

        private void RightAnswerButton_Click(object sender, EventArgs e)
        {
            rightAnswers++;
            SetState(true);
        }

        void SetState(bool question)
        {
            
            AnswerLabel.AutoEllipsis = true;
            RightAnswerButton.Enabled = !question;
            WrongAnswerButton.Enabled = !question;
            WorkButton.Enabled = question;
            if (question)
            {
                position++;
                if (position< number_of_questions && position < list.Count)
                {
                    QuestionLabel.Text = list[position].question;
                    AnswerLabel.Text = "";
                }
                else
                {
                    RightAnswerButton.Enabled = false;
                    WrongAnswerButton.Enabled = false;
                    WorkButton.Enabled = false;
                    MessageBox.Show("Завершено. Процент правильных ответов составляет " + (100*rightAnswers)/position + "%");
                    StartCheckButton.Enabled = true;
                    AnswerLabel.Text = "";
                    QuestionLabel.Text = "";
                    number_of_questions = 0;
                    position = 0;
                    rightAnswers = 0;
                }
            }
            else
            {
                AnswerLabel.Text = list[position].answer;
            }

        }

        private void WrongAnswerButton_Click(object sender, EventArgs e)
        {
            SetState(true);
        }

        private void QuestionLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
