using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using Dados;

namespace APPTeste
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestePadrao();
            //TesteDataTable();
            TesteDataTableFonteDeDados();
        }


        static  void TestePadrao() {


            Console.Write("Teste de leitura de arquivo");


            using (StreamReader LeitorTXT = File.OpenText("C:\\eula.1028.txt"))
            {

                string input = null;

                while ((input = LeitorTXT.ReadLine()) != null)
                {

                    Console.WriteLine(input);

                }

            }
            Console.ReadLine();
        }


        static void TesteDataTableFonteDeDados() 
        {
            int numeroLinha = 1;

            FonteDeDadosOld fonteDados = new FonteDeDadosOld();

            fonteDados.Arquivo = "teste.txt";
            fonteDados.Local = "D:\\";
            fonteDados.Origem = TipoFonteDados.TXT;

            fonteDados.SeparadorTXT = ';' ;

            DataTable tabTeste = fonteDados.NewDataTable("");

            DataTableReader rd = tabTeste.CreateDataReader();
            

            tabTeste.CreateDataReader();

            while (rd.Read())
            {
                Console.WriteLine("Linha numero: " + numeroLinha.ToString());
                Console.WriteLine(rd[0].ToString());
                Console.WriteLine(rd[1].ToString());
                Console.WriteLine(rd[2].ToString());
                Console.WriteLine(rd[3].ToString());
                Console.WriteLine(rd[4].ToString());
                Console.WriteLine(rd[5].ToString());
                numeroLinha++;
            }

            Console.ReadLine();
            

        }


        static void TesteDataTable() 
        {
            DataTable tabTeste = new DataTable();
            DataRow linhaTeste;
            DataColumn col01 = new DataColumn("col01", typeof(string));
            DataColumn col02 = new DataColumn("col02", typeof(string));

            tabTeste.Columns.Add(col01);
            tabTeste.Columns.Add(col02);


            int linha=1;

            while(linha <=10)
            {
                linhaTeste = tabTeste.NewRow();
                linhaTeste["col01"] = "Fabio";
                linhaTeste["col02"] = linha.ToString();


                tabTeste.Rows.Add(linhaTeste);


                linha+=1;

            }


            DataTableReader rd = tabTeste.CreateDataReader();

            while(rd.Read())
            {
                Console.WriteLine(rd[0].ToString());
                Console.WriteLine(rd[1].ToString());

            }

            Console.ReadLine();

            

        }


        static void TesteFonteDados() { 
        
            //To Do
        
        }

    }
}
