using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
namespace lb1TA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _Form1 = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            StringReader reader = new StringReader(textBox1.Text);
            string line;
            try
            {
                while ((line = reader.ReadLine()) != null)
                {
                    Handler.CAS(line, richTextBox2);
                }
                richTextBox2.Text += "===============================" + Environment.NewLine + "Лексический анализ завершен" + Environment.NewLine;
                MessageBox.Show("Лексический анализ завершен");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error!\n {ex.Message}");
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            string[] lines = File.ReadAllLines(textBox3.Text);
            foreach (string s in lines)
            {
                textBox1.Text += s;
                textBox1.Text += Environment.NewLine;
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox2.Clear();
            StringReader reader = new StringReader(textBox1.Text);
            string line;
            string ss = " ";
            while ((line = reader.ReadLine()) != null)
            {
                ss += line + " ";
            }
            List<Token> lex;
            try
            {
                lex = TokenH.Tokenhnd(ss, richTextBox2);
                LR rule = new LR(lex);
                rule.Start();
                richTextBox2.Text += "======================" + Environment.NewLine + "Классификация лексем завершена" + Environment.NewLine;
                MessageBox.Show("Разбор завершён");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error!\n {ex.Message}");
            } //rule.Programm();

        }
        public static Form1 _Form1;
        public void Printt(string print)
        {
            richTextBox1.Text += print + Environment.NewLine;
        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            StringReader reader = new StringReader(textBox1.Text);
            string line;
            try { 
            while ((line = reader.ReadLine()) != null)
            {
                TokenH.Tokenhnd(line, richTextBox2);
            }
            richTextBox2.Text += "===============================" + Environment.NewLine + "Классификация лексем завершена" + Environment.NewLine;
            MessageBox.Show("Классификация лексем завершена");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error!\n {ex.Message}");
            }
        }
    }
}
