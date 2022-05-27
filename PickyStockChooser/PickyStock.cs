using System;
using System.IO; // For file access
using CsvHelper; // Used for reading the CSV file
using System.Globalization; // For namespaces
using CsvHelper.Configuration.Attributes; // Used to apply name attributes for the list


namespace CSVFileReader
{
  public class PickyStock
{
    public static void Main(string[] args)
    {
        using (var streamReader = new StreamReader(@"C:\Users\slgar\Downloads\NASDAQ_Data.csv")) // Using StreamReader to access the csv file
        {
            using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture)) // Using CSV reader to process the files and format them
            {
                var records = csvReader.GetRecords<NasdaqData>().ToList(); // Creating the "records" list for our file that we will use

                int totalfirms = records.Count; // Creating a variable to count the total amount of firms in the sample

                    Console.WriteLine("Total firms in list: " + totalfirms); // Notifying the user how many firms are in the sample

                Console.WriteLine("What Letter do you want the stock symbol to start with?[NA to skip]"); // First filter - Symbol first letter
                var v1 = Console.ReadLine().ToLower(); // Reads user input (ToLower added to match in case user inputs lowecase letter)
                    if (v1 == "na") //If user selects NA - the filter is not applied 
                    {
                        Console.WriteLine("Criteria skipped"); ; // Notifies user that the criteria has been skipped
                    }
                    else 
                    {
                        records = records.Where(Symbol => Convert.ToString(Symbol.Symbol[0]).ToLower() == v1).ToList(); //Filters only for firms that have the same first symbol letter as the one selected by the user
                        totalfirms = records.Count; // Updates the new count with how many firms remain on the list
                        Console.WriteLine("Total firms remaining: " + totalfirms); // Notifies the user on how many firms remain in the list
                    }

                Console.WriteLine("Which country do you want the stock to be from?[NA to skip]"); // Second filter - Country
                var v2 = Console.ReadLine().ToLower(); // Reads user input
                    if (v2 == "na") //If user selects NA - the filter is not applied 
                    {
                        Console.WriteLine("Criteria skipped"); // Notifies user that the criteria has been skipped
                    }
                    else
                    {
                        records = records.Where(country => country.Country.ToLower() == v2).ToList(); //Applies country filter
                        totalfirms = records.Count; // Updates the new count with how many firms remain on the list
                        Console.WriteLine("Total firms remaining: " + totalfirms); // Notifies the user on how many firms remain
                    }
                    
                // As the remaining filters are done in a similar way, only partial comments applied
                Console.WriteLine("Maximum year for an IPO (inclusive) [0 to skip]"); // Third filter - maximum IPO year
                var v3 = Convert.ToInt32(Console.ReadLine());
                    if (v3 == 0)
                    {
                        Console.WriteLine("Criteria skipped"); ;
                    }
                    else
                    {
                        records = records.Where(ipomax => ipomax.IPOYear <= v3).ToList(); //Applies IPO max filter
                        totalfirms = records.Count;
                        Console.WriteLine("Total firms remaining: " + totalfirms);
                    }
                    


                Console.WriteLine("Minimum year for an IPO (inclusive) [0 to skip]"); // Fourth filter - maximum IPO year
                var v4 = Convert.ToInt32(Console.ReadLine());
                    if (v4 == 0)
                    {
                        Console.WriteLine("Criteria skipped"); ;
                    }
                    else
                    {
                        records = records.Where(ipomin => ipomin.IPOYear >= v4).ToList(); // Applies IPO min filter
                        totalfirms = records.Count;
                        Console.WriteLine("Total firms remaining: " + totalfirms);
                    }
                    

                Console.WriteLine("Minimum absolute stock growth in dollars.cents (inclusive) [-999 to skip]"); // Fifth filter - absolute stock price growth
                var v5 = Convert.ToDouble(Console.ReadLine());
                    if (v5 == -999)
                    {
                        Console.WriteLine("Criteria skipped"); ;
                    }
                    else
                    {
                        records = records.Where(stockgrowth => stockgrowth.NetChange >= v5).ToList(); // Applies absoluite price stock growth filter
                        totalfirms = records.Count;
                        Console.WriteLine("Total firms remaining: " + totalfirms);
                    }
                    

                Console.WriteLine("Minimum relative stock growth in percent (inclusive) [-999 to skip]"); // Sixth filter - percentage growth
                var v6 = Convert.ToDouble(Console.ReadLine());
                    if (v6 == -999)
                    {
                        Console.WriteLine("Criteria skipped"); ; 
                    }
                    else
                    {
                        records = records.Where(stockgrowthr => stockgrowthr.PercChange >= v6).ToList(); // Applies percentage growth filter
                        totalfirms = records.Count;
                        Console.WriteLine("Total firms remaining: " + totalfirms);
                    }
                    

                Console.WriteLine("What Sector do you want the firm to be in? [NA to skip]"); // Seventh filter - Sector
                var v7 = Console.ReadLine().ToLower();
                    if (v7 == "na")
                    {
                        Console.WriteLine("Criteria skipped"); ;
                    }
                    else
                    {
                        records = records.Where(sectorselect => sectorselect.Sector.ToLower() == v7).ToList(); // Applies sector filter
                        totalfirms = records.Count;
                        Console.WriteLine("Total firms remaining: " + totalfirms);
                    }

                    if (totalfirms == 0) //Checks if any firms remain in the file
                    {
                        Console.WriteLine("No firms match the search! Widen your search preferences!"); // If no firms remain prints out the fail message
                    }
                    else // If some firms remain, the console prints out the firms that match the criteria
                    {
                        Console.WriteLine("The following stocks match all criteria: "); // Afer the code has been run it prints the full info on the selected stocks
                        Console.WriteLine("Symbol, Name, Last Sale Price, Net Change $, Percentage Change, Country, IPO Year, Sector, Industry");
                        foreach (var record in records) // Using a loop function the program prints out the full data from the list
                        {
                            Console.WriteLine($"{record.Symbol},{record.Name},{record.LastSale},{record.NetChange},{record.PercChange},{record.Country},{record.IPOYear},{record.Sector},{record.Industry}");
                        }
                    }
                }

        }

        }
}
    public class NasdaqData // This is done to define each name in the file as a speficic class
    {
        [Name("Symbol")] // Symbol is a string is it takes the form of letters
        public string Symbol { get; set; } = default!;
        [Name("Name")] // Name of the company needs to be a string
        public string Name { get; set; } = default!;
        [Name("Last Sale")] // Last Sale is a double as the price is denoted in dollars and cents
        public double LastSale { get; set; }
        [Name("Net Change")] // Net change is the price change in dollars and cents so again a double
        public double NetChange { get; set; }
        [Name("% Change")] // percent change taken as a double for convenience 
        public double PercChange { get; set; }
        [Name("Country")] // Country name is a string
        public string Country { get; set; } = default!;
        [Name("IPO Year")] // IPO Year is an integer as years are whole nubers
        public int IPOYear { get; set; } 
        [Name("Sector")] // Sector is a string 
        public string Sector { get; set; } = default!;
        [Name("Industry")] // Industry is a string 
        public string Industry { get; set; } = default!;
    }
}

