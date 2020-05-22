using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommonNet;
using System.Threading;


namespace NetWork
{
    class Server
    {
        class ConnectedCLient
        {
            private Server serv;
            public Socket cSocket;
            private NetMessaging net;
            public static List<ConnectedCLient> clients = new List<ConnectedCLient>();
            public string Name { get; private set; }
            public ConnectedCLient(Socket s,Server serv)
            {
                this.serv = serv;
                cSocket = s;
                net = new NetMessaging(cSocket);
                //net.SendData("LOGIN", "?");
                net.LoginCmdReceived += OnLogin;
                net.MessageCmdReceived += OnMessage;
                net.UserListCmdReceived += OnUsers;
                net.PrivateReceived += OnPrivate;
                net.SendInfo += OnLog;
                new Thread(() =>
                {
                    try
                    {
                        net.Communicate();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Не удалось получить данные от клиента :(");
                        clients.Remove(this);
                    }
                }).Start();

            }
            private void OnPrivate(string user,string data)
            {
                foreach(var cl in clients)
                {
                    if (cl.Name == user)
                    {
                        cl.net.SendData("MESSAGE", this.Name + " Шепнул вам"+": " + data);
                    }
                    if(cl == this)
                    {
                        cl.net.SendData("MESSAGE", " Вы шепнули "+user+ ": " + data);
                    }
                }
            }
            private void OnMessage(string command, string data)
            {
                clients.ForEach((client) =>
                {
                    if (client != this)
                        client.net.SendData("MESSAGE", Name + ": " + data);
                    else client.net.SendData("MESSAGE","Вы: " + data);
                });
            }

            private void OnLogin(string command, string data)
            {
                
                foreach( var cl in clients)
                {
                    if (cl.Name == data)
                    {
                        net.SendData("ERROR", "Этот пользователь уже зарегистрирован,введите другой логин");
                        return;
                    }
                }
                Name = data;
                string list = "";
                clients.Add(this);
                clients.ForEach(client =>
                {
                    list += client.Name + ",";
                });
                net.SendData("USERLIST", list);
                net.SendData("START", "!");
            }
            private void OnUsers(string command,string data)
            {
                string list = "";
                clients.ForEach(client =>
                {
                    list += client.Name + ",";
                });
                net.SendData("USERLIST", list);
            }
            private void OnLog(String info)
            {
                serv.EventPrint?.Invoke(info);
            }
        }
        private String host;
        private Socket sSocket;
        private const int port = 8035;
        public delegate void Log(String info);
        public event Log EventPrint;
        
        public Server()
        {
        }
        public void Start()
        {
            EventPrint?.Invoke("Получение локального адреса сервера");
            try
            {
                host = Dns.GetHostName();
                EventPrint?.Invoke($"Имя хоста: {host}");
                sSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                foreach (var addr in Dns.GetHostEntry(host).AddressList)
                {
                    try
                    {
                        sSocket.Bind(new IPEndPoint(addr, port));
                        EventPrint?.Invoke($"Сокет связан с: {addr}:{port}");
                        break;
                    }
                    catch (Exception e)
                    {
                        EventPrint?.Invoke($"Не удалось связать с: {addr}:{port}");
                    }
                }
                sSocket.Listen(10);
                EventPrint?.Invoke("Прослушивание началось...");

                while (true)
                {
                    EventPrint?.Invoke("Ожидание нового подключения...");
                    var cSocket = sSocket.Accept();
                    EventPrint?.Invoke("Соединение с клиентом установлено!");
                    new ConnectedCLient(cSocket,this);
                }

            }
            catch(Exception e)
            {
                EventPrint?.Invoke("Что то пошло не так");
            }
        }
    }
}
