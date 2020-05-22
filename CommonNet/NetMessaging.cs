using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace CommonNet
{
    public delegate void Receiving(String command, String data);
    public delegate void FeedBack(String data);
    public class NetMessaging
    {
        private Socket cSocket;
        public event Receiving LoginCmdReceived;
        public event Receiving MessageCmdReceived;
        public event Receiving UserListCmdReceived;
        public event Receiving StartCmdReceived;
        public event Receiving ErrorReceived;
        public event Receiving PrivateReceived;
        public event FeedBack SendInfo;
        public NetMessaging(Socket s)
        {
            cSocket = s;
        }
        public void SendData(String command, String data)
        {
            if (cSocket != null)
            {
                try
                {
                    if (data.Trim().Equals("") ||
                        command.Trim().Equals("")) return;
                    var b = Encoding.UTF8.GetBytes(command + "=" + data + "\n");
                    //Console.WriteLine("Отправка сообщения...");
                    SendInfo?.Invoke("Отправка сообщения...");
                    cSocket.Send(b);
                    //Console.WriteLine("Сообщение успешно отправлено!");
                }
                catch (Exception ex)
                {
                    //Console.WriteLine("Не удалось отправить сообщение :(");
                    SendInfo?.Invoke("Не удалось отправить сообщение :(");
                }
            }
        }

        public String ReceiveData()
        {
            String res = "";
            if (cSocket != null)
            {
                var b = new byte[65536];
                //Console.WriteLine("Ожидание данных...");
                SendInfo?.Invoke("Ожидание данных...");
                var i = 0;
                do
                {
                    var cnt = cSocket.Receive(b);
                    //Console.WriteLine("Получена порция данных №{0}", ++i);
                    var r = Encoding.UTF8.GetString(b, 0, cnt);
                    res += r;
                } while (res[res.Length - 1] != '\n');
                //Console.WriteLine("Данные успешно получены");
                SendInfo?.Invoke("Данные успешно получены");
            }
            return res.Trim();
        }

        public void Communicate()
        {
            if (cSocket != null)
            {
                //Console.WriteLine("Начало общения...");
                SendInfo?.Invoke("Начало общения...");
                while (true)
                {
                    String d = ReceiveData();
                    Parse(d);
                }
            }
        }

        private void Parse(string s)
        {
            // КОМАНДА=ЗНАЧЕНИЕ (LOGIN=Иван)
            char[] sep = { '=' };
            var cd = s.Split(sep, 2);
            switch (cd[0])
            {
                case "LOGIN":
                    {
                        LoginCmdReceived?.Invoke(cd[0], cd[1]);
                        //UserListCmdReceived?.Invoke(cd[0], cd[1]);
                        break;
                    }
                case "MESSAGE":
                    {
                        MessageCmdReceived?.Invoke(cd[0], cd[1]);
                        break;
                    }
                case "USERLIST":
                    {
                        UserListCmdReceived?.Invoke(cd[0], cd[1]);
                        break;
                    }
                case "START":
                    {
                        StartCmdReceived?.Invoke(cd[0], cd[1]);
                        break;
                    }
                case "ERROR":
                    {
                        ErrorReceived?.Invoke(cd[0], cd[1]);
                        break;
                    }
                case "PRIVATEMESSAGE":
                    {
                        var pm = cd[1].Split('@');
                        PrivateReceived?.Invoke(pm[0], pm[1]);
                        break;
                    }
                    
            }
        }
    }
}
