using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Prolog;

namespace Dawn_Winery.Prolog
{
    public class Prolog
    {
        private PrologEngine prolog; // Declare prolog as a class-level variable
        private string dosyaIcerigi;
        private string dosyaYolu;
        public Prolog()
        {
            prolog = new PrologEngine(persistentCommandHistory: false);

            dosyaYolu = "C:\\Users\\90531\\source\\repos\\Dawn Winery\\Dawn Winery\\Prolog\\grape_rules.pl";

            // Dosya içeriğini bir dizeye oku
            dosyaIcerigi = File.ReadAllText(dosyaYolu);

            // Dizeyi danışma işlemine tabi tut
            prolog.ConsultFromString(dosyaIcerigi);
        }

        public Tuple<bool, int> g_quality(string grape)
        {
            string tValue = "";
            int qValue = 0;

            string query = $"g_quality({grape}, T, Q).";

            var solution = prolog.GetFirstSolution(query: query);

            if (solution.Solved)
            {
                // Extracting values of T and Q using regular expressions
                string solutionString = solution.ToString();

                Match tMatch = Regex.Match(solutionString, @"T = (\w+)");
                Match qMatch = Regex.Match(solutionString, @"Q = (\d+)");

                if (tMatch.Success && qMatch.Success)
                {
                    tValue = tMatch.Groups[1].Value;

                    // Attempt to convert qValue to an integer
                    int.TryParse(qMatch.Groups[1].Value, out qValue);

                    // Determine if tValue is "red" or "white"
                    bool isWhite = (tValue == "white");
                    return Tuple.Create(isWhite, qValue);
                }
            }

            return Tuple.Create(false, 0);
        }




        public string add_grape(string Name, bool Type, int Alkol, int Sugar, int Acidity, int Body, int Tannin)
        {
            string sonuc = "Grape eklenemedi.";

            string Type_str = Type ? "white" : "red";

            string query = $"add_grape({Name}, {Type_str}, {Alkol}, {Sugar}, {Acidity}, {Body}, {Tannin}).";

            var solution = prolog.GetFirstSolution(query: query);

            if (solution.Solved)
            {
                sonuc = $"{Name} başarıyla eklendi.";

            }

            return sonuc;
        }


        //add_grape(Name, Type, Alkol, Sugar, Acidity, Body, Tannin)

        public string remove_grape(string Name)
        {
            string sonuc = "Grape silinemedi.";

            string query = $"remove_grape({Name}).";

            var solution = prolog.GetFirstSolution(query: query);

            if (solution.Solved)
            {
                sonuc = $"{Name} başarıyla silindi.";

            }

            return sonuc;
        }


        //remove_grape(Name)


        public int aging(string Names, string Tons)
        {
            int qValue = 0;

            string query = $"aging([{Names}],[{Tons}],Year).";


            var solution = prolog.GetFirstSolution(query: query);

            if (solution.Solved)
            {
                string solutionString = solution.ToString();

                Match qMatch = Regex.Match(solutionString, @"Year = (\d+)");

                if (qMatch.Success)
                {


                    int.TryParse(qMatch.Groups[1].Value, out qValue);


                }
            }

            return qValue;
        }
        //aging(Grapes, Tons, Year)

        public string make_recipes(string Grapenames, string Tons)
        {
            string sonuc = "Sonuç bulunamadı";

            string query = $"make_recipes([{Grapenames}], [{Tons}], RecipesN, RecipesT, Qualitys, Total).";

            var solution = prolog.GetFirstSolution(query: query);

            if (solution.Solved)
            {
                string solutionString = solution.ToString();

                Match rnMatch = Regex.Match(solutionString, @"RecipesN = (\[.*\])");
                Match rtMatch = Regex.Match(solutionString, @"RecipesT = (\[.*\])");
                Match qMatch = Regex.Match(solutionString, @"Qualitys = (\[.*\])");
                Match tMatch = Regex.Match(solutionString, @"Total = (\[.*\])");

                if (rnMatch.Success && tMatch.Success)
                {
                    string recipesNValue = rnMatch.Groups[1].Value;
                    string recipesTValue = rtMatch.Groups[1].Value;
                    string qualitysValue = qMatch.Groups[1].Value;
                    string totalValue = tMatch.Groups[1].Value;

                    sonuc = $"{recipesNValue}:::::{recipesTValue}:::::{qualitysValue}:::::{totalValue}";
                }
            }

            return sonuc;
        }



        //make_recipes(Grapenames, Tons, Recipes,Total)



    }
}
