using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Dados;
using System.Diagnostics;

/// <summary>
/// Classe destinada a realizar a leitura da planilha StatusReport do RunrunIT e importa-la para o banco de dados
/// </summary>
namespace ImportadorRunRunIT
{
    class MapeadorExcelStatusReport
    {

        #region "Atributos"


        private string _nomeArquivo = "N/I";
        private string _caminhoArquivo = "N/I";
        private string _dataHoraImportcao = "N/I";
        private int _idTabelaArquivo = 0;
        private bool _layoutCorreto = false;

        private string _servidorBD = "N/I";
        private string _nomeBD = "N/I";
        private string _loginDB = "";
        private string _senhaDB = "";

        //Atributos para representar o contéido da planlha "Status Report"
        private string _cliente = "";
        private string _projeto = "";
        private string _id = "";
        private string _prio = "";
        private string _tarefa = "";
        private string _tipo_de_tarefa = "";
        private string _aberta_por = "";
        private string _criada_em = "";
        private string _para = "";
        private string _equipe = "";
        private string _data_desejada = "";
        private string _entrega_estimada = "";
        private string _entregue_em = "";
        private string _esforço_estimado_h = "";
        private string _primeiro_esforço_estimado_h = "";
        private string _ja_trabalhadas_h = "";
        private string _percentual = "";
        private string _fase = "";
        private string _status = "";
        private string _Reaberta = "";


        private string _ano = "";
        private string _mes = "";
        private string _dia = "";
        private string _hora = "";
        private string _minuto = "";
        private string _segundo = "";
        private string _tipo_projeto = "";
        private string _naturezaTarefa = "NÃO DEFINIDO";
        //*******************************************************************

        #endregion

        #region "Propriedades"

        public string CaminhoArquivo
        {
            get { return _caminhoArquivo; }
            set { _caminhoArquivo = value; }
        }

        public string NomeArquivo
        {

            get { return _nomeArquivo; }
            set { _nomeArquivo = value; }

        }

        public bool LayoutCorreto
        {

            get { return _layoutCorreto; }
        }

        public string ServidorDB
        {
            get { return _servidorBD; }
            set { _servidorBD = value; }
        }

        public string NomeDB
        {
            get { return _nomeBD; }
            set { _nomeBD = value; }
        }

        public string LoginDB
        {
            get { return _loginDB; }
            set { _loginDB = value; }
        }


        public string SenhaDB
        {
            get { return _senhaDB; }
            set { _senhaDB = value; }
        }
        #endregion

        #region "Métodos"
        /// <summary>
        /// A validação será "superficial" apenas o dado da primeira célula será validado, inicialmente.
        /// </summary>
        private void ValidaLayout()
        {

            try
            {

                FonteDeDadosOld PlanilhaStatusReport = new FonteDeDadosOld(TipoFonteDados.Excel, _caminhoArquivo, "[Status Report Geral]", "", "");
                DataTableReader dtrExcelStatusReport = PlanilhaStatusReport.NewReader("Select * from [Status Report Geral$]");


                if (dtrExcelStatusReport != null)
                {

                    dtrExcelStatusReport.Read();

                    String ValorPrimeiraCelula = dtrExcelStatusReport[0].ToString();


                    if (ValorPrimeiraCelula.Equals("Relatório gerado em:"))
                    {
                        _layoutCorreto = true;
                    }
                    else
                    {

                        _layoutCorreto = false;
                    }
                }
                else
                {

                    _layoutCorreto = false;

                }
            }
            catch (Exception ex)
            {
                ///todo    
                throw ex;

            }
        }

