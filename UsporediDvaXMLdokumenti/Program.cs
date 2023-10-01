using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UsporediDvaXMLdokumenti
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Čita xml datoteka koji je u dokument gdje je 'Program.cs' datoteka pa 'bin' dokument pa 'Debug' dokument
            XDocument prviXml = XDocument.Load(new FileStream("prvi.xml", FileMode.Open, FileAccess.Read));
            XDocument drugiXml = XDocument.Load(new FileStream("drugi.xml", FileMode.Open, FileAccess.Read));

            var xmlComb = from xmlBooks1 in prviXml.Descendants("book")
                          from xmlBooks2 in drugiXml.Descendants("book")
                          select new
                          {
                              book1 = new
                              {
                                  id = xmlBooks1.Attribute("id").Value,
                                  image = xmlBooks1.Attribute("image").Value,
                                  name = xmlBooks1.Attribute("name").Value
                              },
                              book2 = new
                              {
                                  id = xmlBooks2.Attribute("id").Value,
                                  image = xmlBooks2.Attribute("image").Value,
                                  name = xmlBooks2.Attribute("name").Value
                              }
                          };


            var xmlRez = from i in xmlComb
                          where (i.book1.id == i.book2.id
                                 || i.book1.image == i.book2.image
                                 || i.book1.name == i.book2.name) 
                                 &&
                                 !(i.book1.id == i.book2.id
                                 && i.book1.image == i.book2.image
                                 && i.book1.name == i.book2.name)
                          select i;

            Console.WriteLine("Issued \t\tIssue type \t\tIssueInFirst \tIssueInSecond \r\n");
            int greska = 0;
            foreach (var rez in xmlRez)
            {
                string message = "";
                if (rez.book1.id != rez.book2.id)
                {
                    greska++;
                    message = greska + ".\t\tid is different\t\t" + rez.book1.id + "\t\t" + rez.book2.id;
                }
                if (rez.book1.image != rez.book2.image)
                {
                    greska++;
                    message = greska + ".\t\timage is different\t" + rez.book1.image + "\t\t" + rez.book2.image;
                }
                if (rez.book1.name != rez.book2.name)
                {
                    greska++;
                    message = greska + ".\t\tname is different\t" + rez.book1.name + "\t\t" + rez.book2.name;
                }
                Console.WriteLine(message);
            }

            Console.ReadKey();
        }
    }
}
