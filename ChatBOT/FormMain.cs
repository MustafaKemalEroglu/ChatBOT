using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatBOT
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }
        private void MessageSound(string SoundType)
        {
            if (SoundType == "Send")
            {
                SoundPlayer Send = new SoundPlayer("SOUND1.wav");  // Send Sound Effect 
                Send.Play();
            }
            if (SoundType == "Recieve")
            {
                SoundPlayer Recieve = new SoundPlayer("SOUND2.wav"); // Recieve Sound Effect
                Recieve.Play();
            }
        }
        public void ChatLog(string Command, string message)   
        {
            if (Command == "Clear")     //ChatLog Temizleme..
            {
                File.Delete(@"chat.log");
                listBox1.Items.Clear();
            }

            else if (Command == "Append")
            {
                //=========== Creates backup of chat from user and bot to the given location ============
                FileStream fs = new FileStream(@"chat.log", FileMode.Append, FileAccess.Write);
                if (fs.CanWrite)
                {
                    byte[] write = System.Text.Encoding.Default.GetBytes(message + Environment.NewLine);
                    fs.Write(write, 0, write.Length);
                }
                fs.Flush();
                fs.Close();
            }
            else if (Command == "Load")
            {
                if (File.Exists("chat.log"))
                {
         //           using (StreamReader sr = File.OpenText("chat.log"))
                    using (StreamReader sr = new StreamReader("chat.log", Encoding.Default))

                    {
                        int i = 0; // to count lines
                        while (sr.Peek() >= 0) // loop till the file ends
                        {
                            if (i % 2 == 0) // check if line is even
                            {
                                listBox1.Items.Add("You: " + sr.ReadLine());
                            }
                            else
                            {
                                listBox1.Items.Add("Machine: " + sr.ReadLine());
                            }
                            i++;
                        }
                    }
                }
            }
            else { MessageBox.Show("ChatLog fonksiyon komutlarında hata var!", "Hata!"); }
        }

        private void SendMessage(string message)    //Diyaloglar Send butonuyla veya Enter ile gönderildiğinde işlenecek komutlar.
        {
            if (textBox1.Text != "")
            {
                listBox1.Items.Add("You: " + message);
                textBox1.Text = "";
                ChatLog("Append", message);
                MessageSound("Send");
                CreateDialogueFunction(message);
            }
        }
        private void CreateDialogueFunction(string message) //Bizim yazdığımıza mesajımıza uygun diyalog geliştiren fonksiyon.
        {
            
        //    if (message == "Hi") { AnswerMessageFunction("Hi, How are You?"); }
            if (string.Compare(message, "Merhaba", true) == 0) { AnswerMessageFunction("Merhaba, Nasılsın?"); }
            else { AnswerMessageFunction("Daha önce tanımlanmayan bir diyalog."); }
        }



        private void AnswerMessageFunction(string AnswerMessage)    //Geliştirilen diyaloğun ekrana yazılması.
        {
            var DelayTimer = new Timer();
            DelayTimer.Interval = 1000 + (AnswerMessage.Length * 100);
            DelayTimer.Tick += (s, d) =>
            {
                listBox1.Items.Add("Machine: " + AnswerMessage);
                DelayTimer.Stop();
                LabelWriting.Text = "";
                ChatLog("Append", AnswerMessage);
                MessageSound("Recieve");
            };
            LabelWriting.Text = "Makine size cevap yazıyor, lütfen bekleyiniz.";
            LabelWriting.Enabled = true;
            DelayTimer.Start();
        }
         

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SendMessage(textBox1.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage(textBox1.Text);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LabelWriting.Text = "";
            ChatLog("Load", "");
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage(textBox1.Text);
                e.SuppressKeyPress = true; // Disable windows error sound
            }
        }

        private void ınformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAboutBox FormAboutBox = new FormAboutBox();
            FormAboutBox.Show();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings FormSettings = new FormSettings();
            FormSettings.Show();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChatLog("Clear", "");
        }
    }
}
