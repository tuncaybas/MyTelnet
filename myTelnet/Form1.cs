using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myTelnet
{

    public partial class myTelnetForm : Form
    {
        public bool baglandi = false;
        TcpClient socket = null;
        StreamWriter output = null;
        StreamReader input = null;
        NetworkStream stream = null;
        Client cliobj = null;
        Thread t;
        islemler islem = null;
        public myTelnetForm()
        {
            InitializeComponent();
            myTelnetForm.CheckForIllegalCrossThreadCalls = false;
            txtMesaj.KeyDown += new KeyEventHandler(KeyDown);
            txtPort.KeyDown += new KeyEventHandler(PortGirildi);
            islem = new islemler();
        }

        private new void PortGirildi(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BaglanBakalim();
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            BaglanBakalim();
        }

        private async void BaglanBakalim()
        {
            if (!String.IsNullOrEmpty(txtServer.Text) && !String.IsNullOrEmpty(txtPort.Text))
            {
                islem.AppendText(txtServer.Text + ":" + txtPort.Text + " -> Connecting. Please wait..." + Environment.NewLine, txtLog, Color.Blue, Color.White);
                Application.DoEvents();
                Thread.Sleep(1000);
                baglandi = await ConnectServer();
                if (baglandi)
                    txtMesaj.Focus();
            }
            else
            {
                MessageBox.Show("You must enter server information.");
                txtServer.Focus();
            }
        }

        private async Task<bool> ConnectServer()
        {
            try
            {
                socket = new TcpClient(txtServer.Text, Int32.Parse(txtPort.Text));
                stream = socket.GetStream();
                output = new StreamWriter(stream);
                input = new StreamReader(stream);

               await Task.Factory.StartNew(delegate
                {
                    cliobj = new Client(input, this);
                    t = new Thread(new ThreadStart(cliobj.Run));
                    t.IsBackground = true;
                    t.Start();

                });


                return true;
            }
            catch (SocketException)
            {
                islem.AppendText("Unknown host - " + txtServer.Text + ". Quitting" + Environment.NewLine, txtLog, Color.Blue, Color.White);
                return false;
            }

            
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            Gonder();
        }

        private void Gonder()
        {
            if (baglandi)
            {
                islem.AppendText(txtMesaj.Text + Environment.NewLine, txtLog, Color.Blue, Color.White);
                output.Write(txtMesaj.Text + "\r\n");
                output.Flush();
            }
            else
            {
                islem.AppendText("No link found" + Environment.NewLine, txtLog, Color.Brown, Color.White);
            }
            txtMesaj.Text = "";
            txtMesaj.Focus();
        }

        private new void KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Gonder();
            }
        }

        public void DisConnect()
        {
            baglandi = false;
        }
    }
}
