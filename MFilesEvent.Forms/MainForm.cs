﻿using MFilesAPI;
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
            listView1.View = System.Windows.Forms.View.Details;
   
        }

        private void listItemMouseDoubleClick(object sender, MouseEventArgs e)
        {


        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadDocuments();
        }

        private void LoadDocuments()
        {
            listView1.Items.Clear();

            int count = 0;

            //Build Search Conditions
            var scs = new SearchConditions();
            var sc = new SearchCondition();

            //Object Type Condition (Document)
            sc.ConditionType = MFConditionType.MFConditionTypeEqual;
            sc.Expression.DataStatusValueType = MFStatusType.MFStatusTypeObjectTypeID;
            sc.TypedValue.SetValue(MFDataType.MFDatatypeLookup, 0);
            scs.Add(-1, sc);

            //Deleted Condition (Not Deleted)
            sc.ConditionType = MFConditionType.MFConditionTypeEqual;
            sc.Expression.DataStatusValueType = MFStatusType.MFStatusTypeDeleted;
            sc.TypedValue.SetValue(MFDataType.MFDatatypeBoolean, false);
            scs.Add(-1, sc);

            //Perform Search & Enumerate
            _loggedInVault
                .ObjectSearchOperations
                .SearchForObjectsByConditions(scs, MFSearchFlags.MFSearchFlagNone, false)
                .Cast<ObjectVersion>()
                .OrderByDescending(x => x.ObjVer.ID)
                .ToList().ForEach(x =>
                {
                    //If it has files :( 
                    if (x.Files.Count > 0 && x.SingleFile)
                    {
                        var firstFile = x.Files[1];

                        //Poulate listview items
                        var lvItem = new ListViewItem(new string[] { $"{firstFile.Title}.{firstFile.Extension}", "Document" });
                        lvItem.Tag = new SimpleInfo { Id = x.ObjVer.ID, Type = x.ObjVer.Type };

                        listView1.Items.Add(lvItem);
                        count++;
                    }

                });

            //Set item count to label.
            lblCount.Text = count.ToString();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() != "")
            {

                listView1.Items.Clear();

                int count = 0;

                //Perform quick search
                _loggedInVault
                    .ObjectSearchOperations
                    .SearchForObjectsByString(textBox1.Text, false, MFFullTextSearchFlags.MFFullTextSearchFlagsLookInMetaData | MFFullTextSearchFlags.MFFullTextSearchFlagsLookInFileData)
                    .Cast<ObjectVersion>()
                    .Where(x => x.ObjVer.Type == 0) //Select only documents :(
                    .OrderByDescending(x => x.ObjVer.ID)
                    .ToList()
                    .ForEach(x =>
                    {
                        //Check for files
                        if (x.Files.Count > 0 && x.SingleFile)
                        {
                            var firstFile = x.Files[1];

                            //Poulate listview items
                            var lvItem = new ListViewItem(new string[] { $"{firstFile.Title}.{firstFile.Extension}", "Document" });
                            lvItem.Tag = new SimpleInfo { Id = x.ObjVer.ID, Type = x.ObjVer.Type };

                            listView1.Items.Add(lvItem);
                            count++;
                        }
                    });

                lblCount.Text = count.ToString();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            //Set creation info details
            var cr = new ObjectCreationInfo { DisallowTemplateSelection = false };
            cr.SetObjectType(0, false);
            cr.SetSingleFileDocument(true, false);
            cr.SetMetadataCardTitle("Test");

            //Show M-Files Dialog
            var result = _loggedInVault.ObjectOperations.ShowNewObjectWindow(this.Handle, MFObjectWindowMode.MFObjectWindowModeInsert, cr);

            if (result.Result != MFObjectWindowResultCode.MFObjectWindowResultCodeCancel)
            {
                //Check In the object & files
                _loggedInVault.ObjectOperations.CheckIn(result.ObjVer);

                //Reload documents list.
                LoadDocuments();
            }

            Cursor = Cursors.Default;

        }

        private void btnDocuments_Click(object sender, EventArgs e)
        {

        }

        private void btnUser_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Select file with dialog
            var opn = new OpenFileDialog();
            opn.Multiselect = false;


            if (opn.ShowDialog() == DialogResult.OK)
            {

                Cursor = Cursors.WaitCursor;

                var fName = Path.GetFileNameWithoutExtension(opn.FileName);
                var extension = Path.GetExtension(opn.FileName).Replace(".", "");

                //Set files
                var sfds = new SourceObjectFiles();
                sfds.AddFile(fName, extension, opn.FileName);

                //Create Title
                var tv = new TypedValue();
                tv.SetValue(MFDataType.MFDatatypeText, fName);

                //Set creation info details
                var cr = new ObjectCreationInfo();
                cr.SetMetadataCardTitle(fName);
                cr.SetObjectType(0, false);
                cr.SetSingleFileDocument(true, false);
                cr.SetSourceFiles(sfds);
                cr.DisallowTemplateSelection = true;
                cr.SetExtension(extension, false);
                cr.SetTitle(tv, false);

                //Show M-Files Dialog
                var result = _loggedInVault.ObjectOperations.ShowNewObjectWindow(this.Handle, MFObjectWindowMode.MFObjectWindowModeInsertSourceFiles, cr);

                if (result.Result != MFObjectWindowResultCode.MFObjectWindowResultCodeCancel)
                {
                    //Check In the object & files
                    _loggedInVault.ObjectOperations.CheckIn(result.ObjVer);

                    //Reload documents list.
                    LoadDocuments();
                }

                Cursor = Cursors.Default;
            }
        }
    }
}
