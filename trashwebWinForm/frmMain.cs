using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trashwebWinForm
{
    public partial class frmMain : Form
    {
        // Tạo biến để set cho form đi tiếp hay lùi lại
        public bool leaveEdit;

        // Tạo biến để nhận chuỗi hoặc ảnh
        private bool recv_str_img = true;
        private bool recv_list_img = false;
        private bool sendString = false;
        private bool sendList = false;

        // Tạo 3 biến lưu giá trị của rác
        public int countRecycle = 0;
        public int countDangerous = 0;
        public int countOther = 0;

        private string message;

        //Khởi tạo biến xác nhận đã connect với client
        private bool client_connected = false;

        // Khởi tạo server
        TcpListener server;
        TcpClient client;
        public frmMain()
        {
            InitializeComponent();
        }

        public void setMove(bool LeaveEdit)
        {
            this.leaveEdit = LeaveEdit;
            Console.WriteLine("LeaveEdit: " +  this.leaveEdit);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            FormInit();
            
            // Khởi tạo thread riêng để lắng nghe kết nối từ client
            Thread listenerThread = new Thread(new ThreadStart(ListenForClients));
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

        private void FormInit()
        {
            txt_Username.Visible = false;
            label7.Text = "";
        }

        private void ListenForClients()
        {
            // Khởi tạo TcpListener lắng nghe kết nối từ client
            IPAddress ipAddress = IPAddress.Parse("192.168.100.29");
            server = new TcpListener(ipAddress, 5565);
            client = new TcpClient();
            server.Start();
            Console.WriteLine(" >> Server Started");

            while (true)
            {
                client = server.AcceptTcpClient();
                Console.WriteLine("\n Connect success!");
                client_connected = true;

                // Tạo một thread mới để xử lý tương tác của client
                Thread clientThread = new Thread(() =>
                {
                    ReceiveStringAndImage(client);
                   
                    ReceiveListImage(client);
                    recv_list_img = false;
                    
                });
                //clientThread.IsBackground = true;
                clientThread.Start();
            }
        }

        private void SendClient(TcpClient client)
        {
            if (client_connected)
            {
                if (sendString==true)
                {
                    SendString(client);
                    sendString = false;
                }
                else if (sendList==true)
                {
                    SendList(client);
                    sendList = false;
                }
            }
        }

        private void ReceiveStringAndImage(TcpClient client)
        {
            
            // Nhận dữ liệu từ client và giải mã chuỗi JSON
            StringBuilder sb = new StringBuilder();
            byte[] buffer = new byte[1024];
            int bytesRead = 0;
            while ((bytesRead = client.GetStream().Read(buffer, 0, buffer.Length)) > 0)
            {
                sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));
            }
            string json_data = sb.ToString();
            dynamic data = JsonSerializer.Deserialize<dynamic>(json_data);

            // Giải mã chuỗi JSON và truy cập các thuộc tính
            JsonDocument doc = JsonDocument.Parse(json_data);
            JsonElement root = doc.RootElement;
            string image_data = root.GetProperty("image").GetString();
            string str_data = root.GetProperty("string_data").GetString();

            // Lấy ảnh từ dữ liệu nhận được và hiển thị trên pictureBox
            byte[] imgData = Convert.FromBase64String(image_data);
            using (MemoryStream ms = new MemoryStream(imgData))
            {
                Image image = Image.FromStream(ms);
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }

            // Lấy chuỗi string từ dữ liệu nhận được và hiển thị trong một MessageBox
            string[] numbers = str_data.Trim('[', ']').Split(',');
            countRecycle = int.Parse(numbers[0]);
            countDangerous = int.Parse(numbers[1]);
            countOther = int.Parse(numbers[2]);
            numericUpDown1.Value = countRecycle;
            numericUpDown2.Value = countDangerous;
            numericUpDown3.Value = countOther;

            Console.WriteLine(str_data);
            
            
        }

        private void SendString(TcpClient client) 
        {
            // Gửi dữ liệu đến client
            message = "sendstring";
            byte[] data = Encoding.UTF8.GetBytes(message);

            client.GetStream().Write(data, 0, data.Length);
        }

        private void SendList(TcpClient client)
        {
            // Gửi dữ liệu đến client
            message = "sendlist";
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.GetStream().Write(data, 0, data.Length);
        }

        private void ReceiveListImage(TcpClient client) 
        {
            // Nhận danh sách tên file ảnh
            StreamReader reader = new StreamReader(client.GetStream());
            string message = reader.ReadToEnd();
            string[] imageFiles = message.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Load các ảnh từ file vào List<Image>
            List<Image> listImage = new List<Image>();
            foreach (string file in imageFiles)
            {
                Image image = Image.FromFile(file);
                listImage.Add(image);
            }


            // Hiển thị các ảnh trong danh sách lên pictureBox
            foreach (var image in listImage)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
                System.Threading.Thread.Sleep(100); // Đợi 1 giây trước khi hiển thị ảnh tiếp theo
            }

        }

        private void disable()
        {
            pictureBox1.Visible = false;
            label2.Visible = false;
            txt_Username.Visible = false;
            label7.Visible = false;
            lblTaiChe.Visible = false;
            lblLoaiKhac.Visible = false;
            lblNguyHiem.Visible = false;
            numericUpDown1.Visible = false;
            numericUpDown2.Visible = false;
            numericUpDown3.Visible = false;
            btnDone.Visible = false;
            button1.Visible = false;

        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Chắn Chắn Đã Hoàn Thành?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                sendList = true;
                disable();
                frmSubmit frmSubmit = new frmSubmit();
                frmSubmit.setNumTrash(countRecycle, countDangerous, countOther);
                frmSubmit.TopLevel = false;
                frmSubmit.AutoScroll = true;
                frmSubmit.FormBorderStyle = FormBorderStyle.None;
                frmSubmit.Dock = DockStyle.Fill;
                frmSubmit.Show();
                panelMain.Controls.Add(frmSubmit);

                if (leaveEdit)
                {
                    frmInputId frmInput = new frmInputId();
                    frmInput.setNumTrash(countRecycle, countDangerous, countOther);
                    frmInput.TopLevel = false;
                    frmInput.AutoScroll = true;
                    frmInput.FormBorderStyle = FormBorderStyle.None;
                    frmInput.Dock = DockStyle.Fill;
                    frmInput.Show();
                    panelMain.Controls.Add(frmInput);
                }
                else if (leaveEdit == false)
                {
                    frmInputId frmInput = new frmInputId();
                    frmInput.setNumTrash(countRecycle, countDangerous, countOther);
                    frmInput.TopLevel = false;
                    frmInput.AutoScroll = true;
                    frmInput.FormBorderStyle = FormBorderStyle.None;
                    frmInput.Dock = DockStyle.Fill;
                    frmInput.Show();
                    panelMain.Controls.Add(frmInput);
                }
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendString = true;
            Console.WriteLine("sendString: " + sendString);
            Console.WriteLine("recv_str_img: " + recv_str_img);
            SendClient(client);
        }
    }
}
