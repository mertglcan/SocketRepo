using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessageLib;
using Message = MessageLib.Message;

namespace ServerApp
{
    public partial class Form1 : Form
    {
        private Socket _socket;
        private TcpListener _tcpListener;
        private NetworkStream _networkStream;
        private BinaryFormatter _bf;
        public Form1()
        {
            _bf = new BinaryFormatter();
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void BtnStartListener_Click(object sender, EventArgs e)
        {
            if (_tcpListener == null)
            {
                _tcpListener = new TcpListener(IPAddress.Parse(txtIpAdress.Text), Convert.ToInt32(txtPort.Text));
                _tcpListener.Start();
                _socket = _tcpListener.AcceptSocket();
                lstPanel.Items.Add(_socket.RemoteEndPoint.ToString() + " bağlandı.");
                //if (((IPEndPoint)_socket.RemoteEndPoint).Address.ToString() != "192.168.2.104")
                //{
                //    _socket.Close();
                //    _tcpListener.Stop();
                //}
                _networkStream = new NetworkStream(_socket);
                Thread threadlisten = new Thread(ListenSocket);
                threadlisten.Start();
                lstPanel.Items.Add(txtIpAdress.Text + ":" + txtPort.Text + " dinlenmeye başlandı.");
            }
        }

        public void BaglantiKabulEdildi(IAsyncResult result) {
        }

        private void ListenSocket()
        {
            while (_socket.Connected)
            {
                Message recievedMessage = (Message)_bf.Deserialize(_networkStream);
                lstPanel.Items.Add(recievedMessage.ToString());
            }
        }
        private void BtnSendMessage_Click(object sender, EventArgs e)
        {

            Message message = new Message
            {
                Content = txtMessage.Text,
                SendedAt = DateTime.Now,
                Sender = "Server Message:"
            };
            _bf.Serialize(_networkStream, message);
            _networkStream.Flush();
            lstPanel.Items.Add(message.ToString());
            txtMessage.Clear();
            txtMessage.Focus();
        }

        private void BtnStopListener_Click(object sender, EventArgs e)
        {
            _networkStream.Flush();
            _tcpListener.Stop();
        }

    }
}
