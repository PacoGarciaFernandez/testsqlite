using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace biodanza
{
    public partial class FrmTreeClases : Form
    {
        public string name;
        public Int32 id;

        public FrmTreeClases()
        {
            InitializeComponent();
        }

        private void FrmTreeClases_Load(object sender, EventArgs e)
        {
            Location = Properties.Settings.Default.location_FrmTreeClases;
            Size = Properties.Settings.Default.size_FrmTreeClases;
        }

        public void CopyTreeNodes(TreeView treeview1)
        {
            TreeNode newTn;
            TreeView treeview2 = treeClases;

            foreach (TreeNode tn in treeview1.Nodes)
            {
                newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.SelectedImageIndex);

                string tipo = ((Tuple<string, Int32>)tn.Tag).Item1;
                Int32  id = ((Tuple<string, Int32>)tn.Tag).Item2;
                newTn.Tag = new Tuple<string, Int32>(tipo, id);

                CopyChildren(newTn, tn);
                treeview2.Nodes.Add(newTn);
            }
            treeClases.Nodes[0].Remove();
            treeClases.ExpandAll();
        }
        public void CopyChildren(TreeNode parent, TreeNode original)
        {
            TreeNode newTn;
            foreach (TreeNode tn in original.Nodes)
            {
                newTn = new TreeNode(tn.Text, tn.ImageIndex, tn.SelectedImageIndex);
                string tipo = ((Tuple<string, Int32>)tn.Tag).Item1;
                Int32 id = ((Tuple<string, Int32>)tn.Tag).Item2;
                newTn.Tag = new Tuple<string, Int32>(tipo, id);

                parent.Nodes.Add(newTn);
                CopyChildren(newTn, tn);
            }
        }

        private void FrmTreeClases_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.location_FrmTreeClases = Location;
            Properties.Settings.Default.size_FrmTreeClases = Size;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TreeNode tn = treeClases.SelectedNode;
            if (tn != null)
            {
                string tipo = ((Tuple<string, Int32>)tn.Tag).Item1;
                Int32 id = ((Tuple<string, Int32>)tn.Tag).Item2;

                if (tipo == "carpeta")
                {
                    MessageBox.Show("No se pueden añadir canciones a Proyectos. Sólo a clases");
                    return;
                }
                this.id = id;
                this.name = tn.Name;
                Close();
            }
        }
    }
}
