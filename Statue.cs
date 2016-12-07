using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Statue
{

        /*static void Main(){
            Statue ustawaZPliku = new Ustawa("nazwa_pliku.html");
            Statue ustawaZeStringu = new Ustawa("<html>.....</html>", true);
            ustawaZPliku.pages[0] <- dostep do zawartosci strony pierwszej ustawy
        }*/ //<- do maina
    internal sealed class Statue
    {
        private HtmlDocument htmlFromFile { get; set; }
        public List<String> pages = new List<String>();

        public Statue(string a, bool readFromString = false)
        {
            HtmlDocument html = readFromString ? ReadHtmlDocumentFromString(a) : ReadHtmlDocumentFromFile(a);
            htmlFromFile = html;
            ReadStatuePagesFromHtml(htmlFromFile); //przerabia html na strony
        }

        public Statue(string a) : this(a, false)
        {

        }

        public void ReadStatuePagesFromHtml(HtmlDocument document)
        {
            HtmlNodeCollection htmlNodes = document.DocumentNode.SelectNodes("(//div[@class='pf w0 h0'])"); //wybieramy wszystkie 'div' o takiej samej klasie
            // XPATH: http://ricostacruz.com/cheatsheets/xpath.html
            foreach (var node in htmlNodes)
            {
                string pageContent = "";
                HtmlNodeCollection pageElements = node.SelectNodes("(div/div[position()>2])");
                foreach (var element in pageElements)
                {
                    if (element.InnerText != "") //jezeli innerText nie jest pusty - (dokument zawiera duzo pustych spanów)
                    {
                        pageContent += element.InnerText; //dodaje tekst do zawartosci strony
                    }
                }

                pages.Add(pageContent); //strona dodana do listy stron

            }
        }

        static public HtmlDocument ReadHtmlDocumentFromFile(string documentPath)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(documentPath, Encoding.UTF8);
            return doc;
        }

        public HtmlDocument ReadHtmlDocumentFromString(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
    }
}
