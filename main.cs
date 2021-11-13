// kektura.csv http://www.infojegyzet.hu/erettsegi/informatika-ismeretek/kozep-prog-2016minta/
//192
//Sumeg, vasutallomas;Sumeg, buszpalyaudvar;1,208;16;6;n
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

class Tura
{
    public string eredetisor;
    public string start_nev   { get; set; }
    public string stop_nev    { get; set; }
    public double hossz       { get; set; }
    public int    emelkedes   { get; set; }
    public int    lejtes      { get; set; }
    public string pecset_hely { get; set; }
    public bool HianyosNev    { get; set; } 
    
    public static int magassag  { get; set; } 
    public int szakasz_magassag { get; set; }

    public Tura(string sor)
    {
        eredetisor = sor;
        var s = sor.Split(';');
        start_nev   = s[0];
        stop_nev    = s[1];
        hossz       = double.Parse(s[2].Replace(",",""));
        emelkedes   = int.Parse(s[3]);
        lejtes      = int.Parse(s[4]);
        pecset_hely = s[5]; 

        HianyosNev          = pecset_hely =="i" && !stop_nev.Contains("pecsetelohely");
        Tura.magassag      += emelkedes - lejtes;
        szakasz_magassag    = Tura.magassag;   
    }
}

class Program 
{
    public static void Main (string[] args) 
    {
        //2. feladat: Adatok beolvasása, tárolása
        var lista = new List<Tura>();
                
        var fr        = new StreamReader("kektura.csv");
        var elsosor   = fr.ReadLine();
        Tura.magassag = int.Parse(elsosor);
        //Console.WriteLine(elsosor);
        while(!fr.EndOfStream)
        {   
            var sor = fr.ReadLine().Trim();
            lista.Add( new Tura(sor) ); 
        }
        fr.Close();

        //3. feladat: Szakaszok száma:
        Console.WriteLine($"3. feladat: Szakaszok száma: {lista.Count()}");

        //4. feladat: A túra teljes hossza:
        var hossz = 
        (
            from sor in lista
            select sor.hossz
        ).Sum();
        Console.WriteLine($"4. feladat: A túra teljes hossza: {hossz/1000} km");

        //5. feladat: A legrövidebb szakasz adatai:
        var legrovidebb = 
        (
            from sor in lista
            orderby sor.hossz
            select sor
        ).First();
        Console.WriteLine($"5. feladat: A legrövidebb szakasz adatai:");
        Console.WriteLine($"        Kezdete: {legrovidebb.start_nev}");
        Console.WriteLine($"        Vége: {legrovidebb.stop_nev}");
        Console.WriteLine($"        Távolság: {legrovidebb.hossz/1000} km");

        //7. feladat: Hiányos Állomásnevek:
        var hianyos = 
        (
            from sor in lista
            where sor.HianyosNev
            select sor.stop_nev
        );

        if (hianyos.Any())
        {
            Console.WriteLine(    $"7. feladat:  Hiányos Állomásnevek:");
            foreach(var sor in hianyos)
            {
                Console.WriteLine($"        {sor}");
            }
        }
        else
        {
            Console.WriteLine(    $"7. feladat:  Nincs hiányos állomásnév:");
        }
        
        // 8. feladat: A túra legmagasabban fekvő végpontja:
        var magassagok = 
        (
            from sor in lista
            orderby sor.szakasz_magassag 
            select sor   
        ).Last();
                
        Console.WriteLine($"8. feladat: A túra legmagassabban fekvő végpontja:");
        Console.WriteLine($"        A végpont neve: {magassagok.stop_nev}");
        Console.WriteLine($"        A végpont tengerszint feletti magassága: {magassagok.szakasz_magassag}");
    
        // 9. feladat:
        var fw = new StreamWriter("kektura2.csv");
        fw.WriteLine(elsosor);
        string res = "";
        foreach(var sor in lista){
            if (sor.HianyosNev)
            {
                res = sor.eredetisor.Replace(sor.stop_nev, sor.stop_nev + " pecsetelohely");
            }
            else
            {
                res = sor.eredetisor;
            }
            fw.WriteLine(res);
        }
        fw.Close();

    } // --------------- Main vége -----------------------------------
}
        