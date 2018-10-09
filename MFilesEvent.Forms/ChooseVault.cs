using MFilesAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MFilesEvent.Forms
{
    public partial class ChooseVault : Form
    {

        private readonly MFilesClientApplication _mfApp;
        private List<VaultConnection> _connections;

        public ChooseVault()
        {
            InitializeComponent();
            _mfApp = new MFilesClientApplication();
        }

        private void ChooseVault_Load(object sender, EventArgs e)
        {
            //TODO: Load Vaults
            _connections = _mfApp.GetVaultConnections().Cast<VaultConnection>().ToList();

            foreach (var item in _connections)
            {
                listBox1.Items.Add(item.Name);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //TODO: Login to Vault
            
        }
    }
}
