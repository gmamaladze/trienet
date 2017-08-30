// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gma.DataStructures.StringSearch.DemoApp
{
    public partial class MainForm : Form
    {
        private readonly ITrie<WordPosition> m_PatriciaTrie;
        private long m_WordCount;

        public MainForm()
        {
            InitializeComponent();
            m_PatriciaTrie = new SuffixTrie<WordPosition>(3);
            //m_PatriciaTrie = new FakeTrie<WordPosition>();
            folderName.Text =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "texts");
        }

        private void LoadFile(string fileName)
        {
            Tuple<WordPosition, string>[] words = GetWords(fileName).ToArray();
            foreach (var word in words)
            {
                string text = word.Item2;
                WordPosition wordPosition = word.Item1;
                m_PatriciaTrie.Add(text, wordPosition);
            }
        }


        private IEnumerable<Tuple<WordPosition, string>> GetWords(string file)
        {
            using (Stream stream = File.Open(file, FileMode.Open))
            {
                var word = new StringBuilder();
                while (true)
                {
                    long position = stream.Position;
                    int data = (char) stream.ReadByte();
                    {
                        if (data > byte.MaxValue) break;
                        var ch = (Char) data;
                        if (char.IsLetter(ch))
                        {
                            word.Append(ch);
                        }
                        else
                        {
                            if (word.Length != 0)
                            {
                                var wordPosition = new WordPosition(position, file);
                                yield return new Tuple<WordPosition, string>(wordPosition, word.ToString().ToLower());
                                word.Clear();
                                m_WordCount++;
                            }
                        }
                    }
                    UpdateProgress(position);
                }
            }
        }

        private void UpdateProgress(long position)
        {
            if (position%1024 != 0) return;
            progressBar1.Value = Math.Min((int) position/1024*2, progressBar1.Maximum);
            Application.DoEvents();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            if (string.IsNullOrEmpty(text) || text.Length < 3) return;
            WordPosition[] result = m_PatriciaTrie.Retrieve(text).ToArray();
            listBox1.Items.Clear();
            foreach (WordPosition wordPosition in result)
            {
                listBox1.Items.Add(wordPosition);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = listBox1.SelectedItem as WordPosition;
            if (item == null) return;
            using (FileStream file = File.Open(item.FileName, FileMode.Open))
            {
                const int bifferSize = 200;
                long position = Math.Max(item.CharPosition - bifferSize/2, 0);
                file.Seek(position, SeekOrigin.Begin);
                var buffer = new byte[bifferSize];
                file.Read(buffer, 0, bifferSize);
                string line = Encoding.ASCII.GetString(buffer);
                richTextBox1.Text = line;

                string serachText = textBox1.Text;
                int index = richTextBox1.Text.IndexOf(serachText, StringComparison.InvariantCultureIgnoreCase);
                if (index < 0) return;
                richTextBox1.Select(index, serachText.Length);
                richTextBox1.SelectionBackColor = Color.Yellow;
                richTextBox1.DeselectAll();
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = folderName.Text;
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            folderName.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadAll();
        }

        private void LoadAll()
        {
            m_WordCount = 0;
            string path = folderName.Text;
            if (!Directory.Exists(path)) return;
            string[] files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            for (int index = 0; index < files.Length; index++)
            {
                string file = files[index];
                progressText.Text =
                    string.Format(
                        "Processing file {0} of {1}: [{2}]",
                        index,
                        files.Length,
                        Path.GetFileName(file));

                var fileInfo = new FileInfo(file);
                progressBar1.Maximum = (int) fileInfo.Length/1024;
                LoadFile(file);
                progressBar1.Value = 0;
            }
            progressText.Text = string.Format("{0:n0} words read. Ready.", m_WordCount);
        }
    }
}