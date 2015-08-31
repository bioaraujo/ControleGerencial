using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;


namespace Dados
{
    public enum TipoFonteDados
    {
        SQLServer = 1,
        Excel = 2,
        TXT = 3
    }

    /// <summary>
    /// Classe destinada para acessar diferentes fontes de dados e retornar objetos da camada "desconectado" do .NET, DataTables e DataTablesReaders
    /// </summary>
    public class FonteDeDadosOld
    {

        #region "Atributos"

        private string _stringConexao;

        private TipoFonteDados _tipoFonteDados;
        private DbConnection _conexao;
        private DbCommand _comando;
        private DbDataAdapter _adaptador;

        private StreamReader _leitorTxt;

        private string _local = "N/I";
        private string _arquivo = "N/I";
        private string _login = "N/I";
        private string _senha = "N/I";

        private char _separadorTxt = '\t';

        private bool _primeiraLinhaCabecalho = false;


        #endregion

        #region "Propriedades"
        public TipoFonteDados Origem
        {
            get { return _tipoFonteDados; }
            set { _tipoFonteDados = value; }
        }

        public string StatusConexao
        {
            get
            {
                return _conexao.State.ToString();
            }
        }
        /// <summary>
        /// Endereço do servidor no caso de utilizar um SGDB ou local do arquivo Excel/TXT.
        /// </summary>
        public string Local
        {

            get { return _local; }
            set { _local = value; }
        }
        /// <summary>
        /// Esse atributo representa o nome do Banco de dados em caso de um SGDB, nome do arquivo Excel ou TXT.
        /// </summary>
        public string Arquivo
        {

            get { return _arquivo; }
            set { _arquivo = value; }
        }

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }

        public string Senha
        {
            get { return _senha; }
            set { _senha = value; }
        }

        /// <summary>
        /// Determina se a promeira linha do arquivo, excel ou txt, é um cabeçalho.
        /// O valor padrão é false.
        /// </summary>
        public bool PrimeiraLinhaCabeçalho
        {
            get { return _primeiraLinhaCabecalho; }
            set { _primeiraLinhaCabecalho = value; }

        }

        /// <summary>
        /// Informa o separado no caso de fonte de dados em arquivo TXT
        /// O valor padrão é "tab".
        /// </summary>
        public char SeparadorTXT 
        {
            get { return _separadorTxt; }
            set { _separadorTxt = value; }        
        }

        #endregion

        #region "Método Construtor"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fonteDeDados">Tipo entre Excel, SQLServer ou arquivo TXT.</param>
        /// <param name="Local">Esse atributo representa o endereço do servidor do banco de dados, o local de grvação do arquivo Excel ou txt.</param>
        /// <param name="Arquivo">Esse atributo representa o nome do Banco de dados em caso de um SGDB,o nome da planilha Excel ou o nome do arquivo TXT.</param>
        /// <param name="Login">Login para SQLServer.</param>
        /// <param name="Senha">Senha para o SQLServer.</param>
        public FonteDeDadosOld(TipoFonteDados fonteDeDados, string Local, string Arquivo, string Login, string Senha)
        {
            try
            {

                _local = Local;
                _arquivo = Arquivo;
                _login = Login;
                _senha = Senha;
                _tipoFonteDados = fonteDeDados;

                if (_tipoFonteDados == TipoFonteDados.SQLServer)
                {
                    _stringConexao = "Data Source=" + _local + ";Initial Catalog=" + _arquivo + ";User Id=" + _login + ";Password=" + _senha;
                    _conexao = new SqlConnection();
                    _comando = new SqlCommand();
                    _adaptador = new SqlDataAdapter();

                }


                if (_tipoFonteDados == TipoFonteDados.Excel)
                {


                    _stringConexao = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + _local + "\\" + _arquivo;

                    if (_primeiraLinhaCabecalho)
                    {

                        _stringConexao += ";Extended Properties='Excel 8.0; HDR=YES'";

                    }
                    else
                    {

                        _stringConexao += ";Extended Properties='Excel 8.0; HDR=NO'";

                    }


                    _conexao = new OleDbConnection(_stringConexao);
                    _comando = new OleDbCommand();
                    _adaptador = new OleDbDataAdapter();
                }


 
                if (_tipoFonteDados == TipoFonteDados.TXT)
                {

                    ConectaArquivoTXT();


                }



            }
            catch (Exception ex)
            {
                //clsUtil.Gravalog("IsicLibrary.dll", "clsBancoDeDados.clsBancoDeDados()", Environment.StackTrace.ToString(), ex.Message);
                throw ex;
            }
        }


        public FonteDeDadosOld() 
        { }


        #endregion

        #region "Metodos"

