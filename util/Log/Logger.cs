using System;
using System.IO;
using System.Net;

namespace Util.Log
{
    public static class Logger
    {
        /// <summary>
        /// Responsável pela gravação do arquivo de Log.
        /// </summary>
        /// <param name="pMensagem"> Mensagem que será gravada no log. </param>
        public static void GravarLogExcecao(string pMensagem)
        {
            StreamWriter sw = null;
            try
            {
                string localGravacaoArquivo = new Registro().PathLog;
                string nomeArquivo = "Log" + System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim() + ".html";
                DirectoryInfo dir = new DirectoryInfo(localGravacaoArquivo);

                if (dir.Exists == false)
                {
                    dir.Create();
                }

                if (!File.Exists(dir.FullName + nomeArquivo))
                {
                    sw = File.CreateText(dir.FullName + nomeArquivo);
                    string strHtml = @"<html lang=""pt-br"">
                <meta http-equiv='Content-Type' content='text/html'; charset='utf-8' />
                <head>
                    <title>Log de Ocorrências</title>
                </head>
                <body>
                <table border=1>" + pMensagem;
                    sw.Write(strHtml);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    WebClient client = new WebClient();
                    client.Encoding = System.Text.Encoding.UTF8;
                    string htmlAtual = client.DownloadString(dir.FullName + nomeArquivo);

                    htmlAtual = htmlAtual.Replace("<!--{PROXIMO_LOG}-->", pMensagem);

                    sw = new StreamWriter(dir.FullName + nomeArquivo);
                    sw.Write(htmlAtual);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// Responsável pela gravação do arquivo de Log de Notificações.
        /// </summary>
        /// <param name="pMensagem"> Mensagem que será gravada no log. </param>
        public static void GravarLogNotificacao(string pMensagem)
        {
            StreamWriter sw = null;
            try
            {
                string localGravacaoArquivo = new Registro().PathLog;
                string nomeArquivo = "Notificacoes" + System.Reflection.Assembly.GetEntryAssembly().GetName().Name.Trim() + ".html";
                DirectoryInfo dir = new DirectoryInfo(localGravacaoArquivo);

                if (dir.Exists == false)
                {
                    dir.Create();
                }

                if (!File.Exists(dir.FullName + nomeArquivo))
                {
                    sw = File.CreateText(dir.FullName + nomeArquivo);
                    string strHtml = @"<html lang=""pt-br"">
                <meta http-equiv='Content-Type' content='text/html'; charset='utf-8' />
                <head>
                    <title>Log de Notificações</title>
                </head>
                <body>
                <table border=1>" + pMensagem;
                    sw.Write(strHtml);
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    WebClient client = new WebClient();
                    client.Encoding = System.Text.Encoding.UTF8;
                    string htmlAtual = client.DownloadString(dir.FullName + nomeArquivo);

                    htmlAtual = htmlAtual.Replace("<!--{PROXIMO_LOG}-->", pMensagem);

                    sw = new StreamWriter(dir.FullName + nomeArquivo);
                    sw.Write(htmlAtual);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception)
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
            }
        }
    }
}