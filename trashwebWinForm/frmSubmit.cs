using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace trashwebWinForm
{
    public partial class frmSubmit : Form
    {
        // Tạo 3 biến lưu giá trị của rác
        private int countRecycle = 0;
        private int countDangerous = 0;
        private int countOther = 0;
        bool leaveEdit = false;
        public frmSubmit()
        {
            InitializeComponent();
        }

        private void frmSubmit_Load(object sender, EventArgs e)
        {

        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận hoàn thành?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
                leaveEdit = false;
                frmMain frmMain = new frmMain();
                frmMain.setMove(leaveEdit);
            }
            
        }

        public void setNumTrash(int recycle, int dangerous, int other)
        {
            this.countRecycle = recycle;
            this.countDangerous = dangerous;
            this.countOther = other;
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            this.Close();
            leaveEdit = true;
            frmMain frmMain = new frmMain();
            frmMain.setMove(leaveEdit);
        }
    }
}
