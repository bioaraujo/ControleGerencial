using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Util.Ocorrencia;

namespace FonteDeDados
{
    /// <summary>
    /// Classe destina a ser a se conecatar com arquivo texto e retornar o conteudo em formatos pré-determindados.
    /// Nessa priemria versão da classe apenas arquivos texto com separdor de coluna definido serão processados.
    /// Criei essa classe para fazer o desmembramento da antiga Classe "FonteDeDados"
    /// Fabio 19/08/2015
    /// </summary>
    class ArquivoTexto
    {
        #region "Atributos"

        private StreamReader _leitorTxt;
        private string _localeCaminho = "";


        private string _localArquivo = "N/I";
        private string _nomeArquivo = "N/I";
        private char _separadorTxt = '\t'; 
        private bool _primeiraLinhaCabecalho = false;


        #endregion

        #region "Propriedades"

        /// <summary>
        /// Deve ser informado o "local" onde o arquivo está gravado.
        /// Informar o endereço "normal" tal como mostrado no Windows sem a barra no final. EX: "C:\PastaTeste
        /// </summary>
        public string LocalArquivo
        {
            get { return _localArquivo; }
            set 
            { 
                _localArquivo =   value.Replace("\\","\\\\") + "\\";
                _localeCaminho = _localArquivo + _nomeArquivo;
            
            }
        }
        /// <summary>
        /// Infromar o nome do arquivo incluindo a extensão.
        /// </summary>
        public string NomeArquivo
        {
            get { return _localArquivo; }
            set 
            { 
                _localArquivo = value;
                _localeCaminho = _localArquivo + _nomeArquivo;
            }
        }
        /// <summary>
        /// Separador que identifica as colunas do arquivos.
        /// No caso de separação por "tab" informar '\t':
        /// O Valor padrão está definido como separador por tab.
        /// </summary>
        public char SeparadorTXT
        {
            get { return _separadorTxt; }
            set { _separadorTxt = value; }
        }
        /// <summary>
        /// Informa se a primeira linha do arquivo contem ou não cabeçalho.
        /// O valor padrão é false.
        /// </summary>
        public bool PrimeiraLinhaCabecalho
        {
            get { return _primeiraLinhaCabecalho; }
            set { _primeiraLinhaCabecalho = value; }
        }        

        #endregion

        #region "Métodos"


        /// <summary>
        /// Verifica se o LocalArquivo informdo é um diretório valido e se o arquivo existe.
        /// Retonra:
        /// 0 -> Local e arquivo foram localizados
        /// 1 -> Local não localizado
        /// 2 -> Arquivo não localizado
        /// 3 -> Erro no processamento
        /// </summary>
        public int VerificaLocaleArquivo() {
            try
            {
                int retorno = 0;

                if (!Directory.Exists(_localArquivo)) 
                {
                    retorno = 1;
                    return retorno;
                }


                if (File.Exists(_localeCaminho))
                {
                    retorno = 2;
                    return retorno;
                }

                return retorno;
            }
            catch (Exception ex)
            {
                TrataExcecao.GravarExcecao(ex,true);       
                throw ex;
                
            }

        }

        /// <summary>
        /// Prepara reader e abre o arquivo.
        /// </summary>
        private void ConectaArquivoTXT()
        {
            try
            {

                int resultadoVerificacao = VerificaLocaleArquivo();

                switch (VerificaLocaleArquivo()) 
                {
                    case 0:
                        _leitorTxt = File.OpenText(_localeCaminho);
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
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
