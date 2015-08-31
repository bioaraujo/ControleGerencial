using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace Util.Email
{
    public class EnvelopeEmail
    {
        #region Atributos

        private MailMessage _mailMsg;
        private SmtpClient _smtpCliente;
        private System.Net.NetworkCredential _credenciais;

        private string _assunto;
        private string _corpoTexto;
        private string _emailDestinatario;
        private string _emailRemetente;
        private List<string> _cc;
        private List<string> _cco;
        private List<string> _anexos;
        private string _emailRecebimentoRetorno;
        private string _smtp;
        private int _portaSmtp;
        private string _userSmtp;
        private string _senhaSmtp;
        private bool _ssl;

        #endregion Atributos

        #region Propriedades

        public string Assunto
        {
            get { return _assunto; }
            set
            {
                _assunto = value;
                _mailMsg.Subject = _assunto;
            }
        }

        public string CorpoTexto
        {
            get { return _corpoTexto; }
            set
            {
                _corpoTexto = value;
                System.Net.Mail.AlternateView plainView = System.Net.Mail.AlternateView.CreateAlternateViewFromString
                                                          (System.Text.RegularExpressions.Regex.Replace(_corpoTexto, @"<(.|\n)*?>", string.Empty), null, "text/plain");
                System.Net.Mail.AlternateView htmlView = System.Net.Mail.AlternateView.CreateAlternateViewFromString(_corpoTexto, null, "text/html");

                _mailMsg.AlternateViews.Add(plainView);
                _mailMsg.AlternateViews.Add(htmlView);
            }
        }

        public string EmailDestinatario
        {
            get { return _emailDestinatario; }
            set
            {
                _emailDestinatario = value;
                _mailMsg.To.Add(new MailAddress(_emailDestinatario));
            }
        }

        public string EmailRemetente
        {
            get { return _emailRemetente; }
            set
            {
                _emailRemetente = value;
                _mailMsg.From = new MailAddress(_emailRemetente);
            }
        }

        public List<string> Cc
        {
            get { return _cc; }
        }

        public List<string> Cco
        {
            get { return _cco; }
        }

        public List<string> Anexos
        {
            get { return _anexos; }
        }

        public string EmailRecebimentoRetorno
        {
            get { return _emailRecebimentoRetorno; }
            set
            {
                _emailRecebimentoRetorno = value;
                _mailMsg.Headers.Add("Disposition-Notification-To", _emailRecebimentoRetorno);
            }
        }

        public string Smtp
        {
            get { return _smtp; }
            set
            {
                _smtp = value;
                _smtpCliente.Host = _smtp;
            }
        }

        public int PortaSmtp
        {
            get { return _portaSmtp; }
            set
            {
                _portaSmtp = value;
                _smtpCliente.Port = _portaSmtp;
            }
        }

        public string UserSmtp
        {
            get { return _userSmtp; }
            set
            {
                _userSmtp = value;
                _credenciais.UserName = _userSmtp;
            }
        }

        public string SenhaSmtp
        {
            get { return _senhaSmtp; }
            set
            {
                _senhaSmtp = value;
                _credenciais.Password = _senhaSmtp;
            }
        }

        public bool Ssl
        {
            get { return _ssl; }
            set
            {
                _ssl = value;
                _smtpCliente.EnableSsl = _ssl;
            }
        }

        #endregion Propriedades

        #region Construtores

        public EnvelopeEmail()
        {
            _mailMsg = new MailMessage();
            _mailMsg.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
            _smtpCliente = new SmtpClient();
            _credenciais = new System.Net.NetworkCredential();

            _cc = new List<string>();
            _cco = new List<string>();
            _anexos = new List<string>();

            EmailRemetente = "sistemaintersic@mfmti.com.br";
            UserSmtp = "sistemaintersic@mfmti.com.br";
            SenhaSmtp = "mudar@123";
            Smtp = "smtp.gmail.com";
            PortaSmtp = 587;
            Ssl = true;
        }

        #endregion Construtores

        #region Métodos

        public void AdicionarCc(string pCc)
        {
            _cc.Add(pCc);
        }

        public void RemoverCc(string pCc)
        {
            int index = -1;
            index = _cc.FindIndex(x => x.StartsWith(pCc));
            if (index >= 0)
            {
                _cc.RemoveAt(index);
            }
            else
            {
                //Não existe o cc informado.
            }
        }

        public void AdicionarCco(string pCco)
        {
            _cco.Add(pCco);
        }

        public void RemoverCco(string pCco)
        {
            int index = -1;
            index = _cco.FindIndex(x => x.StartsWith(pCco));
            if (index >= 0)
            {
                _cco.RemoveAt(index);
            }
            else
            {
                //Não existe o cco informado.
            }
        }

        public void AdicionarAnexo(string pAnexo)
        {
            _anexos.Add(pAnexo);
        }

        public void AdicionarAnexo(string pPath, string pContentId)
        {
            _anexos.Add(pPath + "," + pContentId);
        }

        public void RemoverAnexo(string pAnexo)
        {
            int index = -1;
            index = _anexos.FindIndex(x => x.StartsWith(pAnexo));
            if (index > 0)
            {
                _anexos.RemoveAt(index);
            }
            else
            {
                //Não existe o anexo informado.
            }
        }

        public string EnviarEmail()
        {
            Attachment att;

            if (_anexos != null)
            {
                for (int a = 0; a < _anexos.Count(); a++)
                {
                    if (_anexos[a].Split(',').Length > 1)
                    {
                        att = new Attachment(_anexos[a].Split(',')[0]);
                        att.ContentId = _anexos[a].Split(',')[1];
                    }
                    else
                    {
                        att = new Attachment(_anexos[a]);
                    }

                    _mailMsg.Attachments.Add(att);
                    att = null;
                }
            }

            if (_cc != null)
            {
                for (int a = 0; a < _cc.Count(); a++)
                {
                    _mailMsg.CC.Add(_cc[a]);
                }
            }

            if (_cco != null)
            {
                for (int a = 0; a < _cco.Count(); a++)
                {
                    _mailMsg.Bcc.Add(_cco[a]);
                }
            }

            _smtpCliente.Credentials = _credenciais;
            try
            {
                _smtpCliente.Send(_mailMsg);
                return "OK";
            }
            catch (Exception ex)
            {
                return "ERRO: " + ex.Message.ToString();
            }
        }

        #endregion Métodos
    }
}