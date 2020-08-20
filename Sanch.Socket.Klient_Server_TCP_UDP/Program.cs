using System;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerTCP_UDP //серверная часть
{
    class Program
    {
        //TCP протоколы - четко соединение между клиентом и сервером. Точно будет доставлено. Если пакет не дошел, то еще раз отправляет
        //Он работает медленее. Большая нагрузка на установку соединения для маленького количества клиентов ( для сайтов ) 
        //UDP протоколы - Не гарантирует доставку, но быстрый и легкий для большего количества клиентов (для стримингого вещания - для всех и сразу) 
        static void Main(string[] args)
        {
            #region Tcp
            /*
            //соединение ip и порт
            const string ip = "127.0.0.1";
            const int port = 8080;

            //точка подключения
            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

            //Socket - дверб - дырочка в которую можно заходить (установка соединения)
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //AddressFamily.InterNetwork - использование ip-адреса 4 версии
            //SocketType.Stream - потоковое передача данных для протокола TCP
            //ProtocolType.Tcp - указывает на TCP-протокол

            //перевод socket в режим ожидания - связывание - указать порт для сокета
            tcpSocket.Bind(tcpEndPoint);

            //запуск сокета на прослушивание / очередь для клиентов для подключения одного и того же порта
            tcpSocket.Listen(5);

            //процесс прослушивания
            while (true)
            {
                //делаем сам прослушиватель / обработчик на прием сообщения
                var listener = tcpSocket.Accept(); //создается новый сокет для подключения нового клиента / подсокет

                // буфер - место куда будем принимать данные
                var buffer = new byte[256];

                // переменная для реального количества байтов
                var size = 0;

                // полученные данные собирает
                var data = new StringBuilder(); //  StringBuilder() позволяет удобно форматировать строки

                //получение сообщения / проверяем условие на получение запроса
                do
                {
                    size = listener.Receive(buffer);// получаем 256 байт из сообщения  - количество реально полученных байт их этого сообщения
                    //читаем / раскадировать полученные байты в куски сообщений
                    data.Append(Encoding.UTF8.GetString(buffer, 0, size));
                    //buffer - наш буфер
                    //0 - откуда начинаем
                    //size - наше количество
                }
                while (listener.Available > 0);

                Console.WriteLine(data);// - сообщение

                //обратный ответ можем дать / закодировать и отправить
                listener.Send(Encoding.UTF8.GetBytes("Успех"));

                //нужно закрыть наше подключение корректно
                listener.Shutdown(SocketShutdown.Both);//отключили
                listener.Close();// закрыли



                Console.ReadKey();
               
            }
            */
            #endregion

            #region Udp
            //соединение ip и порт
            const string ip = "127.0.0.1";
            const int port = 8081; //просто меняем порт

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

                // буфер - место куда будем принимать данные
                var buffer = new byte[256];

                // переменная для реального количества байтов
                var size = 0;

                // полученные данные собирает
                var data = new StringBuilder(); //  StringBuilder() позволяет удобно форматировать строки

                //делаем прослушивание
                EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0); //сюда будет сохранен адрес клиента
                do
                {
                    size = udpSocket.ReceiveFrom(buffer, ref senderEndPoint);

                    //продолжаем получать данные через StringBilder
                    data.Append(Encoding.UTF8.GetString(buffer));
                }
                while (udpSocket.Available > 0);

                Console.WriteLine(data); //получаем сообщение

                udpSocket.SendTo(Encoding.UTF8.GetBytes("Сработало"), senderEndPoint); //отправляет сообщению пользователю которые обратился сюда

                //udpSocket.Shutdown(SocketShutdown.Both); //отключаем сокет
                //udpSocket.Close();//закрываем

               
            }
            #endregion
        }
    }
}