        private void ConectaArquivoTXT() 
        {
            try 
            {
                _leitorTxt = File.OpenText(_local + "\\" + _arquivo);
            }
            catch( Exception ex)
            {
                throw ex;
            }
 
        }
        private void ConectaBD()
        {
            try
            {

                _conexao.ConnectionString = _stringConexao;

                _conexao.Open();

            }
            catch (Exception ex)
            {
                //clsUtil.Gravalog("IsicLibrary.dll", "clsBancoDeDados.ConectaDB()", Environment.StackTrace.ToString(), ex.Message);
                throw ex;
            }
        }
        private void DesconectaDB()
        {
            try
            {

                if (_conexao.State.ToString() == "Open")
                {
                    _conexao.Close();

                }

            }
            catch (Exception ex)
            {
                //clsUtil.Gravalog("IsicLibrary.dll", "clsBancoDeDados.DesconectaDB()", ex.StackTrace, ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// Esse método retorna um ResultSet a partir do comando enviado. Caso o comando não tenha resultados, será retornado valor NULL.
        /// </summary>
        /// <param name="pstrQuery">Procedure ou query. </param>
        /// <returns>Um resultset do tipo DatatableReader</returns>
        /// <exception cref="Exception"></exception>
        public DataTableReader NewReader(string pstrQuery)
        {
            try
            {
                if (_tipoFonteDados == TipoFonteDados.TXT)
                {


                    //TO do
                    //Voltar um redaer a partir de um DataTable 
                    return null;

                }
                else 
                {
                    ConectaBD();

                    _comando.CommandText = pstrQuery;
                    _comando.Connection = _conexao;


                    DataTable dtResultado = new DataTable();

                    _adaptador.SelectCommand = _comando;

                    _adaptador.Fill(dtResultado);

                    if (dtResultado.Rows.Count == 0)
                    {
                        DesconectaDB();
                        //Retorna NULL pois no contexto do catch não existe o objecto 'dtResultado'
                        return null;
                    }
                    else
                    {
                        DesconectaDB();
                        return dtResultado.CreateDataReader();
                    }
                }



            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable NewDataTable(string pstrQuery)
        {

            try
            {
                DataTable dtResultado = new DataTable();

                if(_tipoFonteDados == TipoFonteDados.TXT)
                {
                    //TO DO
                    //PreparaStringConexao um DataTable a partir das linhas do arquivo
                    //Criar uma coluna tabela para cada coluna no arquivo
                    //Verificar se o param _primeiraLinhaCabecalho está marcado como sim 
                    //O nome das colunas será sequencial

                    string linhaLida = null;
                    int controleLinha = 0;
                    int numeroColuna = 1;
                    DataRow linhaArquivo;

                    //Verificae se o leitor está inicializado
                    if (_leitorTxt == null)
                    {
                        ConectaArquivoTXT();
                    }                 

                    while ((linhaLida = _leitorTxt.ReadLine()) != null)
                    {
                        controleLinha++;

                        string[] conteudoLinha = linhaLida.Split(_separadorTxt);

                        //se for a primeira linha, cria as colunas da tabela
                        if (controleLinha == 1)
                        {                           
                            

                            //Looping para criar as colunas dinamicamente.
                            while (numeroColuna <= conteudoLinha.Length)
                            {
                                dtResultado.Columns.Add(new DataColumn("col" + numeroColuna.ToString(), typeof(string)));

                                numeroColuna++;
                            }
                        }

                        //Se a primeira linha não for cabeçalho ou for da segunda lina em diante, faz a inclusão do conteudo da linha do arquivo
                        //No dtResultado.
                        if (!_primeiraLinhaCabecalho && controleLinha == 1 || controleLinha >1)
                        {

                            linhaArquivo = dtResultado.NewRow();

                            numeroColuna = 1;

                            while (numeroColuna <= conteudoLinha.Length)
                            {
                                string nomeColuna = "col" + numeroColuna.ToString();

                                linhaArquivo[nomeColuna] = conteudoLinha[numeroColuna - 1];

                                numeroColuna++;
                            }
                            dtResultado.Rows.Add(linhaArquivo);
                        }                        
                    }



                    if (dtResultado.Rows.Count == 0)
                    {
                     
                        return null;
                    }
                    else
                    {                     
                        return dtResultado;
                    }                                        
                }
                else
                {

                    ConectaBD();

                    _comando.CommandText = pstrQuery;
                    _comando.Connection = _conexao;
                  
                    _adaptador.SelectCommand = _comando;
                    _adaptador.Fill(dtResultado);

                    if (dtResultado.Rows.Count == 0)
                    {
                        DesconectaDB();
                        //Retorna NULL pois no contexto do catch não existe o objecto 'dtResultado'
                        return null;
                    }
                    else
                    {
                        DesconectaDB();
                        return dtResultado;
                    }

                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }

        public int Execute(string pstrQuery)
        {
            try
            {
                ConectaBD();

                _comando.CommandText = pstrQuery;
                _comando.Connection = _conexao;


                int intQtdReg = _comando.ExecuteNonQuery();


                DesconectaDB();
                return intQtdReg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String TestarConexão() {
            try {

                ConectaBD();

               string statusTeste =  _conexao.State.ToString();

                DesconectaDB();

                return statusTeste;

            
            }
            catch (Exception ex) {
                
                return "Conexão falhou -- >" + ex.Message;            
            }
        
        
        }

        private void PreparaStringConexao() { 
        
        
        
        
        }

        /// <summary>
        /// Realiza comandos, não query's, no banco. Retorna o número de linhas afetadas. Em caso de erro será retornado -1.
        /// A quantide de linhas afetadas leva em consideração as linhas alteradas por triggers, caso exista uma na tabela em questão.
        /// </summary>
        /// <param name="pstrQuery">Comando a ser executado no banco.</param>
        /// <returns>Esse método não retorna a quantidade de registros afetados.</returns>
        public void ExecuteSemRetonro(string pstrQuery)
        {
            try
            {
                ConectaBD();
                _comando.CommandText = pstrQuery;
                _comando.Connection = _conexao;

                int intQtdReg = _comando.ExecuteNonQuery();


                DesconectaDB();
            }
            catch (Exception ex)
            {

                throw ex;

            }
        }

        #endregion

    }

}
