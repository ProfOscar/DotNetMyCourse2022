#!/usr/bin/env dotnet-script

// LAMBDA TEST

Func<DateTime, bool> canDrive = dob =>
{
    return dob.AddYears(18) < DateTime.Now;
};

DateTime dob = new DateTime(2000, 12, 25);

bool result = canDrive(dob);
Console.WriteLine(result);

Action<DateTime> printDate = date => Console.WriteLine(date);
printDate(dob);


// Una lambda che prende in input due parametri stringa e restituisce la loro concatenazione
Func<string, string, string> stConcat = (st1, st2) => { return st1 + " " + st2; };
Console.WriteLine(stConcat("Paolino", "Paperino"));

// Una lambda che prende in input 3 parametri interi e restituisce il più grande dei 3
Func<int, int, int, int> maxInt = (a, b, c) =>
{
    int max = a;
    if (b > max) max = b;
    if (c > max) max = c;
    return max;
};
Console.WriteLine(maxInt(12, 67, 4));

// Una lambda che prende 2 parametri DateTime e non restituisce nulla, ma stampa a video la minore delle 2 date
Action<DateTime, DateTime> minDate = (d1, d2) =>
{
    if (d1.CompareTo(d2) < 0) Console.WriteLine(d1);
    else Console.WriteLine(d2);
};
minDate(new DateTime(2023, 06, 25), DateTime.Today);