        private string PreparaInsertArquivo()
        {
            try
            {
                StringBuilder sqlInsert = new StringBuilder();


                sqlInsert.AppendLine("INSERT INTO [TAB_RUNRUNIT_STATUS_REPORT_ARQUIVO]");
                sqlInsert.AppendLine("([NOME_ARQUIVO]");
                sqlInsert.AppendLine(",[DATA_ARQUIVO])");
                sqlInsert.AppendLine(" VALUES ");

                sqlInsert.AppendLine("(<NOME_ARQUIVO>");
                sqlInsert.AppendLine(",<DATA_ARQUIVO>)");

                sqlInsert.Replace("<NOME_ARQUIVO>", FormatacaoSQL.ConvertParaVarchar(_nomeArquivo, false));
                sqlInsert.Replace("<DATA_ARQUIVO>", FormatacaoSQL.ConverteParaDateTime(_dataHoraImportcao, TipoDataEntrada.DDMMAA_HHMMSS_COM_BARRA_E_DOIS_PONTOS));

                return sqlInsert.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string PreparaInsertDetalhe()
        {
            try
            {
                StringBuilder sqlInsert = new StringBuilder();

                //Preparação da base do comando
                sqlInsert.AppendLine("INSERT INTO [TAB_RUNRUNIT_STATUS_REPORT_DETALHE] (");
                sqlInsert.AppendLine("[ARQUIVO]");
                sqlInsert.AppendLine(",[CLIENTE]");
                sqlInsert.AppendLine(",[PROJETO]");
                sqlInsert.AppendLine(",[ID]");
                sqlInsert.AppendLine(",[PRIO]");
                sqlInsert.AppendLine(",[TAREFA]");
                sqlInsert.AppendLine(",[TIPO DE TAREFA]");
                sqlInsert.AppendLine(",[ABERTA POR]");
                sqlInsert.AppendLine(",[CRIADA EM]");
                sqlInsert.AppendLine(",[PARA]");
                sqlInsert.AppendLine(",[EQUIPE]");
                sqlInsert.AppendLine(",[DATA DESEJADA]");
                sqlInsert.AppendLine(",[ENTREGA ESTIMADA]");
                sqlInsert.AppendLine(",[ENTREGUE EM]");
                sqlInsert.AppendLine(",[ESFORÇO ESTIMADO H]");
                sqlInsert.AppendLine(",[PRIMEIRO ESFORÇO ESTIMADO H]");
                sqlInsert.AppendLine(",[JÁ TRABALHADAS H]");
                sqlInsert.AppendLine(",[%]");
                sqlInsert.AppendLine(",[FASE]");
                sqlInsert.AppendLine(",[STATUS]");
                sqlInsert.AppendLine(",[REABERTA?]");
                sqlInsert.AppendLine(",[ANO]");
                sqlInsert.AppendLine(",[MES]");
                sqlInsert.AppendLine(",[DIA]");
                sqlInsert.AppendLine(",[HORA]");
                sqlInsert.AppendLine(",[MINUTO]");
                sqlInsert.AppendLine(",[SEGUNDO]");
                sqlInsert.AppendLine(",[NATUREZA_TAREFA]");
                sqlInsert.AppendLine(",[TIPO_PROJETO])");
                sqlInsert.AppendLine("     VALUES (");

                //Posicionamento dos campos
                sqlInsert.AppendLine("<ARQUIVO>");
                sqlInsert.AppendLine(",<CLIENTE>");
                sqlInsert.AppendLine(",<PROJETO>");
                sqlInsert.AppendLine(",<ID>");
                sqlInsert.AppendLine(",<PRIO>");
                sqlInsert.AppendLine(",<TAREFA>");
                sqlInsert.AppendLine(",<TIPO DE TAREFA>");
                sqlInsert.AppendLine(",<ABERTA POR>");
                sqlInsert.AppendLine(",<CRIADA EM>");
                sqlInsert.AppendLine(",<PARA>");
                sqlInsert.AppendLine(",<EQUIPE>");
                sqlInsert.AppendLine(",<DATA DESEJADA>");
                sqlInsert.AppendLine(",<ENTREGA ESTIMADA>");
                sqlInsert.AppendLine(",<ENTREGUE EM>");
                sqlInsert.AppendLine(",<ESFORÇO ESTIMADO H>");
                sqlInsert.AppendLine(",<PRIMEIRO ESFORÇO ESTIMADO H>");
                sqlInsert.AppendLine(",<JÁ TRABALHADAS H>");
                sqlInsert.AppendLine(",<%>");
                sqlInsert.AppendLine(",<FASE>");
                sqlInsert.AppendLine(",<STATUS>");
                sqlInsert.AppendLine(",<REABERTA?>");
                sqlInsert.AppendLine(",<ANO>");
                sqlInsert.AppendLine(",<MES>");
                sqlInsert.AppendLine(",<DIA>");
                sqlInsert.AppendLine(",<HORA>");
                sqlInsert.AppendLine(",<MINUTO>");
                sqlInsert.AppendLine(",<SEGUNDO>");
                sqlInsert.AppendLine(",<NATUREZA_TAREFA>");
                sqlInsert.AppendLine(",<TIPO_PROJETO>)");


                //Substitui pelos atributos correspondentes.
                sqlInsert.Replace("<ARQUIVO>", _idTabelaArquivo.ToString());
                sqlInsert.Replace("<CLIENTE>", FormatacaoSQL.ConvertParaVarchar(_cliente, false));
                sqlInsert.Replace("<PROJETO>", FormatacaoSQL.ConvertParaVarchar(_projeto, false));
                sqlInsert.Replace("<ID>", _id);
                sqlInsert.Replace("<PRIO>", FormatacaoSQL.ConvertParaVarchar(_prio, false));
                sqlInsert.Replace("<TAREFA>", FormatacaoSQL.ConvertParaVarchar(_tarefa, false));
                sqlInsert.Replace("<TIPO DE TAREFA>", FormatacaoSQL.ConvertParaVarchar(_tipo_de_tarefa, false));
                sqlInsert.Replace("<ABERTA POR>", FormatacaoSQL.ConvertParaVarchar(_aberta_por, false));
                sqlInsert.Replace("<CRIADA EM>", FormatacaoSQL.ConverteParaDateTime(_criada_em, TipoDataEntrada.DDMMAAAA_HHMMSS_COM_BARRA_E_DOIS_PONTOS));
                sqlInsert.Replace("<PARA>", FormatacaoSQL.ConvertParaVarchar(_para, false));
                sqlInsert.Replace("<EQUIPE>", FormatacaoSQL.ConvertParaVarchar(_equipe, false));
                sqlInsert.Replace("<DATA DESEJADA>", FormatacaoSQL.ConvertParaVarchar(_data_desejada, false));
                sqlInsert.Replace("<ENTREGA ESTIMADA>", FormatacaoSQL.ConvertParaVarchar(_entrega_estimada, false));
                sqlInsert.Replace("<ENTREGUE EM>", FormatacaoSQL.ConvertParaVarchar(_entregue_em, false));
                sqlInsert.Replace("<ESFORÇO ESTIMADO H>", _esforço_estimado_h.Replace(",", "."));
                sqlInsert.Replace("<PRIMEIRO ESFORÇO ESTIMADO H>", _primeiro_esforço_estimado_h.Replace(",", "."));
                sqlInsert.Replace("<JÁ TRABALHADAS H>", _ja_trabalhadas_h.Replace(",", "."));
                sqlInsert.Replace("<%>", FormatacaoSQL.ConvertParaVarchar(_percentual, false));
                sqlInsert.Replace("<FASE>", FormatacaoSQL.ConvertParaVarchar(_fase, false));
                sqlInsert.Replace("<STATUS>", FormatacaoSQL.ConvertParaVarchar(_status, false));
                sqlInsert.Replace("<REABERTA?>", FormatacaoSQL.ConvertParaVarchar(_Reaberta, false));
                sqlInsert.Replace("<ANO>", _ano);
                sqlInsert.Replace("<MES>", _mes);
                sqlInsert.Replace("<DIA>", _dia);
                sqlInsert.Replace("<HORA>", _hora);
                sqlInsert.Replace("<MINUTO>", _minuto);
                sqlInsert.Replace("<SEGUNDO>", _segundo);
                sqlInsert.Replace("<NATUREZA_TAREFA>", FormatacaoSQL.ConvertParaVarchar(_naturezaTarefa, false));
                sqlInsert.Replace("<TIPO_PROJETO>", FormatacaoSQL.ConvertParaVarchar(_tipo_projeto, false));


                return sqlInsert.ToString();


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        /// <summary>
        /// Método destinado a verificar qu tipo de "projeto" está cadastrado no RunRunIT, bem como o tipo de tarefa a saber
        /// Tipos de "projetos" determinados até o momento:
        ///     Projeto
        ///     Suporte
        ///     Rotina
        /// Tipos de naturaza da tarefa:
        ///     Análise » ANL
        ///     Desenvolvimento » DES
        ///     Testes » TES
        ///     Reunião » REU
        ///     Instalação » INS
        ///     Definição de esforço » ESF
        ///     Correção » COR 
        /// </summary>
        /// <returns></returns>
        private void DeterminaTipoProjetoETarefa()
        {
            try
            {

                switch (_projeto.Substring(4, 1))
                {
                    case "P":
                        _tipo_projeto = "PROJETO";
                        break;
                    case "S":
                        _tipo_projeto = "SUPORTE";
                        break;
                    case "R":
                        _tipo_projeto = "ROTINA";
                        break;
                    default:
                        _tipo_projeto = "OUTROS";
                        break;
                }

                switch (_tarefa.Substring(0, 3))
                {
                    case "ANL":
                        _naturezaTarefa = "ANALISE";
                        break;
                    case "DES":
                        _naturezaTarefa = "DESENVOLVIEMNTO";
                        break;
                    case "TES":
                        _naturezaTarefa = "TESTE";
                        break;
                    case "REU":
                        _naturezaTarefa = "REUNIAO";
                        break;
                    case "INS":
                        _naturezaTarefa = "INSTALACAO";
                        break;
                    case "ESF":
                        _naturezaTarefa = "DEF_ESFORCO";
                        break;
                    case "COR":
                        _naturezaTarefa = "CORRECAO";
                        break;
                    default:
                        _naturezaTarefa = "OUTROS";
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public void ImportarParaDB()
        {
            try
            {

                //Faz a validação parcial do layout
                ValidaLayout();


                if (_layoutCorreto)
                {
                    int contadorLinha = 1;

                    FonteDeDadosOld BancoDeDadosDestino = new FonteDeDadosOld(TipoFonteDados.SQLServer, _servidorBD, _nomeBD, _loginDB, _senhaDB);
                    FonteDeDadosOld PlanilhaStatusReport = new FonteDeDadosOld(TipoFonteDados.Excel, _caminhoArquivo, "[Status Report Geral]", "", "");
                    DataTableReader dtrExcelStatusReport = PlanilhaStatusReport.NewReader("Select * from [Status Report Geral$]");

                    while (dtrExcelStatusReport.Read())
                    {
                        //Na primeira linha vou obter a data do arquivo.
                        if (contadorLinha == 1)
                        {

                            _dataHoraImportcao = dtrExcelStatusReport[1].ToString();

                            //Insere os dados de registro do arquivo
                            BancoDeDadosDestino.ExecuteSemRetonro(PreparaInsertArquivo());

                            DataTableReader dtrExcelIDArquivo = BancoDeDadosDestino.NewReader("SELECT MAX(ID_ARQUIVO) FROM TAB_RUNRUNIT_STATUS_REPORT_ARQUIVO");


                            dtrExcelIDArquivo.Read();

                            _idTabelaArquivo = int.Parse(dtrExcelIDArquivo[0].ToString());



                        }

                        if (contadorLinha > 3)
                        {
                            //o conteúdo do arquvo começa efetivamente na linha 4
                            //Cada linha será lida e os valores armazenados nos atributos indicados

                            _cliente = dtrExcelStatusReport[0].ToString();
                            _projeto = dtrExcelStatusReport[1].ToString();
                            _id = dtrExcelStatusReport[2].ToString();
                            _prio = dtrExcelStatusReport[3].ToString();
                            _tarefa = dtrExcelStatusReport[4].ToString().Replace("'", "");
                            _tipo_de_tarefa = dtrExcelStatusReport[5].ToString();
                            _aberta_por = dtrExcelStatusReport[6].ToString();
                            _criada_em = dtrExcelStatusReport[7].ToString();
                            _para = dtrExcelStatusReport[8].ToString();
                            _equipe = dtrExcelStatusReport[9].ToString();
                            _data_desejada = dtrExcelStatusReport[10].ToString();
                            _entrega_estimada = dtrExcelStatusReport[11].ToString();
                            _entregue_em = dtrExcelStatusReport[12].ToString();
                            _esforço_estimado_h = dtrExcelStatusReport[13].ToString();
                            _primeiro_esforço_estimado_h = dtrExcelStatusReport[14].ToString();
                            _ja_trabalhadas_h = dtrExcelStatusReport[15].ToString();
                            _percentual = dtrExcelStatusReport[16].ToString();
                            _fase = dtrExcelStatusReport[17].ToString();
                            _status = dtrExcelStatusReport[18].ToString();
                            _Reaberta = dtrExcelStatusReport[19].ToString();

                            _ano = _criada_em.Substring(6, 4);
                            _mes = _criada_em.Substring(3, 2);
                            _dia = _criada_em.Substring(0, 2);
                            _hora = _criada_em.Substring(11, 2);
                            _minuto = _criada_em.Substring(14, 2);
                            _segundo = _criada_em.Substring(17, 2);

                            DeterminaTipoProjetoETarefa();

                            BancoDeDadosDestino.ExecuteSemRetonro(PreparaInsertDetalhe());

                        }

                        contadorLinha += 1;

                    }
                    //Finalizando a importação, faz o processamento da analise.
                    //Idéia:    Hoje a alimentação da tabela de anlise é feita via procedure
                    //          posteriormente, quero fazer via C#, criando assim, independencia do BD.

                    BancoDeDadosDestino.ExecuteSemRetonro("PROC_GERA_ANALISE_STATUS_REPORT");


                }
                else
                {
                    throw new Exception("Layout do arquivo inválido!");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

    }
}
