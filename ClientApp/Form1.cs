using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MessageLib;
using Message = MessageLib.Message;

namespace ClientApp
{
    public partial class Form1 : Form
    {
        private TcpClient _tcpClient;
        private BinaryFormatter _bf;
        private NetworkStream _networkStream;
        public Form1()
        {
            _bf = new BinaryFormatter();
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void BtnConnect_Click(object sender, EventArgs e)
        {
            _tcpClient = new TcpClient(txtIpAdress.Text, Convert.ToInt32(txtPort.Text));
            _networkStream = _tcpClient.GetStream();
            Thread threadListen = new Thread(ListenSocket);
            threadListen.Start();
        }

        private void ListenSocket()
        {
            while (true)
            {
                Message message = (Message)_bf.Deserialize(_networkStream);
                lstPanel.Items.Add(message);
            }
        }

        private void BtnSendMessage_Click(object sender, EventArgs e)
        {
            Message message = new Message
            {
                Content = txtMessage.Text,
                SendedAt = DateTime.Now,
                Sender = "Client Message:"
            };

            _bf.Serialize(_networkStream, message);
            _networkStream.Flush();
            lstPanel.Items.Add(message);
            txtMessage.Clear();
            txtMessage.Focus();
        }
    }
}
