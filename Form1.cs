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

namespace Editor_de_texto
{
    public partial class Form1 : Form
    {
        StringReader leitura = null;
        
        public Form1()
        {
            InitializeComponent();
        }

        public void Novo()
        {
            richTextBox.Clear();
            richTextBox.Focus();
        }

        private void novoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Novo();
        }

        private void btn_novo_Click(object sender, EventArgs e)
        {
            Novo();
        }

        public void Salvar()
        {
            this.saveFileDialog.Filter = "Todos arquivos(*.*)|*.*";
            try
            {
                if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FileStream arquivo = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter streamWriter = new StreamWriter(arquivo);
                    streamWriter.Flush();
                    streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                    streamWriter.Write(this.richTextBox.Text);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
            } catch (Exception ex) {
                MessageBox.Show("Erro na gravação: " + ex.Message, "Erro ao gravar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        private void btn_salvar_Click(object sender, EventArgs e)
        {
            Salvar();
        }

        public void Abrir()
        {
            this.openFileDialog.Title = "Abrir arquivo";
            openFileDialog.InitialDirectory = @"D:\";
            openFileDialog.Filter = "Todos arquivos(*.*)|*.*";

            DialogResult dr = this.openFileDialog.ShowDialog();
            if(dr == DialogResult.OK)
            {
                try
                {
                    FileStream arquivo = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                    StreamReader streamReader = new StreamReader(arquivo);
                    streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                    this.richTextBox.Text = "";
                    string linha = streamReader.ReadLine();
                    while (linha != null)
                    {
                        this.richTextBox.Text += linha + "\n";
                        linha = streamReader.ReadLine();
                    }
                    streamReader.Close();
                }
                catch (Exception ex) {
                    MessageBox.Show("Erro de leitura: " + ex.Message, "Erro ao ler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void btn_abrir_Click(object sender, EventArgs e)
        {
            Abrir();
        }

        private void Copiar()
        {
            if(this.richTextBox.SelectionLength > 0)
            {
                richTextBox.Copy();
            }
        }

        private void btn_copiar_Click(object sender, EventArgs e)
        {
            Copiar();
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copiar();
        }

        private void Colar()
        {
            richTextBox.Paste();
        }

        private void colarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Colar();
        }

        private void btn_colar_Click(object sender, EventArgs e)
        {
            Colar();
        }

        private void AplicarEstilo(RichTextBox rtx, FontStyle ft)
        {
            Font fonteSelecionada = rtx.SelectionFont;
            
            ft = fonteSelecionada.Style ^ ft;
            rtx.SelectionFont = new Font(fonteSelecionada, ft);
        }

        private void btn_negrito_Click(object sender, EventArgs e)
        {
            AplicarEstilo(richTextBox, FontStyle.Bold);
        }

        private void negritoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AplicarEstilo(richTextBox, FontStyle.Bold);
        }

        private void btn_italico_Click(object sender, EventArgs e)
        {
            AplicarEstilo(richTextBox, FontStyle.Italic);
        }

        private void itálicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AplicarEstilo(richTextBox, FontStyle.Italic);
        }

        private void btn_sublinhado_Click(object sender, EventArgs e)
        {
            AplicarEstilo(richTextBox, FontStyle.Underline);
        }

        private void sublinhadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AplicarEstilo(richTextBox, FontStyle.Underline);
        }

        private void Alinhamento(HorizontalAlignment alinhamento)
        {
            richTextBox.SelectionAlignment = alinhamento;
        }

        private void btn_esquerda_Click(object sender, EventArgs e)
        {
            Alinhamento(HorizontalAlignment.Left);
        }

        private void esquerdaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Alinhamento(HorizontalAlignment.Left);
        }

        private void btn_centro_Click(object sender, EventArgs e)
        {
            Alinhamento(HorizontalAlignment.Center);
        }

        private void centralizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Alinhamento(HorizontalAlignment.Center);
        }

        private void btn_direita_Click(object sender, EventArgs e)
        {
            Alinhamento(HorizontalAlignment.Right);
        }

        private void direitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Alinhamento(HorizontalAlignment.Right);
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDialog.Document = printDocument;
            string texto = this.richTextBox.Text;
            leitura = new StringReader(texto);
            if(this.printDialog.ShowDialog() == DialogResult.OK)
            {
                this.printDocument.Print();
            }
        }

        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            float linhasPagina = 0;
            float posY = 0;
            int cont = 0;
            float margemEsquerda = e.MarginBounds.Left - 50;
            float margemSuperior = e.MarginBounds.Top - 50;

            if(margemEsquerda < 5)
            {
                margemEsquerda = 20;
            }

            if (margemSuperior < 5)
            {
                margemSuperior = 20;
            }

            string linha = null;
            Font fonte = this.richTextBox.Font;
            SolidBrush pincel = new SolidBrush(Color.Black);
            linhasPagina = e.MarginBounds.Height / fonte.GetHeight(e.Graphics);
            linha = leitura.ReadLine();
            while(cont < linhasPagina)
            {
                posY = (margemSuperior + (cont * fonte.GetHeight(e.Graphics)));
                e.Graphics.DrawString(linha, fonte, pincel, margemEsquerda, posY, new StringFormat());
                cont++;
                linha = leitura.ReadLine();
            }
            if(linha != null)
            {
                e.HasMorePages = true;
            } else
            {
                e.HasMorePages = false;
            }
            pincel.Dispose();
        }

        private void desfazerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox.CanUndo)
            {
                richTextBox.Undo(); 
            }
        }

        private void sairToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
