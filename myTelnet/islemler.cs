using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace myTelnet
{
  public  class islemler
    {
        public void AppendText(string v, RichTextBox txtLog, Color color, Color backColor)
        {
            txtLog.SelectionStart = txtLog.TextLength;
            txtLog.SelectionLength = 0;
            txtLog.SelectionColor = color;
            txtLog.AppendText(v);
            txtLog.SelectionColor = txtLog.ForeColor;
            txtLog.SelectionBackColor = backColor;
            txtLog.ScrollToCaret();
        }
    }
}
