using MFilesAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MFilesEvent.Forms
{

    public partial class MainForm : Form
    {
        private readonly Vault _loggedInVault;

        public MainForm(Vault loggedInVault)
        {
            InitializeComponent();
            _loggedInVault = loggedInVault;
        }

        private void listItemMouseDoubleClick(object sender, MouseEventArgs e)
        {


        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void LoadDocuments()
        {


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void btnNew_Click(object sender, EventArgs e)
        {


        }

        private void btnDocuments_Click(object sender, EventArgs e)
        {

        }

        private void btnUser_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }
    }
}
