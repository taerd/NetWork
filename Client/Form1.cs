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

namespace Client
{
    public partial class Form1 : Form
    {
        private Client client;
        private String userdata;
        private String sHost= "taerd-laptop";
        private int cnt;
        private int infocnt;
        private int chatcnt;
        public Form1()
        {
            InitializeComponent();
            cnt = 0;
            infocnt = 1;
            chatcnt = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            userdata = textBox1.Text;
            switch (cnt)
            {
                case 0://Ввод адреса сервера
                    if (userdata.Length > 0) sHost = userdata;
                    client = new Client();
                    //подписка на события
                    client.EventInfo += OnInfo;
                    client.EventChat += OnChat;
                    client.EventUsers += OnUsers;
                    client.EventError += OnError;
                    client.GetHost(sHost);
                    cnt++;
                    textBox1.Text = "";
                    OnInfo("Введите логин");
                    break;
                case 1://Ввод логина
                    client.GoMessage("LOGIN,"+userdata);
                    textBox1.Text = "";
                    cnt++;
                    break;
                case 2://Общение
                    client.GoMessage("MESSAGE,"+userdata);
                    textBox1.Text = "";
                    break;
            }
        }
        private void OnError(string data)
        {
            OnInfo(data);
            cnt--;
        }
        private void OnUsers(string users)
        {
            if (!labelUsers.InvokeRequired)
            {
                var us = users.Split(',');
                labelUsers.Text = "Сейчас в сети: " + (us.Length-1).ToString();
                foreach( var user in us)
                {
                    labelUsers.Text += "\n" + user;
                }
            }
            else Invoke(new Client.Log(OnUsers), users);
        }
        private void OnInfo(String info)
        {
            switch (infocnt)
            {
                case 0:
                    if (!labelConsole1.InvokeRequired)
                    {
                        labelConsole1.Text = labelConsole2.Text;
                        infocnt++;
                        OnInfo(info);
                    }
                    else Invoke(new Client.Log(OnInfo), info);
                    break;
                case 1:
                    if (!labelConsole2.InvokeRequired)
                    {
                        if (labelConsole2.Text == "")
                        {
                            labelConsole2.Text = info;
                            infocnt++;
                        }
                        else
                        {
                            labelConsole2.Text = labelConsole3.Text;
                            infocnt++;
                            OnInfo(info);
                        }
                    }
                    else Invoke(new Client.Log(OnInfo), info);
                    break;
                case 2:
                    if (!labelConsole3.InvokeRequired)
                    {
                        labelConsole3.Text = info;
                        infocnt = 0;
                    }
                    else Invoke(new Client.Log(OnInfo), info);
                    break;
            }
        }
        private void OnChat(String data)
        {
            switch (chatcnt)
            {
                case 0:
                    if (!labelChat1.InvokeRequired)
                    {
                        if (labelChat1.Text == "")
                        {
                            labelChat1.Text = data;
                            chatcnt++;
                        }
                        else
                        {
                            labelChat1.Text = labelChat2.Text;
                            chatcnt++;
                            OnChat(data);
                        }
                    }
                    else Invoke(new Client.Log(OnChat), data);
                    break;
                case 1:
                    if (!labelChat2.InvokeRequired)
                    {
                        if (labelChat2.Text == "")
                        {
                            labelChat2.Text = data;
                            chatcnt++;
                        }
                        else
                        {
                            labelChat2.Text = labelChat3.Text;
                            chatcnt++;
                            OnChat(data);
                        }
                    }
                    else Invoke(new Client.Log(OnChat), data);
                    break;
                case 2:
                    if (!labelChat3.InvokeRequired)
                    {
                        if (labelChat3.Text == "")
                        {
                            labelChat3.Text = data;
                            chatcnt++;
                        }
                        else
                        {
                            labelChat3.Text = labelChat4.Text;
                            chatcnt++;
                            OnChat(data);
                        }
                    }
                    else Invoke(new Client.Log(OnChat), data);
                    break;
                case 3:
                    if (!labelChat4.InvokeRequired)
                    {
                        if (labelChat4.Text == "")
                        {
                            labelChat4.Text = data;
                            chatcnt++;
                        }
                        else
                        {
                            labelChat4.Text = labelChat5.Text;
                            chatcnt++;
                            OnChat(data);
                        }
                    }
                    else Invoke(new Client.Log(OnChat), data);
                    break;
                case 4:
                    if (!labelChat5.InvokeRequired)
                    {
                        if (labelChat5.Text == "")
                        {
                            labelChat5.Text = data;
                            chatcnt++;
                        }
                        else
                        {
                            labelChat5.Text = labelChat6.Text;
                            chatcnt++;
                            OnChat(data);
                        }
                    }
                    else Invoke(new Client.Log(OnChat), data);
                    break;
                case 5:
                    if (!labelChat6.InvokeRequired)
                    {
                        if (labelChat6.Text == "")
                        {
                            labelChat6.Text = data;
                            chatcnt++;
                        }
                        else
                        {
                            labelChat6.Text = labelChat7.Text;
                            chatcnt++;
                            OnChat(data);
                        }
                    }
                    else Invoke(new Client.Log(OnChat), data);
                    break;
                case 6:
                    if (!labelChat7.InvokeRequired)
                    {
                        if (labelChat7.Text == "")
                        {
                            labelChat7.Text = data;
                            chatcnt++;
                        }
                        else
                        {
                            labelChat7.Text = labelChat8.Text;
                            chatcnt++;
                            OnChat(data);
                        }
                    }
                    else Invoke(new Client.Log(OnChat), data);
                    break;
                case 7:
                    if (!labelChat8.InvokeRequired)
                    {
                        labelChat8.Text = data;
                        chatcnt = 0;
                    }
                    else Invoke(new Client.Log(OnChat), data);
                    break;
            }
        }
    }
}
