using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace NetWork
{
    public partial class Form1 : Form
    {
        private Server server;
        private int cnt;
        public Form1()
        {
            InitializeComponent();
        }

        private void panelMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (server == null)
            {
                server = new Server();
                server.EventPrint += OnPrint;
                cnt = 2;
                label2.Text = "Запуск сервера...";
                new Thread(() =>
                {
                    server.Start();
                }).Start();
            }
        }
        private void OnPrint(String info)
        {
            switch (cnt)
            {
                case 0:
                    if (!label1.InvokeRequired)
                    {
                        label1.Text = label2.Text;
                        cnt++;
                        OnPrint(info);
                    }
                    else Invoke(new Server.Log(OnPrint), info);
                    break;
                case 1:
                    if (!label2.InvokeRequired)
                    {
                        label2.Text = label3.Text;
                        cnt++;
                        OnPrint(info);
                    }
                    else Invoke(new Server.Log(OnPrint), info);
                    break;
                case 2:
                    if (!label3.InvokeRequired)
                    {
                        if (label3.Text == "")
                        {
                            label3.Text = info;
                            cnt++;
                        }
                        else
                        {
                            label3.Text = label4.Text;
                            cnt++;
                            OnPrint(info);
                        }
                    }
                    else Invoke(new Server.Log(OnPrint), info);
                    break;
                case 3:
                    if (!label4.InvokeRequired)
                    {
                        if (label4.Text == "")
                        {
                            label4.Text = info;
                            cnt++;
                        }
                        else
                        {
                            label4.Text = label5.Text;
                            cnt++;
                            OnPrint(info);
                        }
                    }
                    else Invoke(new Server.Log(OnPrint), info);
                    break;
                case 4:
                    if (!label5.InvokeRequired)
                    {
                        label5.Text = info;
                        cnt = 0;
                    }
                    else Invoke(new Server.Log(OnPrint), info);
                    break;
            }
        }
    }
}
