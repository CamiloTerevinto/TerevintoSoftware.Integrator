using Spectre.Console.Cli;
using TerevintoSoftware.Integrator;

var app = new CommandApp();

app.Configure(configurator =>
{
    configurator
        .SetApplicationName("dotnet-integrator")
        .SetApplicationVersion("0.0.1");

    configurator.AddCommand<GenerateCommand>("generate")
        .WithDescription(
            "For each Controller found in the MVC project, generates a Fixture class/file with methods to call all endpoints. " + Environment.NewLine +
            ":warning:[bold red]Warning[/]:warning:: This will overwite the files in the output directory." + Environment.NewLine +
            "Please look at the GitHub project for more information: https://github.com/CamiloTerevinto/TerevintoSoftware.StaticSiteGenerator"
        );
});

return app.Run(args);