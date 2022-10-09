using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Forms.Integration;
using System.IO;
using System.Net.Sockets;

namespace helixxdeneme
{
    public partial class Form1 : Form
    {

        OpenFileDialog file;
        string videoPath;
        string videoName;
        byte[] bytes;

        string temp = Environment.CurrentDirectory;

        TcpClient tcpClient = new TcpClient();
        NetworkStream networkStream = null;
        public StreamWriter clientData;
        private StreamWriter streamWriter;
        private StreamReader streamReader;

        public NetworkStream NetworkStream { get => networkStream; set => networkStream = value; }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            file = ofd;
            if (file.ShowDialog() == DialogResult.OK)
            {
                videoPath = file.FileName;
                videoName = file.SafeFileName;
                MessageBox.Show("Gönderilmek istenen dosya:" + videoPath, "", MessageBoxButtons.OKCancel);
                bytes = File.ReadAllBytes(videoPath);
              /*  String writed = "";
                for (long i = 0; i < bytes.Length; i++)
                {
                    writed += bytes[i] + "\n"; 
                }*/
                textBox.Text = bytes.Length.ToString();

               



            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = temp + @"\Saves\" + "video" + DateTime.Now.ToString("dd.MM.yyyy") + ".avi";

            
            using (FileStream fs = File.Create("video" + DateTime.Now.ToString("dd.MM.yyyy") + ".avi"))
            {
                textBox.Text = videoName;
                // Add some information to the file.
                fs.Write(bytes, 0, bytes.Length);
            }

            // Open the stream and read it back.
            /*using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }*/
        }

        public void CreateConnection()
        {
            tcpClient.Connect("192.168.4.1", 80);
            NetworkStream = tcpClient.GetStream();
            streamWriter = new StreamWriter(NetworkStream);
            if (tcpClient.Connected)
            {
                textBox1.Text = "BAĞLANDI";
            }

        }
        public void Disconnect()
        {
            tcpClient.Dispose();
            tcpClient.Close();
            NetworkStream.Close();
            if (!tcpClient.Connected)
            {
                textBox1.Text = "BAĞLANTI KESİLDİ";
            }

        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            CreateConnection();
        }

        private void sendData_Click(object sender, EventArgs e)
        {
            SendData();
        }

        public void SendData()
        {
            if (NetworkStream.CanWrite)
            {
                NetworkStream.Write(bytes, 0, bytes.Length);
                textBox1.Text = "GÖNDERİLİYOR";
                /*byte[] senddata = {-128};
                NetworkStream.Write([-128],0,1);*/
                NetworkStream.Flush();
               
            }
            else if (!NetworkStream.CanWrite)
            {
                textBox1.Text="You can not read data from this stream";
                
            }
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Disconnect();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NetworkStream = tcpClient.GetStream();
            streamReader = new StreamReader(NetworkStream);
            int komut = -1;
            while ((komut = streamReader.Read()) != -1) 
            {
                char c = (char)komut;
                if (c == 'C')
                {
                    textBox2.Text = "GÖNDERİLDİ";
                    break;
                }

            }
        }
    }
}
