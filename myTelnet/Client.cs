using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myTelnet
{
    public class Client
    {

        StreamReader input = null;
        myTelnetForm anaForm;

        public Client(StreamReader input, myTelnetForm frm)
        {
            this.input = input;
            this.anaForm = frm;
        }

        public void Run()
        {
            String line;
            while ((line = input.ReadLine()) != null)
            {
               new islemler().AppendText(line + Environment.NewLine,anaForm.txtLog,Color.Red,Color.White);
                if (line.StartsWith("221 2.0.0"))
                {
                    new islemler().AppendText("Bağlantı sonlandırıldı." + Environment.NewLine, anaForm.txtLog, Color.Red, Color.White);
                    anaForm.DisConnect();
                    anaForm.txtServer.Text = "";
                    anaForm.txtPort.Text = "";
                }
            }
        }
    }
}
