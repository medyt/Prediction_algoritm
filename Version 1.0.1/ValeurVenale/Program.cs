using System;
using System.IO;
using System.Linq;

namespace ValeurVenale
{
    class Program
    {
        //Type of properties
        public enum HouseTypeEnum
        {
            Other = 0,
            House = 1,
            Commercial = 2,
            Apartment = 3,
            Garage = 4,
            InvestmentBuilding = 5,
            Storage = 6,
            Office = 7,
            Project = 8
        }
        //statistical variable
        static double[] priceDifference;
        static double[] procentOfEroor;
        //algorithmic variable
        static int numberOfProperty;
        static string [] Country;
        static double [] LivingSpaceSurface;
        static int[] NumberOfFacades;
        static int[] ConstructionYears;
        static double[] LandSurfaces;
        static double[] Prices;
        static int[] Types;
        static string[] ApartmentTypes;
        static void Main(string[] args)
        {
            readFromDatabase();
            int i;
            double VN;
            double SUP;
            double indice = 1.15;
            double Tc = 10000;
            //ultima valoare din istoricul indicelui ABEX din istoricul sau 
            double ABEX = 775;

            double VGlob;
            double UG;
            double UT;

            double CG = 1;

            double Vrt;
            double pricepermeter;

            double Values;

            double CI;

            double VV;
            string newProperty = "";

            //statistical variable initialization;
            priceDifference = new double[1000];
            procentOfEroor = new double[1000];

            for (i = 1; i <= numberOfProperty; i++)
            {
                if(Country[i] != null && Prices[i] > 1500)
                {
                    if(LivingSpaceSurface[i] != 0)
                    {
                        SUP = LivingSpaceSurface[i];
                        if (Types[i] == 1)
                        {
                            if (NumberOfFacades[i] != 0)
                            {
                                if (NumberOfFacades[i] == 2)
                                    Tc = 1.153;
                                else
                                {
                                    if (NumberOfFacades[i] == 3)
                                        Tc = 1.55;
                                    else
                                        Tc = 1.65;
                                }
                            }
                        } else
                        {
                            if (Types[i] == 3)
                            {
                                if (ApartmentTypes[i] == "AppartmentTypeSimple")
                                {
                                    Tc = 1.85;
                                }
                                else
                                {
                                    Tc = 2;
                                }
                            }
                        }
                        VN = computeValeurNeuf(SUP, indice, Tc, ABEX);
                        int procent = 0;
                        if (ConstructionYears[i] != 0)
                        {
                            DateTime date = DateTime.Now;
                            int differenceYear;
                            differenceYear = date.Year - ConstructionYears[i];
                            if (differenceYear < 10)
                                procent = differenceYear * 3;
                            else
                            {
                                if(differenceYear < 15)
                                {
                                    procent = differenceYear * 4;
                                }
                                else
                                {
                                    procent = differenceYear * 5;
                                }
                            }
                        }
                        if (procent > 100 || procent == 0)
                        {
                            UG = 0.092 * VN;
                            UT = 0;
                        }
                        else
                        {
                            UG = 0.08 * VN;
                            UT = procent / 100 * VN;
                        }
                        VGlob = computeValeurGloballe(UT, UG);

                        //where is CG 
                        CG = 1;

                        //Price per meter in functie de locatie
                        pricepermeter = 90;
                        Vrt = computeValeurReziduelle(LandSurfaces[i], pricepermeter);
                        //trebuie observate garajul si alte chestii de pe langa casa.
                        Values = 6000;
                        //rata dobanzii unei cladiri
                        CI = 1;
                        VV = computeValeurVenale(VN, VGlob, CG, Vrt, Values, CI);
                        //output in CSV
                        priceDifference[i] = (Prices[i] > VV) ? Prices[i] - VV : VV - Prices[i];
                        procentOfEroor[i] = priceDifference[i] * 100 / Prices[i];
                        newProperty += VV + "," + Prices[i] + "," + priceDifference[i] + "," + procentOfEroor[i] + "," + "\n";          
                        
                    }
                }                
            }
            File.WriteAllText(@"C:\Users\Madalin\Desktop\ValeurVenale\predict.csv", newProperty);
            

            int countPropertyEvaluate = 0;
            double procentageaverage = 0;
            for (i = 1;i <= numberOfProperty; i++)
            {
                if(Types[i] == 1 || Types[i] == 3)
                {
                    countPropertyEvaluate++;
                    procentageaverage += procentOfEroor[i];
                }
            }
            procentageaverage /= countPropertyEvaluate;
            Console.WriteLine(procentageaverage);

        }          
        static void readFromDatabase()
        {
            numberOfProperty = 0;
            string[] words = new string[100];
            Country = new string[1000];
            LivingSpaceSurface = new double[1000];
            NumberOfFacades = new int[1000];
            ConstructionYears = new int[1000];
            LandSurfaces = new double[1000];
            Prices = new double[1000];
            Types = new int[1000];
            ApartmentTypes = new string[1000];
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("C:\\Users\\Madalin\\Desktop\\ValeurVenale\\property.txt"))
                {
                    String line = sr.ReadLine();
                    while (line != null)
                    {
                        if (line.Contains("/*"))
                        {
                            numberOfProperty++;
                        }
                        if (line.Contains("Country"))
                        {
                            words = line.Split('"');
                            Country[numberOfProperty] = words[3];
                        }
                        if (line.Contains("LivingSpaceSurface"))
                        {
                            words = line.Split('"');
                            LivingSpaceSurface[numberOfProperty] = Convert.ToDouble(words[2].Split(':')[1].Split(',')[0]);
                        }
                        if (line.Contains("NumberOfFacades"))
                        {
                            words = line.Split('"');
                            NumberOfFacades[numberOfProperty] = Convert.ToInt32(words[2].Split(':')[1].Split(',')[0]);
                        }
                        if (line.Contains("ConstructionYear"))
                        {
                            words = line.Split('"');
                            if(words[3] == "Unknown")
                            {
                                ConstructionYears[numberOfProperty] = 0;
                            } else {
                                ConstructionYears[numberOfProperty] = Convert.ToInt32(words[3]);
                            }
                            
                        }
                        if (line.Contains("LandSurface"))
                        {
                            words = line.Split('"');
                            
                            if (words[2] == " : ")
                            {
                                LandSurfaces[numberOfProperty] = 0;
                            } else
                            {
                                LandSurfaces[numberOfProperty] = Convert.ToDouble(words[2].Split(':')[1].Split(',')[0]);
                            }                            
                        }
                        if (line.Contains("Price"))
                        {
                            words = line.Split('"');
                            Prices[numberOfProperty] = Convert.ToDouble(words[2].Split(':')[1].Split(',')[0]);
                        }
                        if (line.Contains("Type"))
                        {
                            words = line.Split('"');
                            if(words[1] == "Type")
                                Types[numberOfProperty] = Convert.ToInt32(words[2].Split(':')[1].Split(',')[0]);
                        }
                        if(line.Contains("AppartmentType"))
                        {
                            words = line.Split('"');
                            ApartmentTypes[numberOfProperty] = words[3];
                        }
                        Console.WriteLine(line);
                        line = sr.ReadLine();
                    }
                }
                Console.WriteLine(Country[775]);
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        static double computeValeurVenale(double VN, double VGlob, double CG, double Vrt, double Values, double CI)
        {
            double x = ((VN - VGlob) * CG + Vrt + Values) * CI;
            return x;
        }
        static double computeValeurNeuf(double SUP, double indice, double Tc, double ABEX)
        {
            double x = SUP * indice * Tc * ABEX;
            return x;
        }
        static double computeValeurGloballe(double UT, double UG)
        {
            double x = UT + UG;
            return x;
        }
        static double computeValeurReziduelle(double terrainSurface, double pricepermeterTerrain)
        {
            double x = terrainSurface * 2 / 3 * pricepermeterTerrain;
            return x;
        }
        
    }
}






/*Exemplul din paper
 VN = computeValeurNeuf(181.68, 1.15, 1.53, 729);
            UG = 0.08 * VN;
            VGlob = computeValeurGloballe(19500, UG);
            CG = 1;
            double terrainSurface = 197.97;
            Vrt = computeValeurReziduelle(terrainSurface, 90);
            Values = 6000;
            CI = 1;
            VV = computeValeurVenale(VN, VGlob, CG, Vrt, Values, CI);
            Console.WriteLine(VV);
 */
