using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dados
{

    public enum TipoDataEntrada
    {
        DDMMAAAA_COM_BARRA = 1,
        DDMMAAAA_HHMMSS_COM_BARRA_E_DOIS_PONTOS = 2,
        DDMMAA_HHMMSS_COM_BARRA_E_DOIS_PONTOS = 3
    }

    public static class FormatacaoSQL
    {

        /// <summary>
        /// Método para retornar uma string no formato de data do SqlServer de acordo com o tipo de data de entrada informado
        /// </summary>
        /// <returns></returns>
        public static string ConverteParaDateTime(string dataParaFormatar, TipoDataEntrada tipoData)
        {
            try
            {
                StringBuilder dataFormatada = new StringBuilder();


                switch (tipoData)
                {

                    case TipoDataEntrada.DDMMAAAA_COM_BARRA:
                        dataFormatada.Append("'");
                        dataFormatada.Append(dataParaFormatar.Substring(6, 4));
                        dataFormatada.Append('-');
                        dataFormatada.Append(dataParaFormatar.Substring(3, 2));
                        dataFormatada.Append('-');
                        dataFormatada.Append(dataParaFormatar.Substring(0, 2));
                        dataFormatada.Append("'");
                        break;
                    case TipoDataEntrada.DDMMAAAA_HHMMSS_COM_BARRA_E_DOIS_PONTOS:
                        dataFormatada.Append("'");
                        dataFormatada.Append(dataParaFormatar.Substring(6, 4));
                        dataFormatada.Append('-');
                        dataFormatada.Append(dataParaFormatar.Substring(3, 2));
                        dataFormatada.Append('-');
                        dataFormatada.Append(dataParaFormatar.Substring(0, 2));
                        dataFormatada.Append(' ');
                        dataFormatada.Append(dataParaFormatar.Substring(11, 2));
                        dataFormatada.Append(':');
                        dataFormatada.Append(dataParaFormatar.Substring(14, 2));
                        dataFormatada.Append(':');
                        dataFormatada.Append(dataParaFormatar.Substring(17, 2));
                        dataFormatada.Append("'");
                        break;
                    case TipoDataEntrada.DDMMAA_HHMMSS_COM_BARRA_E_DOIS_PONTOS:
                        dataFormatada.Append("'");
                        dataFormatada.Append("20");
                        dataFormatada.Append(dataParaFormatar.Substring(6, 2));
                        dataFormatada.Append('-');
                        dataFormatada.Append(dataParaFormatar.Substring(3, 2));
                        dataFormatada.Append('-');
                        dataFormatada.Append(dataParaFormatar.Substring(0, 2));
                        dataFormatada.Append(' ');
                        dataFormatada.Append(dataParaFormatar.Substring(9, 2));
                        dataFormatada.Append(':');
                        dataFormatada.Append(dataParaFormatar.Substring(12, 2));
                        dataFormatada.Append(':');
                        dataFormatada.Append(dataParaFormatar.Substring(15, 2));
                        dataFormatada.Append("'");
                        break;

                }

                return dataFormatada.ToString();

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static string ConvertParaVarchar(string valorParaFormatar, bool executaTrim)
        {
            try
            {

                StringBuilder valorFormatado = new StringBuilder();

                valorFormatado.Append("'");

                if (executaTrim)
                {
                    valorFormatado.Append(valorParaFormatar.Trim().Replace("'", ""));
                }
                else
                {
                    valorFormatado.Append(valorParaFormatar.Replace("'", ""));
                }

                valorFormatado.Append("'");

                return valorFormatado.ToString();
            }
            catch (Exception ex)
            {
                throw ex;

            }





        }


    }
}
