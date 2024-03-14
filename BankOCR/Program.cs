// See https://aka.ms/new-console-template for more information
using BankOCR.Utils;
using System;

// test convert digits the numbers
Console.WriteLine("\r\n=>new file 1\r\n");
ReadFile file = new ReadFile(@".\Files\TestFile1.txt");
var content = file.GetContent();

ParseContent parse = new ParseContent();
var flatNumbers = parse.GetOCRDigitCollection(content);
parse.ProcessOCRDigitCollection(flatNumbers);

foreach (var item in flatNumbers)
{
    Console.WriteLine(item);
}

// test convert digits the numbers 
// and check validity
Console.WriteLine("\r\n=>new file 2\r\n");
file = new ReadFile(@".\Files\TestFile2.txt");
content = file.GetContent();

parse = new ParseContent();
flatNumbers = parse.GetOCRDigitCollection(content);
parse.ProcessOCRDigitCollection(flatNumbers);
parse.CheckDigits(flatNumbers);

foreach (var item in flatNumbers)
{
    Console.WriteLine(item);
}

// test convert digits the numbers 
// and check validity
// and display unkown chars as ?
Console.WriteLine("\r\n=>new file 3\r\n");
file = new ReadFile(@".\Files\TestFile3.txt");
content = file.GetContent();

parse = new ParseContent();
flatNumbers = parse.GetOCRDigitCollection(content);
parse.ProcessOCRDigitCollection(flatNumbers);
parse.CheckDigits(flatNumbers);

foreach (var item in flatNumbers)
{
    Console.WriteLine(item);
}

// test convert digits the numbers 
// and check validity
// and re process for possiblities
Console.WriteLine("\r\n=>new file 4\r\n");
file = new ReadFile(@".\Files\TestFile4.txt");
content = file.GetContent();

parse = new ParseContent();
flatNumbers = parse.GetOCRDigitCollection(content);
parse.ProcessOCRDigitCollection(flatNumbers);
parse.CheckDigits(flatNumbers);
parse.ReProcessOCRDigitCollection(flatNumbers);

foreach (var item in flatNumbers)
{
    Console.WriteLine(item);
}

Console.ReadLine();
//var number = parse.GetRealNumbers(flatNumbers);
