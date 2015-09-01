using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FonteDeDados;

namespace ImportadorRunRunIT
{
    public partial class Form1 : Form
    {
        private FonteDeDadosOld testeFonteDeDados;
        private TipoFonteDados tipoSelecionado;

        public Form1()
        {
            InitializeComponent();
            tipoSelecionado = new TipoFonteDados();

       }

        private bool ValidaCamposConexao() {
            try
            {
                string tipoFonteSelecionada = cboTipoFonteDados.SelectedItem.ToString();



                switch (cboTipoFonteDados.SelectedItem.ToString())
                {
                    case "MSSQL Server":
                        tipoSelecionado = TipoFonteDados.SQLServer;
                        break;
                    default:
                        MessageBox.Show("Tipo de fonte de dados não suportada", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tipoFonteSelecionada = "";
                        break;
                }


                if (txtServidor.Text.Length == 0 || txtLogin.Text.Length == 0 || txtSenha.Text.Length == 0 || txtNomeDB.Text.Length == 0 || tipoFonteSelecionada.Length == 0)
                {

                    return false;

                }
                else {

                    return true;
                }



            }
            catch (Exception ex)
            {
                return false;
            }        
        }

        private void btnSelecionarStatusReport_Click(object sender, EventArgs e)
        {

            OpenFileDialog vAbreArq = new OpenFileDialog();
            vAbreArq.Filter = "*.*|*.*";
            vAbreArq.Title = "Selecione o Arquivo";

            if (vAbreArq.ShowDialog() == DialogResult.OK)
            {
                MapeadorStatusReport mapStatusReport = new MapeadorStatusReport();

                mapStatusReport.NomeArquivo = vAbreArq.SafeFileNames[0];
                mapStatusReport.CaminhoArquivo = vAbreArq.FileName;

                mapStatusReport.ServidorDB = "10.10.0.2";
                mapStatusReport.NomeDB = "ACE";
                mapStatusReport.LoginDB = "SA";
                mapStatusReport.SenhaDB = "kalango";

                mapStatusReport.ImportarParaDB();

                MessageBox.Show("Arquivo Importado", "Importador RunRunIT", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         
        }

        private void btnTestarConexao_Click(object sender, EventArgs e)
        {

            if (!ValidaCamposConexao())
            {
                MessageBox.Show("É necessário informar todos os dados de conexão", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else {

                string local = txtServidor.Text;
                string login = txtLogin.Text;
                string senha = txtSenha.Text;
                string arquivo = txtNomeDB.Text;

                testeFonteDeDados = new FonteDeDadosOld(tipoSelecionado, local, arquivo, login, senha);

                MessageBox.Show(testeFonteDeDados.TestarConexão());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CarregaConfig();
        }

        private void btnSelecionarStatusReport_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog vAbreArq = new OpenFileDialog();
            vAbreArq.Filter = "*.*|*.*";
            vAbreArq.Title = "Selecione o Arquivo";

            if (vAbreArq.ShowDialog() == DialogResult.OK)
            {
                MapeadorExcelStatusReport mapStatusReport = new MapeadorExcelStatusReport();

                mapStatusReport.NomeArquivo = vAbreArq.SafeFileNames[0];
                mapStatusReport.CaminhoArquivo = vAbreArq.FileName;

                mapStatusReport.ServidorDB = txtServidor.Text;
                mapStatusReport.NomeDB = txtNomeDB.Text;
                mapStatusReport.LoginDB = txtLogin.Text;;
                mapStatusReport.SenhaDB = txtSenha.Text;;

                mapStatusReport.ImportarParaDB();
            }

            MessageBox.Show("Arquivo Importado", "Importador RunRunIT", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            //Se selecionar a aba ações, é necessário ter preenchido os dados de conexão com o BD
            if (tabControl1.SelectedTab.Text == "Ações")
            {
                if(!ValidaCamposConexao()){

                    MessageBox.Show("É necessário informar todos os dados de conexão antes de executar as ações.", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    tabControl1.SelectedIndex = 0;
                }
            }

        }

        private void CarregaConfig() 
        {
            FonteDeDadosOld arquivoConf = new FonteDeDadosOld();


            arquivoConf.Arquivo = "ImportadorRunRunConf.Imp";
            arquivoConf.Local = AppDomain.CurrentDomain.BaseDirectory;
            arquivoConf.Origem = TipoFonteDados.TXT;
            arquivoConf.PrimeiraLinhaCabeçalho = false;
            arquivoConf.SeparadorTXT = ':';

            DataTableReader dtrConf = arquivoConf.NewReader("");


            while (dtrConf.Read())
            {
                //Ajusta o combo
                if (dtrConf[0].ToString() == "TIPO_CONEXAO_DEFAULT")
                {
                    if (dtrConf[1].ToString() == "MSSQL")
                    {
                        cboTipoFonteDados.SelectedIndex = 0;
                    }
                }


                if (dtrConf[0].ToString() == "SEVIDOR_DB_DEFAULT")
                {
                    txtServidor.Text = dtrConf[1].ToString();
                    
                }


                if (dtrConf[0].ToString() == "NOME_DB_DEFAULT")
                {
                    txtNomeDB.Text = dtrConf[1].ToString();
                }


                if (dtrConf[0].ToString() == "LOGIN_DB_DEFAULT")
                {
                    txtLogin.Text = dtrConf[1].ToString();
                }

                if (dtrConf[0].ToString() == "SENHA_DB_DEFAULT")
                {
                    txtSenha.Text = dtrConf[1].ToString();
                }
                
            
            }
            



        }




    }
}
