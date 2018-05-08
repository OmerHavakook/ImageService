/// <summary>
/// This is the interface of the commands, It's responsible of
/// executing a command.
/// </summary>
namespace ImageService.Commands
{
    interface ICommand
    {
        /// <summary>
        /// This method is executing a command using the args it gets.
        /// Thw result bool changes to false if an error had occured and
        /// to true if the command executed with no exeptions.
        /// </summary>
        /// <param name="args"></param>
        /// as args to the function
        /// <param name="result"></param>
        /// as a boolean to change according to the funcion success or failue
        /// <returns></returns>
        /// string that reports the function success or failure
        string Execute(string[] args, out bool result);
    }
}
