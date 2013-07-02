optparse
========

An implementation of pythons optparse OptionParser for the .NET framework.  This is a WIP but most of the core functionality is all working.


Feel free to send a pull request if you have something to contribute.

Example Usage:
// Define static type -
public class SearchSettings
{
        public string ScanDirectoryPath { get; set; }
        public List<string> SearchTerms { get; set; }
        public string OutputFilePath { get; set; }
        public bool ScanRecursive { get; set; }
}
 
// Create new option parser and hand it the static type that will be parsed into
var parser = new OptionParser<SearchSettings>();
parser.AddOption("-p", "--path", "ScanDirectoryPath", "The base directory to begin scanning.", defaultValue: "C:/");
parser.AddOption("-t", "--terms", "SearchTerms", "Include all the instances that should be scanned.");
parser.AddOption("-o", "--output", "OutputFilePath", "The path to write results to.", defaultValue: "C:/Output");
parser.AddOption("-r", "--recursive", "ScanRecursive", "Indicates if all subdirectories should be scanned recursively.", OptionAction.StoreTrue, defaultValue: configManager.GetScanSubdirectories());
 
// Parse the args (usually passed in from the Main() method)
var config = parser.ParseArgs(args);
 
// Static access
Console.WriteLine(config.Options.ScanDirectoryPath);
// prints "C:/"
