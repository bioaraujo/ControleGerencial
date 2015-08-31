using System;
using System.Configuration;
using Util.Email;
using Util.Log;

namespace Util.Ocorrencia
{
    public sealed class TrataNotificacao
    {
        #region Métodos

        /// <summary>
        /// Grava a notificação no diretório especificado no registro PATH_LOG. O nome do Log será o do projeto que estava sendo executado e também
        /// envia um e-mail com a notificação caso seja solicitado.
        /// </summary>
        /// <param name="pMensagem">Mensagem que será salva no log</param>
        /// <param name="pNotificar">Deve enviar email de notificação?</param>
        public static void GravarNotificacao(string pMensagem, bool pNotificar = false)
        {
            string strMensagemLog = @"
<!--{PROXIMO_LOG}-->
             <tr>
                <td>
                    <table width=100%>
                        <tr bgcolor=#f5f5f5>
                            <td colspan=2 align=center> <b>NOTIFICAÇÃO</b> : {HORA} </td>
                        </tr>

                        <tr bgcolor=#ADCCFF>
                            <td colspan=2  align=Left>
                            &nbsp;
                            </td>
                        </tr>
                        <tr bgcolor=""#ffffcc"">
                            <td colspan=2  align=Left>
                            <b>Aplicação:</b> {APLICACAO} <br>
                            <b>Mensagem:</b> {MENSAGEM} <br>
                            <b>Maquina:</b> {MAQUINA} <br>
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

            Logger.GravarLogNotificacao(strMensagemLog);

            if (pNotificar)
            {
                EnvelopeEmail objetoEmail = new EnvelopeEmail();
                objetoEmail.Assunto = "Notificação - " + System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim() + " | " + DateTime.Now.ToString();
                objetoEmail.CorpoTexto = strMensagemLog;
                objetoEmail.EmailRemetente = "Notificação " + System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim() + " " + "<sistemaintersic@mfmti.com.br>";
                objetoEmail.EmailDestinatario = ConfigurationManager.AppSettings["DESTINATARIO_NOTIFICACAO"];
                objetoEmail.EnviarEmail();
            }
        }

        #endregion Métodos
    }
}