using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace testSendUdp
{
    public partial class UdpBroadcastSender : Form
    {
        Socket socket; //目标socket
        IPEndPoint clientend;
        Thread connectThread; //连接线程
        string sendStr; //发送的字符串
        byte[] sendData = new byte[1024];

        public UdpBroadcastSender()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitSocket();
        }

        void InitSocket()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            clientend = new IPEndPoint(IPAddress.Parse("255.255.255.255"), 24999);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);


            //开启一个线程连接，必须的，否则主线程卡死
            connectThread = new Thread(new ThreadStart(SocketReceive1));
            connectThread.Start();
        }

        void SocketReceive1()
        {
            //进入接收循环
            //while (true)
            {
                //Thread.Sleep(2000);
                //将当前线程休眠2秒
            }
        }


        void SocketSend(string sendStr)
        {
            sendData = new byte[1024];
            //数据类型转换
            sendData = Encoding.ASCII.GetBytes(sendStr);
            socket.SendTo(sendData, clientend);
            textBox1.Text = sendStr;
        }

        void SocketQuit()
        {
            //关闭线程
            if (connectThread != null)
            {
                connectThread.Interrupt();
                connectThread.Abort();
            }
            //最后关闭socket
            if (socket != null)
            {
                socket.Close();
            }
            //print("disconnect");
            Environment.Exit(0);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SocketQuit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = "OPEN";
            SocketSend(s);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = "CLOSE";
            SocketSend(s);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string s = "SHUTDOWN";
            SocketSend(s);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string s = "COPY";
            SocketSend(s);
        }
    }
}
