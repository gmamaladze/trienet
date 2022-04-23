// This code is distributed under MIT license. Copyright (c) 2013 George Mamaladze
// See license.txt or http://opensource.org/licenses/mit-license.php
using Gma.DataStructures.StringSearch;
using System.Diagnostics;

namespace Gma.DataStructures.StringSearch.DemoApp {
    public partial class MainForm : Form {
        private readonly UkkonenTrie<string> m_Trie;
        private static readonly char[] delimiters = new char[] { ' ', '\r', '\n' };
        private long m_WordCount;

        public MainForm() {
            InitializeComponent();
            m_Trie = new UkkonenTrie<string>(3);
            folderName.Text =
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "texts");
        }

        private void LoadFile(string fileName) {
            var word = File.ReadAllText(fileName);
            m_Trie.Add(word, Path.GetFileName(fileName));
            m_WordCount += word.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
            Debug.WriteLine($"Loaded {word.Length} characters.");
            Debug.WriteLine($"Trie size = {m_Trie.Size}");
        }

        private void UpdateProgress(int position) {
            progressBar1.Value = Math.Min(position, progressBar1.Maximum);
            Application.DoEvents();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            string text = textBox1.Text;
            if (string.IsNullOrEmpty(text) || text.Length < 3) return;
            var result = m_Trie.RetrieveSubstrings(text).ToArray();
            listBox1.Items.Clear();
            foreach (var wordPosition in result) {
                listBox1.Items.Add(wordPosition);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            var item = (WordPosition<string>)listBox1.SelectedItem;
            var word = File.ReadAllText(Path.Combine("texts", item.Value));
            const int bifferSize = 300;
            int position = Math.Max((int)item.CharPosition - bifferSize / 2, 0);
            string line = word.Substring(position, bifferSize);
            richTextBox1.Text = line;

            string serachText = textBox1.Text;
            int index = richTextBox1.Text.IndexOf(serachText, StringComparison.InvariantCultureIgnoreCase);
            if (index < 0) return;
            richTextBox1.Select(index, serachText.Length);
            richTextBox1.SelectionBackColor = Color.Yellow;
            richTextBox1.DeselectAll();
        }

        private void buttonBrowse_Click(object sender, EventArgs e) {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = folderName.Text;
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            folderName.Text = folderBrowserDialog.SelectedPath;
        }

        private void btnLoad_Click(object sender, EventArgs e) {
            LoadAll();
        }

        private void LoadAll() {
            m_WordCount = 0;
            string path = folderName.Text;
            if (!Directory.Exists(path)) return;
            string[] files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
            progressBar1.Minimum = 0;
            progressBar1.Maximum = files.Length;
            progressBar1.Step = 1;
            for (int index = 0; index < files.Length; index++) {
                string file = files[index];
                progressText.Text =
                    string.Format(
                        "Processing file {0} of {1}: [{2}]",
                        index + 1,
                        files.Length,
                        Path.GetFileName(file));
                Application.DoEvents();

                LoadFile(file);
                UpdateProgress(index + 1);
            }
            progressText.Text = string.Format("{0:n0} words read. Ready.", m_WordCount);
            UpdateProgress(0);
        }
    }
}