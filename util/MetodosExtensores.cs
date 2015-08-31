using System;
using System.Text.RegularExpressions;

namespace Util
{
    public static class MetodosExtensores
    {
        /// <summary>
        /// Remove valores não númericos da string.
        /// </summary>
        /// <param name="str"> String. </param>
        /// <returns> String formatada. </returns>
        public static string RemoveNaoNumericos(this String str)
        {
            Regex regEx = new Regex(@"\D");
            return regEx.Replace(str, "").Trim();
        }

        /// <summary>
        /// Aplica o Numeric_V2 do importadorCC.
        /// </summary>
        /// <param name="str"> String. </param>
        /// <returns> Valor formatado. </returns>
        public static string ImportadorValorParseNumericV2(this String str)
        {
            string valor;
            try
            {
                valor = (Convert.ToDecimal(str) / 100).ToString("n2");
            }
            catch
            {
                valor = "-1";
            }
            return valor;
        }

        /// <summary>
        /// Método útilizado para converter o formato de data que vem nos arquivos YYYYMMDD para o formato do C#.
        /// </summary>
        /// <param name="str"> String. </param>
        /// <returns> Retorna data formatada, caso não seja possível montar uma data o valor retornado é 1900-01-01. </returns>
        public static DateTime ImportadorDataParseYYYYMMDD(this String str)
        {
            DateTime data;
            try
            {
                data = new DateTime(Convert.ToInt32(str.Substring(0, 4)), /*ano*/
                                    Convert.ToInt32(str.Substring(4, 2)), /*mês*/
                                    Convert.ToInt32(str.Substring(6, 2))); /*dia*/
            }
            catch
            {
                data = new DateTime(1900, /*ano*/
                                    01, /*mês*/
                                    01); /*dia*/
            }
            return data;
        }

        /// <summary>
        /// Método útilizado para converter o formato de data que vem nos arquivos DD/MM/YYYYM para o formato do C#.
        /// </summary>
        /// <param name="str"> String. </param>
        /// <returns> Retorna data formatada, caso não seja possível montar uma data o valor retornado é 1900-01-01. </returns>
        public static DateTime ImportadorDataParseSeparadorDDMMYYYY(this String str)
        {
            DateTime data;
            try
            {
                data = new DateTime(Convert.ToInt32(str.Substring(6, 4)), /*ano*/
                                    Convert.ToInt32(str.Substring(3, 2)), /*mês*/
                                    Convert.ToInt32(str.Substring(0, 2))); /*dia*/
            }
            catch
            {
                data = new DateTime(1900, /*ano*/
                                    01, /*mês*/
                                    01); /*dia*/
            }
            return data;
        }

        /// <summary>
        /// Metodo responsavel por transformar uma String em um Varchar compativel com o Banco de Dados
        /// </summary>
        /// <param name="str">String</param>
        /// <returns>Retorna a string (varchar) preparada para o banco de dados.</returns>
        public static string SQLParse(this String str)
        {
            if (str == null)
            {
                return "''";
            }
            return "'" + str.Replace("'", "''") + "'";
        }

        /// <summary>
        /// Metodo responsavel por transformar um valor Decimal em um decimal compativel com o Banco de Dados.
        /// </summary>
        /// <param name="Integer">Valor Decimal</param>
        /// <returns>Retorna um decimal preparado para o banco de dados.</returns>
        public static string SQLParse(this Decimal dec)
        {
            return dec.ToString().Replace(',', '.');
        }

        /// <summary>
        /// Metodo responsavel por transformar um valor DateTime em um Varchar compativel com o Banco de Dados.
        /// </summary>
        /// <param name="datetime">Valor DateTime</param>
        /// <param name="bolComHora">True para manter a Hora. False para Data sem Hora.</param>
        /// <returns>Retorna a string (varchar) preparada para o banco de dados.</returns>
        public static string SQLParse(this DateTime datetime, bool bolComHora)
        {
            if (datetime.Year <= 1900)
            {
                return "null";
            }
            if (bolComHora)
            {
                return "'" + datetime.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
            else
            {
                return "'" + datetime.ToString("yyyy-MM-dd") + "'";
            }
        }

        /// <summary>
        /// Metodo responsavel por transformar um valor Int em um int compativel com o Banco de Dados.
        /// </summary>
        /// <param name="Integer">Valor Inteiro</param>
        /// <returns>Retorna um int preparado para o banco de dados.</returns>
        public static string SQLParse(this int Integer)
        {
            return Integer.ToString();
        }

        /// <summary>
        /// Metodo responsavel por transformar um valor boleano em um char compativel com o Banco de Dados.
        /// De acordo com as definições do projeto.
        /// </summary>
        /// <param name="boo">Valor boleano</param>
        /// <returns>Retorna N para False e S para True</returns>
        public static string SQLParse(this bool boo)
        {
            if (boo)
            {
                return "'S'";
            }
            else
            {
                return "'N'";
            }
        }

        /// <summary>
        /// Método responsável por transformar valores doubles com valores compativeis com o banco de dados.
        /// </summary>
        /// <param name="dou">Valor double</param>
        /// <returns>Retorna um double preparado para o banco dedados</returns>
        public static string SQLParse(this double dou)
        {
            return dou.ToString().Replace(',', '.');
        }
    }
}