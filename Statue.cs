﻿using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace Prawnotron
{

    /*static void Main(){
        Statue ustawaZPliku = new Ustawa("nazwa_pliku.html");
        Statue ustawaZeStringu = new Ustawa("<html>.....</html>", true);
        ustawaZPliku.pages[0] <- dostep do zawartosci strony pierwszej ustawy
    }*/ //<- do maina

    /// <summary>
    ///
    /// </summary>
    /// <autogeneratedoc />
    public class Statue
    {
        HtmlDocument HtmlFromFile { get; }
        /// <summary>
        /// Lista stron ustawy
        /// </summary>
        /// <autogeneratedoc />
        protected List<string> _pages;

        public Statue(string a, bool readFromString = false)
        {
            _pages = new List<string>();
            HtmlDocument html = readFromString ? ReadHtmlDocumentFromString(a) : ReadHtmlDocumentFromFile(a);
            HtmlFromFile = html;
            ReadStatuePagesFromHtml(HtmlFromFile); //przerabia html na strony
        }

        void ReadStatuePagesFromHtml(HtmlDocument document)
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

                _pages.Add(pageContent); //strona dodana do listy stron

            }
        }

        static HtmlDocument ReadHtmlDocumentFromFile(string documentPath)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(documentPath, Encoding.UTF8);
            return doc;
        }

        HtmlDocument ReadHtmlDocumentFromString(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
          public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var page in this.pages)
            {
                sb.Append(page);
            }
        
            return sb.ToString();
        }



        public void Zapisz()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter("ustawa.txt", true);
            file.WriteLine(this.ToString());
            file.Close();

        }
               public void Wyszukaj(string doc, string a)
        {
            List<String> list = new List<String>();
            string pattern = "Art. " + a ;
            int index = 0;
            int num1 = Int32.Parse(a);
            int num2 = num1 + 1;
            string d = Convert.ToString(num2);
            string pattern2 = "Art. " + d;
            while (true)

            {
                int b = doc.IndexOf(pattern,index);
                if (b == -1)
                    break;
                index = b + pattern.Length;

                int c = doc.IndexOf(pattern2, index);

                list.Add(pattern+doc.Substring(index, c - index));
            }
                foreach (string line in list)
                {
                    Console.WriteLine(line);
                }
            
        }
        /*static void Main(string[] args)
        {
            
           Statue ustawa = new Statue("strona_3.html");
            string p;
            string ustawaString = "";
                foreach (var page in ustawa.pages)
            {
                
                p = Convert.ToString(page);
                ustawaString+= p;

            }
        
           ustawa.Wyszukaj(ustawaString, "44");
         

        Console.ReadKey();
        }
    }*/
    }
}
