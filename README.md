# GermanToCSharpGenerator

## Overview

GermanToCSharpGenerator is a Roslyn Analyzer that translates German-like pseudo-code into valid C# code.
It allows developers to write code using German keywords and structure, which is then automatically converted into proper C# syntax.
This project is intended as a playful experiment, showcasing the power of Roslyn to parse and analyze code structures.

## Features

* Converts German-based code syntax into valid C#.
* Supports basic constructs such as namespaces, classes, methods, variables, and comments.
* Utilizes Roslyn for static code analysis and includes custom diagnostics to aid in the implementation process

## Example

### Input (German-like Code)

    Namenraum SomeNamespace;

    öffentlich Klasse SomeClass
    {
        privat Zeichenkette input = "some comment"; // This is a comment
    
        öffentlich int Check()
        {
            if (input == "some comment")
            {
                return 250;
            }
            else
            {
                return 200;
            }
        }
    }

### Output (Generated C# Code)

    namespace SomeNamespace;

    public class SomeClass
    {
        private string input = "some comment"; // This is a comment
    
        public int Check()
        {
            if (input == "some comment")
            {
                return 250;
            }
            else
            {
                return 200;
            }
        }
    }

## Installation

To use GermanToCSharpGenerator, follow these steps:
* Clone the repository.
* Build the project using .NET SDK.
* Currently no public NuGet available (See `planned improvements`)

## Usage

* Modify Project `GermanToCSharpGenerator.Example`
* Write your code using German keywords and structure. Files must have the `.dcs` ending.
* While building, the generator runs and generates standard C#.

## Planned Improvements
* More Keyword support
* Nuget Release
* Grammar-Aware Keyword Mapping (öffentlich, öffentliche, öffentlicher → public)
* Support for Advanced C# Constructs (e.g. Lists)

## Contributing

Contributions are welcome! If you would like to contribute, please submit a pull request.

## Contact

For questions or feedback, please open an issue on the repository.