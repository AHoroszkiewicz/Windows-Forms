using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;

namespace ToDoList
{
    public partial class Form1: Form
    {
        private BindingList<ToDoEntry> entries = new BindingList<ToDoEntry>();

        public Form1()
        {
            InitializeComponent();
            titleText.DataBindings.Add("Text", entriesSource, "Title", true, DataSourceUpdateMode.OnPropertyChanged);
            dueDatePicker.DataBindings.Add("Value", entriesSource, "DueDate", true, DataSourceUpdateMode.OnPropertyChanged);
            entriesSource.DataSource = entries;
            CreateNewItem();
        }

        private void CreateNewItem()
        {
            ToDoEntry newEntry = (ToDoEntry)entriesSource.AddNew();
            newEntry.Title = "New Item";
            newEntry.Description = "Description";
            newEntry.DueDate = DateTime.Now;
            entriesSource.ResetCurrentItem();
        }

        private void AddNewItem(int index)
        {
            ListViewItem item = new ListViewItem();
            item.SubItems.Add("");
            entriesListView.Items.Insert(index, item);
        }

        private void RemoveListViewItem(int index)
        {
            entriesListView.Items.RemoveAt(index);
        }

        private void UpdateListViewItem(int index)
        {
            ListViewItem item = entriesListView.Items[index];
            ToDoEntry entry = entries[index];
            item.SubItems[0].Text = entry.Title;
            item.SubItems[1].Text = entry.DueDate.ToShortDateString();
        }

        private void entriesSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    AddNewItem(e.NewIndex);
                    break;
                case ListChangedType.ItemDeleted:
                    RemoveListViewItem(e.NewIndex);
                    break;
                case ListChangedType.ItemChanged:
                    UpdateListViewItem(e.NewIndex);
                    break;
            }
        }

        private void newButton_Click(object sender, EventArgs e)
        {
            CreateNewItem();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (entriesListView.SelectedIndices.Count > 0)
                entriesSource.RemoveAt(entriesListView.SelectedIndices[0]);
        }

        private void entriesListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (entriesListView.SelectedIndices.Count > 0)
                entriesSource.Position = entriesListView.SelectedIndices[0];
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveToFile();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            LoadFromFile();
        }

        private void SaveToFile()
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(entries, options);
                File.WriteAllText(saveFileDialog1.FileName, json);
            }
        }

        private void LoadFromFile()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string json = File.ReadAllText(openFileDialog1.FileName);
                entries = JsonSerializer.Deserialize<BindingList<ToDoEntry>>(json);
                entriesSource.DataSource = entries;
                RefreshListView();
            }
        }

        private void RefreshListView()
        {
            entriesListView.Items.Clear();
            foreach (var entry in entries)
            {
                ListViewItem item = new ListViewItem(entry.Title);
                item.SubItems.Add(entry.DueDate.ToShortDateString());
                entriesListView.Items.Add(item);
            }
        }
    }
}
