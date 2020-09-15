using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Writer
{
    public partial class NotepadForm : Form
    {
        private String fileName;
        bool isPowerSost = false;
        bool isWorldWrap = false;
        int charCount = 0;

        bool isDraggingSpravka = false;
        int SpravkacurrentX, SpravkacurrentY;
        bool isDraggingStroka = false;
        int StrokacurrentX, StrokacurrentY;
        bool isDraggingReplace = false;
        int ReplacecurrentX, ReplacecurrentY;
        bool isDraggingFind = false;
        int FindcurrentX, FindcurrentY;

        public NotepadForm()
        {
            InitializeComponent();
            richTextBox.MouseWheel += new MouseEventHandler(onMouseWheel);
        }

        private void onMouseWheel(object sender, MouseEventArgs e)
        {
            labelZoom.Text = "Масштаб: " + richTextBox.ZoomFactor * 100 + "%";
        }

        private void updateoFD()
        {
            fileName = oFD.FileName;
            if (fileName != null) this.Text = fileName;
            отменитьToolStripMenuItem.Enabled = false;
        }
        private void updatesFD()
        {
            fileName = sFD.FileName;
           if(fileName!=null) this.Text = fileName;
            отменитьToolStripMenuItem.Enabled = false;
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            sFD.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sFD.FilterIndex = 1; 
            sFD.RestoreDirectory = true;
            sFD.Title = "Сохранение файла";
            
            if (sFD.ShowDialog() == DialogResult.OK)
            {
                saveFiles();
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(File.Exists(fileName))
            {
                saveFiles();
            }
            else
            {
                сохранитьКакToolStripMenuItem_Click(sender, e);
            }
        }

        private void saveFiles()
        {
            if (fileName == null) updatesFD();
                StreamWriter fileOut = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write));
                fileOut.Write(richTextBox.Text);
                fileOut.Flush();
                fileOut.Close();
                updatesFD();
        }

        private void openFiles()
        {
            updateoFD();
            StreamReader fileIn = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read));
            richTextBox.Text = fileIn.ReadToEnd();
            fileIn.Close();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oFD.Title = "Открытие файла";
            oFD.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            oFD.RestoreDirectory = true;
            if (oFD.ShowDialog() == DialogResult.OK)
            {
                openFiles();
            }
        }

        private void новоеОкноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Writer.exe");
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogExitOrCreate(sender, e);
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dialogExitOrCreate(sender,e);
            Application.Exit();

        }
        private void dialogExitOrCreate(object sender, EventArgs e)
        {
            if (richTextBox.Text.Length > 0)
            {
                DialogResult result = MessageBox.Show("Сохранить текущий файл?", "Внимание!!", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    сохранитьКакToolStripMenuItem_Click(sender, e);
                }
                if (result == DialogResult.No)
                {
                    richTextBox.Text = "";
                }
            }
        }

        private void строкаСостоянияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isPowerSost = !isPowerSost;
            if (isPowerSost)
            {
                строкаСостоянияToolStripMenuItem.Text = "Строка состояния +";
                panel.Visible = true;
                labelCharCount.Text = "Символы: " + richTextBox.Text.Length;
                labelLinesCount.Text = "Линии: " + richTextBox.Lines.Length;
            }
            else
            {
                строкаСостоянияToolStripMenuItem.Text = "Строка состояния";
                panel.Visible = false;
            }
            
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            charCount = richTextBox.Text.Length;
            labelCharCount.Text = "Символы: " + charCount;
            labelLinesCount.Text = "Линии: " + richTextBox.Lines.Length;
            if (!this.Text.StartsWith("*")) this.Text = "*" + this.Text;
            отменитьToolStripMenuItem.Enabled = true;

        }

        private void переносПоСловамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isWorldWrap)
            {
                richTextBox.WordWrap = false;
                isWorldWrap = !isWorldWrap;
                переносПоСловамToolStripMenuItem.Text = "Перенос по словам";
            }
            else
            {
                richTextBox.WordWrap = true;
                isWorldWrap = !isWorldWrap;
                переносПоСловамToolStripMenuItem.Text = "Перенос по словам +";
            }
        }

        private void увеличитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor += 0.1f;
            onMouseWheel(null, null);
        }

        private void уменьшитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor -= 0.1f;
            onMouseWheel(null, null);
        }

        private void восстановитьМасштабПоУмолчаниюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 1.0f;
            onMouseWheel(null, null);
        }

        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(fD.ShowDialog() == DialogResult.OK)
            {
                richTextBox.Font = fD.Font;
            }
        }

        private void времяИДатаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Text += DateTime.Now;
        }

        private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.SelectAll();
        }

        private void перейтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelStroka.Visible = true;
        }

        private void печатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintPageHandler;
            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDialog.Document.Print();
            }
        }
        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox.Text, richTextBox.Font, Brushes.Black, 0, 0);
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelSpravka.Visible = true;
        }

        private void btnCloseSpravka_Click(object sender, EventArgs e)
        {
            panelSpravka.Visible = false;
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Undo();
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Cut();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Copy();
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int charCount = richTextBox.SelectionStart;
            richTextBox.Text = richTextBox.Text.Substring(0, charCount) + Clipboard.GetText() + richTextBox.Text.Substring(charCount);
            richTextBox.SelectionStart = charCount+Clipboard.GetText().Length;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int charCount = richTextBox.SelectionStart;
            richTextBox.Text = richTextBox.Text.Substring(0, charCount)+richTextBox.Text.Substring(charCount+richTextBox.SelectionLength);
            richTextBox.SelectionStart = charCount;
        }

        private void buttonCloseStroka_Click(object sender, EventArgs e)
        {
            panelStroka.Visible = false;
        }

        private void buttonStrokaFind_Click(object sender, EventArgs e)
        {
            int stroka = 0; 
            if (textBoxStroka.Text == null) return;
            try
            {
                stroka = Convert.ToInt32(textBoxStroka.Text);
            }catch(Exception)
            {
                MessageBox.Show("Ошибка", "Внимание!!", MessageBoxButtons.OK);
                return;
            }
            if (richTextBox.Lines.Length < stroka)
            {
                MessageBox.Show("Превышено допустимое количество строк", "Внимание!!", MessageBoxButtons.OK);
                return;
            }

            String s = richTextBox.Text;
            s = s.Replace("\n", "☺");
            int count = 0;
            for(int i = 0; i < stroka; i++)
            {
                count += s.IndexOf("☺");
                s = s.Substring(s.IndexOf("☺")+1);
                count++;
            }
            count--;
            count -= (richTextBox.Lines[stroka - 1].Length);
            richTextBox.SelectionStart = count;
            richTextBox.Focus();
        }

        private void buttonCloseReplace_Click(object sender, EventArgs e)
        {
            panelReplace.Visible = false;
        }

        private void заменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelReplace.Visible = true;
        }

        private void buttonReplaceAll_Click(object sender, EventArgs e)
        {
           richTextBox.Text = richTextBox.Text.Replace(textBoxChto.Text, textBoxChem.Text);
        }

        private void buttonReplaceFirst_Click(object sender, EventArgs e)
        {
          int chto = richTextBox.Text.IndexOf(textBoxChto.Text);
           richTextBox.Text = richTextBox.Text.Substring(0, chto) + textBoxChem.Text + richTextBox.Text.Substring(chto + textBoxChto.Text.Length);
        }

        private void buttonReplaceLast_Click(object sender, EventArgs e)
        {
            int chto = richTextBox.Text.LastIndexOf(textBoxChto.Text);
            richTextBox.Text = richTextBox.Text.Substring(0, chto) + textBoxChem.Text + richTextBox.Text.Substring(chto + textBoxChto.Text.Length);
        }

        private void buttonCloseFind_Click(object sender, EventArgs e)
        {
            panelFind.Visible = false;

        }

        private void найтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelFind.Visible = true;
            radioButtonDown.Checked = false;
            radioButtonUp.Checked = false;

        }

        private void найтиДалееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelFind.Visible = true;
            radioButtonDown.Checked = true;
        }

        private void найтиРанееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelFind.Visible = true;
            radioButtonUp.Checked = true;

        }

        private void buttonFind_Click(object sender, EventArgs e)
        {
            richTextBox.Focus();
            string rt = richTextBox.Text;
            string tb = textBoxFind.Text;
            if (radioButtonDown.Checked)
            {
                int charCount = richTextBox.SelectionStart+1;
                if (charCount > rt.Length) return;
                rt = rt.Substring(charCount);
                charCount+=rt.IndexOf(tb);
                richTextBox.SelectionStart = charCount;
                richTextBox.SelectionLength = tb.Length;
                richTextBox.Focus();
            }
            if (radioButtonUp.Checked)
            {
                int charCount = richTextBox.SelectionStart-1;
                if(charCount<0) return;
                rt = rt.Substring(0, charCount);
                charCount = rt.LastIndexOf(tb);
                if (charCount < 0) return;
                richTextBox.SelectionStart = charCount;
                richTextBox.SelectionLength = tb.Length;
                richTextBox.Focus();
            }
        }

        private void panelSpravka_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraggingSpravka = true;
                SpravkacurrentX = e.X;
                SpravkacurrentY = e.Y;
            }
        }

        private void panelSpravka_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingSpravka)
            {
                panelSpravka.Top = panelSpravka.Top + (e.Y - SpravkacurrentY);
                panelSpravka.Left = panelSpravka.Left + (e.X - SpravkacurrentX);
            }
        }

        private void panelSpravka_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraggingSpravka = false;
            }
        }

        private void panelStroka_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraggingStroka = true;
                StrokacurrentX = e.X;
                StrokacurrentY = e.Y;
            }
        }

        private void panelStroka_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingStroka)
            {
                panelStroka.Top = panelStroka.Top + (e.Y - StrokacurrentY);
                panelStroka.Left = panelStroka.Left + (e.X - StrokacurrentX);
            }
        }

        private void panelStroka_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraggingStroka = false;
            }
        }

        private void panelReplace_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraggingReplace = true;
                ReplacecurrentX = e.X;
                ReplacecurrentY = e.Y;
            }
        }

        private void panelReplace_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingReplace)
            {
                panelReplace.Top = panelReplace.Top + (e.Y - ReplacecurrentY);
                panelReplace.Left = panelReplace.Left + (e.X - ReplacecurrentX);
            }
        }


        private void panelReplace_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraggingReplace = false;
            }
        }

        private void panelFind_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraggingFind = true;
                FindcurrentX = e.X;
                FindcurrentY = e.Y;
            }
        }

        private void panelFind_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingFind)
            {
                panelFind.Top = panelFind.Top + (e.Y - FindcurrentY);
                panelFind.Left = panelFind.Left + (e.X - FindcurrentX);
            }
        }

        private void panelFind_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDraggingFind = false;
            }
        }
    }
}
