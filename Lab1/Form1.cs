using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.VisualBasic;



namespace Lab1
{
    public partial class Form1 : Form
    {
        private string CurentFilePass = "";
        private string filePath = "";
        DialogResult result;

      

        public Form1()
        {

            InitializeComponent();
            //Results.ReadOnly = true;
            
        }

        /// <summary>
        /// The history of the contents of the TextBox.
        /// </summary>
        private Stack<string> _editingHistory = new Stack<string>();

        /// <summary>
        /// The history of TextBox contents that have been undone and can be redone.
        /// </summary>
        private Stack<string> _undoHistory = new Stack<string>();


        private void RecordEdit()
        {
            _editingHistory.Push(Code.Text);
            Return.Enabled = true;
            отменитьToolStripMenuItem.Enabled = true;
            _undoHistory.Clear();
            Repeat.Enabled = false;
            повторитьToolStripMenuItem.Enabled = true;
        }

        private void Results_TextChanged(object sender, EventArgs e)
        {

        }

        private void Code_TextChanged(object sender, EventArgs e)
        {
            if (Code.Modified)
            {
                RecordEdit();
            }
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Return_Click(object sender, EventArgs e)
        {
            if (_editingHistory.Count == 0)
            {
                MessageBox.Show("Заполните поля для работы данной функции");
            }

            else
            {
                _undoHistory.Push(_editingHistory.Pop());
                Repeat.Enabled = true;
                повторитьToolStripMenuItem.Enabled = true;
                Code.Text = _editingHistory.Peek();
                Return.Enabled = _editingHistory.Count > 1;
                отменитьToolStripMenuItem.Enabled = _editingHistory.Count > 1;
            }




           
        } 

        

        private void Repeat_Click(object sender, EventArgs e)
        {

            if (_editingHistory.Count == 0)
            {
                MessageBox.Show("Заполните поля для работы данной функции");
            }
            else
            {
                _editingHistory.Push(_undoHistory.Pop());
                Repeat.Enabled = _undoHistory.Count > 0;
                повторитьToolStripMenuItem.Enabled = _editingHistory.Count > 0;
                Code.Text = _editingHistory.Peek();
                Return.Enabled = true;
                отменитьToolStripMenuItem.Enabled = true;
            }
        }


        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Code.Text != "")

            {
                result = MessageBox.Show("Сохранить текущий файл?", "Сообщение", MessageBoxButtons.YesNo );
                 if (result == DialogResult.Yes)
                 {

                   
                    if (File.Exists(CurentFilePass))
                    {
                        string content = Code.Text;
                        File.WriteAllText(CurentFilePass, content, System.Text.Encoding.UTF8);
                    }

                    else
                    {
                        выходToolStripMenuItem_Click(sender, e);
                    }
                }
                
               

            }

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|cpp files (*.cpp)|*.cpp|h files (*.h)| *.h|cs files (*.sc)|*.cs|py files (*.py)|*.py";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                filePath = openFile.FileName;
                Code.Text = File.ReadAllText(openFile.FileName);
                CurentFilePass = filePath;
            }
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                if (Code.Text != "")

                {
                    result = MessageBox.Show("Сохранить текущий файл?", "Сообщение", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        
                        if (File.Exists(CurentFilePass))
                        {
                            string content = Code.Text;
                            File.WriteAllText(CurentFilePass, content, System.Text.Encoding.UTF8);
                        }

                        else
                        {
                            выходToolStripMenuItem_Click(sender, e);
                        }
                    }


                }
                Code.Clear();
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (File.Exists(CurentFilePass))
            {
                string content = Code.Text;
                File.WriteAllText(CurentFilePass, content, System.Text.Encoding.UTF8);
            }

