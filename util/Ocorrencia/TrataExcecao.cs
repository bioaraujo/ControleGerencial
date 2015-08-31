using System;
using System.Collections;
using System.Configuration;
using Util.Email;
using Util.Log;

namespace Util.Ocorrencia
{
    public sealed class TrataExcecao
    {
        #region Métodos

        /// <summary>
        /// Grava a mensagem de log no diretório especificado no registro PATH_LOG. O nome do Log será o do projeto que estava sendo executado.
        /// </summary>
        /// <param name="pMensagem">Mensagem que será salva no log</param>
        /// <param name="pNotificar">Deve enviar email de notificação?</param>
        public static void GravarExcecao(string pMensagem, bool pNotificar = false)
        {
            string strMensagemLog = @"
<!--{PROXIMO_LOG}-->
                             <tr>
                <td>
                    <table width=100%>
                        <tr bgcolor=#f5f5f5>
                            <td colspan=2 align=center> <b>LOG</b> : {HORA} </td>
                        </tr>

                        <tr bgcolor=#FFADAD>
                            <td colspan=2  align=Left>
                            <b>Dados Ocorrência</b>
                            </td>
                        </tr>
                        <tr bgcolor=""#ffffcc"">
                            <td colspan=2  align=Left>
                            <b>Aplicação:</b> {APLICACAO} <br>
                            <b>Mensagem:</b> {MENSAGEM} <br>
                            <b>Maquina:</b> {MAQUINA} <br>
                            <b>Caminho:</b> {CAMINHO} <br>
                            </td>
                        </tr>
                        <tr bgcolor=#f5f5f5>

                        </tr>
                    </table>
                </td>
            </tr>
            ";
            strMensagemLog = strMensagemLog.Replace("{MENSAGEM}", pMensagem);
            strMensagemLog = strMensagemLog.Replace("{HORA}", DateTime.Now.ToString("dd/MM/yyyy HH:mm ss"));
            strMensagemLog = strMensagemLog.Replace("{APLICACAO}", System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim());
            strMensagemLog = strMensagemLog.Replace("{MAQUINA}", Environment.MachineName);
            strMensagemLog = strMensagemLog.Replace("{CAMINHO}", Environment.CurrentDirectory);
            Logger.GravarLogExcecao(strMensagemLog);
            if (pNotificar)
            {
                EnvelopeEmail objetoEmail = new EnvelopeEmail();
                objetoEmail.Assunto = "Exceção - " + System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim() + " | " + DateTime.Now.ToString();
                objetoEmail.CorpoTexto = strMensagemLog;
                objetoEmail.EmailRemetente = "Exceção " + System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim() + " " + "<sistemaintersic@mfmti.com.br>";
                objetoEmail.EmailDestinatario = ConfigurationManager.AppSettings["DESTINATARIO_EXCECAO"];
                objetoEmail.EnviarEmail();
            }
        }

