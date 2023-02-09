#!/usr/bin/env dotnet-script

// LINQ TEST

class Apple
{
    public string Color { get; set; }
    public int Weight { get; set; }
}

List<Apple> apples = new List<Apple> {
    new Apple {Color= "Red", Weight= 180},
    new Apple {Color= "Green", Weight= 195},
    new Apple {Color= "Red", Weight= 190},
    new Apple {Color= "Green", Weight= 185}
};

// sintassi prolissa
// Func<Apple, bool> takeRedApples = apple => apple.Color == "Red";
// IEnumerable<Apple> redApples = apples.Where(takeRedApples);

// sintassi compatta
IEnumerable<Apple> redApples = apples.Where(a => a.Color == "Red");

// uso simultaneo di Where, Select, Average
double average = apples
    .Where(a => a.Color == "Red")
    .Select(a => a.Weight)
    .Average();
Console.WriteLine($"La media di peso delle mele rosse è {average}");

int minimumWeight = apples
    .Select(a => a.Weight)
    .Min();
Console.WriteLine($"La mela più leggera pesa {minimumWeight}");

string color = apples
    .Where(a => a.Weight == 190)
    .Select(a => a.Color)
    .First();
Console.WriteLine($"La mela che pesa 190 è di colore {color}");

int redAppleCount = apples
    .Where(a => a.Color == "Red")
    .Count();
Console.WriteLine($"Ci sono {redAppleCount} mele rosse");

int totalGreenWeight = apples
    .Where(a => a.Color == "Green")
    .Select(a => a.Weight)
    .Sum();
Console.WriteLine($"Le mele verdi pesano in totale {totalGreenWeight}");