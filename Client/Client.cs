using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommonNet;
using System.Threading;


namespace Client
{
    class Client
    {
        private String serverHost;
        private Socket cSocket;
        private int port = 8035;
        private NetMessaging net;

        public delegate void Log(String info);
        public event Log EventInfo;
        public event Log EventChat;
        public event Log EventUsers;
        public event Log EventError;
        public Client()
        {

        }
        public void GetHost(String serverHost)
        {
            try
            {
                this.serverHost = serverHost;
                //Console.WriteLine("Подключение к {0}", this.serverHost);
                EventInfo?.Invoke($"Подключение к {this.serverHost}");
                cSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                cSocket.Connect(this.serverHost, port);
                net = new NetMessaging(cSocket);
                //net.LoginCmdReceived += OnLogin;
                net.UserListCmdReceived += OnUserList;
                net.StartCmdReceived += OnStart;
                net.MessageCmdReceived += OnMessage;
                net.ErrorReceived += OnError;

                new Thread(() =>
                {
                    try
                    {
                        net.Communicate();
                    }
                    catch (Exception ex)
                    {
                        EventInfo?.Invoke("Не удалось получить данные. Завершение соединения...");
                        //Console.WriteLine("Не удалось получить данные. Завершение соединения...");
                    }
                }).Start();
                new Thread(() =>
                {
                    UpdateUserList();
                }).Start();

            }
            catch(Exception e)
            {
                EventInfo?.Invoke("Что-то пошло не так... :(");
            }
        }
        private void OnError(string command,string data)
        {
            EventError?.Invoke(data);
        }
        private void OnStart(string command, string data)
        {
            EventInfo?.Invoke("Вы можете писать сообщения!");
            EventInfo?.Invoke("Чтобы написать кому то в личку,используйте:");
            EventInfo?.Invoke("имяклиента@сообщение ");
        }
        private void UpdateUserList()//обновление списка клиентов у каждого клиента(поставить условие на while,для закрытия)
        {
            while (true)
            {
                net.SendData("USERLIST", "1111");
                Thread.Sleep(8000);
            }
        }

        private void OnUserList(string command, string data)
        {
            EventUsers?.Invoke(data);
        }

        private void OnMessage(string command, string data)
        {
            EventChat?.Invoke(data);
        }
        public void GoMessage(String userdata)
        {
            var us = userdata.Split(',');
            
            var pm = us[1].Split('@');
            if (pm[0] != us[1] && us[0]!="LOGIN")
            {
                if(pm[1] != "")
                {
                    net.SendData("PRIVATEMESSAGE", us[1]);
                    return;
                }  
            }
            
            if (us[1] != "")
            {
                switch (us[0])
                {
                    case "LOGIN":
                        net.SendData("LOGIN", us[1]);
                        break;
                    case "USERLIST":
                        net.SendData("USERLIST", "1111");
                        break;
                    case "MESSAGE":
                        net.SendData("MESSAGE", us[1]);
                        break;
                }
            }
        }
    }
}
