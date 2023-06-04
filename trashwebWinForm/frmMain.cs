using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

namespace trashwebWinForm
{
    public partial class frmMain : Form
    {
        // Tạo biến string lấy id người dùng sau khi thực hiện
        public string userId;

        // Tạo mảng string để nhận từ frm Con
        List<string> getTypeListAfterEdit;

        // Tạo biến bool để xem xét xóa folder ảnh
        public bool isEdit = false;

        // Tạo biến để set cho form đi tiếp hay lùi lại
        public bool leaveEdit = false;

        // Tạo biến image mới để lưu dữ liệu ảnh từ client để luồng chính có thể set pictureBox
        System.Drawing.Image currentImage = null;

        // Tạo listImage để lưu ảnh
        public ImageList imageList = new ImageList();

        // Tạo 3 biến lưu giá trị của rác
        public int countRecycle = 0;
        public int countDangerous = 0;
        public int countOther = 0;

        // Tạo biến lưu loại rác nhận được
        public string typeTrash;

        // Tạo list<string> image lưu danh sách image dưới dạng string
        public List<string> listImageAsString = new List<string>();

        

        // Tạo list string để gửi đến form con
        public List<string> listType = new List<string>();

        // Tạo list lưu numTrash
        public List<string> listnumTrash = new List<string>();

        private string message = "Start";

        // Khởi tạo server
        private Thread receiveThread;

        private TcpListener _tcpListener; 
        //Khởi tạo biến xác nhận đã connect với client
        private bool _isListening = false;
        private bool stopRecv = false;
        private readonly string _jsonFolderRamPath = "D:\\GitTrashWeb\\trashwebWinForm\\json\\jsonRAM\\";
        private readonly string _jsonFolderPath = "D:\\GitTrashWeb\\trashwebWinForm\\json\\";
        private readonly string _reportPath = "D:\\GitTrashWeb\\trashwebWinForm\\Report\\";

        private string currentFolrder;

        public frmMain()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            FormInit();
            LoadIntroForm();
            autoStartServer();
        }

        private void autoStartServer()
        {
            // Start the receive thread
            receiveThread = new Thread(Start_Server);
            receiveThread.Start();
        }

        private void FormInit()
        {
            pictureBox1.Visible = false;
            label7.Visible = false;
            lblTaiChe.Visible = false;
            lblLoaiKhac.Visible = false;
            lblNguyHiem.Visible = false;
            numericUpDown1.Visible = false;
            numericUpDown2.Visible = false;
            numericUpDown3.Visible = false;
            btnDone.Visible = false;
            label7.Text = "";
            btnCancel.Visible = false;
        }

        private void LoadIntroForm()
        {
            

        }

