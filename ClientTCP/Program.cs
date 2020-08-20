using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientTCP_UDP
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Tcp
            /*
            //соединение ip и порт
            const string ip = "127.0.0.1";
            const int port = 8080;

            //точка подключения
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //AddressFamily.InterNetwork - использование ip-адреса 4 версии
            //SocketType.Stream - потоковое передача данных для протокола TCP
            //ProtocolType.Tcp - указывает на TCP-протокол

            Console.WriteLine("Введите сообщение:");

            //создадим то что будем отправлять
            var message = Console.ReadLine();

            //кодируем сообщение
            var data = Encoding.UTF8.GetBytes(message);

            //делаем подключение для этого сокета
            tcpSocket.Connect(tcpEndPoint); // подключаемя уже как клиент

            //отправляем данные
            tcpSocket.Send(data);

            // буфер - место куда будем принимать данные
            var buffer = new byte[256];

            // переменная для реального количества байтов
            var size = 0;

            // полученные данные собирает
            var answer = new StringBuilder(); //ответ сервера

            //получение сообщения / проверяем условие на получение запроса
            do
            {
                size = tcpSocket.Receive(buffer);// получаем 256 байт из сообщения  - количество реально полученных байт их этого сообщения
                                                //читаем / раскадировать полученные байты в куски сообщений
                answer.Append(Encoding.UTF8.GetString(buffer, 0, size));
                //buffer - наш буфер
                //0 - откуда начинаем
                //size - наше количество
            }
            while (tcpSocket.Available > 0);

            Console.WriteLine(answer.ToString());

            tcpSocket.Shutdown(SocketShutdown.Both);
            tcpSocket.Close();

            Console.ReadKey();
            //Мы клиентом отправляем сообщение по Tcp протоколам. Сообщение кодирует и отправляется серверу
            //Сервер читает это сообщение кодированное и собирает в нормальное сообщение которое отправил клиент
            //Если все хорошо сработало - сервер отправляет сообщение что все хорошо "Успех" - крутая штука
            */
            #endregion

            #region Udp
            //соединение ip и порт
            const string ip = "127.0.0.1";
            const int port = 8082; //просто меняем порт

            //точка подключения
            var udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            var udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //AddressFamily.InterNetwork - использование ip-адреса 4 версии
            //SocketType.Dgram - потоковое передача данных для протокола UDP
            //ProtocolType.Udp - указывает на Udp-протокол

            //подключение
            udpSocket.Bind(udpEndPoint);

            while (true)
            {
                Console.WriteLine("Введи сообщение, эй");
                var message = Console.ReadLine();

                var serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081);
                udpSocket.SendTo(Encoding.UTF8.GetBytes(message), serverEndPoint);

                // буфер - место куда будем принимать данные
                var buffer = new byte[256];

                // переменная для реального количества байтов
                var size = 0;

                // полученные данные собирает
                var data = new StringBuilder(); //  StringBuilder() позволяет удобно форматировать строки


                //делаем прослушивание
                EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0); //сюда будет сохранен адрес сервера
                do
                {
                    size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);

                    //продолжаем получать данные через StringBilder
                    data.Append(Encoding.UTF8.GetString(buffer));
                }
                while (udpSocket.Available > 0);

                Console.WriteLine(data);
                Console.ReadKey();

                //Прикольная приколюха легче чем Tcp
                //Все вышло. Мне понравилось)
            }
            #endregion
        }
    }
}
