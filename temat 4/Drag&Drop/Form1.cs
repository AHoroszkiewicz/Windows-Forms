using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Drag_Drop
{
    public partial class Form1: Form
    {
        public object dragDropSource;

        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            dragDropSource = sender;
            ListBox lb = sender as ListBox;
            if (lb.SelectedItems == null)
                return;
            DragDropEffects effect = lb.DoDragDrop(lb.SelectedItems, DragDropEffects.Copy | DragDropEffects.Move);
            if (effect == DragDropEffects.Move)
            {
                for (int i = lb.SelectedItems.Count - 1; i >= 0; i--)
                {
                    lb.Items.Remove(lb.SelectedItems[i]);
                }
            }
            dragDropSource = null;
        }

        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            if (sender == dragDropSource)
            {
                e.Effect = DragDropEffects.None;
            }
            else
            {
                if ((e.KeyState & 8) == 8)
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                }
            }
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            ListBox lb = sender as ListBox;
            ListBox lbSource = dragDropSource as ListBox;
            int index = lb.IndexFromPoint(lb.PointToClient(new Point(e.X, e.Y)));
            if (index == -1)
            {
                foreach(var item in lbSource.SelectedItems)
                {
                    lb.Items.Add(item);
                }
            }
            else
            {
                foreach (var item in lbSource.SelectedItems)
                {
                    lb.Items.Insert(index, item);
                    index++;
                }
            }
        }
    }
}