            else
            {
                выходToolStripMenuItem_Click(sender, e);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.DefaultExt = ".txt";
            savefile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|cpp files (*.cpp)|*.cpp|h files (*.h)| *.h|cs files (*.sc)|*.cs|py files (*.py)|*.py";
           

            if (savefile.ShowDialog() == System.Windows.Forms.DialogResult.OK && savefile.FileName.Length > 0)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName, true))
                {
                    sw.WriteLine(Code.Text);
                    filePath = savefile.FileName;
                    
                    CurentFilePass = filePath;
                    sw.Close();
                }
            }
        }

        private void повторитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_editingHistory.Count == 0)
            {
                MessageBox.Show("Заполните поля для работы данной функции");
            }

            else
            {

                _editingHistory.Push(_undoHistory.Pop());
                Repeat.Enabled = _undoHistory.Count > 0;
                повторитьToolStripMenuItem.Enabled = _editingHistory.Count > 0;
                Code.Text = _editingHistory.Peek();
                Return.Enabled = true;
                отменитьToolStripMenuItem.Enabled = true;
            }
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Code.Cut();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Code.Copy();
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Code.Paste();

        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Code.Text != "")

            {
                result = MessageBox.Show("Сохранить текущий файл?", "Сообщение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    
                    if (File.Exists(CurentFilePass))
                    {
                        string content = Code.Text;
                        File.WriteAllText(CurentFilePass, content, System.Text.Encoding.UTF8);
                    }

                    else
                    {
                        выходToolStripMenuItem_Click(sender, e);
                    }
                }


            }

            Close();
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Code.SelectedText = "";

        }

    private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Code.SelectAll();

        }

        private void пускToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Start_Click(sender, e);
        }

        private void справкаToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void Open_Click(object sender, EventArgs e)
        {
            if (Code.Text != "")

            {
                result = MessageBox.Show("Сохранить текущий файл?", "Сообщение", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                   
                    if (File.Exists(CurentFilePass))
                    {
                        string content = Code.Text;
                        File.WriteAllText(CurentFilePass, content, System.Text.Encoding.UTF8);
                    }

                    else
                    {
                        выходToolStripMenuItem_Click(sender, e);
                    }
                }


            }

            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*|cpp files (*.cpp)|*.cpp|h files (*.h)| *.h|cs files (*.sc)|*.cs|py files (*.py)|*.py";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                filePath = openFile.FileName;
                Code.Text = File.ReadAllText(openFile.FileName);
                CurentFilePass = filePath;
            }
        }

        private void Save_Click_1(object sender, EventArgs e)
        {
            if (File.Exists(CurentFilePass))
            {
                string content = Code.Text;
                File.WriteAllText(CurentFilePass, content, System.Text.Encoding.UTF8);
            }

            else
            {
                выходToolStripMenuItem_Click(sender, e);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            {
                if (Code.Text != "")

                {
                    result = MessageBox.Show("Сохранить текущий файл?", "Сообщение", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                       
                        if (File.Exists(CurentFilePass))
                        {
                            string content = Code.Text;
                            File.WriteAllText(CurentFilePass, content, System.Text.Encoding.UTF8);
                        }

                        else
                        {
                            выходToolStripMenuItem_Click(sender, e);
                        }
                    }


                }

                Code.Clear();
            }
        }

        private void вызовСправкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                Process.Start(@"Reference2\Reference.html");
                
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("About the program");
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                if (_editingHistory.Count == 0)
                {
                    MessageBox.Show("Заполните поля для работы данной функции");
                }

                else
                {
                    _undoHistory.Push(_editingHistory.Pop());
                    Repeat.Enabled = true;
                    повторитьToolStripMenuItem.Enabled = true;
                    Code.Text = _editingHistory.Peek();
                    Return.Enabled = _editingHistory.Count > 1;
                    отменитьToolStripMenuItem.Enabled = _editingHistory.Count > 1;

                }
            }
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            Code.Copy();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Code.Cut();
        }

        private void Reference_Click(object sender, EventArgs e)
        {
            Process.Start(@"Reference2\Reference.html");
        }
        private void Compiler_Load(object sender, EventArgs e)
        {

        }

        
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void OnFontChanged(object sender, EventArgs e)
        {
            Code.Font = new Font(Code.Font.FontFamily, int.Parse(FontBox.SelectedItem.ToString()));
            Results.Font = new Font(Results.Font.FontFamily, int.Parse(FontBox.SelectedItem.ToString()));

        } 

        private void Start_Click(object sender, EventArgs e)
        {

            Results.Items.Clear();
           
            string[] str = Code.Text.Split(new char[] { '\n' }); 

            for (int j = 0; j < str.Length; j++)
            {
                if (str[j].Contains('('))
                    str[j] = str[j].Replace("(", " ( ");
                if (str[j].Contains(')'))
                    str[j] = str[j].Replace(")", " ) ");
                Results.Items.Add(" Строка " + (j+1) + "\r\n");
                if (str.Length > 0)
                {
                    string temp_str = String.Empty;
                    var basicscaner = new FortranScaner(str[j]);
                    var symbol = basicscaner.Recursive_Syntaxis();
                    int i = 0;
                    while (i < symbol.Length)
                    {
                        if (symbol[i] == '|')
                        {
                            Results.Items.Add(temp_str);
                            temp_str = String.Empty;
                        }
                        else
                        {
                            temp_str += symbol[i];
                        }
                        i++;

                        
                    }
               

                }
               
               

            }
        }


    }

}

/*real :: jfnvjf(4645646456456)
complex : kmfvkfmv(5654646)
iiint :: 12kfmvkfv(gbjngjbgkbm)
character :: vmkfmvfkv(123*/