        /// <summary>
        /// Grava a mensagem de log no diretório especificado no registro PATH_LOG. O nome do Log será o do projeto que estava sendo executado.
        /// </summary>
        /// <param name="pExceptionLog">Exception que será salva no log</param>
        /// <param name="pNotificar">Deve enviar email de notificação?</param>
        public static void GravarExcecao(Exception pExcecao, bool pNotificar = false)
        {
            string strMensagemLog = @"
<!--{PROXIMO_LOG}-->
             <tr>
                <td>
                    <table width=100%>
                        <tr bgcolor=#f5f5f5>
                            <td colspan=2 align=center> <b>LOG</b> : {HORA} </td>
                        </tr>

                        <tr bgcolor=#FFADAD>
                            <td colspan=2  align=Left>
&nbsp;
                            </td>
                        </tr>
                        <tr bgcolor=""#ffffcc"">
                            <td colspan=2  align=Left>
                            <b>Aplicação:</b> {APLICACAO} <br>
                            <b>Maquina:</b> {MAQUINA} <br>
                            <b>Caminho:</b> {CAMINHO} <br>
                            <b>Origem:</b> {ORIGEM} <br>
                            <b>Definição da Classe</b>: {CLASS} <br>
                            <b>Tipo:</b>  {TIPO_MEMBER} <br>
                            {MEMBER} <br>
                            <b>Mensagem:</b> <br>
                            {MENSAGEM} <br>
                            <b>Trace:</b>
                            <code>
                            {TRACE}
                            </code>
                            <BR>
                            {INFO_ADICIONAIS}
                            <BR>
                            {METODO_ANTERIOR}
                            <br>
                            </td>
                        </tr>
                        <tr bgcolor=#f5f5f5>

                        </tr>
                    </table>
                </td>
            </tr>
            ";
            string strInfoAdicionais = "";
            if (pExcecao.Data != null)
            {
                strInfoAdicionais = "<b>Informações Adicionais:</b> <br>".Normalize();
                foreach (DictionaryEntry item in pExcecao.Data)
                {
                    strInfoAdicionais += "<b>" + item.Key.ToString() + ":</b> <br>" + item.Value.ToString() + "<br>";
                }
            }
            string strMetodoAnterior = "";
            if (pExcecao.InnerException != null)
            {
                strMetodoAnterior = "Metodo Anterior: <BR>" + pExcecao.InnerException.TargetSite.ToString();
            }


            strMensagemLog = strMensagemLog.Replace("{HORA}", DateTime.Now.ToString("dd/MM/yyyy HH:mm ss"));
            strMensagemLog = strMensagemLog.Replace("{APLICACAO}", System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim());
            strMensagemLog = strMensagemLog.Replace("{MAQUINA}", Environment.MachineName);
            strMensagemLog = strMensagemLog.Replace("{CAMINHO}", Environment.CurrentDirectory);
            strMensagemLog = strMensagemLog.Replace("{ORIGEM}", pExcecao.Source.ToString());
            strMensagemLog = strMensagemLog.Replace("{CLASS}", pExcecao.TargetSite.DeclaringType.ToString());
            strMensagemLog = strMensagemLog.Replace("{TIPO_MEMBER}", pExcecao.TargetSite.MemberType.ToString());
            strMensagemLog = strMensagemLog.Replace("{MEMBER}", pExcecao.TargetSite.ToString());
            strMensagemLog = strMensagemLog.Replace("{MENSAGEM}", pExcecao.Message.ToString());
            strMensagemLog = strMensagemLog.Replace("{TRACE}", pExcecao.StackTrace.ToString().Replace("at ", "<BR>"));
            strMensagemLog = strMensagemLog.Replace("{INFO_ADICIONAIS}", strInfoAdicionais);
            strMensagemLog = strMensagemLog.Replace("{METODO_ANTERIOR}", strMetodoAnterior);
            Logger.GravarLogExcecao(strMensagemLog);
            if (pNotificar)
            {
                EnvelopeEmail objetoEmail = new EnvelopeEmail();
                objetoEmail.Assunto = "Exceção - " + System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim() + " | " + DateTime.Now.ToString();
                objetoEmail.CorpoTexto = strMensagemLog;
                objetoEmail.EmailRemetente = "Exceção " + System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim() + " " + "<sistemaintersic@mfmti.com.br>";
                objetoEmail.EmailDestinatario = ConfigurationManager.AppSettings["DESTINATARIO_EXCECAO"];
                objetoEmail.EnviarEmail();
            }
        }