        private static string getIdAdress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ipAddress in host.AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork && ipAddress.ToString().StartsWith("192.168."))
                {
                    return ipAddress.ToString();
                }
            }
            return null;
        }

        private async void Start_Server()
        {
            if (_isListening)
            {
                return;
            }
            
            _isListening = true;

            //Khởi tạo TcpListener lắng nghe kết nối từ client
            //IPAddress ipAddress = IPAddress.Parse(getIdAdress());
            IPAddress ipAddress = IPAddress.Parse("172.20.46.128");

            _tcpListener = new TcpListener(ipAddress, 5565);
            _tcpListener.Start();
            Console.WriteLine(ipAddress.ToString() + " Started!");

            while (_isListening)
            {
                try
                {
                    TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                    if (client != null && client.Connected)
                    {
                        Console.WriteLine("Send to client: " + message);
                        await Task.Run(() => ProcessClient(client));
                    }
                }
                catch (ObjectDisposedException)
                {
                    // _tcpListener has been disposed
                    break;
                }
            }
        }

        private void Stop_Server()
        {
            _isListening = false;
            _tcpListener?.Stop();
            Console.WriteLine("Server stopped!");
        }

        private Task ProcessClient(TcpClient client)
        {
            using (client)
            {
                ReceiveData(client);
            }
            return Task.CompletedTask;
        }

        private void CreateFolder()
        {
            string folderName = DateTime.Now.ToString("dd_MM_yyyy_HH_mm");
            string folderPath = Path.Combine(_jsonFolderPath, folderName);
            Directory.CreateDirectory(folderPath);

            currentFolrder = folderPath;

            // Lấy danh sách tất cả các tệp tin trong thư mục
            string[] files = Directory.GetFiles(_jsonFolderRamPath);

            // Sao chép các tệp tin đến thư mục đích
            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                string destinationFilePath = Path.Combine(currentFolrder, fileName);
                File.Copy(file, destinationFilePath, true);
            }
        }

        private void ReceiveData(TcpClient client)
        {
            if (stopRecv == true)
            {
                message = "Stop";
            }
            else 
            {
                message = "Start";
            }
            //try
            //{
                // Accept a client connection
                NetworkStream stream = client.GetStream();

                // Send a response to the client to notify that the server is ready to receive data
                byte[] response = Encoding.ASCII.GetBytes(message);
                stream.Write(response, 0, response.Length);

                // Nhận dữ liệu từ client và giải mã chuỗi JSON
                StringBuilder sb = new StringBuilder();
                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    sb.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                }

                if (sb.Length > 0)
                {
                    string json_data = sb.ToString();

                    // Giải mã chuỗi JSON và truy cập các thuộc tính
                    JsonDocument doc = JsonDocument.Parse(json_data);
                    JsonElement root = doc.RootElement;
                    string image_data = root.GetProperty("image").GetString();
                    string str_data = root.GetProperty("string_data").GetString();
                    string type = root.GetProperty("type").GetString();

                    // Lấy ảnh từ dữ liệu nhận được và hiển thị trên pictureBox
                    byte[] imgData = Convert.FromBase64String(image_data);
                    using (MemoryStream ms = new MemoryStream(imgData))
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                        currentImage = image;
                    }

                    // Lấy chuỗi string từ dữ liệu nhận được và hiển thị trong một MessageBox
                    string[] numbers = str_data.Trim('[', ']').Split(',');
                    countRecycle = int.Parse(numbers[0]);
                    countDangerous = int.Parse(numbers[1]);
                    countOther = int.Parse(numbers[2]);
                    typeTrash = type;

                    setVisual();

                    string fileName = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + ".json";

                    string filePath = Path.Combine(_jsonFolderRamPath, fileName);
                    File.WriteAllText(filePath, json_data);
                    client.Close();
                } 
            //}catch (Exception e) 
            //{
            //    MessageBox.Show("Error: " + e.Message);
            //}
        }

        private void Load_Json()
        {
            CreateFolder();
            string[] jsonFiles = Directory.GetFiles(_jsonFolderRamPath, "*.json");


            foreach (string jsonFile in jsonFiles)
            {
                // Lấy hình ảnh từ file JSON và chuyển đổi thành đối tượng Image
                string jsonString = File.ReadAllText(jsonFile);
                
                JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;
                string image_data = root.GetProperty("image").GetString();

                Console.WriteLine(image_data);

                // Thêm vào danh sách
                listImageAsString.Add(image_data);

                // Lấy ảnh từ dữ liệu nhận được và hiển thị trên pictureBox
                byte[] imgData = Convert.FromBase64String(image_data);
                using (MemoryStream ms = new MemoryStream(imgData))
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                    System.Drawing.Image imageResized = ResizeImage(image, 800, 800);
                    imageList.Images.Add(imageResized);
                }
                string typeTr = root.GetProperty("type").GetString();
                this.listType.Add(typeTr);
                string str_data = root.GetProperty("string_data").GetString();
                this.listnumTrash.Add(str_data);
            }

            Console.WriteLine("Imagelist.count = " + listImageAsString.Count);
        }

        // Hàm resize image
        public static System.Drawing.Image ResizeImage(System.Drawing.Image image, int width, int height)
        {
            // Tạo bitmap mới với kích thước mới
            Bitmap bitmap = new Bitmap(width, height);

            // Tạo đối tượng Graphics từ bitmap mới
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                // Thay đổi kích thước hình ảnh
                graphics.DrawImage(image, 0, 0, width, height);
            }

            // Trả về hình ảnh mới
            return bitmap;
        }

        //Cross-thread operation not valid: Control '' accessed from a thread other than the thread it was created on.'
        private void setVisual()
        {
            //Invoke để đồng bộ hóa và có thể chạy được UI
            this.Invoke((MethodInvoker)delegate {
                DisableFormIntro();
                label1.Text = "DETECTING";
                btnCancel.Visible = false;
                Enable();
                System.Drawing.Image resizedImage = ResizeImage(currentImage, 439, 303);
                // Set hiển thị image và số rác
                this.pictureBox1.Image = resizedImage;
                this.numericUpDown1.Value = countRecycle;
                this.numericUpDown2.Value = countDangerous;
                this.numericUpDown3.Value = countOther;
                this.label7.Text = typeTrash;
            });
            
        }

        public void Enable()
        {
            pictureBox1.Visible = true;
            //txt_Username.Visible = true;
            label7.Visible = true;
            lblTaiChe.Visible = true;
            lblLoaiKhac.Visible = true;
            lblNguyHiem.Visible = true;
            numericUpDown1.Visible = true;
            numericUpDown2.Visible = true;
            numericUpDown3.Visible = true;
            btnDone.Visible = true;
        }

        private void Disable()
        {
            label1.Text = "";
            pictureBox1.Visible = false;
            label7.Visible = false;
            lblTaiChe.Visible = false;
            lblLoaiKhac.Visible = false;
            lblNguyHiem.Visible = false;
            numericUpDown1.Visible = false;
            numericUpDown2.Visible = false;
            numericUpDown3.Visible = false;
            btnDone.Visible = false;
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn Chắn Chắn Đã Hoàn Thành?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                stopRecv = true;
                Load_Json();
                Disable();

                frmSubmit frmSubmit = new frmSubmit();

                frmSubmit.ButtonClicked += frmSubmit_ButtonClicked;
                frmSubmit.frmSubmit_getList += frmSubmit_getList;

                frmSubmit.giveCurrenFolder(currentFolrder);
                frmSubmit.setNumTrash(countRecycle, countDangerous, countOther);
                frmSubmit.setlistType(listType);
                frmSubmit.setImageAsStringList(listImageAsString);

                frmSubmit.TopLevel = false;
                frmSubmit.AutoScroll = true;
                frmSubmit.FormBorderStyle = FormBorderStyle.None;
                frmSubmit.Dock = DockStyle.Fill;
                frmSubmit.Show();
                panelMain.Controls.Add(frmSubmit);
                frmSubmit.LoadListImage();

                frmInputId frmInput = new frmInputId();
                frmInput.setNumTrash(countRecycle, countDangerous, countOther);
                frmInput.TopLevel = false;
                frmInput.AutoScroll = true;
                frmInput.FormBorderStyle = FormBorderStyle.None;
                frmInput.Dock = DockStyle.Fill;
                frmInput.Show();
                panelMain.Controls.Add(frmInput);

                frmInput.frmInputId_getString += frmSubmit_Send_Back_Id_AfterClosed;
                // Đăng ký sự kiện Closed của form con
                frmInput.Closed += frmInput_Closed;
                
            }            
        }

        private void frmSubmit_getList(List<string> myArray)
        {
            // Xử lý dữ liệu myArray được truyền từ form con
            getTypeListAfterEdit = myArray;
        }

        private void frmSubmit_ButtonClicked(object sender, bool isLeftButtonPressed)
        {
            isEdit = isLeftButtonPressed;
        }

        private void frmSubmit_Send_Back_Id_AfterClosed(string UserIdGiveBack)
        {
            userId = UserIdGiveBack;
        }

        private void frmInput_Closed(object sender, EventArgs e)
        {
            // Hiển thị lại Label khi form con đã đóng
            label1.Text = "CHÀO MỪNG BẠN ĐẾN VỚI GREEN AI";
            label1.Location = new Point(143, 131);
            btnStart.Visible = true;
            label5.Visible = true;
            stopRecv = false;
            Stop_Server();
            autoStartServer();

            // Lấy danh sách tất cả các tệp tin trong thư mục RAM
            string[] files = Directory.GetFiles(_jsonFolderRamPath);

            // Xóa từng tệp tin trong danh sách RAM
            foreach (string file in files)
            {
                File.Delete(file);
            }

            // Xóa folder 
            if (isEdit == false)
            {
                // Kiểm tra xem thư mục có tồn tại hay không trước khi xóa
                if (Directory.Exists(currentFolrder))
                {
                    Directory.Delete(currentFolrder, true);
                }
            }
            else
            {
                // Kiểu folder: Ngày --> ID --> Giờ phút
                string folderDay = DateTime.Now.ToString("dd_MM_yyyy");
                string folderOne = Path.Combine(_reportPath, folderDay);
                if (!Directory.Exists(folderOne))
                {
                    Directory.CreateDirectory(folderOne);
                }

                string folderID = userId;
                string folderSecond = Path.Combine(folderOne, folderID);
                if (!Directory.Exists(folderSecond))
                {
                    Directory.CreateDirectory(folderSecond);
                }

                string folderTime = DateTime.Now.ToString("HH_mm_ss"); ;
                string folderThird = Path.Combine(folderSecond, folderTime);
                if (!Directory.Exists(folderThird))
                {
                    Directory.CreateDirectory(folderThird);
                }

                // Tạo thư mục con Image và name
                string imageDirectory = Path.Combine(folderThird, "Image");
                string nameDirectory = Path.Combine(folderThird, "Name");
                Directory.CreateDirectory(imageDirectory);
                Directory.CreateDirectory(nameDirectory);

                // Lưu hình ảnh vào thư mục Image và lưu chuỗi vào thư mục Name
                for (int i = 0; i < getTypeListAfterEdit.Count; i++)
                {
                    string imagePath = Path.Combine(imageDirectory, i + ".jpg");
                    
                    imageList.Images[i].Save(imagePath);

                    string jsonString = JsonConvert.SerializeObject(getTypeListAfterEdit[i]);
                    string namePath = Path.Combine(nameDirectory, i + ".json");
                    File.WriteAllText(namePath, jsonString);
                }
            }
        }

        private void DisableFormIntro()
        {
            btnStart.Visible = false;
            label5.Visible = false;
            label1.Text = "Xác nhận người dùng...";
            label1.Location = new Point(20, 45);
            btnCancel.Visible = true;
            btnCancel.Location = new Point(578, 464);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            DisableFormIntro();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Stop_Server();
            autoStartServer();
            Disable();
            // Hiển thị lại Label khi form con đã đóng
            label1.Text = "CHÀO MỪNG BẠN ĐẾN VỚI GREEN AI";
            label1.Location = new Point(143, 131);
            btnStart.Visible = true;
            label5.Visible = true;
            btnCancel.Visible = false;
        }
    }
}
