using UnityEngine;

public class MoveCommand : ICommand
{
    private string _messageToPrint;
    
    public MoveCommand(string message)
    {
        _messageToPrint = message;
    }

    public void Execute()
    {
        Debug.Log(_messageToPrint);
    }
}