        /// <summary>
        /// Grava a mensagem de log no diretório especificado no registro PATH_LOG. O nome do Log será o do projeto que estava sendo executado.
        /// </summary>
        /// <param name="pExceptionLog">Exception que será salva no log</param>
        /// <param name="pObservacao">Mensagem adicional ao log.</param>
        /// <param name="pNotificar">Deve enviar email de notificação?</param>
        public static void GravarExcecao(Exception pExcecao, string pObservacao, bool pNotificar = false)
        {
            string strMensagemLog = @"
<!--{PROXIMO_LOG}-->
             <tr>
                <td>
                    <table width=100%>
                        <tr bgcolor=#f5f5f5>
                            <td colspan=2 align=center> <b>LOG</b> : {HORA} </td>
                        </tr>

                        <tr bgcolor=#FFADAD>
                            <td colspan=2  align=Left>
&nbsp;
                            </td>
                        </tr>
                        <tr bgcolor=""#ffffcc"">
                            <td colspan=2  align=Left>
                            <b>Aplicação:</b> {APLICACAO} <br>
                            <b>Observação:</b> {OBSERVACAO} <br>
                            <b>Maquina:</b> {MAQUINA} <br>
                            <b>Caminho:</b> {CAMINHO} <br>
                            <b>Origem:</b> {ORIGEM} <br>
                            <b>Definição da Classe</b>: {CLASS} <br>
                            <b>Tipo:</b>  {TIPO_MEMBER} <br>
                            {MEMBER} <br>
                            <b>Mensagem:</b> <br>
                            {MENSAGEM} <br>
                            <b>Trace:</b>
                            <code>
                            {TRACE}
                            </code>
                            <BR>
                            {INFO_ADICIONAIS}
                            <BR>
                            {METODO_ANTERIOR}
                            <br>
                            </td>
                        </tr>
                        <tr bgcolor=#f5f5f5>

                        </tr>
                    </table>
                </td>
            </tr>
            ";
            string strInfoAdicionais = "";
            if (pExcecao.Data != null)
            {
                strInfoAdicionais = "<b>Informações Adicionais:</b> <br>".Normalize();
                foreach (DictionaryEntry item in pExcecao.Data)
                {
                    strInfoAdicionais += "<b>" + item.Key.ToString() + ":</b> <br>" + item.Value.ToString() + "<br>";
                }
            }
            string strMetodoAnterior = "";
            if (pExcecao.InnerException != null)
            {
                strMetodoAnterior = "Metodo Anterior: <BR>" + pExcecao.InnerException.TargetSite.ToString();
            }

            if (pObservacao == null)
            {
                pObservacao = string.Empty;
            }

            strMensagemLog = strMensagemLog.Replace("{HORA}", DateTime.Now.ToString("dd/MM/yyyy HH:mm ss"));
            strMensagemLog = strMensagemLog.Replace("{APLICACAO}", System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim());
            strMensagemLog = strMensagemLog.Replace("{MAQUINA}", Environment.MachineName);
            strMensagemLog = strMensagemLog.Replace("{CAMINHO}", Environment.CurrentDirectory);
            strMensagemLog = strMensagemLog.Replace("{ORIGEM}", pExcecao.Source.ToString());
            strMensagemLog = strMensagemLog.Replace("{CLASS}", pExcecao.TargetSite.DeclaringType.ToString());
            strMensagemLog = strMensagemLog.Replace("{TIPO_MEMBER}", pExcecao.TargetSite.MemberType.ToString());
            strMensagemLog = strMensagemLog.Replace("{MEMBER}", pExcecao.TargetSite.ToString());
            strMensagemLog = strMensagemLog.Replace("{MENSAGEM}", pExcecao.Message.ToString());
            strMensagemLog = strMensagemLog.Replace("{TRACE}", pExcecao.StackTrace.ToString().Replace("at ", "<BR>"));
            strMensagemLog = strMensagemLog.Replace("{INFO_ADICIONAIS}", strInfoAdicionais);
            strMensagemLog = strMensagemLog.Replace("{METODO_ANTERIOR}", strMetodoAnterior);
            strMensagemLog = strMensagemLog.Replace("{OBSERVACAO}", pObservacao);
            Logger.GravarLogExcecao(strMensagemLog);
            if (pNotificar)
            {
                EnvelopeEmail objetoEmail = new EnvelopeEmail();
                objetoEmail.Assunto = "Exceção - " + System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim() + " | " + DateTime.Now.ToString();
                objetoEmail.CorpoTexto = strMensagemLog;
                objetoEmail.EmailRemetente = "Exceção " + System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim() + " " + "<sistemaintersic@mfmti.com.br>";
                objetoEmail.EmailDestinatario = ConfigurationManager.AppSettings["DESTINATARIO_EXCECAO"];
                objetoEmail.EnviarEmail();
            }
        }

        #endregion Métodos
    }
}