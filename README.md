# DebtRatchet [![Join the chat at https://gitter.im/keyboardDrummer/DebtRatchet](https://badges.gitter.im/keyboardDrummer/DebtRatchet.svg)](https://gitter.im/keyboardDrummer/DebtRatchet?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge) [![Build status](https://ci.appveyor.com/api/projects/status/jnq3kagdkp5xtqi5?svg=true)](https://ci.appveyor.com/project/keyboardDrummer/DebtRatchet)
DebtRatchet is a Roslyn diagnostic, meaning it's a plugin for the .NET compiler that runs while you program. Using the principle of a ratchet, it allows you to systematically reduce technical debt in legacy codebases. 

When the plugin detects an increase in technical debt, an error or warning is given depending on your configuration.

The plugin detects technical debt using these simple rules:
* Method has too many parameters
* Method is too long
* Type has too many fields
* Type is too long

The thresholds for these rules can be configured using assembly level attributes.

For the plugin to work it must be allowed to automatically annotate your codebase in places where technical debt occurs.
This allows the plugin to report only *increases* in technical debt.

###### Related work
- [git-ratchet](https://gowalker.org/github.com/iangrunert/git-ratchet) is a tool that allows you to track metrics and apply a ratchet to them.
- [Quality](https://github.com/apiology/quality) is debt ratchet tool for Ruby.

## Demo
Here follows a screenshot showing the plugin in action.

![Screenshot](http://i.imgur.com/6Gp7qMm.png)

 The assembly level annotations at the top specify that a method may not have more than five parameters, and that violations of this rule result in a compile error. Inside the class are four scenario's all related to the rule 'Method has too many parameters'. From top to bottom these scenario's are:

1. A method has exactly five parameters so it is within the limit.
2. A method has six parameters, so it is over the limit and gives an error.
3. A legacy method has more than six parameters. Since it is legacy we don't wish to fix this technical debt right now and have annotated the method so that it does not give an error. These annotations can be generated for an entire solution using a provided command line tool, or by using Visual Studio fixes that come with the Roslyn diagnostic.
4. A legacy method with too many parameters has gained even more parameters after we started using DebtRatchet and annotated it with the DebtMethod annotation. Because we don't want our technical debt to grow, this method does give an error.

