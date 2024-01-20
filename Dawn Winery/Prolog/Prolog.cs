using System;
using System.Diagnostics;
using System.Globalization;
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

        public Tuple<string[][], float[][], int[], int[]> make_recipes(string Grapenames, string Tons)
        {

            string query = $@"make_recipes([{Grapenames}], [{Tons}], RecipesN, RecipesT, Qualitys, Total).";

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

                    // Köşeli parantezleri ve içindeki virgülü kullanarak ayırma
                    string[] recipeNArray = recipesNValue
                        .Replace("[[", "")  // İlk baştaki iki köşeli parantezi kaldır
                        .Replace("]]", "")  // İlk sondaki iki köşeli parantezi kaldır
                        .Split(new string[] { "], [" }, StringSplitOptions.None);

                    // Her bir öğeyi virgül ile ayırarak iç içe dizileri oluştur
                    string[][] recipeName = new string[recipeNArray.Length][];
                    for (int i = 0; i < recipeNArray.Length; i++)
                    {
                        recipeName[i] = recipeNArray[i].Split(',');
                    }


                    string recipesTValue = rtMatch.Groups[1].Value;

                    // Köşeli parantezleri ve içindeki virgülü kullanarak ayırma
                    string[] recipeTArray = recipesTValue
                        .Replace("[[", "")  // İlk baştaki iki köşeli parantezi kaldır
                        .Replace("]]", "")  // İlk sondaki iki köşeli parantezi kaldır
                        .Split(new string[] { "], [" }, StringSplitOptions.None);

                    // Her bir öğeyi virgül ile ayırarak iç içe dizileri oluştur
                    float[][] recipeT = new float[recipeTArray.Length][];
                    for (int i = 0; i < recipeTArray.Length; i++)
                    {
                        string[] innerArray = recipeTArray[i].Split(',');
                        recipeT[i] = new float[innerArray.Length];

                        for (int j = 0; j < innerArray.Length; j++)
                        {
                            if (float.TryParse(innerArray[j], NumberStyles.Any, CultureInfo.InvariantCulture, out float result))
                            {
                                recipeT[i][j] = result;
                            }
                        }
                    }


                    string qualitysValue = qMatch.Groups[1].Value;

                    string[] qualityArray = qualitysValue
                        .Replace("[", "")  // İlk baştaki köşeli parantezi kaldır
                        .Replace("]", "")  // İlk sondaki köşeli parantezi kaldır
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    // Her bir öğeyi int'e çevirerek quality dizisini oluştur
                    int[] quality = new int[qualityArray.Length];
                    for (int i = 0; i < qualityArray.Length; i++)
                    {
                        if (int.TryParse(qualityArray[i], out int result))
                        {
                            quality[i] = result;
                        }

                    }


                    string totalValue = tMatch.Groups[1].Value;

                    string[] totalArray = totalValue
                        .Replace("[", "")  // İlk baştaki köşeli parantezi kaldır
                        .Replace("]", "")  // İlk sondaki köşeli parantezi kaldır
                        .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    // Her bir öğeyi int'e çevirerek quality dizisini oluştur
                    int[] total = new int[totalArray.Length];
                    for (int i = 0; i < totalArray.Length; i++)
                    {
                        if (int.TryParse(totalArray[i], out int result))
                        {
                            total[i] = result;
                        }

                    }

                    return Tuple.Create(recipeName, recipeT, quality, total);
                }


            }

            return null;
        }
 

        //make_recipes(Grapenames, Tons, Recipes,Total)



    }
}
