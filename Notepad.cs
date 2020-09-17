using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

namespace Notepad
{
    public partial class NotepadForm : Form
    {
        private String fileName;

        private bool isDraggingSpravka;
        private bool isDraggingStroka;
        private bool isDraggingReplace;
        private bool isDraggingFind;
        private int currentX, currentY;

        public NotepadForm()
        {
            InitializeComponent();
            richTextBox.MouseWheel += new MouseEventHandler(OnMouseWheel);
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            labelZoom.Text = "Масштаб: " + richTextBox.ZoomFactor * 100 + "%";
        }

        private void UpdateoFD()
        {
            fileName = oFD.FileName;
            if (fileName != null) this.Text = fileName;
            отменитьToolStripMenuItem.Enabled = false;
        }
        private void UpdatesFD()
        {
            fileName = sFD.FileName;
           if(fileName!=null) this.Text = fileName;
            отменитьToolStripMenuItem.Enabled = false;
        }

        private void СохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            sFD.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            sFD.FilterIndex = 1; 
            sFD.RestoreDirectory = true;
            sFD.Title = "Сохранение файла";
            
            if (sFD.ShowDialog() == DialogResult.OK)
            {
                SaveFiles();
            }
        }

        private void СохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(File.Exists(fileName))
            {
                SaveFiles();
            }
            else
            {
                СохранитьКакToolStripMenuItem_Click(sender, e);
            }
        }

        private void SaveFiles()
        {
            if (fileName == null) UpdatesFD();
                StreamWriter fileOut = new StreamWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write));
                fileOut.Write(richTextBox.Text);
                fileOut.Flush();
                fileOut.Close();
                UpdatesFD();
        }

        private void OpenFiles()
        {
            UpdateoFD();
            StreamReader fileIn = new StreamReader(new FileStream(fileName, FileMode.Open, FileAccess.Read));
            richTextBox.Text = fileIn.ReadToEnd();
            fileIn.Close();
        }

        private void ОткрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            oFD.Title = "Открытие файла";
            oFD.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            oFD.RestoreDirectory = true;
            if (oFD.ShowDialog() == DialogResult.OK)
            {
                OpenFiles();
            }
        }

        private void НовоеОкноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Writer.exe");
        }

        private void СоздатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogExitOrCreate(sender, e);
        }

        private void ВыходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogExitOrCreate(sender,e);
            Application.Exit();

        }
        private void DialogExitOrCreate(object sender, EventArgs e)
        {
            if (richTextBox.Text.Length > 0)
            {
                DialogResult result = MessageBox.Show("Сохранить текущий файл?", "Внимание!!", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    СохранитьКакToolStripMenuItem_Click(sender, e);
                }
                if (result == DialogResult.No)
                {
                    richTextBox.Text = "";
                }
            }
        }

        private void СтрокаСостоянияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            строкаСостоянияToolStripMenuItem.Text = panel.Visible ? "Строка состояния" : "Строка состояния +";
            panel.Visible = !panel.Visible;
        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            labelCharCount.Text = "Символы: " + richTextBox.Text.Length;
            labelLinesCount.Text = "Линии: " + richTextBox.Lines.Length;
            if (!this.Text.StartsWith("*")) this.Text = "*" + this.Text;
            отменитьToolStripMenuItem.Enabled = true;
        }

        private void ПереносПоСловамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            переносПоСловамToolStripMenuItem.Text = richTextBox.WordWrap ? "Перенос по словам" : "Перенос по словам +";
            richTextBox.WordWrap = !richTextBox.WordWrap;
        }

        private void УвеличитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor += 0.1f;
            OnMouseWheel(null, null);
        }

        private void УменьшитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor -= 0.1f;
            OnMouseWheel(null, null);
        }

        private void ВосстановитьМасштабПоУмолчаниюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.ZoomFactor = 1.0f;
            OnMouseWheel(null, null);
        }

        private void ШрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(fD.ShowDialog() == DialogResult.OK)
            {
                richTextBox.Font = fD.Font;
            }
        }

        private void ВремяИДатаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Text += DateTime.Now;
        }

        private void ВыделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.SelectAll();
        }

        private void ПерейтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelStroka.Visible = true;
        }

        private void ПечатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintPageHandler;
            PrintDialog printDialog = new PrintDialog
            {
                Document = printDocument
            };
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDialog.Document.Print();
            }
        }
        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(richTextBox.Text, richTextBox.Font, Brushes.Black, 0, 0);
        }

        private void ОПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelSpravka.Visible = true;
        }

        private void BtnCloseSpravka_Click(object sender, EventArgs e)
        {
            panelSpravka.Visible = false;
        }

        private void ОтменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Undo();
        }

        private void ВырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Cut();
        }

        private void КопироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox.Copy();
        }

        private void ВставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int charCount = richTextBox.SelectionStart;
            richTextBox.Text = richTextBox.Text.Substring(0, charCount) + Clipboard.GetText() + richTextBox.Text.Substring(charCount);
            richTextBox.SelectionStart = charCount+Clipboard.GetText().Length;
        }

        private void УдалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int charCount = richTextBox.SelectionStart;
            richTextBox.Text = richTextBox.Text.Substring(0, charCount)+richTextBox.Text.Substring(charCount+richTextBox.SelectionLength);
            richTextBox.SelectionStart = charCount;
        }

        private void ButtonCloseStroka_Click(object sender, EventArgs e)
        {
            panelStroka.Visible = false;
        }

        private void ButtonStrokaFind_Click(object sender, EventArgs e)
        {
            int stroka; 
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
            int count = 0;
            for (int i = 0; i < stroka; i++)
            {
                count += s.IndexOf("\n");
                s = s.Substring(s.IndexOf("\n") + 1);
                count++;
            }
            count--;
            if (stroka - 1 < 0)
            {
                MessageBox.Show("Строка не найдена", "Внимание!!", MessageBoxButtons.OK);
                return;
            }
            count -= (richTextBox.Lines[stroka - 1].Length);
            richTextBox.SelectionStart = count;
            richTextBox.Focus();
        }

        private void ButtonCloseReplace_Click(object sender, EventArgs e)
        {
            panelReplace.Visible = false;
        }

        private void ЗаменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelReplace.Visible = true;
        }

        private void ButtonReplaceAll_Click(object sender, EventArgs e)
        {
           richTextBox.Text = richTextBox.Text.Replace(textBoxChto.Text, textBoxChem.Text);
        }

        private void ButtonReplaceFirst_Click(object sender, EventArgs e)
        {
          int chto = richTextBox.Text.IndexOf(textBoxChto.Text);
           richTextBox.Text = richTextBox.Text.Substring(0, chto) + textBoxChem.Text + richTextBox.Text.Substring(chto + textBoxChto.Text.Length);
        }

        private void ButtonReplaceLast_Click(object sender, EventArgs e)
        {
            int chto = richTextBox.Text.LastIndexOf(textBoxChto.Text);
            richTextBox.Text = richTextBox.Text.Substring(0, chto) + textBoxChem.Text + richTextBox.Text.Substring(chto + textBoxChto.Text.Length);
        }

        private void ButtonCloseFind_Click(object sender, EventArgs e)
        {
            panelFind.Visible = false;
        }

        private void НайтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelFind.Visible = true;
            radioButtonDown.Checked = false;
            radioButtonUp.Checked = false;
        }

        private void НайтиДалееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelFind.Visible = true;
            radioButtonDown.Checked = true;
        }

        private void НайтиРанееToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panelFind.Visible = true;
            radioButtonUp.Checked = true;
        }

        private void ButtonFind_Click(object sender, EventArgs e)
        {
            string rt = richTextBox.Text;
            string tb = textBoxFind.Text;
            int charCount = richTextBox.SelectionStart;
            if (radioButtonDown.Checked)
            {
                charCount +=1;
                if (charCount > rt.Length) return;
                rt = rt.Substring(charCount);
                charCount+=rt.IndexOf(tb);
            }
            if (radioButtonUp.Checked)
            {
                charCount -=1;
                if(charCount<0) charCount = 0;
                rt = rt.Substring(0, charCount+tb.Length);
                charCount = rt.LastIndexOf(tb);
                if (charCount < 0) return;
                
            }
            richTextBox.SelectionStart = charCount;
            richTextBox.SelectionLength = tb.Length;
            richTextBox.Focus();
        }

        private void PanelSpravka_MouseDown(object sender, MouseEventArgs e)
        {
             isDraggingSpravka = PanelMouseDown(e);
        }

        private void PanelSpravka_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingSpravka)
            {
                PanelMouseMove(panelSpravka, e);
            }
        }

        private void PanelSpravka_MouseUp(object sender, MouseEventArgs e)
        {
            isDraggingSpravka = PanelMouseUp(e);
        }

        private void PanelStroka_MouseDown(object sender, MouseEventArgs e)
        {
            isDraggingStroka = PanelMouseDown(e);
        }

        private void PanelStroka_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingStroka)
            {
                PanelMouseMove(panelStroka, e);
            }
        }

        private void PanelStroka_MouseUp(object sender, MouseEventArgs e)
        {
            isDraggingStroka = PanelMouseUp(e);
        }

        private void PanelReplace_MouseDown(object sender, MouseEventArgs e)
        {
             isDraggingReplace = PanelMouseDown(e);
        }

        private void PanelReplace_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingReplace)
            {
                PanelMouseMove(panelReplace, e);
            }
        }

        private void PanelReplace_MouseUp(object sender, MouseEventArgs e)
        {
           isDraggingReplace = PanelMouseUp(e);
        }

        private void PanelFind_MouseDown(object sender, MouseEventArgs e)
        {
            isDraggingFind = PanelMouseDown(e);
        }

        private void PanelFind_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDraggingFind)
            {
                PanelMouseMove(panelFind, e);
            }
        }

        private void PanelFind_MouseUp(object sender, MouseEventArgs e)
        {
            isDraggingFind = PanelMouseUp(e);
        }

        private bool PanelMouseUp(MouseEventArgs e)
        {
            return !(e.Button == MouseButtons.Left);
        }

        private bool PanelMouseDown(MouseEventArgs e)
        {
            currentX = e.X;
            currentY = e.Y;
            return (e.Button == MouseButtons.Left);
        }
        private void PanelMouseMove(Panel panel, MouseEventArgs e)
        {
            panel.Top += (e.Y - currentY);
            panel.Left += (e.X - currentX);
        }
    }
}